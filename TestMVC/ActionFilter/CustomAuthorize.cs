using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestMVC.ActionFilter
{
    public class CustomAuthorize : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if(context.HttpContext.Session.GetString("UserName") != "admin")
            {
                context.Result = new UnauthorizedResult();
                context.HttpContext.Session.SetString("Unauthorize", "<script>alert('You are not authorised to do that');</script>");
                var path = context.HttpContext.Request.Path;
                var x = path.Value.Split('/');
                var values = new RouteValueDictionary(new
                {
                    action = "Index",
                    controller = x[1]
                });                
                context.Result = new RedirectToRouteResult(values);                
            }
        }
    }
}
