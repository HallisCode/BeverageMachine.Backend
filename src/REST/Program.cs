
using Application.IServices;
using Application.IServices.External;
using Application.Services;
using AspNet.Validation;
using Database.IUnitWork;
using FluentValidation;
using LocalImages.ImagesService;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Postgresql;
using Postgresql.UnitWork;
using REST.AccessRules;
using REST.Configuration.Options;
using REST.Configuration.Variables;
using REST.Middlewares;
using REST.Validations.Models;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace REST
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			//________________________
			// Конфигурация сервера
			//________________________

			builder.ConfigurePorts();


			//________________________
			// Конфигурация сервисов
			//________________________

			// Настраивает FluentValidation так, что при первой неудачной проверки в RuleFor(...) =>
			// возвращает ошибку валидации для текущего свойства и переходит к следующему свойству.
			ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Stop;


			//________________________
			// Конфигурация переменных
			//________________________

			string postgresql = builder.GetPostgrsqlConnectionString();


			//_____________________________
			// Подключение сервисов
			//_____________________________ 

			// Сервисы фреймворка
			builder.Services.AddControllers();
			builder.Services.AddSwaggerGen();

			// CORS 
			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AbsolutleFree", builder =>
				{
					builder.AllowAnyHeader();

					builder.AllowAnyMethod();

					builder.AllowAnyOrigin();
				});
			});

			// Сервисы логики
			builder.Services.AddScoped<IBeverageInteractionService, BeverageInteractionService>();
			builder.Services.AddScoped<IBeverageMaintetanceService, BeverageMaintetanceService>();

			builder.Services.AddScoped<IImagesService, LocalImagesService>();


			//_____________________________
			// Подключение Options
			//_____________________________ 

			// Настройки для сервиса IImagesService
			builder.ConfugireLocalImagesOptions();

			// Настройки для QueryKeyAccessMiddleware
			builder.ConfugireQueryKeyAccessOptions();


			//_____________________________
			// Подключение IDatabase Access с имплементацией Postgresql
			//_____________________________ 

			builder.Services.AddDbContext<ApplicationDbContext>(
				options => options.UseNpgsql(postgresql), ServiceLifetime.Scoped
			);
			builder.Services.AddScoped<IUnitWork, UnitWork>();


			//_____________________________
			// Подключение библиотек
			//_____________________________ 

			// Библиотека FluentValidations ( внедрение всех валидаторов через IValidator<T>
			builder.Services.AddValidatorsFromAssemblyContaining<DrinkOrderRequestValidator>();

			// Библиотека SharpGrip.FluentValidation.AutoValidation.Mvc
			// (не работает без внедрения классов валидаторов IValidator<T>).
			// Настраивает автоматическую валидацию входящих запросов через пайплайн, благодаря настроенным классам валидаторам.
			builder.Services.AddFluentValidationAutoValidation(configuration =>
			{
				configuration.DisableBuiltInModelValidation = true;
				configuration.EnableFormBindingSourceAutomaticValidation = true;
				configuration.OverrideDefaultResultFactoryWith<FluentValidationAutoValidationCustomResultFactory>();
			});


			var app = builder.Build();

			//_____________________________
			// Дополнительная конфигурация в зависимости от среды окружения
			//_____________________________ 


			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			// EntityFrameworkCore производит построение таблиц.
			if (app.Environment.IsProduction())
			{
				using (var scope = app.Services.CreateScope())
				{
					var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

					db.Database.EnsureCreated();
				}
			}


			//________________________________
			// Подключение middlewares
			//________________________________

			app.UseHttpsRedirection();

			app.UseCors("AbsolutleFree");

			app.UseGlobalHandleExceptionsMiddleware();

			app.UseQueryKeyAccessMiddleware();

			app.MapControllers();

			// Запуск проекта
			app.Run();
		}
	}
}