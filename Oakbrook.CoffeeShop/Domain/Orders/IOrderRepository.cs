using System.Collections.Generic;

namespace Oakbrook.CoffeeShop.Controllers
{
  public interface IOrderRepository
  {
    int CreateOrder(Order order);
    IEnumerable<Order> GetOrders();
    IEnumerable<Order> GetOrders(int userId);
  }
}