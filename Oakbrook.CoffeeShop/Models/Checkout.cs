namespace Oakbrook.CoffeeShop.Controllers
{
  public class Checkout
  {
    public int HouseNumber { get; set; }
    public string StreetAddress { get; set; }
    public string PhoneNumber { get; set; }
    public string PostCode { get; set; }
    public string CardNumber { get; set; }
    public string ExpiryDate { get; set; }
    public int CVV { get; set; }
    public string DiscountCode { get; set; }
    public decimal Discount { get; set; }
    public int BasketId { get; set; }
    public int UserId { get; set; }
    public int? AddressId { get; set; }
    public int? CardId { get; set; }
  }
}