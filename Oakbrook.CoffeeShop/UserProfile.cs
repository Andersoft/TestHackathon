using System.Collections.Generic;

namespace Oakbrook.CoffeeShop.Controllers
{
  public class UserProfile
  {
    public IEnumerable<Order> OrderHistory { get; set; }
    public IEnumerable<Device> Devices { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public IEnumerable<Address> Addresses { get; set; }
    public IEnumerable<Card> Cards { get; set; }
    public string ProfilePicture { get; set; }
    public string Username { get; set; }
  }
}