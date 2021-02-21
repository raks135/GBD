using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Diagnostics;

namespace GBD
{
	public class Program
	{
		public static void Main(string[] args)
		{

			//Read Configuration from appSettings
			var config = new ConfigurationBuilder()
					.AddJsonFile("appsettings.Development.json")
					.Build();

			//Initialize Logger
			Log.Logger = new LoggerConfiguration()
					.ReadFrom.Configuration(config)
					.CreateLogger();
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args).UseSerilog()
						.ConfigureWebHostDefaults(webBuilder =>
						{
							webBuilder.UseStartup<Startup>();
						});
	}
}
