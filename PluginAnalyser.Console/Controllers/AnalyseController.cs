using PluginAnalyser.Lib.Analysers;
using PluginAnalyser.Lib.Extensions;
using PluginAnalyser.Lib.Reporting;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace PluginAnalyser.Console.Controllers
{
	public class AnalyseController : ApiController
	{
		public IHttpActionResult Get()
		{
			return Redirect(new Uri("/index.html", UriKind.Relative));
		}

		public async Task<HttpResponseMessage> Post(string output = "json")
		{
			if (!Request.Content.IsMimeMultipartContent())
			{
				throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
			}

			Directory.CreateDirectory("Uploads");
			var provider = new MultipartFormDataStreamProvider("Uploads");

			try
			{
				await Request.Content.ReadAsMultipartAsync(provider);

				var config = ConsoleAnalyserConfig.FromFile("config.json");
				var analyser = new Analyser(config);

				foreach (var file in provider.FileData)
				{
					analyser.AddFile(file.LocalFileName);
				}
				var results = analyser.Analyse();


				foreach (var file in provider.FileData)
				{
					var search = System.IO.Path.GetFileName(file.LocalFileName);
					foreach (var analysedResult in results)
					{
						if (analysedResult.Source.Name.Contains(search))
						{
							analysedResult.Source.Name = file.Headers.ContentDisposition.FileName;
						}
					}

					try
					{
						System.IO.File.Delete(file.LocalFileName);
					}
					catch { }
				}

				if (output != "html")
					return Request.CreateResponse(HttpStatusCode.OK, results);

				var reporter = new HtmlReporter(results);
				var html = reporter.Generate();
				return Request.CreateResponse(HttpStatusCode.OK, html);
			}
			catch (System.Exception e)
			{
				return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
			}
		}
	}
}
