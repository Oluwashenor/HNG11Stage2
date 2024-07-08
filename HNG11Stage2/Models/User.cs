using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace HNG11Stage2.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? Address { get; set; }
    }

    //public string UserId { get; set; }
    //public string FirstName { get; set; }
    //public string LastName { get; set; }
    //public required string Email { get; set; }
    //public required string Password { get; set; }
    //public string Phone { get; set; }
}
