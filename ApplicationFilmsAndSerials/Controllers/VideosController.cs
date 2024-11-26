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
            var films = _context.Films.Include(f => f.Genre)
                .ToList();

            var serials = _context.Serials.Include(s => s.Genre).ToList();
            var genres = _context.Genres.ToList();

            var viewModel = new VideosViewModel
            {
                Films = films,
                Serials = serials,
                Genres = genres
            };

            return View("~/Views/VideosPage/AllVideos.cshtml", viewModel);
        }

        public IActionResult Filter(string type, int? ageLimit, int? genre)
        {
            var films = _context.Films.Include(f => f.Genre).AsQueryable();
            var serials = _context.Serials.Include(s => s.Genre).AsQueryable();

            if (!string.IsNullOrEmpty(type))
            {
                if (type == "films")
                {
                    serials = Enumerable.Empty<Serials>().AsQueryable();
                }
                else if (type == "serials")
                {
                    films = Enumerable.Empty<Films>().AsQueryable(); 
                }
            }

            if (ageLimit.HasValue)
            {
                films = films.Where(f => f.AgeLimit == ageLimit.Value);
                serials = serials.Where(s => s.AgeLimit == ageLimit.Value);
            }

            if (genre.HasValue)
            {
                films = films.Where(f => f.GenreId == genre.Value);
                serials = serials.Where(s => s.GenreId == genre.Value);
            }

            var model = new VideosViewModel
            {
                Films = films.ToList(),
                Serials = serials.ToList(),
                Genres = _context.Genres.ToList()
            };

            return View("~/Views/VideosPage/AllVideos.cshtml", model);
        }
    }
}
