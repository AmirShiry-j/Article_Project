using Microsoft.AspNetCore.Mvc;
using Article_Project.Entities.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Article_Project.Dtoes.Dto.AdminArea.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Article_Project.Services.Repositories.UnitOfWorkRepository.Interface;

namespace Article_Project.Web.Areas.Admin.Controllers
{
    [Route("/{Area}/{Controller}/{Action}/")]
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        private readonly ILogger<UsersController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public UsersController(UserManager<User> userManager, RoleManager<Role> roleManager, IUnitOfWork unitOfWork, ILogger<UsersController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;

            _logger = logger;

        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpGet]
        [Route("{Search?}")]
        public IActionResult Index(string Search = "")
        {

            List<User> users = _userManager.Users.Where(w => w.UserName.ToLower().Contains(Search.ToLower())).ToList();

            List<UserListDto> usersInfo = users.Select(u => new UserListDto
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                NameShow = u.NameShow,
                Roles = string.Join(',', _userManager.GetRolesAsync(u).Result)
            }).ToList();

            return View(usersInfo);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult AddUserRole(string UserId)
        {
            var user = _userManager.FindByIdAsync(UserId).Result;

            if (user == null)
            {
                return BadRequest();
            }

            var ListItems = _roleManager.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id
            }).ToList();

            var userForRole = new AddRoleUserDto
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Roles = ListItems
            };

            return View(userForRole);
        }
        
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult AddUserRole(AddRoleUserDto addRoleUser)
        {
            var user = _userManager.FindByIdAsync(addRoleUser.Id).Result;
            var role = _roleManager.FindByIdAsync(addRoleUser.Role).Result;

            if (user == null || role == null)
            {
                return View(addRoleUser);
            }

            var result = _userManager.AddToRoleAsync(user, role.Name).Result;

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            string Message = "";
            foreach (var mes in result.Errors)
            {
                Message += (mes.Description + Environment.NewLine);
            }

            ModelState.AddModelError("", Message);

            var ListItems = _roleManager.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id
            }).ToList();

            addRoleUser.Roles = ListItems;

            return View(addRoleUser);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("{UserName}")]
        public IActionResult RolesList(string UserName)
        {
            var user = _userManager.FindByNameAsync(UserName).Result;

            if (user == null)
            {
                return BadRequest();
            }

            var roles = _userManager.GetRolesAsync(user).Result;

            ViewBag.UserName = user.UserName;
            ViewBag.UserId = user.Id;

            return View(roles);
        }

        [Authorize(Roles = "Admin")]
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
                return RedirectToAction("RolesList", new { UserName = user.UserName });
            }

            return BadRequest();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult AddNewUser()
        {
            var ListItems = _roleManager.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id
            }).ToList();

            ListItems.Add(new SelectListItem { Text = "بدون نقش", Value = "بدون نقش", Selected = true });

            var addNew = new AddNewUserDto
            {
                Roles = ListItems
            };

            return View(addNew);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult AddNewUser(AddNewUserDto newUser)
        {
            var ListItems = _roleManager.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id
            }).ToList();

            newUser.Roles = ListItems;//برای اوکی کردن مقادیر هنگام برگشت

            if (ModelState.IsValid == false)
            {
                return View(newUser);
            }

            var user = new User
            {
                UserName = newUser.UserName,
                NameShow = newUser.NameShow,
                Email = newUser.Email
            };

            var result = _userManager.CreateAsync(user, newUser.Password).Result;

            if (result.Succeeded)
            {
                if (newUser.Role == "بدون نقش")
                {
                    return RedirectToAction("Index");
                }

                var role = _roleManager.FindByIdAsync(newUser.Role).Result;
                if (role == null)
                {
                    return BadRequest();
                }

                var result2 = _userManager.AddToRoleAsync(user, role.Name).Result;

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    string MesRole = "";
                    foreach (var Mes in result2.Errors)
                    {
                        MesRole += Mes;
                    }

                    ModelState.AddModelError("", MesRole);
                    return View(newUser);
                }
            }
            else
            {
                string MesUser = "";
                foreach (var Mes in result.Errors)
                {
                    MesUser += Mes.Description;
                }

                ModelState.AddModelError("", MesUser);
                return View(newUser);
            }
        }

        [Authorize(Roles = "Admin,Manager")]
        [Route("{UserId}")]
        [HttpGet]
        public IActionResult Details(string UserId)
        {
            var user = _userManager.FindByIdAsync(UserId).Result;

            if (user == null)
            {
                return BadRequest();
            }

            UserDetailDto userDetail = new UserDetailDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                VerifyEmail = user.EmailConfirmed,
                TwoFactor = user.TwoFactorEnabled,
                Roles = string.Join(',', _userManager.GetRolesAsync(user).Result),
                IsLocked = _userManager.GetLockoutEnabledAsync(user).Result,
                CountComment = _unitOfWork.CommentRepository.GetAllAsync(comment => comment.UserId == user.Id, null).Result.Count(),
                CountPosts = _unitOfWork.PostRepository.GetAllAsync(post => post.UserId == user.Id).Result.Count(),
                ImageProName = user.ImageProfileName
            };

            return View(userDetail);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("{UserId}")]
        public IActionResult ChangeConfirmEmail(string UserId)
        {
            var user = _userManager.FindByIdAsync(UserId).Result;

            if (user == null)
                return BadRequest();

            user.EmailConfirmed = !user.EmailConfirmed;
            var result = _userManager.UpdateAsync(user).Result;

            return RedirectToAction("Details", new { UserId });
        }
    }
}
