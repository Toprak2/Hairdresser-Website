using Hairdresser_Website.Data;
using Hairdresser_Website.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Make sure to include this for Include()

namespace Hairdresser_Website.Controllers
{
    [Authorize(Roles = "admin")]
    public class SalonController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SalonController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Eagerly load the WorkingHours
            var salons = _context.Salons
                .Include(s => s.WorkingHours)
                .ToList();
            return View(salons);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Salon salon)
        {

            // Remove validation errors for Salon navigation property in working hours
            if (salon.WorkingHours != null)
            {
                for (int i = 0; i < salon.WorkingHours.Count; i++)
                {
                    ModelState.Remove($"WorkingHours[{i}].Salon");
                }
            }

            // Debug logging
            Console.WriteLine($"Received salon: Name={salon.Name}, Location={salon.Location}");
            Console.WriteLine($"ModelState.IsValid: {ModelState.IsValid}");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Salons.Add(salon);
                    _context.SaveChanges();


                    Console.WriteLine($"Saved salon with ID: {salon.SalonId}");
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving salon: {ex.Message}");
                    ModelState.AddModelError("", "Unable to save changes. " + ex.Message);
                }
            }
            else
            {
                foreach (var modelError in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"ModelState error: {modelError.ErrorMessage}");
                }
            }

            return View(salon);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var salon = _context.Salons
                .Include(s => s.WorkingHours)
                .FirstOrDefault(s => s.SalonId == id);

            if (salon == null) return NotFound();

            return View(salon);
        }

        [HttpPost]
        public IActionResult Edit(Salon salon)
        {
            // Remove validation errors for Salon navigation property in working hours
            if (salon.WorkingHours != null)
            {
                for (int i = 0; i < salon.WorkingHours.Count; i++)
                {
                    ModelState.Remove($"WorkingHours[{i}].Salon");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Get the existing salon from the database
                    var existingSalon = _context.Salons
                        .Include(s => s.WorkingHours)
                        .FirstOrDefault(s => s.SalonId == salon.SalonId);

                    if (existingSalon == null)
                    {
                        return NotFound();
                    }

                    // Update scalar properties
                    existingSalon.Name = salon.Name;
                    existingSalon.Location = salon.Location;

                    // Update working hours
                    foreach (var existingHours in existingSalon.WorkingHours.ToList())
                    {
                        _context.Remove(existingHours);
                    }

                    foreach (var workingHour in salon.WorkingHours)
                    {
                        existingSalon.WorkingHours.Add(new SalonWorkingHours
                        {
                            DayOfWeek = workingHour.DayOfWeek,
                            StartTime = workingHour.StartTime,
                            EndTime = workingHour.EndTime
                        });
                    }

                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating salon: {ex.Message}");
                    ModelState.AddModelError("", "Unable to save changes. " + ex.Message);
                }
            }
            else
            {
                foreach (var modelError in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"ModelState error: {modelError.ErrorMessage}");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(salon);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var salon = _context.Salons
                .Include(s => s.WorkingHours)
                .FirstOrDefault(s => s.SalonId == id);

            if (salon == null) return NotFound();

            return View(salon);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var salon = _context.Salons
                .Include(s => s.WorkingHours)
                .FirstOrDefault(s => s.SalonId == id);

            if (salon != null)
            {
                // Remove associated working hours first
                _context.SalonWorkingHours.RemoveRange(salon.WorkingHours);

                // Then remove the salon
                _context.Salons.Remove(salon);

                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}