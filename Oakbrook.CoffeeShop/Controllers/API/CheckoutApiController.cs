using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Oakbrook.CoffeeShop.Controllers
{
  [ApiController]
  [Route("api/checkout")]
  [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
  public class CheckoutApiController : ControllerBase
  {
    private readonly ICheckoutService _checkoutService;
    private readonly IBulkOrderRepository _bulkOrderRepository;

    public CheckoutApiController(ICheckoutService checkoutService, IBulkOrderRepository bulkOrderRepository)
    {
      _checkoutService = checkoutService;
      _bulkOrderRepository = bulkOrderRepository;
    }

    [HttpGet("{id}")]
    public IActionResult BulkOrder([FromRoute]int id)
    {
      BulkOrders bulkOrders = _bulkOrderRepository.Get(id);
      return Ok(bulkOrders);

    }

    [HttpPost("")]
    public async Task<IActionResult> LegacyBulkOrder([FromServices]IWebHostEnvironment environment)
    {
      using MemoryStream stream = new MemoryStream();
      using StreamWriter writer = new StreamWriter(stream);
      await HttpContext.Request.Body.CopyToAsync(stream);
      stream.Seek(0, SeekOrigin.Begin);

      XmlDocument document = new XmlDocument();
      document.XmlResolver = new XmlUrlResolver();
      document.Load(stream);
      var id = User.Id();
      var checkout = Deserialize<Checkout>(document);
      checkout.UserId = User.Id();

      var bulkOrders = new[] { checkout };
      var orderIds = bulkOrders.Select(order => _checkoutService.ProcessCheckout(order)).ToArray();
      int bulkId = _bulkOrderRepository.Create(id, orderIds);
      return Ok(new BulkOrders {Id = bulkId, OrderIds = orderIds});
    }


    public T Deserialize<T>(XmlDocument document) where T : class
    {
      XmlReader reader = new XmlNodeReader(document);
      var serializer = new XmlSerializer(typeof(T));
      T result = (T)serializer.Deserialize(reader);
      return result;
    }
  }

  public class BulkOrders
  {
    public int Id { get; set; }
    public IEnumerable<int> OrderIds { get; set; }
  }
  public interface IBulkOrderRepository
  {
    int Create(int id, IEnumerable<int> orderIds);
    BulkOrders Get(int id);
  }

  class BulkOrderRepository : Repository, IBulkOrderRepository
  {
    public BulkOrderRepository(IOptions<AppSettings> appSettings) : base(appSettings)
    {
      
    }
    public int Create(int id, IEnumerable<int> orderIds)
    {
      return ExecuteWithId($"INSERT INTO BulkOrders (UserId, OrderIds) OUTPUT INSERTED.ID VALUES ({id}, '{string.Join(",", orderIds)}')");
    }

    public BulkOrders Get(int id)
    {
      return Query($"SELECT Id, UserId, OrderIds FROM BulkOrders WHERE Id = {id}", reader => new BulkOrders
      {
        Id = reader.GetInt32(0),
        OrderIds = reader.GetString(2).Split(",").Select(int.Parse)
      });
    }
  }
}