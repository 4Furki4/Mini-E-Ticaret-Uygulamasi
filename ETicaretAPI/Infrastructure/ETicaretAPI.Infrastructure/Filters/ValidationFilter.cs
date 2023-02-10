using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Infrastructure.Filters
{
    public class ValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if(!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(err => err.Value.Errors.Any())
                    .ToDictionary(err => err.Key, err => err.Value.Errors.Select(r => r.ErrorMessage))
                    .ToArray();
                context.Result = new BadRequestObjectResult(errors);
                return;
            }
            await next();
        }
    }
}
