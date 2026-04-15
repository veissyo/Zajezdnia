using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Zajezdnia.Filters;

public class UserContextFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.Controller is not Controller ctrl) return;

        bool zalogowany = context.HttpContext.Items["IsLoggedIn"] is true;
        ctrl.ViewBag.IsLoggedIn   = zalogowany;
        ctrl.ViewBag.UserLogin    = context.HttpContext.Items["UserLogin"]?.ToString();
        ctrl.ViewBag.UserImie     = context.HttpContext.Items["UserImie"]?.ToString();
        ctrl.ViewBag.UserNazwisko = context.HttpContext.Items["UserNazwisko"]?.ToString();
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class WymagaZalogowaniaAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.HttpContext.Items["IsLoggedIn"] is not true)
        {
            var returnUrl = context.HttpContext.Request.Path;
            context.Result = new RedirectToActionResult("Zaloguj", "Account",
                new { returnUrl });
        }
    }
}