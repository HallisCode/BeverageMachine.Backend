using Application.DTO.BeverageMaintenance;
using Domain.ValidationRules.Properties;
using FluentValidation;

namespace REST.Validations.Models
{
	public class LockUnlockDrinkRequestValidator : AbstractValidator<LockUnlockDrinkRequest>
	{
		public LockUnlockDrinkRequestValidator()
		{
			RuleFor(model => model.drinkID).NotEmpty().NaturalNumber();
		}
	}
}
