using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Oakbrook.CoffeeShop;
using Oakbrook.CoffeeShop.Controllers;

namespace Oakbrook.CoffeeShop
{

  public class UserService : IUserService
  {
    private readonly IUserRepository _userRepository;

    private readonly IDeviceRepository _deviceRepository;
    private readonly ICardRepository _cardRepository;
    private readonly IAddressRepository _addressRepository;

    public UserService(IUserRepository userRepository, IDeviceRepository deviceRepository, IAddressRepository addressRepository, ICardRepository cardRepository)
    {
      _userRepository = userRepository;
      _deviceRepository = deviceRepository;
      _addressRepository = addressRepository;
      _cardRepository = cardRepository;
    }

    public void PasswordReset(string host, string userEmail)
    {
      

      var fromAddress = new MailAddress("oakhackathon001@gmail.com", "Jordan Anderson");
      var toAddress = new MailAddress(userEmail, "Jordan Anderson");
      const string fromPassword = "BZG9byp*vpm2vbv9zyb";
      const string subject = "Forgot password";
      
      //todo once only link, verification token
      var token = _userRepository.GetResetToken(userEmail);
      var encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{userEmail}:{token}"));
      string body = $"click here {host}/Account/PasswordReset?token={encoded}";
      var smtp = new SmtpClient
      {
        Host = "smtp.gmail.com",
        Port = 587,
        EnableSsl = true,
        DeliveryMethod = SmtpDeliveryMethod.Network,
        UseDefaultCredentials = false,
        Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
      };
      using var message = new MailMessage(fromAddress, toAddress)
      {
        Subject = subject,
        Body = body
      };
      smtp.Send(message);
    }

    public bool ResetPassword(string token, string password)
    {
      User user = _userRepository.GetUserByToken(token);
      
      if (user == null)
      {
        throw new Exception("Invalid password reset token");
      }

      return _userRepository.TryResetPassword(user.Id, password, token);
    }

    public UserProfile GetProfile(int userId)
    {
      var devices = _deviceRepository.GetDevices(userId);
      var addresses = _addressRepository.GetAll(userId);
      var cards = _cardRepository.GetAll(userId);
      User user = _userRepository.GetUser(userId);
      return new UserProfile
      {
        OrderHistory = new List<Order>(),
        Devices = devices,
        Addresses = addresses,
        Cards = cards,
        FirstName = user.FirstName,
        Email = user.Email,
        LastName = user.LastName,
        ProfilePicture = user.ProfilePicture,
        Username = user.Username
      };
    }

    public void UpdateProfilePicture(string path, int userId)
    {
      User user = _userRepository.GetUser(userId);
      user.ProfilePicture = path;
      _userRepository.Update(user);
    }

    public void UpdateDetails(UserDetails userDetails, int id)
    {
      var user = _userRepository.GetUser(id);
      user.FirstName = userDetails.FirstName;
      user.LastName = userDetails.LastName;
      user.Email = userDetails.Email;
      _userRepository.Update(user);
    }

    public void UpdatePassword(int id, string password)
    {
      var user = _userRepository.GetUser(id);
      user.Password = SecureHashPassword(password);
      _userRepository.Update(user);
    }

    public bool IsCurrentPasswordCorrect(int id, string currentPassword)
    {
      return _userRepository.GetUser(id, SecureHashPassword(currentPassword)) != null;
    }

    public List<Claim> Authenticate(AuthenticateRequest model)
    {
      var user = _userRepository.GetUserWithCredentials(model.Username, SecureHashPassword(model.Password));
      
      if (user == null)
      {
        throw new Exception("Incorrect credentials");
      }

      _deviceRepository.LogDevice(user.Id, model.UserAgent, model.IPAddress);

      return new List<Claim>
      {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.Username),
        new Claim("Role", user.Role),
      };
    }

    public IEnumerable<User> GetAll()
    {
      return _userRepository.SelectUsers();
    }

    public User GetById(int id)
    {
      return _userRepository.GetUser(id);
    }

    public User CreateUser(UserOptions userOptions)
    {
      User user = new User
      {
        Email = userOptions.Email,
        FirstName = userOptions.FirstName,
        LastName = userOptions.LastName,
        Password = SecureHashPassword(userOptions.Password),
        Username = userOptions.Username,
        Role = userOptions.Role,
        ProfilePicture = $"/imgs/default_profile_{new Random().Next(0,4)}.png"
      };

      _userRepository.CreateUser(user);
      return user;
    }

    private static string SecureHashPassword(string input)
    {
      if (input == null) return null;

      using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
      {
        byte[] inputBytes = Encoding.ASCII.GetBytes(input);
        byte[] hashBytes = md5.ComputeHash(inputBytes);

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < hashBytes.Length; i++)
        {
          sb.Append(hashBytes[i].ToString("X2"));
        }
        return sb.ToString();
      }
    }
  }
}