using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using UMS.Persistence;

namespace UMS.Application.Aauthorization
{
    public class RolesAuthorizationHandler : AuthorizationHandler<RolesAuthorizationRequirement>, IAuthorizationHandler
    {
        public readonly MyDbContext _context;
        public RolesAuthorizationHandler(MyDbContext context)
        {
            _context=context;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context
            , RolesAuthorizationRequirement requirement)
        {
            if(context.User==null || !context.User.Identity.IsAuthenticated)
            {
                context.Fail();
                return Task.CompletedTask;
            }
            var validRole = false;
            if (requirement.AllowedRoles == null || requirement.AllowedRoles.Any() == false)
            {
                validRole = true;
            }
            else
            {
                var claims = context.User.Claims;
                var userid = claims.FirstOrDefault(c => c.Type == "userId").Value;
                var roles=requirement.AllowedRoles;

                var roleName= (from u in _context.Users join rol in _context.Roles on u.RoleId equals rol.Id where u.KeycloakId == userid select rol.Name).FirstOrDefault();
                if (roleName==roles.FirstOrDefault())
                {
                    validRole = true;
                }
                else
                {
                    validRole = false;
                }
            }
            if (validRole)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
            return Task.CompletedTask;
        }
    }
}
