using EcomMS.BLL.DTOs;
using EcomMS.BLL.ServiceAccess;
using EcomMS.BLL.Services;
using EcomMS.BLL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Newtonsoft.Json;
using DocumentFormat.OpenXml.Math;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

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
        [HttpPost]
        public IActionResult Login(LoginVM obj, string? ReturnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(obj);
            }
            var user = userService.LoginCustomer(obj.Username, obj.Password);
            if (user == null)
            {
                TempData["error"] = "Invalid username or password!";
                return View(obj);
            }
            TempData["success"] = "Login successfully";
            var sessionObj = JsonConvert.SerializeObject(user);
            HttpContext.Session.SetString("userLoginDetails", sessionObj);
            if (!string.IsNullOrEmpty(ReturnUrl))
            {
                return Redirect(ReturnUrl);
            }
            return RedirectToAction("Index", "Home");
            //await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,);

        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("userLoginDetails");
            TempData["success"] = "You have been logged out";
            return RedirectToAction("Index", "Home");
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
            if (!ModelState.IsValid)
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
                if (!string.IsNullOrEmpty(isEmailUnique))
                {
                    ModelState.AddModelError("Email", isEmailUnique);
                }
                if (!string.IsNullOrEmpty(isUsernameUnique))
                {
                    ModelState.AddModelError("Username", isUsernameUnique);
                }
                return View(cr);
            }
            return RedirectToAction("Login");
        }

    }
}
