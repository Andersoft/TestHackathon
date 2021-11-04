using System.Collections.Generic;
using Oakbrook.CoffeeShop.Controllers;

namespace Oakbrook.CoffeeShop
{
  public interface IDeviceRepository
  {
    void LogDevice(int userId, string userAgent, string ipAddress);
    IEnumerable<Device> GetDevices(int userId);
  }
}