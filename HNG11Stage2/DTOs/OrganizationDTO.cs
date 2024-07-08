namespace HNG11Stage2.DTOs
{
    public class CreateOrganizationDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class OrganizationDTO: CreateOrganizationDTO
    {
        public string OrgId { get; set; }
    }

    public class AddUserToOrg
    {
        public string UserId { get; set; }
    }
}
