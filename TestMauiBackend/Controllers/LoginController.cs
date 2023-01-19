using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TestMauiBackend.Interfaces;
using TestMauiBackend.Models;

namespace TestMauiBackend.Controllers;

[Route("api/[controller]")]
public class LoginController : Controller
{
    private readonly IUserService _userService;

    public LoginController(IUserService userService)
    {
        _userService = userService;
    }
    
    [Authorize]
    [HttpGet("check")]
    public IActionResult Check()
    {
        return Ok();
    }
    
    [Authorize]
    [HttpGet("getUserById")]
    public async Task<IActionResult> GetUserById(long userId)
    {
        return Ok(await _userService.GetUserById(userId));
    }
    
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest model)
    {
        var user = await _userService.Authenticate(model.Username, model.Password);

        if (user == null)
            return BadRequest(new { message = "Username or password is incorrect" });

        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Username)
        };
        // create access token
        var accessToken = CreateToken("access", claims, TimeSpan.FromHours(1));
        // create refresh token
        var refreshToken = CreateToken("refresh", claims, TimeSpan.FromDays(1));

        // return tokens
        return Ok(new
        {
            User = user,
            Tokens = new
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            }
        });
    }
    
    [Authorize]
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        var refreshToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        var claims = new JwtSecurityTokenHandler().ValidateToken(refreshToken, new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "issuer",
            ValidAudience = "audience",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("super-secret-key")),
            ClockSkew = TimeSpan.Zero
        }, out SecurityToken _).Claims;
        return Ok(new
        {
            AccessToken = CreateToken("access", claims, TimeSpan.FromHours(1)),
            RefreshToken = refreshToken
        });
    }

    private string CreateToken(string tokenType, IEnumerable<Claim> claims, TimeSpan expires)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes("super-secret-key");
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = "issuer",
            Audience = "audience",
            Subject = new ClaimsIdentity(claims),
            IssuedAt = DateTime.UtcNow,
            Expires = DateTime.UtcNow.Add(expires),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}