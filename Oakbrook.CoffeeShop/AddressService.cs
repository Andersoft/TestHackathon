using System.Collections.Generic;

namespace Oakbrook.CoffeeShop.Controllers
{
  public class AddressService : IAddressService
  {
    private readonly IAddressRepository _addressRepository;

    public AddressService(IAddressRepository addressRepository)
    {
      _addressRepository = addressRepository;
    }

    public int SaveAddress(Address address)
    {
      return _addressRepository.Save(address);
    }

    public Address GetAddress(int id)
    {
      return _addressRepository.Get(id);
    }

    public IEnumerable<Address> GetAddresses(int userId)
    {
      return _addressRepository.GetAll(userId);
    }
  }
}