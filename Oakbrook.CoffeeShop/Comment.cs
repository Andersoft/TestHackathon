using System;

namespace Oakbrook.CoffeeShop
{
  public class Comment
  {
    public string Name { get; set; }
    public int Rating { get; set; }
    public string Value { get; set; }
    public int Id { get; set; }
    public int ProductId { get; set; }
    public DateTime PostedAt { get; set; }
    public string ProfilePicture { get; set; }
    public string Summary { get; set; }
  }
}