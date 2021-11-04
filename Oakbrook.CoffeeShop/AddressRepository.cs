using System.Collections.Generic;
using Microsoft.Extensions.Options;

namespace Oakbrook.CoffeeShop.Controllers
{
  public class AddressRepository : Repository, IAddressRepository
  {
    public AddressRepository(IOptions<AppSettings> appSettings) : base(appSettings)
    {
      
    }
    public int Save(Address address)
    {
      int addressId = ExecuteWithId($"INSERT INTO Address (PostCode, StreetAddress, HouseNumber, PhoneNumber, UserId) OUTPUT INSERTED.ID VALUES ('{address.PostCode}','{address.StreetAddress}',{address.HouseNumber},'{address.PhoneNumber}', {address.UserId})");
      return addressId;
    }

    public Address Get(int id)
    {
      return Query($"SELECT PostCode, StreetAddress, HouseNumber, PhoneNumber, UserId FROM Address WHERE Id = {id}", reader =>
      {
        if (reader.Read())
        {
          return new Address
          {
            Id = id,
            UserId = reader.GetInt32(4),
            PhoneNumber= reader.GetString(3),
            HouseNumber = reader.GetInt32(2),
            StreetAddress = reader.GetString(1),
            PostCode = reader.GetString(0)
          };
        }

        return null;
      });
    }

    public IEnumerable<Address> GetAll(int userId)
    {
      return Query($"SELECT Id, PostCode, StreetAddress, HouseNumber, PhoneNumber, UserId FROM Address WHERE UserId = {userId}", reader =>
      {
        List<Address> addresses = new List<Address>();
        while (reader.Read())
        {
          addresses.Add(new Address
          {
            UserId = reader.GetInt32(5),
            PhoneNumber = reader.GetString(4),
            HouseNumber = reader.GetInt32(3),
            StreetAddress = reader.GetString(2),
            PostCode = reader.GetString(1),
            Id = reader.GetInt32(0),
          });
        }

        return addresses;
      });
    }
  }
}