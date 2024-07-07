// <copyright file="HasPermissionRequirement.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Mgtt.ECom.Web.Handlers
{
    using Microsoft.AspNetCore.Authorization;

    /// <summary>
    /// Defines a requirement for having a specific permission.
    /// </summary>
    public class HasPermissionRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// Gets the required permission.
        /// </summary>
        public string Permission { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="HasPermissionRequirement"/> class.
        /// </summary>
        /// <param name="permission">The required permission.</param>
        /// <exception cref="ArgumentNullException">Thrown when the provided permission is null.</exception>
        public HasPermissionRequirement(string permission)
        {
            this.Permission = permission ?? throw new ArgumentNullException(nameof(permission));
        }
    }
}
