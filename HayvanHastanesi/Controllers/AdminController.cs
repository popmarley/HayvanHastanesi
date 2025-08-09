using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HayvanHastanesi.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {

        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Admin()
        {
            var iletisimListesi = _context.Iletisims
              .OrderByDescending(x => x.OlusturmaTarihi)
              .ToList();

            return View(iletisimListesi);
        }

        [HttpGet]
        public IActionResult EmailRead(int id)
        {
            var iletisim = _context.Iletisims.FirstOrDefault(i => i.IletisimID == id);
            if (iletisim == null) return NotFound();

            // Okundu olarak işaretle
            if (iletisim.Okundu != "E")
            {
                iletisim.Okundu = "E";
                _context.SaveChanges();
            }

            return View(iletisim); 
        }
            
    }
}
