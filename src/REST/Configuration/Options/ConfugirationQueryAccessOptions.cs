using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using REST.AccessRules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace REST.Configuration.Options
{
	public static class ConfugirationQueryAccessOptions
	{
		static private readonly string section = "Options:QueryKeyAccessOptions";

		/// <summary>
		/// Внедряет <see cref="QueryKeyOptions"/> через <see cref="IOptions{TOptions}"/>для работы <see cref="LocalImagesService"/>.
		/// </summary>
		/// <param name="builder"></param>
		public static void ConfugireQueryKeyAccessOptions(this WebApplicationBuilder builder)
		{
			List<string> nullVariableNames = new List<string>();


			string queryKeyName = builder.Configuration[$"{section}:QueryKeyName"];

			string key = builder.Configuration[$"{section}:Key"];


			if (string.IsNullOrWhiteSpace(queryKeyName)) nullVariableNames.Add(nameof(queryKeyName));

			if (string.IsNullOrWhiteSpace(key)) nullVariableNames.Add(nameof(key));


			if (nullVariableNames.Any())
			{
				throw new QueryKeyOptionsConfugeredException($"Переменные : {string.Join(", ", nullVariableNames)} для работы QuerykeyAccess не заданы.");
			}

			builder.Services.Configure<QueryKeyOptions>(builder.Configuration.GetSection(section));

		}

		public class QueryKeyOptionsConfugeredException : Exception
		{
			public QueryKeyOptionsConfugeredException(string message) : base(message)
			{
			}

			public QueryKeyOptionsConfugeredException(string message, Exception innerException) : base(message, innerException)
			{
			}
		}
	}
}

