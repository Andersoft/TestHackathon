using System.Collections.Generic;

namespace Oakbrook.CoffeeShop.Controllers
{
  public interface ICardService
  {
    int SaveCard(Card card);
    IEnumerable<Card> GetCards(int userId);
  }
}