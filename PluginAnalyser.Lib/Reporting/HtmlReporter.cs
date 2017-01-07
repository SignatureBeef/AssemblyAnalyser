using PluginAnalyser.Lib.Analysers.Sources;
using PluginAnalyser.Lib.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PluginAnalyser.Lib.Reporting
{
	public class HtmlReporter
	{
		public IEnumerable<AnalysedResult> Results { get; private set; }

		public HtmlReporter(IEnumerable<AnalysedResult> results)
		{
			this.Results = results;
		}

		private void BuildContents(StringBuilder builder)
		{
			foreach (var item in this.Results)
			{
				builder.Append("<h3>Results for ");
				var analyserFile = item.Source as AnalyserFile;
				if (analyserFile != null && System.IO.File.Exists(item.Source.Name))
				{
					var filename = System.IO.Path.GetFileName(item.Source.Name);
					builder.Append(filename);
				}
				else
				{
					builder.Append(item.Source.Name);
				}
				builder.Append("</h3>");

				if (item.Results == null || item.Results.Count() == 0)
				{
					builder.Append("<h6>No results detected</h6>");
				}
				else
				{
					builder.Append("<div class=\"container-fluid\">");

					builder.Append("<div class=\"row row-headers\">");
					builder.Append("<div class=\"col-md-2\">Severity</div>");
					builder.Append("<div class=\"col-md-5\">Location</div>");
					builder.Append("<div class=\"col-md-5\">Message</div>");
					builder.Append("</div>");

					foreach (var result in item.Results)
					{
						var match = result as MatchedResult;

						if (match != null)
						{
							var cssSeverity = match.Action.Severity.ToString().ToLower();
							builder.Append($"<div class=\"row al-severity-{cssSeverity}\">");

							builder.Append("<div class=\"col-md-2\">");
							builder.Append(match.Action.Severity);
							builder.Append("</div>");
							builder.Append("<div class=\"col-md-5\">");
							builder.Append(match.Match);
							if (!String.IsNullOrWhiteSpace(match.DeclaringMethod))
							{
								builder.Append(" in ");
								builder.Append(match.DeclaringMethod);
							}
							builder.Append("</div>");
							builder.Append("<div class=\"col-md-5\">");
							builder.Append(match.Action.DisplayMessage);
							builder.Append("</div>");

							builder.Append("</div>");
						}
						else
						{
							var post = result as PostAnalyseResult;

							if (post != null)
							{
								var cssSeverity = post.Severity.ToString().ToLower();
								builder.Append($"<div class=\"row al-severity-{cssSeverity}\">");

								builder.Append("<div class=\"col-md-2\">");
								builder.Append(post.Severity);
								builder.Append("</div>");
								builder.Append("<div class=\"col-md-5\">");
								builder.Append(String.Join("<br/>", post.Locations
									.Distinct()
									.OrderBy(x => x)
								));
								builder.Append("</div>");
								builder.Append("<div class=\"col-md-5\">");
								builder.Append(post.DisplayMessage);
								builder.Append("</div>");

								builder.Append("</div>");
							}
							else
							{
								var actionResult = result as IAnalyseActionResult;

								if (actionResult != null)
								{
									var cssSeverity = actionResult.Action.Severity.ToString().ToLower();
									builder.Append($"<div class=\"row al-severity-{cssSeverity}\">");

									builder.Append("<div class=\"col-md-2\">");
									builder.Append(actionResult.Action.Severity);
									builder.Append("</div>");
									builder.Append("<div class=\"col-md-5\">");
									var resource = actionResult as EmbeddedResourceResult;
									if (resource != null)
									{
										builder.Append(resource.Match);
									}
									builder.Append("</div>");
									builder.Append("<div class=\"col-md-5\">");
									builder.Append(actionResult.Action.DisplayMessage);
									builder.Append("</div>");

									builder.Append("</div>");
								}
							}
						}
					}

					builder.Append("</div>");
				}
			}
		}

		public void SaveTo(string path)
		{
			var html = Generate();

			System.IO.File.WriteAllText(path, html);
		}

		public string Generate()
		{
			var sb = new StringBuilder();

			sb.Append("<!DOCTYPE html>");
			sb.Append("<html lang=\"en-AU\">");
			sb.Append("<head><title>Plugin Analyser Results ");
			sb.Append(System.DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss"));
			sb.Append("</title>");
			sb.Append("<link rel=\"stylesheet\" href=\"https://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css\">");
			sb.Append("<style type=\"text/css\">");
			sb.Append(".row-headers{font-size:17px}");
			sb.Append(".al-severity-info{color: #719af9;}");
			sb.Append(".al-severity-warning{color: #e4b74d;}");
			sb.Append(".al-severity-error{color: #e44d4d;}");
			sb.Append("body{background:#eee;}");
			sb.Append(".main-container {background: #fff;border-top: 1px solid #dedede;border-bottom: 1px solid #dedede;margin-top: 15px;box-shadow: 0 0 5px #dedede;padding-bottom: 20px;}");
			sb.Append("</style></head>");
			sb.Append("<body>");

			sb.Append("<div class=\"main-container\">");
			BuildContents(sb);
			sb.Append("</div>");

			sb.Append("</body>");

			return sb.ToString();
		}
	}
}
