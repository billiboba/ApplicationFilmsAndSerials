using Microsoft.AspNetCore.Mvc;
using ApplicationFilmsAndSerials.Data;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ApplicationFilmsAndSerials.Models;

namespace ApplicationFilmsAndSerials.Controllers
{
    public class VideosController : Controller
    {
        private readonly FilmsAndSerialsContext _context;

        public VideosController(FilmsAndSerialsContext context)
        {
            _context = context;
        }

        public IActionResult MainPage()
        {
            var films = _context.Films.Include(f => f.Genre).ToList();
            return View("~/Views/VideosPage/AllVideos.cshtml", films);
        }
        
    }
}
