using System.Collections.Generic;

namespace Oakbrook.CoffeeShop.Controllers
{
  public class CheckoutViewModel
  {
    public IEnumerable<Address> Addresses { get; set; }
    public IEnumerable<Card> Cards { get; set; }
  }
}