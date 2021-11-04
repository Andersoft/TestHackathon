using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace Oakbrook.CoffeeShop.Controllers
{
  [Route("[controller]")]
  public class ProductController : Controller
  {
    private readonly IProductRepository _productRepository;
    private readonly ICaptchaRepository _captchaRepository;
    private readonly IProductService _productService;

    public ProductController(IProductRepository productRepository, ICaptchaRepository captchaRepository, IProductService productService)
    {
      _productRepository = productRepository;
      _captchaRepository = captchaRepository;
      _productService = productService;
    }

    [HttpGet("")]
    public IActionResult Index()
    {
    
        return View(new ProductsViewModel
        {
          Products = _productRepository.GetProducts(),
          Filter = null
        });
    }


    [HttpGet("search")]
    public IActionResult Search([FromQuery(Name = "term")] string term)
    {
      var products = _productRepository.GetProducts(term);

      if (string.IsNullOrWhiteSpace(term))
      {
        return View("Index",new ProductsViewModel
        {
          Products = products,
          Filter = null
        });
      }
      else
      {
        var filtered = products.Where(x => x.Name.ToLower().Contains(term.ToLower()));
        filtered = filtered.Any() ? filtered : products;
        var model = new ProductsViewModel
        {
          Products = filtered,
          Filter = $"{term} (No Results)"
        };
        return View("Index", model);
      }
    }

    [HttpPost("")]
    public IActionResult Index([FromForm] string search)
    {
      var products = _productRepository.GetProducts(search);

      if (string.IsNullOrWhiteSpace(search))
      {
        return View(new ProductsViewModel
        {
          Products = products,
          Filter = null
        });
      }
      else
      {
        var filtered = products.Where(x => x.Name.ToLower().Contains(search.ToLower()));
        filtered = filtered.Any() ? filtered : products;
        var model = new ProductsViewModel
        {
          Products = filtered,
          Filter = $"{search} (No Results)"
        };
        return View(model);
      }
    }

    [HttpGet("{id:int}")]
    public IActionResult Product([FromRoute]int id)
    {
      var captchas = _captchaRepository.GetAll();
      Random random = new Random();
      var index = random.Next(captchas.Count() - 1);
      ;
      return View(new ProductViewModel
      {
        Product = _productRepository.GetProducts().First(x => x.Id == id),
        NewComment = new NewComment
        {
          CaptchaId = captchas[index].Id,
          UserId = User.Id(),
          ProductId = id,
          Name = User.Username(),
          CaptchaQuestion = captchas[index].Question
        },
      });
    }

    

    [HttpPost("{id}")]
    public IActionResult AddComment([FromRoute] int id, [FromForm]NewComment comment)
    {
      _productService.PostComment(id, comment);

      return RedirectToAction("Product", new { id = id });
    }
  }
}