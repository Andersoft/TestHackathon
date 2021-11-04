using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Oakbrook.CoffeeShop.Controllers;

namespace Oakbrook.CoffeeShop
{
  public class DeviceRepository : Repository, IDeviceRepository
  {
    public DeviceRepository(IOptions<AppSettings> appSettings) : base(appSettings)
    {
      
    }
    public void LogDevice(int userId, string userAgent, string ipAddress)
    {
      var parameters = new[]
      {
        new SqlParameter("@userId", SqlDbType.Int) {Value = userId},
        new SqlParameter("@UserAgent", SqlDbType.NVarChar) {Value = userAgent},
        new SqlParameter("@IPAddress", SqlDbType.NVarChar) {Value = ipAddress}
      }; 
      Execute($"INSERT INTO Devices (UserId, UserAgent, IPAddress) VALUES (@userId, @UserAgent, @IPAddress)", parameters);
    }

    public IEnumerable<Device> GetDevices(int userId)
    {
      return Query($"SELECT Id, UserAgent, IPAddress, UserId FROM Devices WHERE UserId = {userId}", reader =>
      {
        List<Device> devices = new List<Device>();
        while (reader.Read())
        {
          devices.Add(new Device
          {
            Id = reader.GetInt32(0),
            UserAgent = reader.GetString(1),
            IPAddress = reader.GetString(2),
            UserId = reader.GetInt32(3),
          });
        }

        return devices;
      });
    }
  }
}