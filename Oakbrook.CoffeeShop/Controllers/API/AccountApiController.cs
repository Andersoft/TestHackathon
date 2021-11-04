using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Oakbrook.CoffeeShop.Controllers
{

  [ApiController]
  [Route("api/accounts")]
  public class AccountApiController : ControllerBase
  {
    private readonly IUserService _userService;
    private readonly AppSettings _appSettings;

    public AccountApiController(IUserService userService, IOptions<AppSettings> appSettings)
    {
      _userService = userService;
      _appSettings = appSettings.Value;
    }

    [HttpPost("authenticate")]
    public IActionResult Authenticate(AuthenticateRequest model)
    {
      var claims = _userService.Authenticate(model);
      if (claims == null)
        return BadRequest(new { message = "Username or password is incorrect" });
      // generate token that is valid for 7 days
      var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
          Subject = new ClaimsIdentity(claims),
          Expires = DateTime.UtcNow.AddDays(7),
          SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);

      return Ok(new { token = tokenHandler.WriteToken(token) });
    }

    [HttpPost("signup")]
    public IActionResult Authenticate(UserOptions userOptions)
    {
      _userService.CreateUser(userOptions);
      var claims = _userService.Authenticate(new AuthenticateRequest
      {
        Password = userOptions.Password,
        Username = userOptions.Username,
        IPAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString() ?? String.Empty,
        UserAgent = Request.Headers.TryGetValue("User-Agent",out var agent) ? agent.ToString() : string.Empty
      });

      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(claims),
        Expires = DateTime.UtcNow.AddDays(7),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };
      var token = tokenHandler.CreateToken(tokenDescriptor);
      
      return Ok(new AuthResponse { Token = tokenHandler.WriteToken(token) });
    }


    [HttpGet("Profile")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public IActionResult Profile()
    {
      var id = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
      UserProfile userProfile = _userService.GetProfile(int.Parse(id));
      return Ok(userProfile);
    }

    [HttpGet("claims")]
    [Authorize]
    public IActionResult Claims()
    {
      
      return Ok(User.Claims.ToDictionary(x => x.Type, x => x.Value));
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "AdminsOnly")]
    [HttpGet]
    public IActionResult GetAll()
    {
      var users = _userService.GetAll();
      return Ok(users);
    }
  }

  public class AuthResponse
  {
    public string Token { get; set; }
  }
}
