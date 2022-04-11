using GoMovies.Data;
using GoMovies.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoMovies.Controllers
{
    public class UserController_eski : Controller
    {

        private readonly GoMovieContext _context;
        public UserController_eski(GoMovieContext context)
        {
            _context = context;
        }

        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateUser(UserModel model)
        {
           
            
            
            var users = _context.AppUsers.Select(u => u.userName).ToList();

            var user = model.UserName;

            if(users.Any(u=>u==user))
            {
                ModelState.AddModelError(nameof(model.UserName), "Bu kullanıcı daha önce kullanılmış. Lütfen başka bir kullanıcı adı ile kayıt olmayı deneyiniz.");
            }

            return View();
        }
        [AcceptVerbs("GET","POST")]
        public IActionResult VerifyUserNameIsExist(string UserName)
        {

            var users = _context.AppUsers.Select(u => u.userName).ToList();

            if (users.Any(u => u == UserName))
            {
                return Json($" \" {UserName} \" isimli kullanıcı daha önce alınmıştır.");
            }
            return Json(true);
        }




    }
}
