using CarBank.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Net;

namespace CarBank.WebApi.Filters
{
    public class ActionValidationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {

            if (!context.ModelState.IsValid)
            {
                var errors = new List<string>();
                foreach (var state in context.ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                context.Result = new BadRequestObjectResult(new AuthenticationResponse<List<string>>() { StatusCode = HttpStatusCode.BadRequest,Value = errors });
            }
        }
    }
}
