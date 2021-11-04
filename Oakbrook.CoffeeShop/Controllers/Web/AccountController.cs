using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Oakbrook.CoffeeShop;
using Oakbrook.CoffeeShop.Models;

namespace Oakbrook.CoffeeShop.Controllers
{
  [Route("[controller]")]
  public class AccountController : Controller
  {
    private readonly IUserRepository _userRepository;
    private readonly IUserService _userService;

    public AccountController(IUserRepository userRepository, IUserService userService)
    {
      _userRepository = userRepository;
      _userService = userService;
    }

    [HttpGet("login")]
    public IActionResult Index(ErrorViewModel? errorViewModel)
    {
      return View();
    }

    [HttpGet("PasswordReset")]
    public IActionResult Reset([FromQuery(Name = "token")]string resetToken)
    {
      var decoded = Convert.FromBase64String(resetToken);
      var value = Encoding.UTF8.GetString(decoded);


      return View(new PasswordReset
      {
        Email = value.Split(":")[0],
        Token = value.Split(":")[1]
      });
    }

    [HttpPost("PasswordReset")]
    public IActionResult Reset([FromForm]PasswordResetModel model)
    {
      if (_userService.ResetPassword(model.Token, model.Password))
      {
        return RedirectToAction("Index", "Product");
      }
      return View();
    }

    [HttpGet("ForgotPassword")]
    public IActionResult ForgotPassword()
    {
      return View();
    }

    [HttpPost("ForgotPassword")]
    public IActionResult ForgotPassword([FromForm]ForgottenPassword model)
    {
      _userService.PasswordReset($"{Request.Scheme}://{Request.Host}", model.Email);

      return RedirectToAction("ForgotPassword");
    }


    [HttpGet("Profile")]
    public IActionResult Profile()
    {
      var id = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
      UserProfile userProfile = _userService.GetProfile(int.Parse(id));
      return View(userProfile);
    }

    [HttpPost("Profile")]
    public IActionResult UpdatePassword([FromForm]UserPassword userPassword)
    {
      _userService.UpdatePassword(User.Id(), userPassword.Password);
      return RedirectToAction("Profile");

    }

    [HttpPost("Profile/Picture")]
    public async Task<IActionResult> UploadProfilePicture([FromServices] IWebHostEnvironment env, [FromForm(Name = "file")] IFormFile file)
    {
      var directory = Path.Combine(env.WebRootPath, "avatars", User.Id().ToString());
      if (!Directory.Exists(directory))
      {
        Directory.CreateDirectory(directory);
      }

      using (var stream = System.IO.File.Create(Path.Combine(directory, $"profile-picture{Path.GetExtension(file.FileName)}")))
      {
        await file.CopyToAsync(stream);
      }

      var relativePath = "\\"+Path.Combine("avatars", User.Id().ToString(), $"profile-picture{Path.GetExtension(file.FileName)}");
      _userService.UpdateProfilePicture(relativePath, User.Id());

      return RedirectToAction("Profile");
    }

    [HttpPost("Profile/Details")]
    public IActionResult ChangeAccountDetails([FromServices] IWebHostEnvironment env, [FromForm] UserDetails userDetails)
    {
      _userService.UpdateDetails(userDetails, User.Id());

      return RedirectToAction("Profile");
    }

    [HttpGet("signup")]
    public IActionResult Signup(ErrorViewModel? errorViewModel)
    {
      return View();
    }

    [HttpPost("signup")]
    public async Task<IActionResult> Signup([FromForm]UserOptions userOptions)
    {
      userOptions.Role = "User";
      _userService.CreateUser(userOptions);
      var claims = _userService.Authenticate(new AuthenticateRequest
      {
        Password = userOptions.Password,
        Username = userOptions.Username,
        IPAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
        UserAgent = Request.Headers["User-Agent"].ToString()
      });
    
      var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
      var principal = new ClaimsPrincipal(identity);
      
      await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties { IsPersistent = true });

      return RedirectToAction("Index", "Product");
    }

    [HttpGet("logout")]
    public async Task<IActionResult> Logout(ErrorViewModel? errorViewModel)
    {
      await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
      return RedirectToAction("Index");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromForm]Credentials credentials)
    {
      var claims = _userService.Authenticate(new AuthenticateRequest
      {
        Password = credentials.Password,
        Username = credentials.Username,
        IPAddress = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
        UserAgent = Request.Headers["User-Agent"].ToString()
      });

      if (claims == null || !claims.Any())
      {
        return RedirectToAction("Index", new ErrorViewModel { });
      }

      var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
      var principal = new ClaimsPrincipal(identity);

      await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties { IsPersistent = true });
      
      return RedirectToAction("Index", "Product");
    }

    [HttpPost("ChangePassword")]
    public IActionResult ChangePassword([FromForm]ChangePasswordModel model)
    {
      if (!_userService.IsCurrentPasswordCorrect(User.Id(), model.CurrentPassword))
      {
        throw new Exception("Not current password");
      }

      if (model.ConfirmPassword != model.Password)
      {
        throw new Exception("New Passwords don't match ");
      }

      _userService.UpdatePassword(User.Id(), model.Password);

      return RedirectToAction("Profile");
    }
  }

  public class ChangePasswordModel
  {
    public string CurrentPassword { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
  }
}