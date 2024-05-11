using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;

namespace REST.Validations.Property
{
	public static class DrinkCoverRule
	{
		public static IRuleBuilderOptionsConditions<T, IFormFile> DrinkCover<T>(this IRuleBuilder<T, IFormFile> ruleBuilder)
		{
			return ruleBuilder.Custom((file, context) =>
			{
				string[] allowedExtension = new string[] { ".png", ".jpg", ".svg" };

				FileInfo fileInfo = new FileInfo(file.FileName);


				if (file.Length > 16 * 1024 * 1024)
				{
					context.AddFailure("Размер изображения не должен превышать 16 мб.");
				}


				if (!allowedExtension.Contains(fileInfo.Extension))
				{
					context.AddFailure("Разрешены только расширения .jpg , .png, .svg для картинок.");
				}
			});
		}

	}
}
