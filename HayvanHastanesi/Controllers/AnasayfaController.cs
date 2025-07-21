using Microsoft.AspNetCore.Mvc;

namespace HayvanHastanesi.Controllers
{
    public class AnasayfaController : Controller
    {
        public IActionResult Anasayfa()
        {
            return View();
        }
    }
}
