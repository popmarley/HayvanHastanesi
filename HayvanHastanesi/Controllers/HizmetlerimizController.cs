using Microsoft.AspNetCore.Mvc;

namespace HayvanHastanesi.Controllers
{
    public class HizmetlerimizController : Controller
    {
        public IActionResult Hizmetlerimiz()
        {
            return View();
        }
    }
}
