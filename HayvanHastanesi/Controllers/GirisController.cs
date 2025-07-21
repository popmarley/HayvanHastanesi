using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Data;

namespace HayvanHastanesi.Controllers
{
    public class GirisController : Controller
    {
        private readonly ApplicationDbContext _context;

        public GirisController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Giris()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Giris(string email, string password, bool rememberMe)
        {
            var kullanici = _context.Kullanicis
                .FirstOrDefault(k => k.Eposta == email && k.Sifre == password);

            if (kullanici != null)
            {
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, kullanici.KullaniciAdi),
            new Claim(ClaimTypes.Role, kullanici.Rol)
        };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = rememberMe, // "Beni Hatırla"
                    ExpiresUtc = rememberMe ? DateTimeOffset.UtcNow.AddDays(7) : DateTimeOffset.UtcNow.AddHours(1)
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

                return RedirectToAction("Admin", "Admin");
            }

            ModelState.AddModelError("", "E-posta veya şifre hatalı.");
            return View();
        }
    }
}
