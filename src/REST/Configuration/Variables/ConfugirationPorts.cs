using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;

namespace REST.Configuration.Variables
{
	public static class ConfugirationPorts
	{
		public static void ConfigurePorts(this WebApplicationBuilder builder)
		{
			string urls = Environment.GetEnvironmentVariable("URLS") ?? null;

			urls = urls ?? builder.Configuration[$"URLS"];

			if (urls is not null)
			{
				builder.WebHost.UseUrls(urls);
			}
		}
	}
}
