﻿using EcomMS.BLL.DTOs;
using EcomMS.BLL.ServiceAccess;
using EcomMS.BLL.Services;
using EcomMS.BLL.ViewModels;
using EcomMS.Web.Auth;
using Microsoft.AspNetCore.Mvc;

namespace EcomMS.Web.Controllers
{
    public class OrderController : Controller
    {
        private OrderService orderService;
        private OrderProductService orderProductService;
        private OrderStatusHistoryService orderStatusHistory;
        public OrderController(IBusinessService serviceAccess)
        {
            orderService = serviceAccess.OrderService;
            orderProductService = serviceAccess.OrderProductService;
            orderStatusHistory = serviceAccess.OrderStatusHistoryService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [Route("Order/Checkout")] // place order
        public IActionResult Checkout(CartSummaryVM obj)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "One or more required fields are invalid";
                return RedirectToAction("CartSummary", "Cart");
            }
            obj.CustomerId = (int)HttpContext.Session.GetInt32("userId");
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
            int cusId = (int)HttpContext.Session.GetInt32("userId");
            int totalCount = 0;
            int filteredCount = 0;
            List<OrderWithStatusHistoriesDTO> result;
            //if (string.IsNullOrEmpty(search))
            //{
                result = orderService.GetCustomized(o => o.CustomerId == cusId, start, length, out totalCount, out filteredCount, "OrderStatusHistories");
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
        [AdminAccess]
        public IActionResult AllOrder()
        {
            return View();
        }

        public IActionResult GetAllOrderCustomized(int draw, int start, int length, string search, int orderColumn, string orderDirection)
        {
            int totalCount = 0;
            int filteredCount = 0;
            var result = orderService.GetCustomized(start, length, out totalCount, out filteredCount, "OrderStatusHistories");
            var response = new
            {
                draw = draw,
                recordsTotal = totalCount,
                recordsFiltered = filteredCount,
                data = result
            };
            return Json(response);
        }
        [HttpPut]
        [Route("Order/ProcessOrderAdmin/{orderId}")]
        public IActionResult ProcessOrderAdmin(int orderId)
        {
            var empId = (int)HttpContext.Session.GetInt32("userId"); ;
            var result = orderStatusHistory.ProcessOrder(orderId, empId);
            if (result) return Json(new { success = true, msg = "Order status changed" });
            return Json(new { success = false, msg = "Internal server error" });
        }
        [HttpPut]
        [Route("Order/ShipOrderAdmin/{orderId}")]
        public IActionResult ShipOrderAdmin(int orderId)
        {
            var empId = (int)HttpContext.Session.GetInt32("userId"); ;
            var result = orderStatusHistory.ShipOrder(orderId, empId);
            if (result) return Json(new { success = true, msg = "Order status changed" });
            return Json(new { success = false, msg = "Internal server error" });
        }
        [HttpPut]
        [Route("Order/CancelOrderAdmin/{orderId}")]
        public IActionResult CancelOrderAdmin(int orderId)
        {
            var empId = (int)HttpContext.Session.GetInt32("userId"); ;
            var result = orderStatusHistory.CancelOrderByAdmin(orderId, empId);
            if (result) return Json(new { success = true, msg = "Order cancelled!" });
            return Json(new { success = false, msg = "Internal server error" });
        }
        
        [HttpPut]
        [Route("Order/CancelOrderCustomer/{orderId}")]
        public IActionResult CancelOrderCustomer(int orderId)
        {
            var empId = (int)HttpContext.Session.GetInt32("userId"); ;
            var result = orderStatusHistory.CancelOrderByCustomer(orderId);
            if (result) return Json(new { success = true, msg = "Order cancelled!" });
            return Json(new { success = false, msg = "Internal server error" });
        }
        [LoggedAccess]
        [HttpGet]
        [Route("Order/OrderDetails/{orderId}")]
        public IActionResult OrderDetails(int orderId)
        {
            var data = orderProductService.GetAll(op => op.OrderId == orderId, "Product");
            var order = orderService.Get(o => o.Id == orderId);
            var statusHistory = orderStatusHistory.Get(sh => sh.OrderId == orderId);
            ViewBag.StatusHistory = statusHistory;
            ViewBag.PaymentMethod = order.PaymentMethod;
            return View(data);
        }
    }
}
