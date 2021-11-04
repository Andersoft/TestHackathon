using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;

namespace Oakbrook.CoffeeShop
{
  public class Repository
  {
    private readonly string _connectionString;

    public Repository(IOptions<AppSettings> appSettings)
    {
      _connectionString = appSettings.Value.ConnectionString;
    }

    protected SqlConnection OpenConnection()
    {
      var connection = new SqlConnection(_connectionString);
      connection.Open();
      return connection;
    }

    protected T Query<T>(string commandText, Func<SqlDataReader, T> action)
    {
      using var connection = OpenConnection();
      using var command = new SqlCommand();
      command.Connection = connection;
      command.CommandType = CommandType.Text;
      command.CommandText = commandText;
      SqlDataReader reader = command.ExecuteReader();
      return action(reader);
    }

    protected int ExecuteWithId(string commandText)
    {
      using var connection = OpenConnection();
      using var command = new SqlCommand();
      command.Connection = connection;
      command.CommandType = CommandType.Text;
      command.CommandText = commandText;
      return (int)command.ExecuteScalar();
      
    }
    protected int Execute(string commandText, params SqlParameter[] parameters)
    {
      using var connection = OpenConnection();
      using var command = new SqlCommand();
      command.Connection = connection;
      command.CommandType = CommandType.Text;
      command.CommandText = commandText;
      command.Parameters.AddRange(parameters);
      return command.ExecuteNonQuery();
    }
  }
}