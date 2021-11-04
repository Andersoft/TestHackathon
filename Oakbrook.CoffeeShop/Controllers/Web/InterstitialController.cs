using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Oakbrook.CoffeeShop.Controllers
{
  [Route("[controller]")]
  public class InterstitialController : Controller
  {
    private readonly ICheckoutService _checkoutService;

    [HttpGet("")]
    [Authorize]
    public IActionResult Index([FromQuery(Name = "redirectTo")]string redirect)
    {
      if (redirect.Contains("oakbrookfinance.com"))
      {
        return View(new InterstitialViewModel { RedirectTo = redirect });
      }
      return BadRequest();
    }

    [HttpPost("")]
    [Authorize]
    public IActionResult Index([FromForm] Checkout checkout)
    {
      checkout.UserId = int.Parse(User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
      var id = _checkoutService.ProcessCheckout(checkout);
      return RedirectToAction("Index", "Order", new { orderId = id });
    }
  }
}