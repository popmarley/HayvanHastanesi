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
        [HttpGet]
        public IActionResult MailMeta()
        {
            var lastId = _context.Iletisims
                .OrderByDescending(i => i.IletisimID)
                .Select(i => i.IletisimID)
                .FirstOrDefault();

            var unreadCount = _context.Iletisims.Count(i => i.Okundu == "H");
            return Json(new { lastId, unreadCount });
        }

        [HttpGet]
        public IActionResult CheckMail(long afterId = 0)
        {
            var newItems = _context.Iletisims
                .Where(i => i.IletisimID > afterId)
                .OrderBy(i => i.IletisimID)
                .Select(i => new {
                    iletisimID = i.IletisimID,
                    mesaj = i.Mesaj,
                    okundu = i.Okundu,
                    olusturmaTarihi = i.OlusturmaTarihi
                })
                .ToList();

            var unreadCount = _context.Iletisims.Count(i => i.Okundu == "H");
            return Json(new { unreadCount, newItems });
        }

    }
}
