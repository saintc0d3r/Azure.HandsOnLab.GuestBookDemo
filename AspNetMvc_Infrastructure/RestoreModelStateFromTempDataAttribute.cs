using System.Web.Mvc;

namespace AspNetMvc_Infrastructure
{
    public class RestoreModelStateFromTempDataAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            if (filterContext.Controller.TempData.ContainsKey("ModelState"))
            {
                filterContext.Controller.ViewData.ModelState.Merge(filterContext.Controller.TempData["ModelState"] as ModelStateDictionary);
            }
        }
    }
}