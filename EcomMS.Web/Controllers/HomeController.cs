using EcomMS.BLL.DTOs;
using EcomMS.BLL.ServiceAccess;
using EcomMS.BLL.Services;
using EcomMS.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace EcomMS.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private CategoryService categoryService;
        private ProductService productService;

        public HomeController(IBusinessService serviceAccess/*ILogger<HomeController> logger*/)
        {
            productService = serviceAccess.ProductService;
            categoryService = serviceAccess.CategoryService;
            //_logger = logger;
        }

        public IActionResult Index([FromQuery] int filterbycat)
        {
            List<ProductImageMapDTO> result;
            if(filterbycat == 0)
            {
            result = productService.GetAll(p => p.IsLive == true, "Images");
            }
            else
            {
                result = productService.GetAll(p => p.IsLive == true && p.CategoryId == filterbycat, "Images");
            }
            IEnumerable<SelectListItem> catList = categoryService.Get().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString(),
            });
            ViewBag.catList = catList;
            return View(result);
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
