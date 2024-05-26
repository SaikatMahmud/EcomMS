using EcomMS.BLL.ServiceAccess;
using EcomMS.BLL.Services;
using EcomMS.BLL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EcomMS.Web.Controllers
{
    public class OrderController : Controller
    {
        private OrderService orderService;
        public OrderController(IBusinessService serviceAccess)
        {
            orderService = serviceAccess.OrderService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [Route("Order/Checkout")]
        public IActionResult Checkout(CartSummary obj)
        {
            if(!ModelState.IsValid)
            {
                TempData["error"] = "One or more required fields are invalid";
                return RedirectToAction("CartSummary","Cart");
            }
            obj.CustomerId = 2;
            string msg = string.Empty;
            var result = orderService.PlaceOrder(obj, out msg);
            TempData["success"] = msg;
            return Ok();
            //string msg = string.Empty;
            //var order = new OrderDTO()
            //{
            //    CustomerId = 2,
            //    Amount = int.Parse(Request.Form["Amount"]),
            //    PaymentMethod = Request.Form["PaymentMethod"],
            //    DeliveryAddress = Request.Form["DeliveryAddress"]
            //};
            //var result = orderService.Checkout(order, out msg);
            //TempData["success"] = msg;
            //return RedirectToAction("Index", "Home");
        }   
    }
}
