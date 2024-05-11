using Application.DTO.BeverageMaintenance;
using Domain.ValidationRules.Properties;
using FluentValidation;

namespace REST.Validations.Models
{
	public class ChangeNumberCoinsRequestValidator : AbstractValidator<ChangeNumberCoinsRequest>
	{
		public ChangeNumberCoinsRequestValidator()
		{
			RuleFor(model => model.NumberOneRuble).NaturalNumber().When(model => model.NumberOneRuble is not null);

			RuleFor(model => model.NumberTwoRuble).NaturalNumber().When(model => model.NumberTwoRuble is not null);

			RuleFor(model => model.NumberFiveRuble).NaturalNumber().When(model => model.NumberFiveRuble is not null);

			RuleFor(model => model.numberTenRuble).NaturalNumber().When(model => model.numberTenRuble is not null);
		}
	}
}
