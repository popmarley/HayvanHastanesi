﻿using Data;
using Microsoft.AspNetCore.Mvc;

namespace HayvanHastanesi.Controllers
{
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
    }
}
