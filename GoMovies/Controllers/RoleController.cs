using GoMovies.Data;
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
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly GoMovieContext context;
        private readonly RoleManager<ApplicationRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;


        public RoleController(RoleManager<ApplicationRole> _roleManager, UserManager<ApplicationUser> _userManager, GoMovieContext _context)
        {
            roleManager = _roleManager;
            userManager = _userManager;
            context = _context;
        }

        public IActionResult ListRoles()
        {

            return View(roleManager.Roles.ToList());
        }

        [HttpGet]
        public IActionResult CreateRole()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleEditModel m)
        {
            if (ModelState.IsValid)
            {
                ApplicationRole role = new ApplicationRole()
                {
                    Name = m.RoleName
                };

                var result = await roleManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                }

            }
            return View(m);

        }

        public IActionResult Edit()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Edit(RoleEditModel model)
        {
            var user = await userManager.FindByNameAsync(model.UserName);
            if (user != null)
            {
                var result = await userManager.AddToRoleAsync(user, model.RoleName);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                else
                {
                    return RedirectToAction("Edit", "ListRoles");
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult UpdateRole(int id)
        {
            var value = roleManager.Roles.FirstOrDefault(x => x.Id == id);
            RoleUpdateViewModel model = new RoleUpdateViewModel
            {
                Id = value.Id,
                Name = value.Name
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRole(RoleUpdateViewModel model)
        {
            var value = roleManager.Roles.Where(x => x.Id == model.Id).FirstOrDefault();
            value.Name = model.Name;

            var result = await roleManager.UpdateAsync(value);

            if (result.Succeeded)
            {
                return RedirectToAction("ListRoles", "Role");

            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }



        public async Task<IActionResult> DeleteRole(int id)
        {
            var value = roleManager.Roles.FirstOrDefault(x => x.Id == id);


            var result = await roleManager.DeleteAsync(value);

            if (result.Succeeded)
            {
                return RedirectToAction("ListRoles", "Role");

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




        public async Task<IActionResult> AssignRole(int id)
        {
            var user = userManager.Users.FirstOrDefault(x => x.Id == id);
            var roles = roleManager.Roles.ToList();

            TempData["UserId"] = user.Id;
            TempData["UserName"] = user.UserName;


            var userRoles = await userManager.GetRolesAsync(user);

            List<RoleAssignViewModel> model = new List<RoleAssignViewModel>();
            foreach (var role in roles)
            {
                RoleAssignViewModel m = new RoleAssignViewModel
                {
                    RoleID = role.Id,
                    Name = role.Name,
                    IsRoleAlreadyAssignedToUser = userRoles.Contains(role.Name)
                };
                model.Add(m);
            }

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> AssignRole(List<RoleAssignViewModel> RolesModel)
        {

            var userid = (int)TempData["UserId"];
            var user = userManager.Users.FirstOrDefault(x => x.Id == userid);

            foreach (var role in RolesModel)
            {
                if (role.IsRoleAlreadyAssignedToUser)
                {
                    await userManager.AddToRoleAsync(user, role.Name);
                }
                else
                {
                    await userManager.RemoveFromRoleAsync(user, role.Name);
                }
            }
            return RedirectToAction("ListUsers", "User");
        }
    }
}
