namespace Oakbrook.CoffeeShop.Controllers
{
  public class OrderedProduct
  {
    public int OrderId { get; set; }
    public Product Product { get; set; }
    public int Quantity { get; set; }
  }
}