using Chatison.Helpers;
using System.Web.Mvc;

namespace Chatison.Areas.Admin.Controllers
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            ViewBag.Area = "admin";
            ViewBag.BaseUrl = filterContext.HttpContext.Request.GetBaseUrl();
        }
    }
}