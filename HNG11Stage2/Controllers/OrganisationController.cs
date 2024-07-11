using HNG11Stage2.DTOs;
using HNG11Stage2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HNG11Stage2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrganisationController(IOrganizationService organizationService) : ControllerBase
    {
        [HttpPost("/api/organisations")]
        public async Task<IActionResult> CreateOrganization(CreateOrganizationDTO? model)
        {
            string user = HttpContext.User.Claims?.FirstOrDefault(x => x.Type == "userId")?.Value;
            if (user == null) return Unauthorized();
            var created = await organizationService.CreateOrganization(model, user);
            return StatusCode(created.StatusCode,created);
        }

        [HttpGet("/api/organisations")]
        public async Task<IActionResult> GetOrganization()
        {
            string user = HttpContext.User.Claims?.FirstOrDefault(x => x.Type == "userId")?.Value;
            if (user == null) return Unauthorized();
            var created = await organizationService.GetOrganization(user);
            return StatusCode(created.StatusCode, created);
        }

        [HttpGet("/api/organisations/{orgId}")]
        public async Task<IActionResult> GetOrganization(string orgId)
        {
            string user = HttpContext.User.Claims?.FirstOrDefault(x => x.Type == "userId")?.Value;
            if (user == null) return Unauthorized();
            var created = await organizationService.GetOrganization(user, orgId);
            return StatusCode(created.StatusCode, created);
        }

        [HttpPost("/api/organisations/{orgId}/users")]
        public async Task<IActionResult> AddToOrg(string orgId,[FromBody] AddUserToOrg model)
        {
            string user = HttpContext.User.Claims?.FirstOrDefault(x => x.Type == "userId")?.Value;
            if (user == null) return Unauthorized();
            var created = await organizationService.AddToOrg(model.UserId, orgId, user);
            return StatusCode(created.StatusCode, created);
        }
    }
}
