// <copyright file="HasPermissionHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Mgtt.ECom.Web.Handlers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;

    /// <summary>
    /// Handles authorization requirements for permissions.
    /// </summary>
    public class HasPermissionHandler : AuthorizationHandler<HasPermissionRequirement>
    {
        /// <summary>
        /// Makes a decision if authorization is granted based on user claims.
        /// </summary>
        /// <param name="context">The authorization context.</param>
        /// <param name="requirement">The specific requirement to evaluate.</param>
        /// <returns>A completed task once the requirement is handled.</returns>
        /// <inheritdoc/>
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
