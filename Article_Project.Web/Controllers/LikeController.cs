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
using System.Linq;
using System.Threading.Tasks;

namespace Article_Project.Web.Controllers
{
    [Route("/{Controller}/{Action}/{PostId}")]
    [Authorize]
    public class LikeController : Controller
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<LikeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        private readonly ServiceCommon _serviceCommon;

        public LikeController(UserManager<User> userManager
            , IAuthorizationService authorizationService
            , IUnitOfWork unitOfWork
            , ILogger<LikeController> logger)
        {
            _userManager = userManager;
            _authorizationService = authorizationService;
            _logger = logger;
            _unitOfWork = unitOfWork;

            _serviceCommon = new ServiceCommon();
        }

        [Route("~/Like/Yours")]
        public IActionResult GetYours()
        {
            User userActive = _userManager.GetUserAsync(User).Result;

            if (userActive == null)
            {
                return RedirectToAction("Error", "Home");
            }


            IEnumerable<Like> likes = _unitOfWork.LikeRepository.GetAllAsync(like => like.UserId == userActive.Id, null).Result;
            likes = likes.OrderByDescending(like => like.LikeId).ToList();

            List<Post> posts = new List<Post>();

            foreach (var like in likes)
            {
                Post post = _unitOfWork.PostRepository.GetByIdAsync(like.PostId).Result;
                if (post != null)
                {
                    posts.Add(post);
                }

            }

            posts = posts.OrderByDescending(post => post.TimeCreate).ToList();

            var ShortDisplayPosts = posts.Select(p => new ShortDisplayPostDto
            {
                UserId = p.UserId,
                Description = _serviceCommon.GetDescription(p.Texts),
                ImageName = _serviceCommon.GetNameFirstImage(p.ImageNames),
                PostId = p.PostId,
                Subject = p.Subject,
                TimeCreate = p.TimeCreate,
                Title = p.Title

            }).ToList();

            return View(ShortDisplayPosts);
        }

        [HttpPost]
        public bool Add(long PostId)
        {
            var post = _unitOfWork.PostRepository.GetByIdAsync(PostId).Result;

            if (post == null)
            {
                return false;
            }

            var userId = _userManager.GetUserId(User);

            if (RemoveLikesFromThisUserForPost(userId, PostId))
            {
                Like newLike = new Like
                {
                    PostId = PostId,
                    UserId = userId
                };

                var result = _unitOfWork.LikeRepository.AddAsync(newLike).Result;

                return result;
            }
            else
            {
                return false;
            }
        }

        [HttpDelete]
        public bool Remove(long PostId)
        {
            var post = _unitOfWork.PostRepository.GetByIdAsync(PostId).Result;

            if (post == null)
            {
                return false;
            }

            var userId = _userManager.GetUserId(User);

            if (RemoveLikesFromThisUserForPost(userId, PostId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [NonAction]
        public bool RemoveLikesFromThisUserForPost(string UserId, long PostId)
        {
            var likes = _unitOfWork.LikeRepository.GetAllAsync(p => p.UserId == UserId &&
                                                                 p.PostId == PostId).Result;

            if (likes != null)
            {
                return _unitOfWork.LikeRepository.RemoveRangeAsync(likes.ToList()).Result;
            }

            return true;
        }
    }
}
