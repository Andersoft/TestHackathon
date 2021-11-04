using System.Collections.Generic;

namespace Oakbrook.CoffeeShop.Controllers
{
  public interface IAddressService
  {
    int SaveAddress(Address address);
    Address GetAddress(int id);
    IEnumerable<Address> GetAddresses(int userId);
  }
}