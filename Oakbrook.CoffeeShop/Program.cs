using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using Serilog.Events;

namespace Oakbrook.CoffeeShop
{
  public class Program
  {
    public static Task Main(string[] args) => MainAsync(args, CancellationToken.None);
    public static Task MainAsync(string[] args, CancellationToken token)
    {
      Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
        .Enrich.FromLogContext()
        .WriteTo.File("Logs/oakbrook-cafe-.log", rollingInterval:RollingInterval.Day)
        .CreateLogger();

      try
      {
        Log.Information("Challenge flag: CBA65CD8-FD50-4230-85B2-E7188C7A63A8");
        return CreateHostBuilder(args).Build().RunAsync(token: token);
      }
      catch (Exception ex)
      {
        Log.Fatal(ex, "Host terminated unexpectedly");
      }
      finally
      {
        Log.CloseAndFlush();
      }
      return Task.CompletedTask;
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
          .UseSerilog()
          .ConfigureWebHostDefaults(webBuilder =>
            {
              webBuilder.UseStartup<Startup>();
              webBuilder.UseContentRoot(
                "O:\\Github\\Oakbrook.CoffeeShop\\Oakbrook.CoffeeShop\\Oakbrook.CoffeeShop");
            });
  }
}
