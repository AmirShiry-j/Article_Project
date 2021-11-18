using Article_Project.Dtoes.Dto.AdminArea.Comments;
using Article_Project.Entities.Entity;
using Article_Project.Services.Repositories.UnitOfWorkRepository.Interface;
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
    [Authorize(Roles = "Admin,Manager")]
    [Route("/{Area}/{Controller}/{Action}/")]
    public class CommentsController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        private readonly IUnitOfWork _unitOfWork;

        public CommentsController(UserManager<User> userManager, RoleManager<Role> roleManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _roleManager = roleManager;

            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("{UserId}/{Search?}")]
        public IActionResult ListComments(string UserId, string Search = "")
        {
            var user = _userManager.FindByIdAsync(UserId).Result;
            if (user == null)
            {
                return BadRequest();
            }

            ViewData["UserId"] = UserId;
            ViewData["UserName"] = user.UserName;

            List<Comment> comments = _unitOfWork.CommentRepository.GetAllAsync(comment => comment.UserId == user.Id).Result.Where(c => c.Text.Contains(Search)).ToList();

            List<ListCommentDto> listComments = comments.Select(c => new ListCommentDto
            {
                Id = c.CommentId,
                Text = c.Text,
                IsReply = c.ReplyComment == 0 ? false : true,
                CountReplys = _unitOfWork.CommentRepository.GetAllAsync(w => w.ReplyComment == c.CommentId, null).Result.Count(),
                postId = c.PostId

            }).ToList();

            return View(listComments);
        }


        [Route("{CommentId}/{UserId}")]
        public IActionResult Delete(long CommentId, string UserId)
        {
            List<Comment> comments = _unitOfWork.CommentRepository.GetAllAsync(c => c.CommentId == CommentId || c.ReplyComment == CommentId, null).Result.ToList();

            var resultDeletes = _unitOfWork.CommentRepository.RemoveRangeAsync(comments).Result;

            return RedirectToAction("ListComments", new { UserId = UserId });
        }

 
        [Route("{UserId}")]
        public IActionResult DeleteAllComment(string UserId)
        {
            var user = _userManager.FindByIdAsync(UserId).Result;
            if (user == null)
            {
                return BadRequest();
            }

            List<Comment> commentsWithReplays = _unitOfWork.CommentRepository.GetAllAsync(c => c.UserId == user.Id).Result.ToList();

            var results = _unitOfWork.CommentRepository.RemoveRangeAsync(commentsWithReplays).Result;

            return RedirectToAction("Details", "Users", new { UserId = user.Id });
        }
    }
}
