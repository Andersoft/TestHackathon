using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Oakbrook.CoffeeShop.Controllers
{
  public class OrderRepository : Repository, IOrderRepository
  {
    public OrderRepository(IOptions<AppSettings> appSettings) : base(appSettings)
    {
      
    }
    public int CreateOrder(Order order)
    {
      string insertOrder = @$"  
INSERT INTO Orders (CreatedAt, UserId, AddressId, CardId, Discount, DiscountCode, Delivery) 
OUTPUT INSERTED.ID
VALUES ('{DateTime.UtcNow.ToString("yyyy-MM-ddThh:mm:ss")}', '{order.UserId}', '{order.AddressId}', {order.CardId}, '{order.Discount}', '{order.DiscountCode}', '{order.Delivery}');";

      int orderId = ExecuteWithId(insertOrder);
      
      string insertOrderedItems = string.Empty;
      foreach (var item in order.Items)
      {
        insertOrderedItems += @$"INSERT INTO OrderedProducts (OrderId, ProductId, Quantity, Price) VALUES ('{orderId}', '{item.Product.Id}', '{item.Quantity}', '{item.Product.Price}');";
      }

      Execute(insertOrderedItems);
      return orderId;
    }

    public IEnumerable<Order> GetOrders()
    {
      var queryOrders = @$"SELECT Orders.Id, Orders.CreatedAt, Orders.UserId, Address.PostCode, Address.StreetAddress, Address.HouseNumber, Address.PhoneNumber, Orders.Discount, Orders.DiscountCode, Orders.Delivery, Cards.Number, Cards.CVV, Cards.Expiry 
FROM Orders
JOIN Address on Address.Id=Orders.AddressId
JOIN Cards on Cards.Id=Orders.CardId
";
      var queryOrderedItems = @$"
SELECT OrderedProducts.Id, OrderedProducts.OrderId, OrderedProducts.ProductId, OrderedProducts.Quantity, OrderedProducts.Price, Products.Name 
FROM OrderedProducts 
JOIN Products ON OrderedProducts.ProductId=Products.Id";

      var orders = Query(queryOrders, GetOrders);
      var orderedItems = Query<List<OrderedProduct>>(queryOrderedItems, GetOrderedProducts);

      foreach (var order in orders)
      {
        order.Items = orderedItems.Where(x => x.OrderId == order.Id);
      }

      return orders;
    }

    public IEnumerable<Order> GetOrders(int userId)
    {
      var queryOrders = @$"SELECT Orders.Id, Orders.CreatedAt, Orders.UserId, Address.PostCode, Address.StreetAddress, Address.HouseNumber, Address.PhoneNumber, Orders.Discount, Orders.DiscountCode, Orders.Delivery,  Cards.Number, Cards.CVV, Cards.Expiry 
FROM Orders
JOIN Address on Address.Id = Orders.AddressId
JOIN Cards on Cards.Id = Orders.CardId
WHERE Orders.UserId = {userId}
";
      var queryOrderedItems = @$"
SELECT OrderedProducts.Id, OrderedProducts.OrderId, OrderedProducts.ProductId, OrderedProducts.Quantity, OrderedProducts.Price, Products.Name 
FROM OrderedProducts 
JOIN Products ON OrderedProducts.ProductId=Products.Id";

      var orders = Query(queryOrders, GetOrders);
      var orderedItems = Query<List<OrderedProduct>>(queryOrderedItems, GetOrderedProducts);

      foreach (var order in orders)
      {
        order.Items = orderedItems.Where(x => x.OrderId == order.Id);
      }

      return orders;
    }

    private static List<OrderedProduct> GetOrderedProducts(SqlDataReader reader)
    {
      List<OrderedProduct> products = new List<OrderedProduct>();
      while (reader.Read())
      {
        products.Add(new OrderedProduct
        {
          OrderId = reader.GetInt32(1),
          Quantity = reader.GetInt32(3),
          Product = new Product
          {
            Id = reader.GetInt32(2),
            Price = decimal.Parse(reader.GetString(4)),
            Name = reader.GetString(5)
          }
        });
      }

      return products;
    }

    private static List<Order> GetOrders(SqlDataReader reader)
    {
      List<Order> orders = new List<Order>();
        while (reader.Read())
        {
          orders.Add(new Order
          {
            Id = reader.GetInt32(0),
            CreatedAt = reader.GetDateTime(1),
            UserId = reader.GetInt32(2),
            Address = new Address
            {
              PostCode = reader.GetString(3),
              StreetAddress = reader.GetString(4),
              HouseNumber = reader.GetInt32(5),
              PhoneNumber = reader.GetString(6),
            },
            Discount = decimal.Parse(reader.GetString(7)),
            DiscountCode = reader.GetString(8),
            Delivery = decimal.Parse(reader.GetString(9)),
            Card = new Card
            {
              Number = reader.GetString(10),
              CVV = reader.GetInt32(11),
              Expiration = reader.GetString(12)
            }

          });
        }
        return orders;
    }
  }
}