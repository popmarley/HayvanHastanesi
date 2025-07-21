using Microsoft.AspNetCore.Mvc;

namespace HayvanHastanesi.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Admin()
        {
            return View();
        }
    }
}
