using System.ComponentModel.DataAnnotations;

namespace HNG11Stage2.DTOs
{

    public class RegistrationResponseDTO
    {
        public string AccessToken {  get; set; }
        public UserDTO User { get; set; }
    }

    public class UserDTO
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public required string Email { get; set; }
        public string Phone { get; set; }
    }

    public class CreateUserDTO
    {
        public string? FirstName { get; set; }
        public  string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Phone { get; set; }
    }

    public class TokenDTO
    {
        public required string Token { get; set; }
    }

    public class LoginDTO
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
