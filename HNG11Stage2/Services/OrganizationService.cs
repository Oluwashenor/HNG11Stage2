using HNG11Stage2.Data;
using HNG11Stage2.DTOs;
using HNG11Stage2.Models;
using Microsoft.EntityFrameworkCore;

namespace HNG11Stage2.Services
{
    public class OrganizationService(AppDBContext context) :IOrganizationService
    {
        public async Task<ResponseModel<OrganizationDTO>> CreateOrganization(CreateOrganizationDTO model, string userId)
        {
            Dictionary<string, string> errors = new();
            if (string.IsNullOrEmpty(model.Name))
            {
                errors.Add("name", "This field is required");
            }
            if (errors.Count > 0)
            {
                return ResponseModel<OrganizationDTO>.MultiError(errors = errors, statusCode: 422);
            }
            var findSimilarOrg = await context.Organizations.FirstOrDefaultAsync(x => x.Name == model.Name);
            var organization = new Organization()
            {
                Name = model.Name,
                Description = model.Description,
                CreatedBy = userId
            };
            await context.AddAsync(organization);
            await context.UserOrganizations.AddAsync(new()
            {
                OrganizationId = organization.Id,
                UserId = userId,
                Owner = true
            });
            var saved = await context.SaveChangesAsync() > 0;
            var response = new OrganizationDTO()
            {
                Description = model.Description,
                Name = model.Name,
                OrgId = organization.Id
            };
            return ResponseModel<OrganizationDTO>.Success(response, statusCode: 201, message: "Organisation created successfully");
        }

        public async Task<ResponseModel<List<OrganizationDTO>>> GetOrganization(string userId)
        {
            var userOrganizations = await context.UserOrganizations.Where(x => x.UserId == userId).Include(x=>x.Organization).ToListAsync();
            var organizations = userOrganizations.Select(x => new OrganizationDTO()
            {
                Description = x.Organization.Description,
                Name = x.Organization.Name,
                OrgId = x.Organization.Id
            }).ToList();
            return ResponseModel<List<OrganizationDTO>>.Success(organizations);    
        }

        public async Task<ResponseModel<bool>> AddToOrg(string userId, string orgId, string adderId)
        {
            var organization = await context.Organizations.FirstOrDefaultAsync(x => x.Id == orgId);
            if (organization == default) return ResponseModel<bool>.Error("Organization not found");
            if (organization.CreatedBy != adderId) return ResponseModel<bool>.Error("You are unauthorized to add users to this organization");
            var userPresentInOrg =  await context.UserOrganizations.FirstOrDefaultAsync(x => x.UserId == userId && x.OrganizationId == orgId);
            if (userPresentInOrg != default) return ResponseModel<bool>.Error("User already belong to organization");
             await context.UserOrganizations.AddAsync(new()
            {
                OrganizationId = organization.Id,
                UserId = userId,
                Owner = false
            });
            await context.SaveChangesAsync();
            return ResponseModel<bool>.Success(true, message: "User added to organisation successfully");
        }

        public async Task<ResponseModel<List<OrganizationDTO>>> GetOrganization(string userId, string orgId)
        {
            var userOrganizations = await context.UserOrganizations.Where(x => x.UserId == userId && x.OrganizationId == orgId).Include(x => x.Organization).ToListAsync();
            var organizations = userOrganizations.Select(x => new OrganizationDTO()
            {
                Description = x.Organization.Description,
                Name = x.Organization.Name,
                OrgId = x.Organization.Id
            }).ToList();
            return ResponseModel<List<OrganizationDTO>>.Success(organizations);
        }
    }

    public interface IOrganizationService
    {
        Task<ResponseModel<bool>> AddToOrg(string userId, string orgId, string adderId);
        Task<ResponseModel<OrganizationDTO>> CreateOrganization(CreateOrganizationDTO model, string userId);
        Task<ResponseModel<List<OrganizationDTO>>> GetOrganization(string userId);
        Task<ResponseModel<List<OrganizationDTO>>> GetOrganization(string userId, string orgId);
    }
}
