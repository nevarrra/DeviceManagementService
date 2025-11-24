using DeviceManagementService.Api.Middleware;
using DeviceManagementService.Application.Behaviors;
using DeviceManagementService.Application.Commands;
using DeviceManagementService.Application.Options;
using DeviceManagementService.Infrastructure.Abstractions;
using DeviceManagementService.Infrastructure.Data;
using DeviceManagementService.Infrastructure.Data.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DeviceManagementService.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddOpenApi();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DevicesDb")));

            builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();

            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateDeviceCommand>());

            builder.Services.Configure<DeviceValidationOptions>(builder.Configuration.GetSection("Device"));

            builder.Services.AddValidatorsFromAssemblyContaining<CreateDeviceCommand>();

            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            var app = builder.Build();

            // Seed DB
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.Database.Migrate();
            }

            app.UseGlobalExceptionHandler();

            app.MapOpenApi();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/openapi/v1.json", "Device Management API");
            });
            

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
