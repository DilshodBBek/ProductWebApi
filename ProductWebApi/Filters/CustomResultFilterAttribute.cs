using Microsoft.AspNetCore.Mvc.Filters;

namespace ProductWebApi.Filters
{
    public class CustomResultFilterAttribute :Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            Microsoft.Extensions.Primitives.StringValues auth = context.HttpContext.Request.Headers.Authorization;
            
        }
    }
}
