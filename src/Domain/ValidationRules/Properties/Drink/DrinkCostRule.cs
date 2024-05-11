using FluentValidation;

namespace Domain.ValidationRules.Properties.Drink
{
	public static class DrinkCostRule
	{
		public static IRuleBuilderOptionsConditions<T, int> DrinkCost<T>(this IRuleBuilder<T, int> ruleBuilder)
		{
			return ruleBuilder.Custom((cost, context) =>
			{

				if (cost < 1)
				{
					context.AddFailure("Стоимость не может быть меньше или ровна 0.");
				}
				else if (cost > int.MaxValue)
				{
					context.AddFailure($"Стоимость превышает максимально допустимое значение \"{int.MaxValue}\".");
				}
			});
		}

		public static IRuleBuilderOptionsConditions<T, int?> DrinkCost<T>(this IRuleBuilder<T, int?> ruleBuilder)
		{
			return ruleBuilder.Custom((cost, context) =>
			{
				if (cost is null) return;

				if (cost < 1)
				{
					context.AddFailure("Стоимость не может быть меньше или ровна 0.");
				}
				else if (cost > int.MaxValue)
				{
					context.AddFailure($"Стоимость превышает максимально допустимое значение \"{int.MaxValue}\".");
				}
			});
		}
	}
}
