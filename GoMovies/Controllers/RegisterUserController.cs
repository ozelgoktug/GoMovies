using GoMovies.Entity;
using GoMovies.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoMovies.Controllers
{
    [AllowAnonymous]
    public class RegisterUserController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;

        public RegisterUserController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task <IActionResult> Index(UserSignUpViewModel p, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser()
                {
                    Email = p.Mail,
                    UserName = p.UserName,

                    Name = p.Name,
                    SurName = p.Surname,

                    
                };
                
                var result = await _userManager.CreateAsync(user , p.Password);

                if(result.Succeeded)
                {
                    var u = await _userManager.FindByNameAsync(p.UserName);
                    
                    if (p.RoleName != null)
                    {

                        await _userManager.AddToRoleAsync(u, p.RoleName);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(u,"USER");
                    }

                    return Redirect(returnUrl ?? "/"); 
                    
                }
                else
                {
                    foreach(var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }

            return View(p);
        }

    }
}
