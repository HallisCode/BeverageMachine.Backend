using LocalImages.Exceptions;
using LocalImages.ImagesService;
using LocalImages.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace REST.Configuration.Options
{
	public static class ConfugirationLocalImages
	{
		static private readonly string section = "Options:LocalImagesServiceOptions";

		/// <summary>
		/// Внедряет <see cref="LocalImagesServiceOptions"/> через <see cref="IOptions{TOptions}"/>для работы <see cref="LocalImagesService"/>.
		/// </summary>
		/// <param name="builder"></param>
		public static void ConfugireLocalImagesOptions(this WebApplicationBuilder builder)
		{
			List<string> nullVariableNames = new List<string>();


			string directoryPath = builder.Configuration[$"{section}:DirectoryPath"];

			if (string.IsNullOrWhiteSpace(directoryPath)) nullVariableNames.Add(nameof(directoryPath));


			if (nullVariableNames.Any())
			{
				throw new LocalImagesOptionsConfugeredException($"Переменные : {string.Join(", ", nullVariableNames)} для работы LocalImagesService не заданы.");
			}

			if (!Directory.Exists(directoryPath))
			{
				throw new LocalImagesOptionsConfugeredException($"Указанная папка {directoryPath} не существует.");
			}


			builder.Services.Configure<LocalImagesServiceOptions>(builder.Configuration.GetSection(section));

		}
	}
}
