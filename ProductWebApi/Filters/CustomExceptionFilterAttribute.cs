using Microsoft.AspNetCore.Mvc.Filters;

namespace ProductWebApi.Filters
{
    public class CustomExceptionFilterAttribute: ExceptionFilterAttribute
    {

        public override Task OnExceptionAsync(ExceptionContext context)
        {
            if(context.Exception.GetType() ==typeof(NullReferenceException))
            {

            }

            if (context.Exception.GetType() == typeof(ArgumentException))
            {

            }

            if (context.Exception.GetType() == typeof(NullReferenceException))
            {
                
            }

             return base.OnExceptionAsync(context);
        }
    }
}
