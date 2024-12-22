using Hairdresser_Website.Data;
using Hairdresser_Website.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Don't forget to include this

namespace Hairdresser_Website.Controllers
{
    [Authorize(Roles = "admin")]
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var employees = _context.Employees
                .Include(e => e.Salon)
                .Include(e => e.EmployeeAvailabilities) // Eagerly load Availabilities
                .ToList();
            return View(employees);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Salons = new SelectList(_context.Salons, "SalonId", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            // Remove validation errors for navigation properties
            ModelState.Remove("Salon");
            ModelState.Remove("Appointments");

            if (employee.EmployeeAvailabilities != null)
            {
                for (int i = 0; i < employee.EmployeeAvailabilities.Count; i++)
                {
                    ModelState.Remove($"EmployeeAvailabilities[{i}].Employee");
                }
            }

            if (ModelState.IsValid)
            {
                _context.Employees.Add(employee);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Salons = new SelectList(_context.Salons, "SalonId", "Name", employee.SalonId);
            return View(employee);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var employee = _context.Employees
                .Include(e => e.EmployeeAvailabilities)
                .FirstOrDefault(e => e.EmployeeId == id);

            if (employee == null) return NotFound();

            ViewBag.Salons = new SelectList(_context.Salons, "SalonId", "Name", employee.SalonId);
            return View(employee);
        }

        [HttpPost]
        public IActionResult Edit(Employee employee)
        {
            // Remove validation errors for navigation properties
            ModelState.Remove("Salon");
            ModelState.Remove("Appointments");

            if (employee.EmployeeAvailabilities != null)
            {
                for (int i = 0; i < employee.EmployeeAvailabilities.Count; i++)
                {
                    ModelState.Remove($"EmployeeAvailabilities[{i}].Employee");
                }
            }

            if (ModelState.IsValid)
            {
                var existingEmployee = _context.Employees
                    .Include(e => e.EmployeeAvailabilities)
                    .FirstOrDefault(e => e.EmployeeId == employee.EmployeeId);

                if (existingEmployee == null)
                {
                    return NotFound();
                }

                // Update scalar properties
                existingEmployee.Name = employee.Name;
                existingEmployee.Expertise = employee.Expertise;
                existingEmployee.SalonId = employee.SalonId;

                // Update availabilities
                foreach (var existingAvailability in existingEmployee.EmployeeAvailabilities.ToList())
                {
                    _context.Remove(existingAvailability);
                }

                foreach (var availability in employee.EmployeeAvailabilities)
                {
                    existingEmployee.EmployeeAvailabilities.Add(new EmployeeAvailability
                    {
                        DayOfWeek = availability.DayOfWeek,
                        StartTime = availability.StartTime,
                        EndTime = availability.EndTime
                    });
                }

                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Salons = new SelectList(_context.Salons, "SalonId", "Name", employee.SalonId);
            return View(employee);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var employee = _context.Employees
                .Include(e => e.Salon)
                .Include(e => e.EmployeeAvailabilities)
                .FirstOrDefault(e => e.EmployeeId == id);

            if (employee == null) return NotFound();

            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var employee = _context.Employees
                .Include(e => e.EmployeeAvailabilities)
                .FirstOrDefault(e => e.EmployeeId == id);

            if (employee != null)
            {
                // Remove associated availabilities first
                _context.EmployeeAvailability.RemoveRange(employee.EmployeeAvailabilities);

                // Then remove the employee
                _context.Employees.Remove(employee);

                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}