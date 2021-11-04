namespace Oakbrook.CoffeeShop.Controllers
{
  public interface IProductService
  {
    void PostComment(int id, NewComment comment);
  }
}