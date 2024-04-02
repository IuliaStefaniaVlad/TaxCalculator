using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using TaxCalculator.Models.Shared;
using TaxCalculator.Models.Shared.Options;

namespace TaxCalculator.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IOptions<JWTOptions> _options;
    public AuthenticationController(UserManager<IdentityUser> userManager, IOptions<JWTOptions> options)
    {
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }

    [HttpPost]
    [Route("registerUser")]
    public async Task<IActionResult> Register([FromBody] UserInputModel model)
    {

        var userExists = await _userManager.FindByNameAsync(model.Name);
        if (userExists != null)
        {
            var identityErrors = new IdentityError[]
            {
                new IdentityError()
                {
                    Code = "UserExists",
                    Description = "User already registered."
                }
            };
            return BadRequest(identityErrors);
        }

        IdentityUser user = new IdentityUser()
        {
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = model.Name,
            Id = Guid.NewGuid().ToString(),
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result);
        }

        return Ok(result);
    
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] UserInputModel model)
    {
      
        var user = await _userManager.FindByNameAsync(model.Name);
        if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.Secret));

            var token = new JwtSecurityToken(
                issuer: _options.Value.ValidIssuer,
                audience: _options.Value.ValidAudience,
                expires: DateTime.Now.AddHours(3),
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return Ok(new LoginResultModel()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
            });
        }

        return Unauthorized();
    
    }
}