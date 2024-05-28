using EcomMS.BLL.DTOs;
using EcomMS.BLL.ServiceAccess;
using EcomMS.BLL.Services;
using EcomMS.Web.Auth;
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
        [CustomerAccess]
        public IActionResult Index()
        {
            IEnumerable<SelectListItem> catList = categoryService.Get().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString(),
            });
            ViewBag.catList = catList;
            return View();
        }
        [HttpGet]
        public IActionResult GetAllCustomized([FromQuery] int filterbycat, int draw, int start, int length, string search, int orderColumn, string orderDirection)
        {
            int totalCount = 0;
            int filteredCount = 0;
            List<ProductDTO> result;
            if(filterbycat == 0)
            {
                if (string.IsNullOrEmpty(search))
                {
                    result = productService.GetCustomized(start, length, out totalCount, out filteredCount, "Category");
                }
                else
                {
                    result = productService.GetCustomized(p => p.Name.Contains(search) || p.Category.Name.Contains(search), start, length, out totalCount, out filteredCount, "Category");
                }
            }
            else
            {
                if (string.IsNullOrEmpty(search))
                {
                    result = productService.GetCustomized(p => p.CategoryId == filterbycat, start, length, out totalCount, out filteredCount, "Category");
                }
                else
                {
                    result = productService.GetCustomized(p => p.CategoryId == filterbycat && (p.Name.Contains(search) || p.Category.Name.Contains(search)), start, length, out totalCount, out filteredCount, "Category");
                }
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
        public IActionResult GetAllOfflineCustomized([FromQuery] int filterbycat, int draw, int start, int length, string search, int orderColumn, string orderDirection)
        {
            int totalCount = 0;
            int filteredCount = 0;
            List<ProductDTO> result;
            if (filterbycat == 0)
            {
                if (string.IsNullOrEmpty(search))
                {
                    result = productService.GetCustomized(p => p.IsLive == false, start, length, out totalCount, out filteredCount, "Category");
                }
                else
                {
                    result = productService.GetCustomized(p => p.IsLive == false && (p.Name.Contains(search) || p.Category.Name.Contains(search)), start, length, out totalCount, out filteredCount, "Category");
                }
            }
            else
            {
                if (string.IsNullOrEmpty(search))
                {
                    result = productService.GetCustomized(p => p.IsLive == false && p.CategoryId == filterbycat, start, length, out totalCount, out filteredCount, "Category");
                }
                else
                {
                    result = productService.GetCustomized(p => p.IsLive == false && p.CategoryId == filterbycat && (p.Name.Contains(search) || p.Category.Name.Contains(search)), start, length, out totalCount, out filteredCount, "Category");
                }
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
        public IActionResult GetAllLowStockCustomized([FromQuery] int filterbycat, int draw, int start, int length, string search, int orderColumn, string orderDirection)
        {
            int totalCount = 0;
            int filteredCount = 0;
            List<ProductDTO> result;
            if (filterbycat == 0)
            {
                if (string.IsNullOrEmpty(search))
                {
                    result = productService.GetCustomized(p => p.Quantity < p.ReorderQuantity, start, length, out totalCount, out filteredCount, "Category");
                }
                else
                {
                    result = productService.GetCustomized(p => p.Quantity < p.ReorderQuantity && (p.Name.Contains(search) || p.Category.Name.Contains(search)), start, length, out totalCount, out filteredCount, "Category");
                }
            }
            else
            {
                if (string.IsNullOrEmpty(search))
                {
                    result = productService.GetCustomized(p => p.Quantity < p.ReorderQuantity && p.CategoryId == filterbycat, start, length, out totalCount, out filteredCount, "Category");
                }
                else
                {
                    result = productService.GetCustomized(p => p.Quantity < p.ReorderQuantity && p.CategoryId == filterbycat && (p.Name.Contains(search) || p.Category.Name.Contains(search)), start, length, out totalCount, out filteredCount, "Category");
                }
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

        [HttpGet]
        public IActionResult Create()
        {
            IEnumerable<SelectListItem> catList = categoryService.Get().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString(),
            });
            ViewBag.catList = catList;
            return View();
        }
        [HttpPost]
        public IActionResult Create(ProductDTO product)
        {
            if (!ModelState.IsValid)
            {
           
                return View(product);
            }
            var result = productService.Create(product);
            if (result)
            {
                TempData["success"] = "Product added successfully!";
                return RedirectToAction("Index");
            }
            else TempData["error"] = "Could not added. Server error!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("Product/ImageUpload/{id}")]
        public IActionResult ImageUpload(int id, IFormFile file)
        {
            if (id == 0 || file == null) return Json(new { success = false, msg = "No file provided or invalid product!" }) ;
            var data = productService.Get(i => i.Id == id);
            if (data == null) return Json(new { success = false, msg = "Invalid product!" });
            string wwwRootPath = webHostEnvironment.WebRootPath;
            string imagePath = Path.Combine(wwwRootPath, @"uploads\images\products");
            string imageName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            using (var fileStream = new FileStream(Path.Combine(imagePath, imageName), FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
            productService.UploadImage(id, @"\uploads\images\products\" + imageName);
            return Json(new { success = true, msg = "Image uploaded!"});
        }

        [HttpDelete]
        // [Route("Item/ImageUpload/{id}")]
        public IActionResult DeleteImage(int id)
        {
            var imageUrl = productService.DeleteImage(id);
            var imagePath = Path.Combine(webHostEnvironment.WebRootPath, imageUrl.TrimStart('\\')); // remove leading backslash
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }
            return Json(new { success = true, msg = "Image deleted!" });
        }
    }
}
