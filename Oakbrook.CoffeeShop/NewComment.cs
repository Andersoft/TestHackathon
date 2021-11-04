namespace Oakbrook.CoffeeShop.Controllers
{
  public class NewComment
  {
    public int UserId { get; set; }
    public int Rating { get; set; }
    public string Review { get; set; }
    public int ProductId { get; set; }
    public int CaptchaId { get; set; }
    public string CaptchaAnswer { get; set; }
    public string Name { get; set; }
    public string CaptchaQuestion { get; set; }
    public string Summary { get; set; }
  }
}