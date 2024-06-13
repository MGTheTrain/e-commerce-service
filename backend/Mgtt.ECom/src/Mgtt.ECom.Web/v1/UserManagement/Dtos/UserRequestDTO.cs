using System.ComponentModel.DataAnnotations;

namespace Mgtt.ECom.Web.v1.UserManagement.DTOs
{
    public class UserRequestDTO
    {
        [Required]
        [StringLength(100)]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string Role { get; set; }
    }
}
