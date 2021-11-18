using Article_Project.Dtoes.Dto.Post;
using Article_Project.Entities.Entity;
using Article_Project.Services.Repositories.UnitOfWorkRepository.Interface;
using Article_Project.Web.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Article_Project.Web.Controllers
{
    public class HomeController : Controller
    {

        Pagination pagination;
        ServiceCommon _serviceCommon;

        private readonly IUnitOfWork _unitOfWork;
        public HomeController(IUnitOfWork unitOfWork)
        {
            pagination = new Pagination(this);
            _serviceCommon = new ServiceCommon();

            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(int Page = 0)
        {
            List<ShortDisplayPostDto> posts = _unitOfWork.PostRepository.GetAllAsync(null,null).Result.Select(p => new ShortDisplayPostDto
            {
                PostId = p.PostId,
                ImageName = _serviceCommon.GetNameFirstImage(p.ImageNames),
                Title = p.Title,
                Subject = p.Subject,
                Description = _serviceCommon.GetDescription(p.Texts),
                UserId = p.UserId,
                TimeCreate = p.TimeCreate

            }).OrderByDescending(p=>p.TimeCreate).ToList();

            pagination.SetPagination(posts, Page);

            return View();
        }

        [HttpGet]
        [Route("Error")]
        public IActionResult Error()
        {
            return View();
        }

    }
}
