using System.Collections.Generic;

namespace Oakbrook.CoffeeShop.Controllers
{
  public interface IAddressRepository
  {
    int Save(Address address);
    Address Get(int id);
    IEnumerable<Address> GetAll(int userId);
  }
}