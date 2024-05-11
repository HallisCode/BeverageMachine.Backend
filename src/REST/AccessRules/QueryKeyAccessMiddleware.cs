using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Options;
using REST.Exceptions;
using System.Reflection;
using System.Threading.Tasks;

namespace REST.AccessRules
{
	/// <summary>
	/// Выдаёт доступ к ендпоинтам с аттрибутом <see cref="QueryKeyAccessAttribute"/>. <br/>
	/// На основе проверки соответствия ключа из параметра запроса с ключом из конфига <see cref="QueryKeyOptions"/>.
	/// </summary>
	public class QueryKeyAccessMiddleware
	{
		protected readonly RequestDelegate _next;


		public QueryKeyAccessMiddleware(RequestDelegate next)
		{
			this._next = next;
		}

		public async Task InvokeAsync(HttpContext httpContext, IOptions<QueryKeyOptions> options)
		{
			Endpoint endpoint = httpContext.GetEndpoint();

			if (endpoint is not null && CheckNecessaryKey(endpoint))
			{
				string queryKey = httpContext.Request.Query[options.Value.QueryKeyName];

				if (queryKey != options.Value.Key || options.Value.Key is null)
				{
					throw new AccessDeniedException("Доступ к данному ресурсу запрещен.");
				}
			};

			await _next.Invoke(httpContext);
		}

		protected bool CheckNecessaryKey(Endpoint endpoint)
		{
			ControllerActionDescriptor endpointController = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();


			TypeInfo controllerType = endpointController.ControllerTypeInfo;

			MethodInfo actionType = endpointController.MethodInfo;


			bool isActionRequired = actionType.GetCustomAttribute(typeof(QueryKeyAccessAttribute), false)
				is null ? false : true;

			bool isControllerRequired = controllerType.GetCustomAttribute(typeof(QueryKeyAccessAttribute), false)
				is null ? false : true;


			return isActionRequired || isControllerRequired;
		}

	}

	public static class QueryKeyAccessMiddlewareExtension
	{
		public static void UseQueryKeyAccessMiddleware(this IApplicationBuilder app)
		{
			app.UseMiddleware<QueryKeyAccessMiddleware>();
		}
	}
}
