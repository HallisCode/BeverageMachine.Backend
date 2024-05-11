using FluentValidation;
using System.Text.RegularExpressions;


namespace Domain.ValidationRules.Properties.Drink
{
	public static class DrinkTitleRule
	{
		private static Regex allowedSymbols = new Regex("^[a-zA-Zа-яА-ЯёЁ0-9 ]+$");

		public static IRuleBuilderOptionsConditions<T, string> DrinkTitle<T>(this IRuleBuilder<T, string> ruleBuilder)
		{
			return ruleBuilder.Custom((title, context) =>
			{

				if (title.Length > 32)
				{
					context.AddFailure("Длина не может быть больше 32 символов.");
				}

				if (!allowedSymbols.IsMatch(title))
				{
					context.AddFailure("Может содержать только цифры, буквы русского и латинского алфавита.");
				}
			});
		}
	}
}
