using Hairdresser_Website.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hairdresser_Website.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult CorrectLogin()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Admin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Admin(AdminLogin login)
        {
            if ((login.Username == "b221210587@ogr.sakarya.edu.tr" && login.Password == "iskenderpassword") || (login.Username == "b221210058@sakary.edu.tr") && login.Password == "muhammedpassword")
            {
                return RedirectToAction("CorrectLogin");
            }
            return RedirectToAction("Admin");
        }
    }
}
