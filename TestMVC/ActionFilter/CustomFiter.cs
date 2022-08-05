using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace TestMVC.ActionFilter
{
    public class CustomFiter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {        
            if(context.HttpContext.Session.Keys.Count() == 0 || (context.HttpContext.Session.Keys.Count() > 0 && context.HttpContext.Session.GetString("UserName").Length == 0))
            {
                var values = new RouteValueDictionary(new
                {
                    action = "Index",
                    controller = "Login"
                });
                context.Result = new RedirectToRouteResult(values);
            }

            //base.OnActionExecuting(context);
        }
    }
}
