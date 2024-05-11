using Domain.ValidationRules.Properties;
using Domain.ValidationRules.Properties.Drink;
using FluentValidation;
using REST.DTO;
using REST.Validations.Property;

namespace REST.Validations.Models
{
	public class AddDrinkRequestValidator : AbstractValidator<AddDrinkRequest>
	{
		public AddDrinkRequestValidator()
		{
			RuleFor(model => model.Title).NotEmpty().DrinkTitle();

			RuleFor(model => model.Image).NotEmpty().DrinkCover();

			RuleFor(model => model.Cost).DrinkCost();

			RuleFor(model => model.Count).NaturalNumber();
		}
	}
}
