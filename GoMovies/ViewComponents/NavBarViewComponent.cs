using GoMovies.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoMovies.ViewComponents
{
    public class NavBarViewComponent: ViewComponent
    {

        private readonly GoMovieContext _context;
        public NavBarViewComponent(GoMovieContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            return View(_context.AppUsers.ToList());               
        }                                                     
    }
}
