namespace Mgtt.ECom.Web.v1.UserManagement.DTOs
{
    public class UserResponseDTO
    {
        public Guid UserID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}