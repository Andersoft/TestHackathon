using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;

namespace Oakbrook.CoffeeShop
{
  public class BlackListedExtensions
  {
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public BlackListedExtensions(RequestDelegate next, IConfiguration configuration)
    {
      _next = next;
      _configuration = configuration;
    }

    public Task Invoke(HttpContext context)
    {
      var extensions = _configuration["blocked"].Split(",");
      
      if (extensions.Any(extension => context.Request.GetEncodedPathAndQuery().Contains(extension)))
      {
        context.Response.StatusCode = 404;
        return Task.CompletedTask;
      }
      else
      {
        return _next(context);
      }
    }
  }
}