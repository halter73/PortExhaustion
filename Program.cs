using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace PortExhaustion
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            using var host = CreateHostBuilder(args).Build();
            await host.StartAsync();

            using var httpClient = new HttpClient();

            async Task MakeRequest()
            {
                try
                {
                    await httpClient.GetAsync("http://localhost:5000/");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception! {ex}");
                }
            }

            Console.WriteLine("Starting requests");

            // This should be enough to run out of ephemeral ports
            for (int i = 0; i < 65_535; i++)                            
            {
                _ = MakeRequest();
            }

            Console.WriteLine("Requests started");

            await host.WaitForShutdownAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
