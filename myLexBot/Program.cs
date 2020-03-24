using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace myLexBot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {

                webBuilder.ConfigureKestrel(o =>
                {
                    o.ListenLocalhost(5000);
                })
                .UseStartup<Startup>();
            });
    }
}
