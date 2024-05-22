using Domain.ValidationRules.Properties;
using FluentValidation;

namespace REST.Validations.Models
{
	public class DrinkOrderRequestValidator : AbstractValidator<DTO.DrinkOrderRequest>
	{

		public DrinkOrderRequestValidator()
		{
			RuleFor(model => model.DrinkId).NotEmpty().NaturalNumber();

			RuleFor(model => model.Coins).NotEmpty().ForEach(coin => coin.SetValidator(new RubleCoinDtoValidator()));
		}
	}
}
