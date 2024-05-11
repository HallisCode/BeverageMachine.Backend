using FluentValidation;


namespace Domain.ValidationRules.Properties
{
	public static class NaturaNumberRule
	{
		public static IRuleBuilderOptionsConditions<T, int> NaturalNumber<T>(this IRuleBuilder<T, int> ruleBuilder)
		{
			return ruleBuilder.Custom((cost, context) =>
			{

				if (cost < 0)
				{
					context.AddFailure("Число не может быть отрицательным.");
				}
				else if (cost > int.MaxValue)
				{
					context.AddFailure($"Число превышает максимально допустимое значение \"{int.MaxValue}\".");
				}
			});
		}

		public static IRuleBuilderOptionsConditions<T, long> NaturalNumber<T>(this IRuleBuilder<T, long> ruleBuilder)
		{
			return ruleBuilder.Custom((cost, context) =>
			{
				if (cost < 0)
				{
					context.AddFailure("Число не может быть отрицательным.");
				}
				else if (cost > int.MaxValue)
				{
					context.AddFailure($"Число превышает максимально допустимое значение \"{long.MaxValue}\".");
				}
			});
		}


		public static IRuleBuilderOptionsConditions<T, long?> NaturalNumber<T>(this IRuleBuilder<T, long?> ruleBuilder)
		{
			return ruleBuilder.Custom((cost, context) =>
			{
				if (cost is null) return;

				if (cost < 0)
				{
					context.AddFailure("Число не может быть отрицательным.");
				}
				else if (cost > int.MaxValue)
				{
					context.AddFailure($"Число превышает максимально допустимое значение \"{long.MaxValue}\".");
				}
			});
		}

		public static IRuleBuilderOptionsConditions<T, int?> NaturalNumber<T>(this IRuleBuilder<T, int?> ruleBuilder)
		{
			return ruleBuilder.Custom((cost, context) =>
			{
				if (cost is null) return;

				if (cost < 0)
				{
					context.AddFailure("Число не может быть отрицательным.");
				}
				else if (cost > int.MaxValue)
				{
					context.AddFailure($"Число превышает максимально допустимое значение \"{int.MaxValue}\".");
				}
			});
		}
	}
}
