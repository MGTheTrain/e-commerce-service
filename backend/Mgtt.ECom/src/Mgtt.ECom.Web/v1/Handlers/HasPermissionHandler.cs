using Microsoft.AspNetCore.Authorization;

namespace Mgtt.ECom.Web.V1.OrderManagement.Handlers
{
    public class HasPermissionHandler : AuthorizationHandler<HasPermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasPermissionRequirement requirement)
        {
            if (context.User.HasClaim(c => c.Type == "permissions" && c.Value == requirement.Permission))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}