using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Oakbrook.CoffeeShop.Controllers.Web
{
  [Route("[controller]")]
  public class OrderController : Controller
  {
    private readonly IOrderRepository _orderRepository;

    public OrderController(IOrderRepository orderRepository)
    {
      _orderRepository = orderRepository;
    }

    [HttpGet("track/{orderId}")]
    [Authorize]
    public IActionResult Index([FromRoute]int orderId)
    {
      var order = _orderRepository.GetOrders(User.Id()).First(x => x.Id == orderId);

      return View(order);
    }

    [HttpGet("History/All")]
    [Authorize(Policy = "AdminsOnly")]
    public IActionResult HistoryAll()
    {
      IEnumerable<Order> orders = _orderRepository.GetOrders();
      
      return View(orders);
    }

    [HttpGet("History")]
    [Authorize]
    public IActionResult History()
    {
      IEnumerable<Order> orders = _orderRepository.GetOrders(User.Id());

      return View(orders);
    }
  }
}