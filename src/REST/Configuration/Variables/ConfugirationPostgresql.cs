using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;

namespace REST.Configuration.Variables
{
	public static class ConfigurationPostgresql
	{
		/// <summary>
		/// Формирует строку подключения к Postgresql из переменных окружения. <br/>
		/// В случае если они пустые - подтягивает настройки с файла конфигурации.
		/// </summary>
		/// <exception cref="PostgresqlConfiguredException"></exception>
		/// <param name="builder"></param>
		/// <returns></returns>
		public static string GetPostgrsqlConnectionString(this WebApplicationBuilder builder)
		{
			string username, password, port, database, host;


			string postgresql = "Connections:Postgresql";

			username = builder.Configuration[$"{postgresql}:Username"];

			password = builder.Configuration[$"{postgresql}:Password"];

			port = builder.Configuration[$"{postgresql}:Port"];

			database = builder.Configuration[$"{postgresql}:Database"];

			host = builder.Configuration[$"{postgresql}:Host"];


			if (string.IsNullOrWhiteSpace(username)) username = Environment.GetEnvironmentVariable("POSTGRES_USER");

			if (string.IsNullOrWhiteSpace(password)) password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");

			if (string.IsNullOrWhiteSpace(database)) database = Environment.GetEnvironmentVariable("POSTGRES_DATABASE");

			if (string.IsNullOrWhiteSpace(host)) host = Environment.GetEnvironmentVariable("POSTGRES_HOST");

			if (string.IsNullOrWhiteSpace(port)) port = Environment.GetEnvironmentVariable("POSTGRES_PORT");


			List<string> nullVariableNames = new List<string>();

			if (string.IsNullOrWhiteSpace(username)) nullVariableNames.Add(nameof(username));

			if (string.IsNullOrWhiteSpace(password)) nullVariableNames.Add(nameof(password));

			if (string.IsNullOrWhiteSpace(database)) nullVariableNames.Add(nameof(database));

			if (string.IsNullOrWhiteSpace(host)) nullVariableNames.Add(nameof(host));

			if (string.IsNullOrWhiteSpace(port)) nullVariableNames.Add(nameof(port));


			if (nullVariableNames.Any())
			{
				throw new PostgresqlConfiguredException($"Переменные : {string.Join(", ", nullVariableNames)} для подключения к Postgresql не заданы.");
			}


			return $"Server={host};Port={port};Database={database};Username={username};Password={password}";
		}

		public class PostgresqlConfiguredException : Exception
		{
			public PostgresqlConfiguredException(string message) : base(message)
			{
			}

			public PostgresqlConfiguredException(string message, Exception innerException) : base(message, innerException)
			{
			}
		}
	}
}
