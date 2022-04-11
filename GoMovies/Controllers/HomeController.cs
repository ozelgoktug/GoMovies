using GoMovies.Data;
using GoMovies.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoMovies.Controllers
{
    public class HomeController : Controller
    {
        private readonly GoMovieContext _context;
        public HomeController(GoMovieContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var model = new HomePageViewModel()
            {
                PopularMovies = _context.Movies.ToList()
            };

            return View(model);
        }

        public IActionResult About()
        {
            return View();
        }
    }
}
