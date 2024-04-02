using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;

using TaxCalculator.Controllers;
using TaxCalculator.Models.Shared;
using TaxCalculator.Models.Shared.Options;
using Xunit;
using IdentityUser = Microsoft.AspNetCore.Identity.IdentityUser;

namespace TaxCalculator.Api.Tests;

public class AuthenticationControllerTests
{
    private readonly Mock<UserManager<IdentityUser>> _mgr = MockManager.MockUserManager<IdentityUser>();
    private readonly IOptions<JWTOptions> _options = Options.Create(new JWTOptions() { Secret = "RZgp2C3zLnK8yPp4s9QFVWxEqHJ6mAdBYiTh1UZuNX0o", ValidAudience = "ValidAudienceURL", ValidIssuer = "ValidIssuerURL" });

    [Fact]
    public async void Register_OK()
    {
        //arrange
        _mgr.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(It.IsAny<IdentityUser>());
        _mgr.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

        var controller = new AuthenticationController(_mgr.Object, _options);
        var model = new UserInputModel() { Name = "TestUser", Password = "TestUser1!" };
        //act
        var response = await controller.Register(model);

        //assert
        Assert.NotNull(response);
        var okResponse = Assert.IsType<OkObjectResult>(response);
        Assert.Equal("Succeeded", okResponse?.Value?.ToString());
    }

    [Fact]
    public async void Register_IncorrectPassword()
    {
        //arrange

        var identityErrors = new IdentityError[]
        {
            new IdentityError()
            {
                Code = "PasswordTooShort",
                Description = "Passwords must be at least 6 characters."
            },
            new IdentityError()
            {
                Code = "PasswordRequiresNonAlphanumeric",
                Description =  "Passwords must have at least one non alphanumeric character."
            }
        };
        _mgr.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(It.IsAny<IdentityUser>());
        _mgr.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>())).Returns(Task.FromResult(IdentityResult.Failed(identityErrors)));

        var controller = new AuthenticationController(_mgr.Object, _options);
        var model = new UserInputModel() { Name = "TestUser", Password = "teS5!ser" };
        //act
        var response = await controller.Register(model);

        //assert
        Assert.NotNull(response);
        var brResponse = Assert.IsType<BadRequestObjectResult>(response);
        var errorArray = Assert.IsType<IdentityResult>(brResponse.Value);
        Assert.Contains("Failed", brResponse?.Value?.ToString());
        Assert.Equal(2, errorArray.Errors.Count());
    }

    [Fact]
    public async void Register_TenantAlreadyExists()
    {
        //arrange
        var identityErrors = new IdentityError[]
            {
                new IdentityError()
                {
                    Code = "UserExists",
                    Description = "User already registered."
                }
            };
        var model = new UserInputModel() { Name = "TestUser", Password = "TestUser1!" };
        _mgr.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new IdentityUser() { UserName = model.Name });
        var controller = new AuthenticationController(_mgr.Object, _options);

        //act
        var response = await controller.Register(model);

        //assert
        Assert.NotNull(response);
        var brResponse = Assert.IsType<BadRequestObjectResult>(response);
        var errorArray = Assert.IsType<IdentityError[]>(brResponse.Value);
        Assert.Single(errorArray);
        Assert.Contains(identityErrors[0].Code, errorArray[0].Code);
    }

    [Fact]
    public async void Login_OK()
    {
        //arrange
        var model = new UserInputModel() { Name = "TestUser", Password = "TestUser1!" };
        _mgr.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new IdentityUser() { UserName = model.Name });
        _mgr.Setup(x => x.CheckPasswordAsync(It.IsAny<IdentityUser>(), It.IsAny<string>())).ReturnsAsync(true);
        
        var controller = new AuthenticationController(_mgr.Object, _options);

        //act
        var response = await controller.Login(model);

        //assert

        //NOTEL: Updated tests to use LoginResultModel
        Assert.NotNull(response);
        var okResponse = Assert.IsType<OkObjectResult>(response);
        Assert.NotNull(okResponse.Value);
        var result = Assert.IsType<LoginResultModel>(okResponse.Value);
        Assert.NotEmpty(result.Token);
        Assert.NotNull(result.Token);
    }

    [Fact]
    public async void Login_IncorrectPassword()
    {
        //arrange
        var model = new UserInputModel() { Name = "TestUser", Password = "TestUser1!" };
        _mgr.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new IdentityUser() { UserName = model.Name });
        _mgr.Setup(x => x.CheckPasswordAsync(It.IsAny<IdentityUser>(), It.IsAny<string>())).ReturnsAsync(false);
        
        var controller = new AuthenticationController(_mgr.Object, _options);

        //act
        var response = await controller.Login(model);

        //assert
        Assert.NotNull(response);
        var okResponse = Assert.IsType<UnauthorizedResult>(response);
        Assert.Equal(401, okResponse.StatusCode);

    }

    [Fact]
    public async void Login_InvalidUsername()
    {
        //arrange
        var model = new UserInputModel() { Name = "TestUser", Password = "TestUser1!" };
        _mgr.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(null as IdentityUser);

        var controller = new AuthenticationController(_mgr.Object, _options);

        //act
        var response = await controller.Login(model);

        //assert
        Assert.NotNull(response);
        var okResponse = Assert.IsType<UnauthorizedResult>(response);
        Assert.Equal(401, okResponse.StatusCode);

    }
}
