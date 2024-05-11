using Domain.ValidationRules.Properties;
using Domain.ValidationRules.Properties.Drink;
using FluentValidation;
using REST.DTO;
using REST.Validations.Property;

namespace REST.Validations.Models
{
	public class UpdateDrinkRequestValidator : AbstractValidator<UpdateDrinkRequest>
	{
		public UpdateDrinkRequestValidator()
		{
			RuleFor(model => model.DrinkId).NaturalNumber();

			RuleFor(model => model.Title).DrinkTitle().When(model => model.Title is not null);

			RuleFor(model => model.Image).DrinkCover().When(model => model.Image is not null);

			RuleFor(model => model.Cost).DrinkCost().When(model => model.Cost is not null);

			RuleFor(model => model.Count).NaturalNumber().When(model => model.Count is not null);

		}
	}
}
