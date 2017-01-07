using Microsoft.Owin.FileSystems;
using Microsoft.Owin.Hosting;
using Microsoft.Owin.StaticFiles;
using Owin;
using PluginAnalyser.Lib;
using System;
using System.Web.Http;

namespace PluginAnalyser.Console
{
	public class Startup
	{
		// This code configures Web API. The Startup class is specified as a type
		// parameter in the WebApp.Start method.
		public void Configuration(IAppBuilder appBuilder)
		{
			// Configure Web API for self-host. 
			HttpConfiguration config = new HttpConfiguration();
			config.Routes.MapHttpRoute(
				name: "DefaultApi",
				routeTemplate: "{controller}/{id}",
				defaults: new { id = RouteParameter.Optional }
			);


			appBuilder.UseFileServer(new FileServerOptions()
			{
				FileSystem = new PhysicalFileSystem("Web")
			});
			appBuilder.UseWebApi(config);
		}
	}

	public class ConsoleAnalyserConfig : AnalyserConfig
	{
		public string[] Files { get; set; }

		public static ConsoleAnalyserConfig FromFile(string path)
		{
			var json = System.IO.File.ReadAllText(path);
			return Newtonsoft.Json.JsonConvert.DeserializeObject<ConsoleAnalyserConfig>(json);
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			//var config = ConsoleAnalyserConfig.FromFile("config.json");
			//var analyser = new Analyser(config);

			//foreach (var file in config.Files)
			//{
			//	analyser.AddFile(file);
			//}

			//var results = analyser.Analyse();
			//System.Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(results, Newtonsoft.Json.Formatting.Indented));

			//var reporter = new HtmlReporter(results);
			//reporter.SaveTo("results.html");

			//System.Diagnostics.Process.Start("results.html");


			//System.Console.WriteLine(String.Join(",", args));
			string baseAddress = "http://localhost:2625/";
			if (args != null && args.Length > 0)
			{
				baseAddress = args[0];
			}

			// Start OWIN host 
			using (WebApp.Start<Startup>(url: baseAddress))
			{
				//System.Diagnostics.Process.Start(baseAddress + "analyse/?file=test");

				System.Console.WriteLine("Press ENTER to exit...");
				System.Console.ReadLine();
			}
		}
	}
}
