using Hairdresser_Website.Data;
using Hairdresser_Website.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            var salons = _context.Salons.ToList();
            return View(salons);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(string Name, string Location, string WorkingHours)
        {
            Salon salon=new Salon();
            salon.Name = Name;
            salon.Location = Location;
            salon.WorkingHours = WorkingHours;
            // Debug logging
            Console.WriteLine($"Received salon: Name={salon.Name}, Location={salon.Location}");
            Console.WriteLine($"ModelState.IsValid: {ModelState.IsValid}");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Salons.Add(salon);
                    _context.SaveChanges();
                    // After saving, the salon object should have an ID
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
            var salon = _context.Salons.Find(id);
            if (salon == null) return NotFound();

            return View(salon);
        }

        [HttpPost]
        public IActionResult Edit(Salon salon)
        {
            if (ModelState.IsValid)
            {
                _context.Salons.Update(salon);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(salon);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var salon = _context.Salons.Find(id);
            if (salon == null) return NotFound();

            return View(salon);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var salon = _context.Salons.Find(id);
            if (salon != null)
            {
                _context.Salons.Remove(salon);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
