using EcomMS.BLL.ServiceAccess;
using EcomMS.BLL.Services;
using EcomMS.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EcomMS.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ProductService productService;

        public HomeController(IBusinessService businessService/*ILogger<HomeController> logger*/)
        {
            productService = businessService.ProductService;
            //_logger = logger;
        }

        public IActionResult Index()
        {
            var data = productService.Get("Images");
            return View(data);
        }
        [Route("Product/Details/{id}")]
        public IActionResult ProductDetails(int id)
        {
            var data = productService.Get(p => p.Id == id, "Category, Images");
            return View(data);
        }

        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
