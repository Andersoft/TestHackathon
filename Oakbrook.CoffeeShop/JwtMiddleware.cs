using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Oakbrook.CoffeeShop
{
  public class JwtMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly AppSettings _appSettings;

    public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
    {
      _next = next;
      _appSettings = appSettings.Value;
    }

    public async Task Invoke(HttpContext context, IUserService userService)
    {
      //todo: only invoke on /api endpoints
      var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

      if (token != null)
        attachUserToContext(context, userService, token);

      await _next(context);
    }

    private void attachUserToContext(HttpContext context, IUserService userService, string token)
    {
      try
      {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
        var signingKey = new SymmetricSecurityKey(key);

        tokenHandler.ValidateToken(token, new TokenValidationParameters
          {
            IssuerSigningKey = signingKey,
            ClockSkew = TimeSpan.Zero,
            ValidateAudience = false,
            RequireSignedTokens = false,
            ValidateIssuer = false,
          }, out var validatedToken);
        
        

        var jwtToken = (JwtSecurityToken)validatedToken;
        var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

        // attach user to context on successful jwt validation
        context.Items["User"] = userService.GetById(userId);
      }
      catch(Exception e)
      {
        Console.WriteLine(e.Message);
      }
    }
  }
}
