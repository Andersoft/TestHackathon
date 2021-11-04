using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Oakbrook.CoffeeShop.Controllers;
using Serilog;

namespace Oakbrook.CoffeeShop
{
  // {{flag}}
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      
      services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        {
          var appSettings = new AppSettings();
          Configuration.GetSection("AppSettings").Bind(appSettings);
          var key = Encoding.ASCII.GetBytes(appSettings.Secret);
          var signingKey = new SymmetricSecurityKey(key);
          options.TokenValidationParameters = new TokenValidationParameters
          {
            IssuerSigningKey = signingKey,
            ClockSkew = TimeSpan.Zero,
            ValidateAudience = false,
            RequireSignedTokens = false,
            ValidateIssuer = false,
          };
        })
        .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
        {
          options.LoginPath = "/Account/Login";
          options.LogoutPath = "/Account/Logout";
          options.AccessDeniedPath = "/Account/AccessDenied";
          options.ReturnUrlParameter = "ReturnUrl";
        });
      services.Configure<ForwardedHeadersOptions>(options =>
      {
        options.ForwardedHeaders =
          ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
      });
      services.AddAuthorization(options =>
      {
        options.AddPolicy("AdminsOnly", policy => policy.RequireClaim("Role", "Admin"));
      });

      services.AddCors();
      services.AddControllersWithViews();
      var keyPath = Path.Join(Directory.GetCurrentDirectory(), "SharedDrive", "keys");
      services.AddDataProtection()
        .PersistKeysToFileSystem(new DirectoryInfo(keyPath));

      // configure strongly typed settings object
      services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
      services.AddDirectoryBrowser();
      // configure DI for application services
      services.AddSingleton<IUserService, UserService>();
      services.AddSingleton<IDeviceRepository, DeviceRepository>();
      services.AddSingleton<IUserRepository, UserRepository>();
      services.AddSingleton<IProductRepository, ProductRepository>();
      services.AddSingleton<IOrderRepository, OrderRepository>();
      services.AddSingleton<IProductService, ProductService>();
      services.AddSingleton<ICheckoutService, CheckoutService>();
      services.AddSingleton<IBasketRepository, BasketRepository>();
      services.AddSingleton<ICaptchaRepository, CaptchaRepository>();
      services.AddSingleton<ICardService, CardService>();
      services.AddSingleton<ICardRepository, CardRepository>();
      services.AddSingleton<IAddressRepository, AddressRepository>();
      services.AddSingleton<IAddressService, AddressService>();
      services.AddSingleton<IBulkOrderRepository, BulkOrderRepository>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      
      app.UseSerilogRequestLogging();
      app.UseDeveloperExceptionPage();
      app.UseForwardedHeaders();
      app.UseRouting();
      app.UseStaticFiles();

      var SitePoliciesPath = Path.Combine(env.ContentRootPath, "SharedDrive");

      app.UseDirectoryBrowser(new DirectoryBrowserOptions
      {
        FileProvider = new PhysicalFileProvider(SitePoliciesPath),
        RequestPath = "/documents",
      });

      var path = Path.Combine(SitePoliciesPath, "Legal");
      app.UseStaticFiles(new StaticFileOptions
      {
        FileProvider = new PhysicalFileProvider(path),
        RequestPath = "/documents/legal",
        ServeUnknownFileTypes = true
      });
      // global cors policy
      app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());

      // custom jwt auth middleware
      //app.UseMiddleware<JwtMiddleware>();
      app.UseMiddleware<BlackListedExtensions>();
      app.UseAuthentication();
      app.UseAuthorization();
      app.UseEndpoints(x => x.MapControllers());
    }
  }
}
