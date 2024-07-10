using HNG11Stage2.Data;
using HNG11Stage2.DTOs;
using HNG11Stage2.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HNG11Stage2.Services
{
    public class UserService(UserManager<User> userManager, AppDBContext context, SignInManager<User> signInManager, IOrganizationService organizationService) : IUserService
    {
        public async Task<ResponseModel<RegistrationResponseDTO>> CreateUser(CreateUserDTO model)
        {
            Dictionary<string, string> errors = new();
            if (string.IsNullOrEmpty(model.FirstName))
            {
                errors.Add("firstName", "This field is required");
            }
            if (string.IsNullOrEmpty(model.LastName))
            {
                errors.Add("lastName", "This field is required");
            }
            if (string.IsNullOrEmpty(model.Password))
            {
                errors.Add("password", "This field is required");
            }
            if (string.IsNullOrEmpty(model.Email))
            {
                errors.Add("email", "This field is required");
            } if (string.IsNullOrEmpty(model.Phone))
            {
                errors.Add("phone", "This field is required");
            }
            if (errors.Count > 0)
            {
                return ResponseModel<RegistrationResponseDTO>.MultiError(errors = errors, statusCode: 422);
            }
            
            var newUser = new User
            {
                UserName = model.Email.ToLower(),
                Email = model.Email.Trim().ToLower(),
                Name = model.FirstName + " " + model.LastName,
                PhoneNumber = model.Phone
            };
            var emailExist = await userManager.FindByEmailAsync(newUser.Email);
            if (emailExist != null) return ResponseModel<RegistrationResponseDTO>.Error("Email Address Already Exist. Please Proceed to Login");
            var createUser = await userManager.CreateAsync(newUser, model.Password);
            if (!createUser.Succeeded) return ResponseModel<RegistrationResponseDTO>.Error("Registration unsuccessful");
            var user = await userManager.FindByEmailAsync(model.Email.Trim().ToLower());
            var createdOrganization = await organizationService.CreateOrganization(new CreateOrganizationDTO()
            {
                Name = model.FirstName + "'s Organisation",
                Description = ""
            }, user.Id);
            if (createdOrganization.StatusCode != 201) return ResponseModel<RegistrationResponseDTO>.Error("Unable to create your organisation");
            var token = await GenerateToken(user);
            var response = MapData(user, token);
            return ResponseModel<RegistrationResponseDTO>.Success(response, "Registration successful", statusCode: 201);
        }

        private RegistrationResponseDTO MapData(User user, string token)
        {
            var response = new RegistrationResponseDTO
            {
                User = new UserDTO()
                {
                    Email = user.Email,
                    FirstName = user.Name.Split(" ")[0],
                    LastName = user.Name.Split(" ")[1],
                    UserId = user.Id,
                    Phone = user.PhoneNumber
                },
                AccessToken = token
            };
            return response;
        }

        public async Task<ResponseModel<RegistrationResponseDTO>> Login(LoginDTO model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null) return ResponseModel<RegistrationResponseDTO>.Error("Account not found");
            var signIn = await signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!signIn.Succeeded) return ResponseModel<RegistrationResponseDTO>.Error("Authentication failed", statusCode: 401);
            var token = await GenerateToken(user);
            var response = MapData(user, token);
            return ResponseModel<RegistrationResponseDTO>.Success(response, "Login successful");
        }

       
        public async Task<ResponseModel<UserDTO>> GetUser(string userId, string requestUser)
        {
            var requester = await userManager.FindByIdAsync(requestUser);
            if (requester == null) return ResponseModel<UserDTO>.Error("Unauthorized user");
            if(userId == requestUser)
            {
                var data =new UserDTO()
                {
                    Email = requester.Email,
                    FirstName = requester.Name.Split(" ")[0],
                    LastName = requester.Name.Split(" ")[1],
                    UserId = requester.Id,
                    Phone = requester.PhoneNumber
                };
                ResponseModel<UserDTO>.Success(data, "successful operation");
            }
            var user = await userManager.FindByIdAsync(userId);
            if (user == null) return ResponseModel<UserDTO>.Error("Account not found");
            var requesterOrganizations = await context.UserOrganizations.Where(x => x.UserId == requestUser).Select(x=>x.OrganizationId).ToListAsync();
            var userInRequesterOrg = await context.UserOrganizations.FirstOrDefaultAsync(x => requesterOrganizations.Contains(x.OrganizationId) && x.UserId == user.Id);
            if(userInRequesterOrg != null)
            {
                var response = new UserDTO()
                {
                    Email = user.Email,
                    FirstName = user.Name.Split(" ")[0],
                    LastName = user.Name.Split(" ")[1],
                    UserId = user.Id,
                    Phone = user.PhoneNumber
                };
                return ResponseModel<UserDTO>.Success(response, "successful operation");
            }
           

          return ResponseModel<UserDTO>.Error("Account not found");
        }

        private async Task<string> GenerateToken(User user)
        {
            var key = "e927d252ae2a43598155936ddc73ae831f2a3b1db59d46668757fd489d2c822b";
            var securityStamp = await userManager.GetSecurityStampAsync(user);
            var claims = new Claim[]
            {
                new("email", user.Email ?? string.Empty),
                new("userId", user.Id),
                new("name", user.Name),
                new("phone", user.PhoneNumber ?? ""),
                new("address", user.Address ?? ""),
                new("securityClaims", securityStamp)
            };
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var data = new JwtSecurityToken(
                issuer: "Sample",
                audience: "Sample",
                signingCredentials: signingCredentials,
                claims: claims,
                expires: DateTime.Now.AddDays(1)
                );
            return new JwtSecurityTokenHandler().WriteToken(data);
        }


    }

    public interface IUserService
    {
        Task<ResponseModel<RegistrationResponseDTO>> CreateUser(CreateUserDTO model);
        Task<ResponseModel<RegistrationResponseDTO>> Login(LoginDTO model);
        Task<ResponseModel<UserDTO>> GetUser(string user, string requestUser);
    }
}
