using Microsoft.AspNetCore.Authorization;

namespace Mgtt.ECom.Web.V1.OrderManagement.Handlers
{
    public class HasPermissionRequirement : IAuthorizationRequirement
    {
        public string Permission { get; }

        public HasPermissionRequirement(string permission)
        {            
            Permission = permission ?? throw new ArgumentNullException(nameof(permission));;
        }
    }
}