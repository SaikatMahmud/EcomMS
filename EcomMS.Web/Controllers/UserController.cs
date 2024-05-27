using EcomMS.BLL.DTOs;
using EcomMS.BLL.ServiceAccess;
using EcomMS.BLL.Services;
using EcomMS.BLL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace EcomMS.Web.Controllers
{
    public class UserController : Controller
    {
        private UserService userService;

        public UserController(IBusinessService serviceAccess)
        {
            userService = serviceAccess.UserService;
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(CustomerRegistrationVM cr)
        {
            string isEmailUnique = string.Empty;
            string isUsernameUnique = string.Empty;
            if(!ModelState.IsValid)
            {
                return View(cr);
            }
            var customerDTO = new CustomerDTO()
            {
                Name = cr.Name,
                Email = cr.Email,
                Mobile = cr.Mobile,
                Address = "No address yet!"
            };
            var result = userService.RegisterCustomer(customerDTO, cr.Username, cr.Password, out isEmailUnique, out isUsernameUnique);
            if (!result)
            {

            }
            return View();
        }

    }
}
