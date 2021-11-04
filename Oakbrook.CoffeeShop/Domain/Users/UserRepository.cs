using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Oakbrook.CoffeeShop
{
  public class UserRepository : Repository, IUserRepository
  {
    private const string SelectUser = "SELECT Id, FirstName, LastName, Username, Password, Email, Role, ProfilePicture FROM Users";
    public UserRepository(IOptions<AppSettings> appSettings) : base(appSettings)
    {
    }

   private User MapUser(SqlDataReader reader)
    {
      return new User
      {
        Id = reader.GetInt32(0),
        FirstName = reader.GetString(1),
        LastName = reader.GetString(2),
        Username = reader.GetString(3),
        Password = reader.GetString(4),
        Email = reader.GetString(5),
        Role = reader.GetString(6),
        ProfilePicture = reader.GetString(7)
      };
    }

    public User GetUserWithCredentials(string username, string password)
    {
      string commandText = @$"{SelectUser} WHERE Username = '{username}' AND Password = '{password}';"; //todo: this need a unique constraint

      return Query(commandText, reader => reader.Read() ? MapUser(reader) : null);
    }

    public User GetUser(int id, string password = null)
    {
      string commandText = @$"{SelectUser} WHERE Id = {id}";

      if (!string.IsNullOrWhiteSpace(password))
      {
        commandText += $"AND Password = '{password}';";
      }

      return Query(commandText, reader => reader.Read() ? MapUser(reader) : null);
    }

    public List<User> SelectUsers()
    {
      
      using var connection = OpenConnection();
      using var command = new SqlCommand();

      command.Connection = connection;
      command.CommandType = CommandType.Text;
      command.CommandText = @$"{SelectUser}";
      return Query(SelectUser, reader =>
      {
        List<User> users = new List<User>();
        while (reader.Read())
        {
          users.Add(MapUser(reader));
        }

        return users;
      });
    }

    public Guid GetResetToken(string userEmail)
    {
      var user = GetUserByEmail(userEmail);
      if (user == null)
      {
        throw new Exception("User does not exist");
      }

      var token = Guid.NewGuid();
      Execute(@$"INSERT INTO ResetToken (Token, UserId, Valid) VALUES ('{token}', '{user.Id}', 'true');");
      return token;
    }

    public User GetUserByEmail(string email)
    {
      return Query<List<User>>(@$"{SelectUser} WHERE Email = '{email}'",
        reader =>
        {
          List<User> users = new List<User>();
          while (reader.Read())
          {
            users.Add(MapUser(reader));
          } 
          return users;
        }).FirstOrDefault();
    }

    public bool TryResetPassword(int userId, string password, string token)
    {
      var isTokenValid = Execute($"UPDATE ResetToken SET Valid = 'false' WHERE Token = '{token}'") == 1;
      if (isTokenValid is false)
      {
        throw new Exception("Invalid reset token");
      }

      var hasPasswordReset = Execute($"UPDATE Users SET Password = '{password}' WHERE Id = {userId}") == 1;
      if (hasPasswordReset is false)
      {
        throw new Exception("Unable to reset password");
      }

      return true;
    }


    public User GetUserByToken(string token)
    {
      var commandText = @$"
SELECT Users.Id, Users.FirstName, Users.LastName, Users.Username, Users.Password, Users.Email, Users.Role
FROM Users
LEFT JOIN ResetToken on ResetToken.UserId = Users.Id
WHERE ResetToken.Token = '{token}' AND ResetToken.Valid = '{true}'";

      return Query(commandText, reader => reader.Read() ? MapUser(reader) : null);
    }

    public void Update(User user)
    {
      Execute($"UPDATE Users SET " +
              $"FirstName = '{user.FirstName}', " +
              $"LastName = '{user.LastName}', " +
              $"Username = '{user.Username}', " +
              $"Password = '{user.Password}', " +
              $"Email = '{user.Email}', " +
              $"Role = '{user.Role}', " +
              $"ProfilePicture = '{user.ProfilePicture}' WHERE Id = {user.Id};");
    }

    public void CreateUser(User user)
    {
      var commandText = @$"INSERT INTO Users (FirstName, LastName, Username, Password, Email, Role, ProfilePicture) VALUES ('{user.FirstName}', '{user.LastName}', '{user.Username}', '{user.Password}', '{user.Email}', '{user.Role}', '{user.ProfilePicture}');";
      Execute(commandText);
    }
  }
}