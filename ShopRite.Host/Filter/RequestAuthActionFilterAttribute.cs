using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ShopRite.Host.Extensions;

namespace ShopRite.Host.Filter
{
    public class RequestAuthActionFilterAttribute : IActionFilter
    {

        #region This method will be executed after an action method has executed
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
        #endregion

        #region This method will be executed before execution of an action method 
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState.GetApiResponse());
                return;
            }
        } 
        #endregion
    }
}
