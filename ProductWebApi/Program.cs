using Application;
using Infrastructure;
namespace ProductWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            IConfiguration configuration = builder.Configuration;

            builder.Services.AddInfrastructure(configuration);
            builder.Services.AddApplication(configuration);


            var app = builder.Build();

            app.MapControllers();
            app.UseFileServer();
            app.UseRouting();
            app.UseStaticFiles();

            app.Run();
        }
    }
}