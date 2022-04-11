using GoMovies.Data;
using GoMovies.Entity;
using GoMovies.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace GoMovies.Controllers
{

    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {

        private readonly GoMovieContext context;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public UserController(RoleManager<ApplicationRole> _roleManager, UserManager<ApplicationUser> _userManager, GoMovieContext _context)
        {
            roleManager = _roleManager;
            userManager = _userManager;
            context = _context;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ListUsers()
        {

            return View(userManager.Users.ToList());
        }

        // [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult CreateUser()
        {

            var roles = roleManager.Roles.ToList();

            List<RoleAssignViewModel> model = new List<RoleAssignViewModel>();

            foreach (var role in roles)
            {
                RoleAssignViewModel m = new RoleAssignViewModel
                {
                    RoleID = role.Id,
                    Name = role.Name,
                };
                model.Add(m);
            }

            UserSignUpViewModel modelSignUp = new UserSignUpViewModel()
            {
                RoleListModel = model
            };

            return View(modelSignUp);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserSignUpViewModel p)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser()
                {
                    Email = p.Mail,
                    UserName = p.UserName,

                    Name = p.Name,
                    SurName = p.Surname
                };

                var result = await userManager.CreateAsync(user, p.Password);

                if (result.Succeeded)
                {
                    var u = await userManager.FindByNameAsync(p.UserName);

                    if (p.RoleListModel.Count != 0)
                    {

                        foreach (var role in p.RoleListModel)
                        {
                            if (role.IsRoleAlreadyAssignedToUser)
                            {
                                await userManager.AddToRoleAsync(u, role.Name);
                            }
                            else
                            {
                                await userManager.RemoveFromRoleAsync(u, role.Name);
                            }
                        }
                    }
                    else
                    {
                        await userManager.AddToRoleAsync(u, "USER");
                    }

                    return RedirectToAction("ListUsers", "User");

                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }

            return View(p);
        }


        public async Task<IActionResult> UpdateUser(string userName)
        {

            //var value = userManager.Users.FirstOrDefault(x => x.Id == Convert.ToInt32(userid));
            var value = userManager.Users.FirstOrDefault(x => x.UserName == userName);

            if (User.Identity.IsAuthenticated && User.IsInRole("Admin"))
            {
                UserUpdateViewModel model = new UserUpdateViewModel
                {
                    Mail = value.Email,
                    UserName = value.UserName,
                    existingUserName = value.UserName,
                    Name = value.Name,
                    Surname = value.SurName,
                    Comments = new List<Models.Comment>(),
                    ImageUrl = value.imageUrl
                };

                //Rollerin listelenmesi
                List<RoleAssignViewModel> RoleModel = new List<RoleAssignViewModel>();

                var roles = roleManager.Roles.ToList();
                var userRoles = await userManager.GetRolesAsync(value);


                foreach (var role in roles)
                {
                    RoleAssignViewModel m = new RoleAssignViewModel
                    {
                        RoleID = role.Id,
                        Name = role.Name,
                        IsRoleAlreadyAssignedToUser = userRoles.Contains(role.Name)
                    };
                    RoleModel.Add(m);
                }

                model.RoleListModel = RoleModel;

                return View(model);
            }

            return RedirectToAction("ListUsers", "User");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(UserUpdateViewModel p, IFormFile fileImg)
        {

            if (ModelState.IsValid)
            {
                var user = userManager.Users.Where(x => x.UserName == p.existingUserName).FirstOrDefault();
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
                    if (p.Password != null && p.Password == p.ConfirmPassword)
                    {
                        user.PasswordHash = userManager.PasswordHasher.HashPassword(user, p.Password);
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
                    }
                    if (fileImg == null)
                    {
                        p.ImageUrl = user.imageUrl;
                        // user.imageUrl = "avatar-icon.png";
                    }


                    var result = await userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                      
                        var u = await userManager.FindByNameAsync(p.UserName);



                        foreach (var role in p.RoleListModel)
                        {
                            if (role.IsRoleAlreadyAssignedToUser)
                            {
                                await userManager.AddToRoleAsync(u, role.Name);
                            }
                            else
                            {
                                await userManager.RemoveFromRoleAsync(u, role.Name);
                            }
                        }

                        p.existingUserName = p.UserName;
                        ModelState.Clear();
                        ViewBag.UpdateSuccess = true;
                        return View(p);

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


            return View(p);
        }





        public async Task<IActionResult> DeleteUser(int id)
        {
            var value = userManager.Users.FirstOrDefault(x => x.Id == id);


            var result = await userManager.DeleteAsync(value);

            if (result.Succeeded)
            {
                return RedirectToAction("ListUsers", "User");

            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View();
        }

    }
}
