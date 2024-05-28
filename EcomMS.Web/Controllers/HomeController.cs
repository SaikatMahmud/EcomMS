using EcomMS.BLL.DTOs;
using EcomMS.BLL.ServiceAccess;
using EcomMS.BLL.Services;
using EcomMS.Web.Auth;
using EcomMS.Web.Models;
using Microsoft.AspNetCore.Authorization;
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


        [CustomerAccess]
        //[Authorize]
        public IActionResult Index([FromQuery] int? filterbycat, [FromQuery] string? search)
        {
            //List<ProductImageMapDTO> result;
            //if(filterbycat == 0 || filterbycat == null)
            //{
            //result = productService.GetAll(p => p.IsLive == true, "Images");
            //}
            //else
            //{
            //    result = productService.GetAll(p => p.IsLive == true && p.CategoryId == filterbycat, "Images");
            //}
            IEnumerable<SelectListItem> catList = categoryService.Get().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString(),
            });
            ViewBag.catList = catList;
            return View();
        }

        public IActionResult GetHomePageProducts([FromQuery] int? filterbycat, [FromQuery] string search, [FromQuery] int? page)
        {
            List<ProductImageMapDTO> result;
            int totalCount = 0;
            int filteredCount = 0;
            if (page == 0 || page == null) page = 1;
            int pageNumber = (int)page;
            if (filterbycat == 0 || filterbycat == null)
            {
                if (string.IsNullOrEmpty(search))
                {
                    result = productService.GetHomePageProductCustomized(p => p.IsLive == true, (pageNumber - 1) * 12, 12, out totalCount, out filteredCount, "Images");
                }
                else
                {
                    result = productService.GetHomePageProductCustomized(p => p.IsLive == true && p.Name.Contains(search), (pageNumber - 1) * 12, 12, out totalCount, out filteredCount, "Images");
                }
            }
            else
            {
                if (string.IsNullOrEmpty(search))
                {
                    result = productService.GetHomePageProductCustomized(p => p.IsLive == true && p.CategoryId == filterbycat, (pageNumber - 1) * 12, 12, out totalCount, out filteredCount, "Images");
                }
                else
                {
                    result = productService.GetHomePageProductCustomized(p => p.IsLive == true && p.CategoryId == filterbycat && p.Name.Contains(search), (pageNumber - 1) * 12, 12, out totalCount, out filteredCount, "Images");
                }
            }
            return Json(new { data = result , totalPage = filteredCount/12+1});
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
