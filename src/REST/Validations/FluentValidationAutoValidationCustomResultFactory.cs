
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using REST.DTO;
using REST.Exceptions;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Results;

namespace AspNet.Validation
{
	public class FluentValidationAutoValidationCustomResultFactory : IFluentValidationAutoValidationResultFactory
	{
		public IActionResult CreateActionResult(ActionExecutingContext context, ValidationProblemDetails validationProblemDetails)
		{
			ExceptionDTO exception = new ExceptionDTO(
				nameof(ValidationException),
				validationProblemDetails.Title,
				validationProblemDetails.Errors);

			return new BadRequestObjectResult(exception);
		}
	}
}
