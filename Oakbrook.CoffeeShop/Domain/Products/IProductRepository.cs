using System.Collections.Generic;

namespace Oakbrook.CoffeeShop
{
  public interface IProductRepository
  {
    List<Product> GetProducts();
    List<Product> GetProducts(string search);
    List<Product> GetProducts(IEnumerable<int> ids);
    void PostComment(int id, int userId, string value, int rating, string summary);
  }
}