// <copyright file="HasePermissionRequirement.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Mgtt.ECom.Web.Handlers
{
    using Microsoft.AspNetCore.Authorization;

    public class HasPermissionRequirement : IAuthorizationRequirement
    {
        public string Permission { get; }

        public HasPermissionRequirement(string permission)
        {
            this.Permission = permission ?? throw new ArgumentNullException(nameof(permission));
        }
    }
}