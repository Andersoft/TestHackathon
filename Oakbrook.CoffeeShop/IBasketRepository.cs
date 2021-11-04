using System.Collections.Generic;

namespace Oakbrook.CoffeeShop.Controllers
{
  public interface IBasketRepository
  {
    int CreateNewBasket(int userId);

    void AddToBasket(int id, int productId);

    List<Product> GetBasket(int id);
    void RemoveFromBasket(int basketId, int productId);
    void DeleteBasket(int basketId);
  }
}