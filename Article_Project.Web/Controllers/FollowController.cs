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
    [Authorize]
    [Route("/{Controller}/{Action}/{UserId}")]
    public class FollowController : Controller
    {
        private readonly UserManager<User> _userManager;

        private readonly IUnitOfWork _unitOfWork;
        public FollowController(UserManager<User> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;

        }

        [HttpPost]
        public bool AddUser(string UserId)
        {
            var userForFollow = _userManager.FindByIdAsync(UserId).Result;

            if (userForFollow == null)
            {
                return false;
            }

            var userActiveId = _userManager.GetUserId(User);

            if (RemoveFollowUserIfHas(userActiveId, userForFollow.Id))
            {
                var newFollowRecord = new Follow
                {
                    UserId = userActiveId,
                    FollowTo = userForFollow.Id
                };

                var resultAdd = _unitOfWork.FollowRepository.AddAsync(newFollowRecord).Result;

                return resultAdd;
            }

            return false;
        }

        [HttpDelete]
        public bool RemoveUser(string UserId)
        {
            var userForUnFollow = _userManager.FindByIdAsync(UserId).Result;

            if (userForUnFollow == null)
            {
                return false;
            }

            var userActiveId = _userManager.GetUserId(User);

            if (RemoveFollowUserIfHas(userActiveId, userForUnFollow.Id))
            {

                return true;
            }

            return false;
        }

        [HttpDelete]
        public bool RemoveFollower(string UserId)
        {
            var userForRemoveFromFollower = _userManager.FindByIdAsync(UserId).Result;

            if (userForRemoveFromFollower == null)
            {
                return false;
            }

            var userActiveId = _userManager.GetUserId(User);

            var followRecord = _unitOfWork.FollowRepository.GetAllAsync(p => p.FollowTo == userActiveId &&
                                                                             p.UserId == UserId).Result.FirstOrDefault();

            if(followRecord!=null)
            {

                var resultRemoveFollower = _unitOfWork.FollowRepository.RemoveAsync(followRecord).Result;

                return resultRemoveFollower;
            }

            return false;
        }


        [NonAction]
        public bool RemoveFollowUserIfHas(string userActiveId, string userIdForFollowUser)
        {
            var followRecoreds = _unitOfWork.FollowRepository.GetAllAsync(p => p.UserId == userActiveId &&
                                                                              p.FollowTo == userIdForFollowUser).Result;
            if (followRecoreds != null)
            {
                return _unitOfWork.FollowRepository.RemoveRangeAsync(followRecoreds.ToList()).Result;
            }

            return true;
        }
    }
}
