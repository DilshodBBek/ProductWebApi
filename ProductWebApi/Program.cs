using Application;
using Infrastructure;
using ProductWebApi.ExceptionHandler;
using Serilog;

namespace ProductWebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();

        //try
        //{
            Log.Information("Application started!");

            builder.Host.UseSerilog();

            builder.Services.AddControllers();
            IConfiguration configuration = builder.Configuration;

            builder.Services.AddInfrastructure(configuration);
            builder.Services.AddApplication(configuration);

            var app = builder.Build();

            app.UseSerilogRequestLogging();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseFileServer();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCustomHandler();

            app.MapControllers();
            app.Run();
        //}
        //catch (Exception ex)
        //{
        //    Log.Fatal(ex, "Catch block");
        //}
        //finally
        //{
        //    Log.CloseAndFlush();
        //}

    }
}