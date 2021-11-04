using System;

namespace Oakbrook.CoffeeShop.Controllers
{
  public class ProductService : IProductService
  {
    
    private readonly IProductRepository _productRepository;
    private readonly ICaptchaRepository _captchaRepository;

    public ProductService(IProductRepository productRepository, ICaptchaRepository captchaRepository)
    {
      _productRepository = productRepository;
      _captchaRepository = captchaRepository;
    }

    public void PostComment(int id, NewComment comment)
    {
      if(!ValidateCaptcha(comment.CaptchaId, comment.CaptchaAnswer))
      {
        throw new Exception("Invalid captcha");
      }

      _productRepository.PostComment(id, comment.UserId, comment.Review, comment.Rating, comment.Summary);
    }

    private bool ValidateCaptcha(int captchaId, string answer)
    {
      Captcha captcha = _captchaRepository.Get(captchaId);
      return captcha.Answer == answer;
    }
  }
}