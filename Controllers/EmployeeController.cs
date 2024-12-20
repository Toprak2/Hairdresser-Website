using Hairdresser_Website.Data;
using Hairdresser_Website.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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
                .Include(e => e.Salon)  // This loads the Salon data
                .ToList();
            return View(employees);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var salons = _context.Salons.ToList();
            ViewBag.Salons = new SelectList(_context.Salons, "SalonId", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee employee)
        {

            ModelState.Remove("Salon");
            ModelState.Remove("Appointments");

            if (ModelState.IsValid)
            {
                _context.Employees.Add(employee);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var modelError in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"ModelState error: {modelError.ErrorMessage}");
                }
            }

            ViewBag.Salons = new SelectList(_context.Salons, "SalonId", "Name", employee.SalonId);
            return View(employee);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var employee = _context.Employees.Find(id);
            if (employee == null) return NotFound();

            ViewBag.Salons = new SelectList(_context.Salons, "SalonId", "Name", employee.SalonId);
            return View(employee);
        }

        [HttpPost]
        public IActionResult Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                var existingEmployee = _context.Employees.Find(employee.EmployeeId);
                if (existingEmployee == null)
                {
                    return NotFound();
                }

                // Update the properties
                existingEmployee.Name = employee.Name;
                existingEmployee.Expertise = employee.Expertise;
                existingEmployee.Availability = employee.Availability;
                existingEmployee.SalonId = employee.SalonId;

                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var modelError in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"ModelState error: {modelError.ErrorMessage}");
                }
            }

            ViewBag.Salons = new SelectList(_context.Salons, "SalonId", "Name", employee.SalonId);
            return View(employee);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var employee = _context.Employees
                .Include(e => e.Salon)
                .FirstOrDefault(e => e.EmployeeId == id);

            if (employee == null) return NotFound();

            return View(employee);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var employee = _context.Employees.Find(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
