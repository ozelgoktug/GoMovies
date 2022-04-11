using GoMovies.Data;
using GoMovies.Entity;
using GoMovies.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoMovies.Controllers
{
    public class LoginController : Controller
    {

        private readonly GoMovieContext _context;
        public LoginController(GoMovieContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(AppUser appUser)
        {

            var _user = _context.AppUsers.FirstOrDefault(x=> x.userName == appUser.userName);

            if(!_user.isOnline && _user.password == appUser.password)
            {

                if (_user.isOnline == false)
                {
                    _user.isOnline = true;
                }
                else
                {
                    _user.isOnline = false;
                }

                _context.Update(_user);
                _context.SaveChanges();

                return RedirectToAction("Index", "Home", new
                {
                    userName = _user.userName,
                    isOnline = _user.isOnline,
                });
            }

           return View();
        }

        [HttpPost]
        public IActionResult Logout(string userName)
        {

            var _user = _context.AppUsers.FirstOrDefault(x => x.userName == userName);
          
            if (_user.isOnline)
            {
                if (_user.isOnline == false)
                {
                    _user.isOnline = true;
                }
                else
                {
                    _user.isOnline = false;
                }
                _context.Update(_user);
                _context.SaveChanges();

                return RedirectToAction("Index", "Home", new
                {
                    userName = _user.userName,
                    isOnline = _user.isOnline,
                });
            }
            else
            {
                return RedirectToAction("Index", "Home", new
                {
                    userName = _user.userName,
                    isOnline = _user.isOnline,
                });
            }
        }
    }
}
