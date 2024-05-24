using EcomMS.BLL.DTOs;
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
            return View();
        }
        public IActionResult GetAllCustomized(int draw, int start, int length, string search)
        {
            int totalCount = 0;
            int filteredCount = 0;
            List<CategoryDTO> result;
            if (string.IsNullOrEmpty(search))
            {
                result = categoryService.GetCustomized(start, length, out totalCount, out filteredCount);
            }
            else
            {
                result = categoryService.GetCustomized(c => c.Name.Contains(search) || c.Id.Equals(search), start, length, out totalCount, out filteredCount);
            }
            var response = new
            {
                draw = draw,
                recordsTotal = totalCount,
                recordsFiltered = filteredCount,
                data = result
            };
            return Json(response);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var data = categoryService.Get(c => c.Id == id);
            return View(data);
        }
        [HttpPost]
        public IActionResult Edit(CategoryDTO ct)
        {
            if (!ModelState.IsValid)
            {
                return View(ct);
            }
            var result = categoryService.Update(ct);
            TempData["success"] = "Category updated successfully!";
            return RedirectToAction("Index");
        }
    }
}
