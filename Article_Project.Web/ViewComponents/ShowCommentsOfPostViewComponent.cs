using Article_Project.Entities.Entity;
using Article_Project.Services.Repositories.UnitOfWorkRepository.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Article_Project.Web.ViewComponents
{
    public class ShowCommentsOfPostComponent : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        public ShowCommentsOfPostComponent(IUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync(long PostId)
        {
            var user = await _userManager.GetUserAsync(UserClaimsPrincipal);

            var post = await _unitOfWork.PostRepository.GetByIdAsync(PostId);

            var commentsOfPost = await _unitOfWork.CommentRepository.GetAllAsync(p => p.PostId == PostId, p => p.Post);

            ViewBag.PostId = PostId;
            ViewBag.AuthorId = post.UserId;
            

            commentsOfPost = commentsOfPost.OrderByDescending(p => p.Time);

            var tuple = new Tuple<User, IEnumerable<Comment>>(user, commentsOfPost);

            return View("ShowCommentsOfPost", tuple);
        }
    }
}
