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

            return View(new Iletisim { RandevuIstiyorum = false });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Iletisim(Iletisim model)
        {
            var gercekKod = HttpContext.Session.GetString("GuvenlikKodu");
            if (model.GirilenGuvenlikKodu != gercekKod)
                ModelState.AddModelError("GirilenGuvenlikKodu", "Güvenlik kodu hatalı.");

            // >>> Sadece randevu istiyorsa tarih-saat zorunlu + çakışma kontrolü
            if (model.RandevuIstiyorum)
            {
                if (!model.RandevuTarihi.HasValue)
                    ModelState.AddModelError("RandevuTarihi", "Randevu tarihi zorunludur.");

                if (!model.RandevuSaati.HasValue)
                    ModelState.AddModelError("RandevuSaati", "Randevu saati zorunludur.");

                if (model.RandevuTarihi.HasValue && model.RandevuSaati.HasValue)
                {
                    var doluSaatler = _context.Iletisims
                        .Where(x => x.RandevuTarihi.HasValue &&
                                    x.RandevuTarihi.Value.Date == model.RandevuTarihi.Value.Date)
                        .Select(x => x.RandevuSaati)
                        .ToList();

                    ViewBag.DoluSaatler = doluSaatler;

                    var cakismaVar = _context.Iletisims.Any(x =>
                        x.RandevuTarihi.HasValue &&
                        x.RandevuSaati.HasValue &&
                        x.RandevuTarihi.Value.Date == model.RandevuTarihi.Value.Date &&
                        x.RandevuSaati.Value == model.RandevuSaati.Value);

                    if (cakismaVar)
                        ModelState.AddModelError("RandevuSaati", "Bu saat için zaten bir randevu alınmış. Lütfen başka bir saat seçiniz.");
                }
            }

            if (ModelState.IsValid)
            {
                _context.Iletisims.Add(new Iletisim
                {
                    Ad = model.Ad,
                    Soyad = model.Soyad,
                    Mail = model.Mail,
                    Telefon = model.Telefon,
                    Mesaj = model.Mesaj,
                    OlusturmaTarihi = DateTime.Now,
                    RandevuTarihi = model.RandevuIstiyorum ? model.RandevuTarihi : null,
                    RandevuSaati = model.RandevuIstiyorum ? model.RandevuSaati : null,
                    Okundu = "H"
                });
                _context.SaveChanges();

                TempData["Success"] = model.RandevuIstiyorum
                    ? "Randevunuz başarıyla alınmıştır."
                    : "Mesajınız başarıyla gönderilmiştir.";

                return RedirectToAction("Iletisim");
            }

            var yeniKod = _random.Next(1000, 9999).ToString();
            HttpContext.Session.SetString("GuvenlikKodu", yeniKod);
            ViewBag.GuvenlikKodu = yeniKod;

            return View(model);
        }

        [HttpGet]
        public IActionResult GetMusaitSaatler(string tarih)
        {
            if (!DateTime.TryParse(tarih, out DateTime secilenTarih))
                return Json(new List<object>());

            var tumSaatler = new List<TimeSpan>();
            var baslangic = new TimeSpan(9, 0, 0);
            var bitis = new TimeSpan(19, 0, 0);
            var aralik = new TimeSpan(0, 15, 0);

            for (var saat = baslangic; saat <= bitis; saat += aralik)
                tumSaatler.Add(saat);

            var doluSaatler = _context.Iletisims
                .Where(x => x.RandevuTarihi.HasValue && x.RandevuTarihi.Value.Date == secilenTarih.Date)
                .Select(x => x.RandevuSaati)
                .ToList();

            var saatler = tumSaatler
                .Select(s => new
                {
                    Saat = s.ToString(@"hh\:mm"),
                    Dolu = doluSaatler.Contains(s)
                })
                .ToList();

            return Json(saatler);
        }


    }
}
