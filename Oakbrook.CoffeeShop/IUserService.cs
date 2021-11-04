using System.Collections.Generic;
using System.Security.Claims;
using Oakbrook.CoffeeShop.Controllers;

namespace Oakbrook.CoffeeShop
{
  public interface IUserService
  {
    List<Claim> Authenticate(AuthenticateRequest model);
    IEnumerable<User> GetAll();
    User GetById(int id);
    User CreateUser(UserOptions userOptions);
    void PasswordReset(string host, string userEmail);
    bool ResetPassword(string token, string password);
    UserProfile GetProfile(int userId);
    void UpdateProfilePicture(string path, int userId);
    void UpdateDetails(UserDetails userDetails, int id);
    void UpdatePassword(int id, string password);
    bool IsCurrentPasswordCorrect(int id, string currentPassword);
    
  }
}