using Oakbrook.CoffeeShop.Controllers;

namespace Oakbrook.CoffeeShop
{
  public interface ICheckoutService
  {
    int ProcessCheckout(Checkout checkout);
  }
}