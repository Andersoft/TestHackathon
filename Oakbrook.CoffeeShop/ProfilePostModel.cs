using Microsoft.AspNetCore.Http;

namespace Oakbrook.CoffeeShop.Controllers
{
  public class ProfilePostModel
  {
    public IFormFile Image { get; set; }
  }
}