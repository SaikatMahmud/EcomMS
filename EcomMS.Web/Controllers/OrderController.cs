using EcomMS.BLL.DTOs;
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
            if (!ModelState.IsValid)
            {
                TempData["error"] = "One or more required fields are invalid";
                return RedirectToAction("CartSummary", "Cart");
            }
            obj.CustomerId = 2;
            string msg = string.Empty;
            var result = orderService.PlaceOrder(obj, out msg);
            if (result) TempData["success"] = msg;
            else TempData["error"] = msg;
            return RedirectToAction("Index", "Cart");
        }
        public IActionResult OrderHistory()
        {
            return View();
        }
        public IActionResult GetOrderHistoryCustomized(int draw, int start, int length, string search, int orderColumn, string orderDirection)
        {
            int totalCount = 0;
            int filteredCount = 0;
            List<OrderWithStatusHistoriesDTO> result;
            //if (string.IsNullOrEmpty(search))
            //{
                result = orderService.GetCustomized(start, length, out totalCount, out filteredCount, "OrderStatusHistories");
            //}
            //else
            //{
            //    result = orderService.GetCustomized(c => c.Name.Contains(search) || c.Category.Name.Contains(search), start, length, out totalCount, out filteredCount, "Category");
            //}
            var response = new
            {
                draw = draw,
                recordsTotal = totalCount,
                recordsFiltered = filteredCount,
                data = result
            };
            return Json(response);
        }
    }
}
