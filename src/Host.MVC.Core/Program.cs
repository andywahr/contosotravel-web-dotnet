using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Globalization;
using Microsoft.Extensions.Logging;

namespace ContosoTravel.Web.Host.MVC.Core
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .ConfigureServices(services => services.AddAutofac())
                   .ConfigureLogging((hostingContext, logging) =>
                   {
                       logging.AddConsole();
                       logging.AddDebug();
                   })
                   .UseApplicationInsights()
                   .UseStartup<Startup>();
    }
}
