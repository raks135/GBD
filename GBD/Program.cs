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
			CreateHostBuilder(args).Build().Run();

			//Read Configuration from appSettings
			var config = new ConfigurationBuilder()
					.AddJsonFile("appsettings.Development.json")
					.Build();

			//Initialize Logger
			Log.Logger = new LoggerConfiguration()
					.ReadFrom.Configuration(config)
					.CreateLogger();
			Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg));
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args).UseSerilog()
						.ConfigureWebHostDefaults(webBuilder =>
						{
							webBuilder.UseStartup<Startup>();
						});
	}
}
