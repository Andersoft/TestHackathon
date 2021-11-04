using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Oakbrook.CoffeeShop.Controllers
{
  [Route("[controller]")]
  public class ComplaintsController : Controller
  {
    [HttpGet("")]
    public IActionResult Index()
    {
      /*using (var memoryStream = new MemoryStream())
      {
        using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
        {
          var demoFile = archive.CreateEntry(@"..\..\..\..\..\..\..\..\..\..\..\Temp\evil.txt");

          using (var entryStream = demoFile.Open())
          using (var streamWriter = new StreamWriter(entryStream))
          {
            streamWriter.Write("Bar!");
          }
        }

        using (var fileStream = new FileStream(@"C:\Temp\test.zip", FileMode.Create))
        {
          memoryStream.Seek(0, SeekOrigin.Begin);
          memoryStream.CopyTo(fileStream);
        }
      }*/

      return View();
    }

    [HttpPost("")]
    public async Task<IActionResult> Upload([FromServices] IWebHostEnvironment env, [FromForm(Name = "file")]IFormFile file)
    {
      var directory = Path.Combine(env.WebRootPath, "complaints", User.Id().ToString());
      if (!Directory.Exists(directory))
      {
        Directory.CreateDirectory(directory);
      }

      using ZipArchive archive = new ZipArchive(file.OpenReadStream());
      foreach (var entry in archive.Entries)
      {
        entry.ExtractToFile(Path.Combine(directory, entry.FullName), true);
      }
      return RedirectToAction("Index","Product");
    }
  }
}