using System.Collections.Generic;

namespace Oakbrook.CoffeeShop.Controllers
{
  public class ProductsViewModel
  {
    public string Filter { get; set; }
    public IEnumerable<Product> Products { get; set; }
  }
}