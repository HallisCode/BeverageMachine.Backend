using FluentValidation;
using REST.DTO;

namespace REST.Validations.Models
{
	public class LockUnlockCoinRequestValidator : AbstractValidator<RubleCoinDTO>
	{
		public LockUnlockCoinRequestValidator()
		{
			RuleFor(model => model).SetValidator(new RubleCoinDtoValidator());
		}
	}
}
