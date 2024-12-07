using Hairdresser_Website.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hairdresser_Website.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Appointment() 
        { 
            return View();
        }
        public IActionResult AIHair()
        {
            return View();
        }
    }
}
