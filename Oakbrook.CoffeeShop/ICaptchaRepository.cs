using System.Collections.Generic;

namespace Oakbrook.CoffeeShop.Controllers
{
  public interface ICaptchaRepository
  {
    Captcha Get(int id);
    List<Captcha> GetAll();
  }
}