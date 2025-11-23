using DeviceManagementService.Application.Commands;
using DeviceManagementService.Application.Options;
using MediatR;

namespace DeviceManagementService.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateDeviceCommand>());

            builder.Services.Configure<DeviceValidationOptions>(builder.Configuration.GetSection("Device"));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
