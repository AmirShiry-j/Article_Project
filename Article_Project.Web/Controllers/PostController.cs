using Article_Project.DataLayer.Context;
using Article_Project.Dtoes.Dto.Post;
using Article_Project.Entities.Entity;
using Article_Project.Services.Repositories.UnitOfWorkRepository.Interface;
using Article_Project.Web.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Article_Project.Web.Controllers
{
    [Route("/{Controller}/{Action}/")]
    public class PostController : Controller
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<User> _userManager;


        private readonly ServiceCommon _serviceCommon;
        private readonly ServicePost _servicePost;
        private readonly Pagination _pagination;

        private readonly ILogger<PostController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public PostController(UserManager<User> userManager, IAuthorizationService authorizationService, IUnitOfWork unitOfWork, ILogger<PostController> logger)
        {
            _userManager = userManager;

            _serviceCommon = new ServiceCommon();
            _servicePost = new ServicePost(_unitOfWork);
            _pagination = new Pagination(this);
            _authorizationService = authorizationService;

            _logger = logger;


            _unitOfWork = unitOfWork;
        }
        
        
        [Route("{Subject}/{Page=0}")]
        public IActionResult Subject(string Subject, int Page = 0)
        {
            Subject = _servicePost.GetSubject(Subject);

            if (Subject == null)
            {
                return RedirectToAction("Error", "Home");
            }

            IEnumerable<Post> posts = _unitOfWork.PostRepository.GetAllAsync(post => post.Subject == Subject, null).Result;
            posts = posts.OrderByDescending(p => p.TimeCreate).ToList();

            var shortDisplayPosts = posts.Select(p => new ShortDisplayPostDto
            {
                UserId = p.UserId,
                Description = _serviceCommon.GetDescription(p.Texts),
                PostId = p.PostId,
                ImageName = _serviceCommon.GetNameFirstImage(p.ImageNames),
                Subject = p.Subject,
                TimeCreate = p.TimeCreate,
                Title = p.Title
            }).ToList();

            _pagination.SetPagination(shortDisplayPosts, Page);

            return View("Subject", Subject.ToString());
        }

        [Authorize]
        [Route("{Page=0}")]
        [HttpGet]
        public IActionResult Friends(int Page = 0)
        {

            User user = _userManager.GetUserAsync(User).Result;

            if (user == null)
            {
                return RedirectToAction("Error", "Home");
            }

            IEnumerable<Follow> followingRows = _unitOfWork.FollowRepository.GetAllAsync(f => f.UserId == user.Id, null).Result;

            List<Post> AllPosts = new List<Post>();

            if (followingRows != null)
            {

                foreach (var followingRow in followingRows)
                {
                    IEnumerable<Post> Posts = _unitOfWork.PostRepository.GetAllAsync(post => post.UserId == followingRow.FollowTo, null).Result;

                    AllPosts.AddRange(Posts);

                }
            }

            AllPosts = AllPosts.OrderByDescending(post => post.TimeCreate).ToList();

            var shorDisplayPosts = AllPosts.Select(p => new ShortDisplayPostDto
            {
                UserId = p.UserId,
                Description = _serviceCommon.GetDescription(p.Texts),
                ImageName = _serviceCommon.GetNameFirstImage(p.ImageNames),
                PostId = p.PostId,
                Subject = p.Subject,
                TimeCreate = p.TimeCreate,
                Title = p.Title

            }).ToList();

            _pagination.SetPagination(shorDisplayPosts, Page);

            return View();
        }

        [HttpGet]
        [Route("{Tag}/{Page=0}")]
        public IActionResult Search(string Tag, int Page = 0)
        {
            if (Tag == null)
            {

                return RedirectToRoute(new { Controller = "Home", Action = "Error" });
            }

            Tag = Tag.Trim();

            IEnumerable<Post> posts = _unitOfWork.PostRepository.GetAllAsync(p => (p.Tags != null && p.Tags.ToLower().Contains(Tag.ToLower())) || (p.Subject == Tag) || (p.Title.ToLower().Contains(Tag.ToLower())), null).Result;
            posts = posts.OrderByDescending(post => post.TimeCreate);

            List<ShortDisplayPostDto> shortDisplayPosts = posts.Select(p => new ShortDisplayPostDto
            {
                UserId = p.UserId,
                PostId = p.PostId,
                Description = _serviceCommon.GetDescription(p.Texts),
                ImageName = _serviceCommon.GetNameFirstImage(p.ImageNames),
                Subject = p.Subject,
                TimeCreate = p.TimeCreate,
                Title = p.Title

            }).OrderByDescending(o => o.TimeCreate).ToList();


            _pagination.SetPagination(shortDisplayPosts, Page);

            return View("Search", Tag);
        }

        [HttpGet]
        [Route("{PostId}")]
        public IActionResult Show(long PostId)
        {
            Post post = _unitOfWork.PostRepository.GetAllAsync(post => post.PostId == PostId
            //Inclues
            , p => p.User
            , p => p.Likes
            , p => p.Comments
            , p => p.Bookmarks
            ).Result.SingleOrDefault();

            ViewBag.CountComments = post.Comments.Count;

            if (post == null)
            {
                return RedirectToRoute(new { Controller = "Home", Action = "Error" });
            }

            post.Views++;
            var resultUpdate = _unitOfWork.PostRepository.UpdateAsync(post).Result;

            return View(post);
        }

        [Authorize]
        [Route("{PostId}")]
        public IActionResult Delete(long PostId)
        {
            User user = _userManager.GetUserAsync(User).Result;

            if (user == null)
            {
                return RedirectToAction("Error", "Home");
            }

            Post post = _unitOfWork.PostRepository.GetByIdAsync(PostId).Result;

            if (post == null || user == null)
            {
                return RedirectToAction("Error", "Home");
            }

            if (_authorizationService.AuthorizeAsync(User, "AdminUsers").Result.Succeeded ||
                _authorizationService.AuthorizeAsync(User, "ManagerUsers").Result.Succeeded ||
                _authorizationService.AuthorizeAsync(User, post.UserId, "IsIdForUser").Result.Succeeded)
            {

                string[] ImageNames = post.ImageNames.Split("#*$", StringSplitOptions.RemoveEmptyEntries);
                foreach (var imgName in ImageNames)
                {
                    _servicePost.DeleteImagePost(imgName);
                }

                var result = _unitOfWork.PostRepository.RemoveAsync(post).Result;

                User authorPost = _userManager.FindByIdAsync(post.UserId).Result;

                return RedirectToRoute(new { Controller = "User", Action = "Profile", UserName = authorPost.UserName });

            }
            else
            {
                return new ChallengeResult();
            }
        }

        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreadPostDto creadPost, IFormFile[] formFiles)
        {
            var userActive = _userManager.FindByNameAsync(User.Identity.Name).Result;

            if (userActive == null)

            {
                return RedirectToAction("Error", "Home");

            }
            if (ModelState.IsValid && formFiles.Length > 0)
            {
                string ImgNames = "";

                foreach (var file in formFiles)
                {
                    string ImgName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    ImgNames += ImgName;
                    ImgNames += "#*$";

                    string savePath = Path.Combine(
                            Directory.GetCurrentDirectory(), "wwwroot/Images/ImgPost/", ImgName);

                    using (var stream = new FileStream(savePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }

                string[] ArryNotCorrectTexts = _servicePost.GetArryOfString(creadPost.Texts);
                string[] ArryNotCorrectOrders = _servicePost.GetArryOfString(creadPost.Orders);
                string[] ArryOrders = _servicePost.GetCorrectOrders(ArryNotCorrectOrders, ArryNotCorrectTexts);
                string[] ArryImgs = ImgNames.Split("#*$", StringSplitOptions.RemoveEmptyEntries);
                string[] ArryTexts = creadPost.Texts.Split("#*$", StringSplitOptions.RemoveEmptyEntries);


                Post post = new Post();
                post.Title = creadPost.Title;
                post.Tags = creadPost.Tags;
                post.Texts = string.Join("#*$", ArryTexts);
                post.ImageNames = string.Join("#*$", ArryImgs);
                post.Subject = _servicePost.GetSubject(creadPost.Subject);
                post.Orders = string.Join("#*$", ArryOrders);
                post.TimeCreate = DateTime.Now;
                post.UserId = userActive.Id;


                if (_unitOfWork.PostRepository.AddAsync(post).Result)
                {
                    return RedirectToRoute(new { controller = "Post", action = "Show", PostId = post.PostId });

                }
                else
                {

                    string[] ImgNamesForDelete = ImgNames.Split("#*$");

                    foreach (var NameDelete in ImgNamesForDelete)
                    {
                        _servicePost.DeleteImagePost(NameDelete);
                    }

                    return RedirectToRoute(new { controller = "Home", action = "Error" });
                }
            }
            else
            {
                return RedirectToRoute(new { controller = "Home", action = "Error" });
            }
        }       
    }
}
