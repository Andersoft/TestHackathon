using System.Collections.Generic;

namespace Oakbrook.CoffeeShop.Controllers
{
  public interface ICardRepository
  {
    int Save(Card card);
    IEnumerable<Card> GetAll(int userId);
  }
}