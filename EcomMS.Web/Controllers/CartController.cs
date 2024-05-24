using Microsoft.AspNetCore.Mvc;

namespace EcomMS.Web.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
