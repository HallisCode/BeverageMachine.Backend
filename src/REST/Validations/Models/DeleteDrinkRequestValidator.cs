using Application.DTO.BeverageMaintenance;
using Domain.ValidationRules.Properties;
using FluentValidation;

namespace REST.Validations.Models
{
	public class DeleteDrinkRequestValidator : AbstractValidator<DeleteDrinkRequest>
	{
		public DeleteDrinkRequestValidator()
		{
			RuleFor(model => model.drinkID).NotEmpty().NaturalNumber();
		}
	}
}
