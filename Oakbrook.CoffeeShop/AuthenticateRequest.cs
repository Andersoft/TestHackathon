using System.ComponentModel.DataAnnotations;

namespace Oakbrook.CoffeeShop
{
  public class AuthenticateRequest
  {
    public string IPAddress;

    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }

    public string UserAgent { get; set; }
  }
}