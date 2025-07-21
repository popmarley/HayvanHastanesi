using Microsoft.AspNetCore.Mvc;

namespace HayvanHastanesi.Controllers
{
    public class HakkimizdaController : Controller
    {
        public IActionResult Hakkimizda()
        {
            return View();
        }
    }
}
