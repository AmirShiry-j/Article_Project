using Article_Project.Dtoes.Dto.AdminArea.Role;
using Article_Project.Entities.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Article_Project.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [Route("/{Area}/{Controller}/{Action}/")]
    public class RoleController : Controller
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;
        public RoleController(RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            List<RoleListDto> Roles = _roleManager.Roles.ToList().Select(r => new RoleListDto()
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                CountUsers = _userManager.GetUsersInRoleAsync(r.Name).Result.Count
            }).ToList();

            return View(Roles);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(NewRoleDto newRole)
        {
            if (ModelState.IsValid == false)
            {
                return View(newRole);
            }

            Role role = new Role()
            {
                Name = newRole.Name,
                Description = newRole.Description
            };

            var result = _roleManager.CreateAsync(role).Result;

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                string Message = "";
                foreach (var error in result.Errors)
                    Message += error.Description;

                ModelState.AddModelError("", Message);

                return View(newRole);
            }
        }

        [HttpGet]
        [Route("{Id}")]
        public IActionResult Delete(string Id)
        {

            Role role = _roleManager.FindByIdAsync(Id).Result;

            if (role == null)
            {
                return BadRequest();
            }

            if (role.Name == "Admin" || role.Name == "Manager")
            {
                return BadRequest();
            }

            var result = _roleManager.DeleteAsync(role).Result;

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("{Id}")]
        public IActionResult UsersInRole(string Id)
        {
            Role role = _roleManager.FindByIdAsync(Id).Result;

            if (role == null)
            {
                return BadRequest();
            }

            var usersInRole = _userManager.GetUsersInRoleAsync(role.Name).Result;

            var usersInfo = usersInRole.Select(u => new UsersInRoleDto
            {
                Id = u.Id,
                Email = u.Email,
                UserName = u.UserName,
                NameShow = u.NameShow,
                Roles = string.Join(',', _userManager.GetRolesAsync(u).Result)

            }).ToList();

            ViewData["RoleName"] = role.Name;

            return View(usersInfo);
        }

        [HttpGet]
        [Route("{Name}/{UserId}")]
        public IActionResult DeleteUserRole(string Name, string UserId)
        {
            var user = _userManager.FindByIdAsync(UserId).Result;
            var role = _roleManager.FindByNameAsync(Name).Result;

            if (user == null || role == null)
            {
                return BadRequest();
            }

            if (Name == "Admin" && _userManager.GetUsersInRoleAsync("Admin").Result.Count == 1)
            {
                return BadRequest();
            }

            var result = _userManager.RemoveFromRoleAsync(user, role.Name).Result;

            if (result.Succeeded)
            {
                return RedirectToAction("UsersInRole", new { Id=role.Id });
            }

            return BadRequest();
        }
    }
}
