using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting.WindowsServices;
using Microsoft.Extensions.Logging;
using ContosoTravel.Web.Functions.ServiceBus;
using Microsoft.Azure.WebJobs;
using ContosoTravel.Web.Function.EventGrid;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;

namespace ContosoTravel.Web.Host.Service
{
    class Program
    {
        static async Task Main(string[] args)
        {
            bool webJobs = false;
            if (webJobs)
            {
                var sbAssem = PurchaseItineraryServiceBus._thisAssembly;
                bool isService = true;
                IHostBuilder builder = new HostBuilder();
                builder.ConfigureWebJobs(b =>
                {
                    b.AddAzureStorageCoreServices();
                    b.AddExecutionContextBinding();
                    b.AddServiceBus(config =>
                    {
                        config.MessageHandlerOptions.AutoComplete = true;
                        config.MessageHandlerOptions.MaxConcurrentCalls = 5;
                    });
                });

                builder.ConfigureLogging((context, b) =>
                {
                    b.AddConsole();
                    b.SetMinimumLevel(LogLevel.Trace);
                });

                if (isService)
                {
                    builder = builder.UseServiceBaseLifetime();
                }

                var host = builder.Build();
                using (host)
                {
                    await host.RunAsync();
                }
            }
            else
            {
                IWebHostBuilder webBuilder = CreateWebHostBuilder(args);
                webBuilder.Build().Run();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
           .ConfigureLogging((hostingContext, logging) =>
           {
               logging.AddConsole();
               logging.AddDebug();
           })
            .Configure(appBuilder =>
            {
                appBuilder.UseMvc(routes =>
                {
                    routes.MapRoute(
                        name: "default",
                        template: "{controller=Flights}/{action=Index}/{id?}");
                });
            })
            .ConfigureServices((context, servicesCollection) =>
            {
                servicesCollection.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
                servicesCollection.AddHttpContextAccessor();
                servicesCollection.AddLogging();
            })
           .UseApplicationInsights();
    }
}
