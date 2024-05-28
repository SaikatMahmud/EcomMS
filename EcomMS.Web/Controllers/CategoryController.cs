using EcomMS.BLL.DTOs;
using EcomMS.BLL.ServiceAccess;
using EcomMS.BLL.Services;
using EcomMS.Web.Auth;
using Microsoft.AspNetCore.Mvc;

namespace EcomMS.Web.Controllers
{
    [AdminAccess]
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
            if (result)
            {
                TempData["success"] = "Category updated successfully!";
                return RedirectToAction("Index");
            }
            else TempData["error"] = "Could not updated. Server error!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(CategoryDTO ct)
        {
            if (!ModelState.IsValid)
            {
                return View(ct);
            }
            var result = categoryService.Create(ct);
            if (result)
            {
                TempData["success"] = "Category added successfully!";
                return RedirectToAction("Index");
            }
            else TempData["error"] = "Could not added. Server error!";
            return RedirectToAction("Index");
        }
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var result = categoryService.Delete(id);
            if (!result) return Json(new { success= false, msg = "Delete failed. Category not found!" });
            return Json(new { success = true, msg = "Category deleted!" });
        }

        [HttpPost]
        public async Task<IActionResult> UploadCategoryBulk(IFormFile file)
        {
            if (file == null) return Json(new { msg = "No file provided!" });
            var result = await categoryService.UploadFromExcel(file.OpenReadStream());
            if (result) return Json(new { success = true, msg = "Categories uploaded" });
            return Json(new { success = false, msg = "Error! Could not upload categories!" });
        }
    }
}
