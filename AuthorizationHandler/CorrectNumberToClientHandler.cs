using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt.AuthorizationHandler
{
    public class CorrectNumberToClientHandler : AuthorizationHandler<CorrectNumberToClientRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CorrectNumberToClientHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CorrectNumberToClientRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == "client"))
            {
                File.WriteAllText("Log.txt", "pierwsze");
                return Task.CompletedTask;
            }

            var httpContext = _httpContextAccessor.HttpContext;

            int claimedNumber;
            if (!int.TryParse(context.User.FindFirst(c => c.Type == "client").Value, out claimedNumber))
            {
                File.WriteAllText("Log.txt", "drugie");
                return Task.CompletedTask;
            }
            int clientID;
            if (!int.TryParse(httpContext.Request.Query["idUser"].ToString(), out clientID))
            {
                File.WriteAllText("Log.txt", "trzecie");
                return Task.CompletedTask;
            }

            if(claimedNumber == clientID)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

    public class CorrectNumberToClientRequirement : IAuthorizationRequirement
    {
        
    }
}
