using System.Linq;
using System.Security.Claims;

namespace Oakbrook.CoffeeShop.Controllers
{
  public static class ClaimsPrincipalExtensions
  {
    public static int Id(this ClaimsPrincipal claimsPrincipal)
    {
      return int.Parse(claimsPrincipal.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
    }
    public static string Username(this ClaimsPrincipal claimsPrincipal)
    {
      return claimsPrincipal.Claims.First(x => x.Type == ClaimTypes.Name).Value;
    }
  }
}