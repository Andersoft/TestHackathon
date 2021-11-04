using System.Linq;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Oakbrook.CoffeeShop.Controllers
{

  [Route("api/basket")]
  [ApiController]
  [Authorize(AuthenticationSchemes = "Bearer,Cookies")]
  public class BasketApiController : ControllerBase
  {
    private readonly IBasketRepository _basketRepository;

    public BasketApiController(IBasketRepository basketRepository)
    {
      _basketRepository = basketRepository;
    }
    
    [HttpPost("{basketId}")]
    public IActionResult Index([FromRoute]int basketId, [FromBody]BasketPostModel model)
    {
      _basketRepository.AddToBasket(basketId, model.ProductId);

      return Ok();
    }

    [HttpDelete("{basketId}/{productId}")]
    public IActionResult Index([FromRoute] int basketId, [FromRoute]int productId)
    {
      _basketRepository.RemoveFromBasket(basketId, productId);

      return Ok();
    }

    [HttpGet("{basketId}")]
    public IActionResult Get([FromRoute] int basketId)
    {
      var items = _basketRepository.GetBasket(basketId);
      var consolidated = items.GroupBy(x => x.Id).Select(x => new BasketItem
      {
        Product = x.First(),
        Quantity = x.Count()
      });
      return Ok(new Basket {Id = basketId, Items = consolidated });
    }

    [HttpPost("")]
    public IActionResult CreateNewBasket()
    {
      var id = _basketRepository.CreateNewBasket(User.Id());

      return Ok(new Basket { Id = id });
    }
  }
}