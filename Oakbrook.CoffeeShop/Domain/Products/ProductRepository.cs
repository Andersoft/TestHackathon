using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Oakbrook.CoffeeShop
{
  public class ProductRepository : Repository, IProductRepository
  {
    public ProductRepository(IOptions<AppSettings> appSettings) : base(appSettings)
    {
      
    }
    public List<Product> GetProducts()
    {
      var commandText = @$"SELECT Id, Name, Image, Price FROM Products;";
      var products = Query(commandText, reader =>
      {
        List<Product> items = new List<Product>();
        while (reader.Read())
        {
          items.Add(new Product
          {
            Id = reader.GetInt32(0),
            Name = reader.GetString(1),
            Image = reader.GetString(2),
            Price = decimal.Parse(reader.GetString(3)),
            Comments = new List<Comment>()
          });
        }

        return items;
      });
      var comments = GetComments();
      products.ForEach(x => x.Comments = comments.Where(c => c.ProductId == x.Id));
      return products;
    }

    public List<Product> GetProducts(string search)
    {
      var commandText = @$"SELECT Id, Name, Image, Price FROM Products WHERE Name LIKE '%{search}%';";
      var products = Query(commandText, reader =>
      {
        List<Product> items = new List<Product>();
        while (reader.Read())
        {
          items.Add(new Product
          {
            Id = reader.GetInt32(0),
            Name = reader.GetString(1),
            Image = reader.GetString(2),
            Price = decimal.Parse(reader.GetString(3)),
            Comments = new List<Comment>()
          });
        }

        return items;
      });
      var comments = GetComments();
      products.ForEach(x => x.Comments = comments.Where(c => c.ProductId == x.Id));
      return products;
    }

    public List<Product> GetProducts(IEnumerable<int> ids)
    {
      var commandText = @$"SELECT Id, Name, Image, Price FROM Products WHERE Id IN ({string.Join(",", ids)}) ;";
      var products = Query(commandText, reader =>
      {
        List<Product> items = new List<Product>();
        while (reader.Read())
        {
          items.Add(new Product
          {
            Id = reader.GetInt32(0),
            Name = reader.GetString(1),
            Image = reader.GetString(2),
            Price = decimal.Parse(reader.GetString(3)),
            Comments = new List<Comment>()
          });
        }

        return items;
      });
      

      var comments = GetComments();
      products.ForEach(x => x.Comments = comments.Where(c => c.ProductId == x.Id));
      return products;
    }

    public List<Comment> GetComments()
    {
      using var connection = OpenConnection();
      using var command = new SqlCommand();

      command.Connection = connection;
      command.CommandType = CommandType.Text;
      command.CommandText = @$"SELECT Comments.Id, Comments.ProductId, Users.Username, Comments.Value, Comments.PostedAt, Comments.Rating, Users.ProfilePicture, Comments.Summary FROM Comments INNER JOIN Users on Users.Id = Comments.UserId";

      SqlDataReader reader = command.ExecuteReader();
      List<Comment> comments = new List<Comment>();
      while (reader.Read())
      {
        comments.Add(new Comment
        {
          Id = reader.GetInt32(0),
          ProductId = reader.GetInt32(1),
          Name = reader.GetString(2),
          Value = reader.GetString(3),
          PostedAt = reader.GetDateTime(4),
          Rating = reader.GetInt32(5),
          ProfilePicture = reader.GetString(6),
          Summary = reader.GetString(7)
        });
      }

      return comments;
    }

    public void PostComment(int id, int userId, string value, int rating, string summary)
    {
      string commandText = @$"  
INSERT INTO Comments (ProductId, UserId, Value, Rating, Summary, PostedAt) 
VALUES (@id, @userId, @value, @rating, @summary, '{DateTime.UtcNow:yyyy-MM-ddThh:mm:ss}');";
      var parameters = new[]
      {
        new SqlParameter("@id", SqlDbType.Int) {Value = id},
        new SqlParameter("@userId", SqlDbType.Int) {Value = userId},
        new SqlParameter("@value", SqlDbType.NVarChar) {Value = value},
        new SqlParameter("@rating", SqlDbType.Int) {Value = rating},
        new SqlParameter("@summary", SqlDbType.NVarChar) {Value = summary}
      };
      int affectedRows = Execute(commandText, parameters);
      if (affectedRows != 1)
      {
        throw new Exception("Unable to insert comment");
      }
    }
  }
}