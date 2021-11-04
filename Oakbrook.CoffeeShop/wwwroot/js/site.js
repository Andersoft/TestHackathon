// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function addToBasket(productId) {
  /// json post to basket api
  // if no basket id then create get basket
  if (localStorage.getItem('basket') == null) {
    fetch(`/api/basket`,
      {
        method: 'POST',
        mode: 'cors',
        cache: 'no-cache',
        credentials: 'same-origin',
        headers: {
          'Content-Type': 'application/json'
        },
        redirect: 'follow', // manual, *follow, error
        referrerPolicy:
          'no-referrer', // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
        body: JSON.stringify({}) // body data type must match "Content-Type" header
      })
      .then(response => response.json())
      .then(data => {
        localStorage.setItem('basket', data.id);
        updateBasket(data.id, productId);
      });
  } else {
    let basketId = localStorage.getItem('basket');
    updateBasket(basketId, productId);
  }

}// Write your JavaScript code.
function removeFromBasket(productId) {
  if (localStorage.getItem('basket') != null) {
    let basketId = localStorage.getItem('basket');
    fetch(`/api/basket/${basketId}/${productId}`,
      {
        method: 'DELETE',
        mode: 'cors',
        cache: 'no-cache',
        credentials: 'same-origin',
        headers: {
          'Content-Type': 'application/json'
        },
        redirect: 'follow', // manual, *follow, error
        referrerPolicy:
          'no-referrer' // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
      })
      .then(() => buildBasket(false));
  }
}
function updateBasket(basketId, productId) {
  return fetch(`/api/basket/${basketId}`,
    {
      method: 'POST',
      mode: 'cors',
      cache: 'no-cache',
      credentials: 'same-origin',
      headers: {
        'Content-Type': 'application/json'
      },
      redirect: 'follow', // manual, *follow, error
      referrerPolicy:
        'no-referrer', // no-referrer, *no-referrer-when-downgrade, origin, origin-when-cross-origin, same-origin, strict-origin, strict-origin-when-cross-origin, unsafe-url
      body: JSON.stringify({ productId: productId }) // body data type must match "Content-Type" header
    })
    .then(() => buildBasket(false));
}
function buildBasket() {
  calculateBasket();
  var basketId = localStorage.getItem('basket');
  if (basketId != null) {
    fetch(`/api/basket/${basketId}`)
      .then(response => response.json())
      .then(data => {
        var basketList = $('#basket-list');
        basketList.empty();
        if (data.items.length > 0) {
          let subtotal = data.items.map(x => x.product.price * x.quantity).reduce((a, b) => a + b);
          $('#subtotal').text(subtotal.toFixed(2));
          for (let item of data.items) {
            basketList.append($(`<li><span class="cust-btn material-icons" onclick="addToBasket('${item.product.id}')";>add_circle_outline</span><span class="quantity">${item.quantity}</span><span class="cust-btn material-icons" onclick="removeFromBasket('${item.product.id}')">remove_circle_outline</span>${item.product.name}</ li>`));
          }
        }
        calculateBasket();
      });
  }
}
function calculateBasket() {
  let total =
    parseFloat($('#subtotal').text()) +
    parseFloat($("#delivery").text()) -
    parseFloat($('#Discount').text());

  $('#total').text(total.toFixed(2));
}
function addPromotion() {
  let code = $('#promocode').val();
  console.log(code);
  if (validate(code)) {
    console.log("Code is valid");
    applyPromotion(code);
  } else {
    console.log("Code is invalid");
    rejectPromotion();
  }
}
function applyPromotion(code) {
  console.log("applying promotion");
  
  $("#promotion-tags").empty();
  $("#promotion-tags").append(`<div class="row"><div class="col"><span class="badge bg-success">${validCodes[code].text}</span></div></div>`)
  $("#applied-discount").val(validCodes[code].discount);
  $("#applied-discountCode").val(code);

  console.log(`SubTotal: ${$('#subtotal').text()}`);
  console.log(`Discount off: ${validCodes[code].discount}`);
  let discount = $('#subtotal').text() * validCodes[code].discount;
  console.log(`Discount: ${discount}`);
  
  $("#Discount").text(discount.toFixed(2));
  calculateBasket();
}
function rejectPromotion() {

}
function validate(code) {

  if (code in validCodes) {
    console.log("code exists");
    console.log(`${validCodes[code].endOfLife} > ${Date.now()} = ${validCodes[code].endOfLife > Date.now()}`);
    return validCodes[code].endOfLife > Date.now();
  }
  console.log("code is missing");
  return false;
}

const validCodes = {
  'OBLOANS21': {
    endOfLife: 1735173977723,
    discount: 0.9,
    text: "Free 5k loan!"
  },
  'TEST07': {
    endOfLife: 10351739,
    discount: 0.1,
    text: "DEBUG!"
  }
}