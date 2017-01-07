using Mono.Cecil;
using PluginAnalyser.Lib.Analysers;
using PluginAnalyser.Lib.Analysers.Sources;
using PluginAnalyser.Lib.Results;
using System.Collections.Generic;

namespace PluginAnalyser.Lib.Actions
{
	public class AssemblyEmbeddedResourceUsage : IAssemblyAction
	{
		public AnalyserConfig Config { get; set; }

		public string DisplayMessage { get; set; }

		public string Id { get; set; }

		public ActionSeverity Severity { get; set; }

		public IEnumerable<IAnalyseResult> Analyse(AssemblyAnalyser analyser)
		{
			foreach (var module in analyser.AssemblyDefinition.Modules)
			{
				foreach (var resouce in module.Resources)
				{
					var eb = resouce as EmbeddedResource;
					if (eb != null)
					{
						var srm = eb.GetResourceStream();
						var resourseAnalyser = new AnalyserFileStream
						(
							this.Config,
							$"{nameof(EmbeddedResource)}+{resouce.Name}", 
							srm
						);

						yield return new EmbeddedResourceResult()
						{
							Action = this,
							Match = resourseAnalyser.Name,
							Locations = new[] { analyser.AssemblyDefinition.FullName }
						};

						foreach (var result in resourseAnalyser.Analyse())
							yield return result;
					}
				}
			}
		}
	}
}
