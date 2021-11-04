using System.Collections.Generic;

namespace Oakbrook.CoffeeShop.Controllers
{
  public class CardService : ICardService
  {
    private readonly ICardRepository _cardRepository;

    public CardService(ICardRepository cardRepository)
    {
      _cardRepository = cardRepository;
    }
    public int SaveCard(Card card)
    {
      return _cardRepository.Save(card);
    }

    public IEnumerable<Card> GetCards(int userId)
    {
      return _cardRepository.GetAll(userId);
    }
  }
}