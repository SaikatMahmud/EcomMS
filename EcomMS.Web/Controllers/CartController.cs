using EcomMS.BLL.DTOs;
using EcomMS.BLL.ServiceAccess;
using EcomMS.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcomMS.Web.Controllers
{
    public class CartController : Controller
    {
        private CartService cartService;
        private CustomerService customerService;
        public CartController(IBusinessService serviceAccess)
        {
            cartService = serviceAccess.CartService;
            customerService = serviceAccess.CustomerService;
        }
        public IActionResult Index()
        {
            int cusId = 2;
            var data = cartService.GetCartWithProduct(ct => ct.CustomerId == cusId, "Product, Product.Images");
            return View(data);
        }
        [HttpPost]
        [Route("Cart/Add")]
        public IActionResult AddToCart()
        {
            string msg = string.Empty;
            var cusId = 2;
            var cart = new CartDTO()
            {
                CustomerId = cusId,
                ProductId = int.Parse(Request.Form["ProductId"]),
                Quantity = int.Parse(Request.Form["Quantity"])
            };
            var result = cartService.AddItemToCart(cart, out msg);
            TempData["success"] = msg;
            return RedirectToAction("Index","Home");
        }
        [Route("Cart/increase/{cartId}")]
        public IActionResult IncreaseCartProduct(int cartId)
        {
            var result = cartService.Increase(cartId);
            return RedirectToAction("Index");
        }
        [Route("Cart/decrease/{cartId}")]
        public IActionResult DeductCartProduct(int cartId)
        {
            var result = cartService.Deduct(cartId);
            return RedirectToAction("Index");
        }
        
        [Route("Cart/remove/{cartId}")]
        public IActionResult RemoveFromCart(int cartId)
        {
            var result = cartService.Delete(cartId);
            TempData["success"] = "Product deleted from your cart.";
            return RedirectToAction("Index");
        }

        public IActionResult CartSummary()
        {
            var cusId = 2;
            var cartData = cartService.GetAll(ct => ct.CustomerId == cusId, "Product");
            var customerData = customerService.Get(c => c.Id == cusId);
            ViewBag.Customer = customerData;
            return View(cartData);
        }
       
    }
}
