using System.Collections.Generic;

namespace Oakbrook.CoffeeShop.Controllers
{
  public class Basket
  {
    public int Id { get; set; }
    public IEnumerable<BasketItem> Items { get; set; }
  }
}