using Domain.ValidationRules.Properties;
using FluentValidation;
using REST.DTO;

namespace REST.Validations.Models
{
	public class RubleCoinDtoValidator : AbstractValidator<RubleCoinDTO>
	{
		public RubleCoinDtoValidator()
		{
			RuleFor(model => model.Value).RubleCoin();
		}
	}
}
