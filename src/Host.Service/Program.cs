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

namespace ContosoTravel.Web.Host.Service
{
    class Program
    {
        static async Task Main(string[] args)
        {

            var builder = new HostBuilder();
            builder.ConfigureWebJobs(b =>
            {
                b.AddServiceBus();
            });
            builder.ConfigureLogging((context, b) =>
            {
                b.AddConsole();
            });
            var host = builder.Build();
            using (host)
            {
                host.Run();
            }
        }
    }

}
