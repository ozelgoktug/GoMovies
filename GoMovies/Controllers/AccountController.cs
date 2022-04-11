using GoMovies.Data;
using GoMovies.Entity;
using GoMovies.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GoMovies.Controllers
{

    //[Authorize]
    public class AccountController : Controller
    {

        //private readonly GoMovieContext _context;
        //public AccountController(GoMovieContext context)
        //{
        //    _context = context;
        //}


        private readonly SignInManager<ApplicationUser> singInManager;
        private readonly UserManager<ApplicationUser> userManager;

        public AccountController(SignInManager<ApplicationUser> _singInManager, UserManager<ApplicationUser> _userManager)
        {
            singInManager = _singInManager;
            userManager = _userManager;
        }


        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        // [ValidateAntiForgeryToken] //güvenlik için token dönecek onun için gerekli
        public async Task<IActionResult> Login(UserLoginModel model, string returnUrl)
        {

            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.User);
                if (user != null)
                {
                    await singInManager.SignOutAsync();
                    var result = await singInManager.PasswordSignInAsync(user, model.Password, false, false);
                    if (result.Succeeded)
                    {

                        return Redirect(returnUrl ?? "/");
                    }
                }
                ModelState.AddModelError(nameof(model.Email), "Hatalı kullanıcı adı veya parola girilmiştir.");
            }
            return View(model);
        }


        [AllowAnonymous]
        // [ValidateAntiForgeryToken] //güvenlik için token dönecek onun için gerekli
        public async Task<IActionResult> Logout()
        {
            await singInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }



        public IActionResult UpdateAccount(string userName, bool UpdateSituation)
        {

            //var value = userManager.Users.FirstOrDefault(x => x.Id == Convert.ToInt32(userid));
            var value = userManager.Users.FirstOrDefault(x => x.UserName == userName);

            if (User.Identity.IsAuthenticated && value.UserName == User.Identity.Name)
            {
                UserUpdateViewModel model = new UserUpdateViewModel
                {
                    Mail = value.Email,
                    UserName = value.UserName,
                    existingUserName = value.UserName,
                    Name = value.Name,
                    Surname = value.SurName,
                    ImageUrl = value.imageUrl
                };
                if (model.ImageUrl==null)
                {
                    model.ImageUrl = "avatar-icon.png";
                }
                ViewBag.UpdateSuccess = UpdateSituation;
                return View(model);
            }


            return RedirectToAction("Index", "Home");


        }

        [HttpPost]
        public async Task<IActionResult> UpdateAccount(UserUpdateViewModel p, IFormFile fileImg)
        {

            var user = userManager.Users.Where(x => x.UserName == p.existingUserName).FirstOrDefault();

            if (ModelState.IsValid)
            {
               
                // var newpassword = userManager.HashPassword(user, p.Password);
                // if (user!= null && user.PasswordHash == newpassword)

                //  var currentPasswordHash = userManager.PasswordHasher.HashPassword(user, p.Password);
                var passvalid = await userManager.ChangePasswordAsync(user, p.CurrentPassword, p.CurrentPassword);
                if (passvalid.Succeeded)

                {
                    // Valid Password

                    user.Email = p.Mail;
                    user.UserName = p.UserName;
                    user.Name = p.Name;
                    user.SurName = p.Surname;
                    
                    string existingUserImageUrl = user.imageUrl;
                   

                    //parola değiştirmek
                    if (p.Password != null && p.Password == p.ConfirmPassword)
                    {
                        user.PasswordHash = userManager.PasswordHasher.HashPassword(user, p.Password);
                        p.CurrentPassword = p.Password;
                    }


                    //Profil fotoğrafını eklemek:
                    if (fileImg != null)
                    {
                        var extension = Path.GetExtension(fileImg.FileName); // dosya uzantısını alır .jpg , .png gibi
                        var uniqFileName = string.Format($"{Guid.NewGuid()}{extension}"); //benzersiz bir resim ismi oluşturduk. NewGuid random benzersiz sayı üretir
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", uniqFileName);
                        user.imageUrl = uniqFileName;

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await fileImg.CopyToAsync(stream); //asenkron yaptık bu yüzden action medodu da Taske çevirdik
                                                               // fileImg.CopyTo(stream); //Olarak da kullanabilirdik
                     
                        }
                        p.ImageUrl = uniqFileName;


                        //Eskimovie fotoğrafı silmek
                        if (existingUserImageUrl!=null)
                        {
                            var deletePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", existingUserImageUrl);

                            if (System.IO.File.Exists(deletePath))
                            {
                                System.IO.File.Delete(deletePath);
                            }
                        }
                     


                        
                    }
                   

                    var result = await userManager.UpdateAsync(user);
                   
                    if (result.Succeeded)
                    { 

                       
                        ModelState.Clear();
                        ViewBag.UpdateSuccess = true;

                        //kullanıcı adı değiştirmek
                        if (p.existingUserName!=p.UserName)
                        {
                            p.existingUserName = p.UserName;
                            var ChangedUser = userManager.Users.Where(x => x.UserName == p.existingUserName).FirstOrDefault();
                            if (ChangedUser != null)
                            {
                                await singInManager.SignOutAsync();
                                var signResult = await singInManager.PasswordSignInAsync(ChangedUser, p.CurrentPassword, false, false);

                            }
                        }

                        //return View(p);
                        return RedirectToAction("UpdateAccount", "Account",new { userName=p.existingUserName, UpdateSituation = true });

                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }

                }
                else
                {
                    ModelState.AddModelError("", "Bilgilerinizi güncellemek için güncel parolanızı doğru olarak giriniz.");
                }

              

            }
            if (fileImg == null)
            {
                p.ImageUrl = user.imageUrl;
                // user.imageUrl = "avatar-icon.png";
            }

            return View(p);
        }


        public IActionResult AccessDenied()
        {
            return View();
        }


        //Eski login ve logout işlemlerim...
        //[HttpPost]
        //public IActionResult Login(AppUser appUser)
        //{

        //    var _user = _context.AppUsers.FirstOrDefault(x=> x.userName == appUser.userName);

        //    if(!_user.isOnline && _user.password == appUser.password)
        //    {

        //        if (_user.isOnline == false)
        //        {
        //            _user.isOnline = true;
        //        }
        //        else
        //        {
        //            _user.isOnline = false;
        //        }

        //        _context.Update(_user);
        //        _context.SaveChanges();

        //        return RedirectToAction("Index", "Home", new
        //        {
        //            userName = _user.userName,
        //            isOnline = _user.isOnline,
        //        });
        //    }

        //   return View();
        //}

        //[HttpPost]
        //public IActionResult Logout(string userName)
        //{

        //    var _user = _context.AppUsers.FirstOrDefault(x => x.userName == userName);

        //    if (_user.isOnline)
        //    {
        //        if (_user.isOnline == false)
        //        {
        //            _user.isOnline = true;
        //        }
        //        else
        //        {
        //            _user.isOnline = false;
        //        }
        //        _context.Update(_user);
        //        _context.SaveChanges();

        //        return RedirectToAction("Index", "Home", new
        //        {
        //            userName = _user.userName,
        //            isOnline = _user.isOnline,
        //        });
        //    }
        //    else
        //    {
        //        return RedirectToAction("Index", "Home", new
        //        {
        //            userName = _user.userName,
        //            isOnline = _user.isOnline,
        //        });
        //    }
        //}
    }
}
