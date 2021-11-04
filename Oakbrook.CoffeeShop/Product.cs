using System.Collections.Generic;

namespace Oakbrook.CoffeeShop
{
  public class Product
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Image { get; set; }
    public IEnumerable<Comment> Comments { get; set; }
    public Comment PostModel { get; set; }
    public decimal Price { get; set; }
  }
}