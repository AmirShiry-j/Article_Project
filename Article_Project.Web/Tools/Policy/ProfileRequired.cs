using Article_Project.Dtoes.Dto;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Article_Project.Web.Tools.Policy
{
    public class ProfileRequired : IAuthorizationRequirement
    {
    }

    public class IPrfileForUserAuthorizationHandler : AuthorizationHandler<ProfileRequired, string>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ProfileRequired requirement, string UserId)
        {
            if (context.User.Identity.IsAuthenticated)
            {
                if (requirement != null && !string.IsNullOrEmpty(UserId))
                {
                    if (context.User.FindFirst(ClaimTypes.NameIdentifier).Value == UserId)
                    {
                        context.Succeed(requirement);
                    }
                }
            }
            
            return Task.CompletedTask;
        }
    }
}
