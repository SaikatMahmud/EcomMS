using EcomMS.BLL.DTOs;
using EcomMS.BLL.ServiceAccess;
using EcomMS.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EcomMS.Web.Controllers
{
    public class ProductController : Controller
    {
        private CategoryService categoryService;
        private ProductService productService;
        private readonly IWebHostEnvironment webHostEnvironment;
        public ProductController(IBusinessService serviceAccess, IWebHostEnvironment _webHostEnvironment)
        {
            categoryService = serviceAccess.CategoryService;
            productService = serviceAccess.ProductService;
            webHostEnvironment = _webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetAllCustomized(int draw, int start, int length, string search)
        {
            int totalCount = 0;
            int filteredCount = 0;
            List<ProductDTO> result;
            if (string.IsNullOrEmpty(search))
            {
                result = productService.GetCustomized(start, length, out totalCount, out filteredCount, "Category");
            }
            else
            {
                result = productService.GetCustomized(c => c.Name.Contains(search) || c.Category.Name.Contains(search), start, length, out totalCount, out filteredCount, "Category");
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
            var data = productService.Get(i => i.Id == id, "Images");
            if (data == null) return NotFound();
            IEnumerable<SelectListItem> catList = categoryService.Get().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString(),
            });
            ViewBag.catList = catList;
            return View(data);
        }
        [HttpPost]
        public IActionResult Edit(ProductDTO product)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<SelectListItem> catList = categoryService.Get().Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString(),
                });
                ViewBag.catList = catList;
                return View(product);
            }
            var result = productService.Update(product);
            if (result)
            {
                TempData["success"] = "Product updated successfully!";
                return RedirectToAction("Index");
            }
            else TempData["error"] = "Could not updated. Server error!";
            return RedirectToAction("Index");
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var result = productService.Delete(id);
            if (!result) return Json(new { success = false, msg = "Delete failed. Product not found!" });
            return Json(new { success = true, msg = "Product deleted!" });
        }

        [HttpPost]
        public async Task<IActionResult> UploadCategoryBulk(IFormFile file)
        {
            if (file == null) return Json(new { msg = "No file provided!" });
            var result = await productService.UploadFromExcel(file.OpenReadStream());
            if (result) return Json(new { success = true, msg = "Categories uploaded" });
            return Json(new { success = false, msg = "Error! Could not upload categories!" });
        }
    }
}
