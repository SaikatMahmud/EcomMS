using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using EcomMS.BLL.DTOs;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace EcomMS.Web.Auth
{
    public class CustomerAccessAttribute : Attribute /*AuthorizeAttribute for Claims */, IAuthorizationFilter
    {
        public CustomerAccessAttribute()
        {
            
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var sessionData = context.HttpContext.Session.GetString("userLoginDetails");
            if(string.IsNullOrEmpty(sessionData))
            {
                //context.Result = new ChallengeResult(CookieAuthenticationDefaults.AuthenticationScheme);
                //return;
                var ReturnUrl = context.HttpContext.Request.Path.Value;
                context.Result = new RedirectToActionResult("Login", "User", new { ReturnUrl });
                return;
            }
            var sessionObj = JsonConvert.DeserializeObject<UserCustomerDTO>(sessionData);
            if (sessionObj == null && sessionObj.Type != "Customer")
            {
                context.Result = new ForbidResult();
                return;
            }
        }
    }
}











//public class MultiRoleAccessAttribute : AuthorizeAttribute, IAuthorizationFilter
//{
//    private readonly string[] _roles;

//    public MultiRoleAccessAttribute(params string[] roles)
//    {
//        _roles = roles;
//    }

//    public void OnAuthorization(AuthorizationFilterContext context)
//    {
//        var userRoles = GetUserRoles(context.HttpContext); // implement this method to get user roles

//        if (_roles.Any(role => userRoles.Contains(role)))
//        {
//            // user has at least one of the required roles, allow access
//            return;
//        }

//        context.Result = new ForbidResult();
//    }

//    private IEnumerable<string> GetUserRoles(HttpContext httpContext)
//    {
//        // implement this method to get user roles from session, database, or other storage
//    }
//}