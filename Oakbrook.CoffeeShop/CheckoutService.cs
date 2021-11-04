using System.Collections.Generic;
using System.Linq;
using Oakbrook.CoffeeShop.Controllers;

namespace Oakbrook.CoffeeShop
{
  public class CheckoutService : ICheckoutService
  {
    private readonly ICardService _cardService;
    private readonly IAddressService _addressService;
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly IBasketRepository _basketRepository;

    public CheckoutService(ICardService cardService, IAddressService addressService, IOrderRepository orderRepository, IProductRepository productRepository, IBasketRepository basketRepository)
    {
      _cardService = cardService;
      _addressService = addressService;
      _orderRepository = orderRepository;
      _productRepository = productRepository;
      _basketRepository = basketRepository;
    }

    public int ProcessCheckout(Checkout checkout)
    {
      // save address
      if (!checkout.AddressId.HasValue)
      {
        var addressId = _addressService.SaveAddress(new Address
        {
          UserId = checkout.UserId,
          PostCode = checkout.PostCode,
          StreetAddress = checkout.StreetAddress,
          HouseNumber = checkout.HouseNumber,
          PhoneNumber = checkout.PhoneNumber
        });
        checkout.AddressId = addressId;
      }

      if (!checkout.CardId.HasValue)
      {
        int cardId = _cardService.SaveCard(new Card
        {
          Number = new string(checkout.CardNumber.Skip(checkout.CardNumber.Length - 4).ToArray()),
          Expiration = checkout.ExpiryDate,
          CVV = checkout.CVV,
          UserId = checkout.UserId
        });

        checkout.CardId = cardId;
      }

      var items = _basketRepository.GetBasket(checkout.BasketId);
      var orderId = _orderRepository.CreateOrder(new Order
      {
        AddressId = checkout.AddressId.Value,
        CardId = checkout.CardId.Value,
        UserId = checkout.UserId,
        Discount = checkout.Discount,
        DiscountCode = checkout.DiscountCode,
        Delivery = 5.99m,
        Items = items.GroupBy(x => x.Id).Select(x => new OrderedProduct
        {
          Product = x.First(),
          Quantity = x.Count()
        }),
      });
      _basketRepository.DeleteBasket(checkout.BasketId);
      return orderId;
    }
  }
}