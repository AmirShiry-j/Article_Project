using Article_Project.Dtoes.Dto.Comment;
using Article_Project.Entities.Entity;
using Article_Project.Services.Repositories.UnitOfWorkRepository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Article_Project.Web.Controllers
{
    [Route("/{Controller}/{Action}/")]
    public class CommentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        public CommentController(IUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        [HttpPost]
        [Authorize]
        public bool Add(CreateCommentDto model)
        {

            if (ModelState.IsValid == false)
            {
                return false;
            }

            Comment newComment = new Comment
            {
                PostId = model.PostId,
                Text = model.TextComment,
                Time = DateTime.Now,
                UserId = _userManager.GetUserId(User),
                ReplyComment = model.ReplayComment != 0 ? model.ReplayComment : null
            };

            var resultAdd = _unitOfWork.CommentRepository.AddAsync(newComment).Result;

            return resultAdd;
        }
    }
}
