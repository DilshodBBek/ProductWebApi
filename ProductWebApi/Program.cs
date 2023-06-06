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
        builder.Services.AddMemoryCache();
        builder.Services.AddLazyCache();
        builder.Services.AddControllers();

        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = builder.Configuration.GetConnectionString("RedisDB");
        });
        IConfiguration configuration = builder.Configuration;

        builder.Services.AddInfrastructure(configuration);
        builder.Services.AddApplication(configuration);
        //builder.Services.AddResponseCaching();
        builder.Services.AddOutputCache();

        var app = builder.Build();
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        app.UseSerilogRequestLogging();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            c.DisplayRequestDuration()
            );
        }
        app.UseFileServer();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        // app.UseResponseCaching();
        app.UseOutputCache();
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