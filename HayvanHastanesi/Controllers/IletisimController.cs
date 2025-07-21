using Data;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace HayvanHastanesi.Controllers
{
    public class IletisimController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly Random _random = new();

        public IletisimController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Iletisim()
        {
            // 4 haneli rastgele güvenlik kodu üret
            var guvenlikKodu = _random.Next(1000, 9999).ToString();
            HttpContext.Session.SetString("GuvenlikKodu", guvenlikKodu);
            ViewBag.GuvenlikKodu = guvenlikKodu;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Iletisim(Iletisim model)
        {
            var gercekKod = HttpContext.Session.GetString("GuvenlikKodu");

            if (model.GirilenGuvenlikKodu != gercekKod)
            {
                ModelState.AddModelError("GirilenGuvenlikKodu", "Güvenlik kodu hatalı.");
            }

            if (ModelState.IsValid)
            {
                // Veritabanına güvenlik kodu dışında her şey kaydedilir
                var kayit = new Iletisim
                {
                    Ad = model.Ad,
                    Soyad = model.Soyad,
                    Mail = model.Mail,
                    Telefon = model.Telefon,
                    Mesaj = model.Mesaj,
                    OlusturmaTarihi = model.OlusturmaTarihi = DateTime.Now

                };

                _context.Iletisims.Add(kayit);
                _context.SaveChanges();

                TempData["Success"] = "Mesajınız başarıyla gönderildi.";
                return RedirectToAction("Iletisim");
            }

            // Kod hata varsa yeniden kod üret
            var yeniKod = _random.Next(1000, 9999).ToString();
            HttpContext.Session.SetString("GuvenlikKodu", yeniKod);
            ViewBag.GuvenlikKodu = yeniKod;

            return View(model);
        }
    }
}
