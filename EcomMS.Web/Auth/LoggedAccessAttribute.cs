using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using EcomMS.BLL.DTOs;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace EcomMS.Web.Auth
{
    public class LoggedAccessAttribute : Attribute /*AuthorizeAttribute for Claims */, IAuthorizationFilter
    {
        public LoggedAccessAttribute()
        {
            
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var sessionData = context.HttpContext.Session.GetString("userType");
            if (string.IsNullOrEmpty(sessionData))
            {
                //context.Result = new ChallengeResult(CookieAuthenticationDefaults.AuthenticationScheme);
                //return;
                var ReturnUrl = context.HttpContext.Request.Path.Value;
                context.Result = new RedirectToActionResult("Login", "User", new { ReturnUrl });
                return;
            }
            //if (!string.IsNullOrEmpty(sessionData))
            //{
            //    context.Result = new RedirectToActionResult("AccessDenied", "User", null);
            //    //context.Result = new ForbidResult();
            //    //return;
            //}
            //var sessionObj = JsonConvert.DeserializeObject<UserCustomerDTO>(sessionData);
            //if (sessionObj == null && sessionObj.Type != "Customer")
            //{
            //    context.Result = new ForbidResult();
            //    return;
            //}
        }
    }
}

