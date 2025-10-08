using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Common.Results.Errors;

namespace Restaurants.API.Controllers;

[ApiController]
[Produces("application/json")]
public class ApiController : ControllerBase
{
    protected ActionResult Problem(List<Error> errors)
    {
        if (errors.Count is 0)
            return Problem();

        if (errors.All(err => err.StatusCode == System.Net.HttpStatusCode.UnprocessableEntity))
            return ValidationProblem(errors);

        return Problem(errors[0]);
    }

    private ObjectResult Problem(Error error)
        => Problem(statusCode: (int)error.StatusCode, title: error.Description);

    private ActionResult ValidationProblem(List<Error> errors)
    {
        errors.ForEach(err => ModelState.AddModelError(err.Code, err.Description));

        return ValidationProblem(statusCode: StatusCodes.Status422UnprocessableEntity, modelStateDictionary: ModelState);

    }
}