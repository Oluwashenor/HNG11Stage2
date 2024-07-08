using System.ComponentModel.DataAnnotations;

namespace HNG11Stage2.Models
{
    public class Organization
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public string Description { get; set; }
        public string? CreatedBy { get; set; }
    }


    public class UserOrganization
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public string OrganizationId { get; set; }
        public Organization Organization { get; set; }
        public bool Owner { get; set; }
    }

}
