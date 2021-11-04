using System.Collections.Generic;
using Microsoft.Extensions.Options;

namespace Oakbrook.CoffeeShop.Controllers
{
  public class CardRepository : Repository, ICardRepository
  {
    public CardRepository(IOptions<AppSettings> appSettings) : base(appSettings)
    {
      
    }
    public int Save(Card card)
    {
      return ExecuteWithId($"INSERT INTO Cards (Number, Expiry, CVV, UserId) OUTPUT INSERTED.ID VALUES ('{card.Number}','{card.Expiration}',{card.CVV}, {card.UserId})");
    }

    public IEnumerable<Card> GetAll(int userId)
    {
      return Query($"SELECT Id, Number, Expiry, CVV FROM Cards WHERE UserId = {userId}", reader =>
      {
        List<Card> cards = new List<Card>();
        while (reader.Read())
        {
          cards.Add(new Card
          {
            UserId = userId,
            CVV = reader.GetInt32(3),
            Expiration = reader.GetString(2),
            Number = reader.GetString(1),
            Id = reader.GetInt32(0)
          });
        }

        return cards;
      });
    }
  }
}