using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Filters
{
    public class CustomActionFilter : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var controller = context.Controller as Controller;
                if (controller != null)
                {
                    context.Result = controller.View(context.ActionArguments.Values.FirstOrDefault());
                }
                return;
            }
            await next();
        }
    }
}
