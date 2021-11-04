using System;
using System.Collections.Generic;

namespace Oakbrook.CoffeeShop
{



  public interface IUserRepository
  {
    void CreateUser(User user);
    User GetUserWithCredentials(string username, string password);
    List<User> SelectUsers();
    User GetUser(int id, string password = null);
    Guid GetResetToken(string userEmail);
    User GetUserByEmail(string email);
    bool TryResetPassword(int userId, string password, string token);
    User GetUserByToken(string token);
    void Update(User user);
  }
}