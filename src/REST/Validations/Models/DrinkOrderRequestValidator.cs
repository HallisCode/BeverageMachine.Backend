using Domain.ValidationRules.Properties;
using FluentValidation;

namespace REST.Validations.Models
{
	public class DrinkOrderRequestValidator : AbstractValidator<DTO.DrinkOrderRequest>
	{

		public DrinkOrderRequestValidator()
		{
			RuleFor(model => model.DrinkId).NaturalNumber();

			RuleForEach(model => model.Coins).SetValidator(new RubleCoinDtoValidator());
		}
	}
}
