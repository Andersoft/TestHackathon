using System.Collections.Generic;
using Microsoft.Extensions.Options;

namespace Oakbrook.CoffeeShop.Controllers
{
  public class BasketRepository : Repository, IBasketRepository
  {
    public BasketRepository(IOptions<AppSettings> appSettings) : base(appSettings)
    {
      
    }
    public int CreateNewBasket(int userId)
    {
      return ExecuteWithId($@"INSERT INTO Basket (UserId) OUTPUT INSERTED.ID VALUES ('{userId}')");
    }

    public void AddToBasket(int id, int productId)
    {
      Execute(@$"INSERT INTO BasketItems (basketId, productId) VALUES ({id},{productId})");
    }
    
    public List<Product> GetBasket(int id)
    {
      return Query(@$"SELECT Products.Id, Products.Name, Products.Image, Products.Price FROM Products INNER JOIN BasketItems on BasketItems.ProductId = Products.Id WHERE BasketItems.BasketId = {id}",
        reader =>
        {
          List<Product> products = new List<Product>();
          while (reader.Read())
          {
            products.Add(new Product
            {
              Comments = new List<Comment>(),
              Id = reader.GetInt32(0),
              Name = reader.GetString(1),
              Image = reader.GetString(2),
              Price = decimal.Parse(reader.GetString(3))
            });
          }

          return products;
        });
    }

    public void RemoveFromBasket(int basketId, int productId)
    {
      Execute($"DELETE TOP (1) FROM BasketItems WHERE productId = {productId}");
    }

    public void DeleteBasket(int basketId)
    {
      Execute(@$"DELETE FROM BasketItems WHERE basketId = {basketId}");
      Execute(@$"DELETE FROM Basket WHERE Id = {basketId}");
    }
  }
}