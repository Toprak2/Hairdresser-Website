using Microsoft.AspNetCore.Mvc;

namespace Hairdresser_Website.Controllers
{
    public class NewUserController : Controller
    {
        public IActionResult NewUser()
        {
            return View();
        }
    }
}
