using System;
using System.Collections.Generic;
using System.Linq;

namespace Oakbrook.CoffeeShop.Controllers
{
  public class Order
  {
    public int Id { get; set; }
    public int UserId { get; set; }
    public Address Address { get; set; }
    public Card Card { get; set; }
    public IEnumerable<OrderedProduct> Items { get; set; }
    public DateTime CreatedAt { get; set; }

    public decimal Total
    {
      get
      {
        var sum = Items.Sum(x => x.Product.Price * x.Quantity);
        return Delivery + sum - (sum * Discount);
      }
    }

    public decimal Discount { get; set; }
    public decimal Delivery { get; set; }
    public int CardId { get; set; }
    public int AddressId { get; set; }
    public string DiscountCode { get; set; }
  }
}