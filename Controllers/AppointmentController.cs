using Hairdresser_Website.Data;
using Hairdresser_Website.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Hairdresser_Website.Controllers
{
    [Authorize] // Or [Authorize(Roles = "customer")] if only customers can book
    public class AppointmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppointmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Appointment/Create
        public IActionResult Create()
        {
            // Get the current user's ID from the custom claim
            string userId = User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Create", "Appointment") });
            }

            // Get the list of salons for the dropdown
            var salons = _context.Salons.ToList();
            ViewBag.Salons = new SelectList(salons, "SalonId", "Name");

            // You might need to pass other data (like services) to the view later

            return View();
        }

        // POST: Appointment/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Appointment appointment, int salonId, int serviceId, int employeeId, DateTime appointmentDate, TimeSpan appointmentTime)
        {

            // Get the current user's ID from the custom claim
            string userId = User.FindFirstValue("UserId");
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Create", "Appointment") });
            }

            // Combine date and time
            appointmentDate = appointmentDate.Date.Add(appointmentTime);
            appointmentDate = DateTime.SpecifyKind(appointmentDate, DateTimeKind.Utc);

            if (!IsAppointmentSlotAvailable(employeeId, appointmentDate, serviceId))
            {
                TempData["Error"] = "The selected time slot is not available.";
                return RedirectToAction(nameof(Create));
            }

            // Create a new appointment without navigation properties
            var newAppointment = new Appointment
            {
                UserId = userId,
                ServiceId = serviceId,
                EmployeeId = employeeId,
                AppointmentDate = appointmentDate,
                Status = AppointmentStatus.Confirmed
            };

            try
            {
                _context.Appointments.Add(newAppointment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while saving the appointment. Please try again.";
                return RedirectToAction(nameof(Create));
            }
        }

        private bool IsAppointmentSlotAvailable(int employeeId, DateTime appointmentDate, int serviceId)
        {
            try
            {
                // Get service duration
                var service = _context.Services.Find(serviceId);
                if (service == null)
                {
                    // Log or handle service not found
                    return false;
                }
                int serviceDuration = service.Duration;

                // Check for overlapping appointments (there shouldn't be any as DB is empty)
                bool hasOverlap = _context.Appointments.Any(a =>
                    a.EmployeeId == employeeId &&
                    a.AppointmentDate.Date == appointmentDate.Date &&
                    !(appointmentDate.AddMinutes(serviceDuration) <= a.AppointmentDate ||
                      appointmentDate >= a.AppointmentDate.AddMinutes(a.Service.Duration)));

                // Get day of week (0 = Sunday, 1 = Monday, etc.)
                var dayOfWeek = (Models.DayOfWeek)appointmentDate.DayOfWeek;
                var timeOfDay = appointmentDate.TimeOfDay;

                // Check employee availability
                var availability = _context.EmployeeAvailability
                    .FirstOrDefault(ea =>
                        ea.EmployeeId == employeeId &&
                        ea.DayOfWeek == dayOfWeek);

                if (availability == null)
                {
                    // Log that no availability was found for this day
                    return false;
                }

                bool isEmployeeAvailable =
                    timeOfDay >= availability.StartTime &&
                    timeOfDay.Add(TimeSpan.FromMinutes(serviceDuration)) <= availability.EndTime;

                // For debugging
                Console.WriteLine($"Employee: {employeeId}");
                Console.WriteLine($"Date: {appointmentDate}");
                Console.WriteLine($"Day of Week: {dayOfWeek}");
                Console.WriteLine($"Time of Day: {timeOfDay}");
                Console.WriteLine($"Service Duration: {serviceDuration}");
                Console.WriteLine($"Has Overlap: {hasOverlap}");
                Console.WriteLine($"Availability Found: {availability != null}");
                Console.WriteLine($"Start Time: {availability?.StartTime}");
                Console.WriteLine($"End Time: {availability?.EndTime}");
                Console.WriteLine($"Is Employee Available: {isEmployeeAvailable}");

                return isEmployeeAvailable && !hasOverlap;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error in IsAppointmentSlotAvailable: {ex.Message}");
                return false;
            }
        }

        // GET: /Appointment/GetEmployeesByServiceAndSalon
        [HttpGet]
        public IActionResult GetEmployeesByServiceAndSalon(int salonId, int serviceId)
        {
            // Get the employees who provide the selected service and work at the selected salon
            var employees = _context.Employees
                .Where(e => e.SalonId == salonId && e.Expertise != null)
                .Select(e => new { e.EmployeeId, e.Name })
                .ToList();

            return Json(employees);
        }

        // GET: /Appointment/GetServicesBySalon
        [HttpGet]
        public async Task<IActionResult> GetServicesBySalon(int salonId)
        {
            var services = await _context.Services
                .Where(s => s.SalonId == salonId)
                .Select(s => new {
                    serviceId = s.ServiceId,
                    name = s.Name,
                    price = s.Price,
                    duration = s.Duration
                })
                .ToListAsync();

            return Json(services);
        }

        [HttpGet]
        public async Task<IActionResult> GetAvailableTimeSlots(int employeeId, DateTime date, int serviceId)
        {
            try
            {
                // Convert input date to UTC
                date = DateTime.SpecifyKind(date.Date, DateTimeKind.Utc);

                // Get the service duration
                var service = await _context.Services.FindAsync(serviceId);
                if (service == null) return BadRequest("Service not found");
                int serviceDuration = service.Duration;

                // Get employee availability for the selected day of week
                var dayOfWeek = (Models.DayOfWeek)date.DayOfWeek;
                var availability = await _context.EmployeeAvailability
                    .Where(ea => ea.EmployeeId == employeeId && ea.DayOfWeek == dayOfWeek)
                    .ToListAsync();

                if (!availability.Any()) return Json(new List<TimeSlot>());

                // Get existing appointments for the selected date
                var existingAppointments = await _context.Appointments
                    .Include(a => a.Service)
                    .Where(a => a.EmployeeId == employeeId &&
                               a.AppointmentDate.Date == date.Date &&
                               a.Status != AppointmentStatus.Cancelled)
                    .Select(a => new {
                        StartTime = a.AppointmentDate.TimeOfDay,
                        EndTime = a.AppointmentDate.TimeOfDay.Add(TimeSpan.FromMinutes(a.Service.Duration))
                    })
                    .ToListAsync();

                // Generate time slots
                var timeSlots = new List<TimeSlot>();
                foreach (var av in availability)
                {
                    var currentTime = av.StartTime;
                    while (currentTime.Add(TimeSpan.FromMinutes(serviceDuration)) <= av.EndTime)
                    {
                        var slotEndTime = currentTime.Add(TimeSpan.FromMinutes(serviceDuration));
                        bool isAvailable = !existingAppointments.Any(a =>
                            !(slotEndTime <= a.StartTime || currentTime >= a.EndTime));

                        timeSlots.Add(new TimeSlot
                        {
                            Time = currentTime,
                            IsAvailable = isAvailable
                        });

                        currentTime = currentTime.Add(TimeSpan.FromMinutes(30)); // 30-minute intervals
                    }
                }

                return Json(timeSlots);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                // Log the error
                return Json(new List<TimeSlot>());
            }
        }

        public class TimeSlot
        {
            public TimeSpan Time { get; set; }
            public bool IsAvailable { get; set; }
        }
    }
}
