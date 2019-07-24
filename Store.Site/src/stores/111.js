window.a = setInterval(() => {
  if (document.querySelector('.all-products-loaded .nm-infload-to-top') == null) {
    var b = document.querySelector('.nm-infload-btn');
    if (b) b.dispatchEvent(new Event('click'));
  } else {
    clearInterval(window.a);
  }
}, 500);

while {}
return document.querySelectorAll('.product')