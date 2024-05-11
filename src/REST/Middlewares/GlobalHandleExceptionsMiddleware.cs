using Domain.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using REST.DTO;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace REST.Middlewares
{
	public class GlobalHandleExceptionsMiddleware
	{
		private readonly RequestDelegate next;

		public GlobalHandleExceptionsMiddleware(RequestDelegate next)
		{
			this.next = next;
		}

		public async Task InvokeAsync(HttpContext httpContext)
		{
			try
			{
				await next.Invoke(httpContext);
			}
			catch (Exceptions.ValidationException exception)
			{
				httpContext.Response.StatusCode = 400;

				ExceptionDTO exceptionDTO = new ExceptionDTO(exception.GetType().Name, exception.Message, exception.Details);

				await httpContext.Response.WriteAsJsonAsync(exceptionDTO);

				return;
			}
			catch (Exceptions.AccessDeniedException exception)
			{
				httpContext.Response.StatusCode = 403;

				ExceptionDTO exceptionDTO = new ExceptionDTO(exception.GetType().Name, exception.Message, exception.Details);

				await httpContext.Response.WriteAsJsonAsync(exceptionDTO);

				return;
			}
			catch (DetailedException exception)
			{
				httpContext.Response.StatusCode = 400;

				ExceptionDTO exceptionDTO = new ExceptionDTO(exception.GetType().Name, exception.Message, exception.Details);

				await httpContext.Response.WriteAsJsonAsync(exceptionDTO);

				return;
			}
			catch (Exception exception)
			{
				Type type = exception.GetType();

				httpContext.Response.StatusCode = 500;

				ExceptionDTO exceptionDTO = new ExceptionDTO("UnhandledError", exception.Message, exception.StackTrace);

				await httpContext.Response.WriteAsync(JsonSerializer.Serialize(exceptionDTO));
			}
		}
	}

	public static class GlobalHandleExceptionsMiddlewareExtension
	{
		public static void UseGlobalHandleExceptionsMiddleware(this IApplicationBuilder app)
		{
			app.UseMiddleware<GlobalHandleExceptionsMiddleware>();
		}
	}
}
