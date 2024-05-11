using FluentValidation;
using System.Linq;

namespace Domain.ValidationRules.Properties
{
	public static class RubleCoinRule
	{
		public static IRuleBuilderOptionsConditions<T, int> RubleCoin<T>(this IRuleBuilder<T, int> ruleBuilder)
		{
			return ruleBuilder.Custom((coin, context) =>
			{
				bool isAllowed = ValueObjects.RubleCoin.AllSupportedCoins.Where(ruble => ruble.Value == coin).Count() > 0;

				if (!isAllowed)
				{
					context.AddFailure($"Монеты наминалом \"{coin}\" не принимаются.");
				}
			});
		}

		public static IRuleBuilderOptionsConditions<T, int[]> RubleCoin<T>(this IRuleBuilder<T, int[]> ruleBuilder)
		{
			return ruleBuilder.Custom((coins, context) =>
			{
				foreach (int coin in coins)
				{
					bool isAllowed = ValueObjects.RubleCoin.AllSupportedCoins.Where(ruble => ruble.Value == coin).Count() > 0;


					if (!isAllowed)
					{
						context.AddFailure($"Монеты наминалом \"{coin}\" не принимаются.");
					}
				}
			});
		}
	}
}
