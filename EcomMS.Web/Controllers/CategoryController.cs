using EcomMS.BLL.ServiceAccess;
using EcomMS.BLL.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcomMS.Web.Controllers
{
    public class CategoryController : Controller
    {
        private CategoryService categoryService;
        public CategoryController(IBusinessService serviceAccess)
        {
            categoryService = serviceAccess.CategoryService;
        }
        public IActionResult Index()
        {
            var result = categoryService.Get();
            return View(result);
        }
    }
}
