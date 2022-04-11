using GoMovies.Data;
using GoMovies.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoMovies.ViewComponents
{
    public class GenresViewComponent: ViewComponent
    {
        private readonly GoMovieContext _context;
        public GenresViewComponent(GoMovieContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            var genres = _context.Genres.ToList();

            ViewBag.SelectedGenereId = RouteData.Values["id"]; // burada route içindeki id değerini View'e gönderiyoruz.
            return View(genres);                //böylece kategori menüsünde basılan butonun id değeri sayfanın id değeri
        }                                                       //ile eşit ise buton aktif olacak ve mavi renkli olacak.
   
    }
}
