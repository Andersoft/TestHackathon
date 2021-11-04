using System.Linq;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Oakbrook.CoffeeShop.Controllers
{
  [Route("[controller]")]
    public class CheckoutController : Controller
  {
    private readonly IAddressService _addressService;
    private readonly ICheckoutService _checkoutService;
    private ICardService _cardService;

    public CheckoutController(ICheckoutService checkoutService, IAddressService addressService, ICardService cardService)
    {
      _checkoutService = checkoutService;
      _addressService = addressService;
      _cardService = cardService;
    }

    [HttpGet("")]
    [Authorize]
    public IActionResult Index()
    {
      var addresses = _addressService.GetAddresses(User.Id());
      var cards = _cardService.GetCards(User.Id());

      return View(new CheckoutViewModel
      {
        Addresses = addresses,
        Cards = cards
      });
    }

    [HttpPost("")]
    [Authorize]
    public IActionResult Index([FromForm] Checkout checkout)
    {
      checkout.UserId = User.Id();
      var id = _checkoutService.ProcessCheckout(checkout);
      return RedirectToAction("Index", "Order", new {orderId = id});
    }
  }
}