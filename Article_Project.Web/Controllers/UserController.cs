using Article_Project.Dtoes.Dto.Post;
using Article_Project.Dtoes.Dto.User;
using Article_Project.Entities.Entity;
using Article_Project.Services.Repositories.UnitOfWorkRepository.Interface;
using Article_Project.Web.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Article_Project.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;

        private readonly IUnitOfWork _unitOfWork;
        private readonly ServiceCommon _serviceCommon;
        public UserController(UserManager<User> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;

            _serviceCommon = new ServiceCommon();


        }

        [Route("~/Profile/{UserName}")]
        [HttpGet]
        public IActionResult Profile(string UserName)
        {

            var user = _unitOfWork.UserRepository.GetAllAsync(p => p.UserName == UserName
            , p => p.Posts
            , p => p.Followings
            , p => p.Followers
            ).Result.FirstOrDefault();

            if (user == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var posts = user.Posts.Select(p => new ShortDisplayPostDto
            {
                PostId = p.PostId,
                ImageName = _serviceCommon.GetNameFirstImage(p.ImageNames),
                Title = p.Title,
                Subject = p.Subject,
                Description = _serviceCommon.GetDescription(p.Texts),
                UserId = p.UserId,
                TimeCreate = p.TimeCreate
            }).OrderByDescending(o => o.TimeCreate).ToList();

            var userProfile = new ProfileDto
            {
                Id = user.Id,
                Biography = user.Biography,
                UserName = user.UserName,
                NameShow = user.NameShow,
                ImageProfileName = user.ImageProfileName,
                CountPosts = user.Posts.Count,
                CountFollowers = user.Followers.Count,
                CountFollowing = user.Followings.Count
            };

            ViewData["Followers"] = user.Followers;

            Tuple<ProfileDto, IEnumerable<ShortDisplayPostDto>> tuple = new Tuple<ProfileDto, IEnumerable<ShortDisplayPostDto>>(userProfile, posts);

            return View(tuple);
        }

        [HttpGet]
        [Route("~/UserFollowers/{UserName}")]
        public IActionResult Followers(string UserName)
        {

            User user = _unitOfWork.UserRepository.GetAllAsync(p => p.UserName == UserName, p => p.Followers).Result.FirstOrDefault();

            if (user == null)
            {
                return RedirectToAction("Error", "Home");
            }

            IEnumerable<Follow> Followers = user.Followers;

            MainInfoUserDto mainInfoUser = new MainInfoUserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                ImageProfileName = user.ImageProfileName,
                NameShow = user.NameShow
            };

            var tuple = new Tuple<MainInfoUserDto, List<Follow>>(mainInfoUser, Followers.ToList());

            return View(tuple);
        }

        [HttpGet]
        [Route("~/UserFollowings/{UserName}")]
        public IActionResult Followings(string UserName)
        {
            User user = _unitOfWork.UserRepository.GetAllAsync(p => p.UserName == UserName, p => p.Followings).Result.FirstOrDefault();

            if (user == null)
            {
                return RedirectToAction("Error", "Home");
            }

            IEnumerable<Follow> Following = user.Followings;

            MainInfoUserDto mainInfoUser = new MainInfoUserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                ImageProfileName = user.ImageProfileName,
                NameShow = user.NameShow
            };

            var tuple = new Tuple<MainInfoUserDto, List<Follow>>(mainInfoUser, Following.ToList());

            return View(tuple);
        }

    }
}
