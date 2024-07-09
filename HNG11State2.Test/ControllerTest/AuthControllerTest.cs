using FakeItEasy;
using FluentAssertions;
using HNG11Stage2.Controllers;
using HNG11Stage2.DTOs;
using HNG11Stage2.Models;
using HNG11Stage2.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;
namespace HNG11State2.Test.ControllerTest
{
    public class AuthControllerTest
    {

        private readonly UserController _userController;
        private readonly IUserService _userService;

        public AuthControllerTest()
        {
            _userService = A.Fake<IUserService>();
            _userController = new UserController(_userService);
        }

        [Fact]
        public async Task UserController_Register_ReturnsCreated()
        {
            //Arrange
            var input = new CreateUserDTO()
            {
                Email = "test@gmail.com",
                FirstName = "test",
                LastName = "papa",
                Password = "Sample@123",
                Phone = "09051458897"
            };
            A.CallTo(() => _userService.CreateUser(input)).Returns(
                new ResponseModel<RegistrationResponseDTO>()
                {
                    Data = new RegistrationResponseDTO()
                    {
                        AccessToken = "samplestring",
                        User = new()
                        {
                            Email = input.Email,
                            FirstName = input.FirstName,
                            Phone = input.Phone,
                            LastName = input.LastName,
                            UserId = Guid.NewGuid().ToString()
                        }
                    },
                    Status = "success",
                    StatusCode = 201
                }
                );
            //Act
            var result = await _userController.Register(input);
            var resultObject = result as ObjectResult;
            //Assert
            resultObject?.StatusCode.Should().Be((int)HttpStatusCode.Created);
        }

        [Fact]
        public async Task UserController_Login_ReturnsOk()
        {
            //Arrange
            var input = new LoginDTO()
            {
                Email = "test@gmail.com",
                Password = "Sample@123",
            };
            A.CallTo(() => _userService.Login(input)).Returns(
                new ResponseModel<RegistrationResponseDTO>()
                {
                    Data = new RegistrationResponseDTO()
                    {
                        AccessToken = "samplestring",
                        User = new()
                        {
                            Email = "Email",
                            FirstName = "FirstName",
                            Phone = "Phone",
                            LastName = "LastName",
                            UserId = Guid.NewGuid().ToString()
                        }
                    },
                    Status = "success",
                    StatusCode = 201
                }
                );
            //Act
            var result = await _userController.Login(input);
            var resultObject = result as ObjectResult;
            //Assert
            resultObject?.StatusCode.Should().Be((int)HttpStatusCode.Created);
        }

    }
}
