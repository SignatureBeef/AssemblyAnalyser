using Mono.Cecil;
using PluginAnalyser.Lib.Actions;
using PluginAnalyser.Lib.Results;
using System.Collections.Generic;
using System.IO;

namespace PluginAnalyser.Lib.Analysers
{
	public class AssemblyAnalyser
	{
		private AnalyserConfig config;

		public Stream Source { get; protected set; }

		public AssemblyDefinition AssemblyDefinition { get; private set; }

		public IEnumerable<IAssemblyAction> Actions { get; set; }

		public AssemblyAnalyser() { }
		public AssemblyAnalyser(AnalyserConfig config, Stream source)
		{
			this.config = config;
			this.Source = source;
			this.Actions = this.ResolveActions(config);
		}

		protected IEnumerable<IAssemblyAction> ResolveActions(AnalyserConfig config)
		{
			foreach (var analyser in config.Analysers)
			{
				var value = analyser.Resolve<IAssemblyAction>();

				if (value != null)
				{
					value.Id = analyser.Id;
					value.DisplayMessage = analyser.DisplayMessage;
					value.Severity = analyser.Severity;
					value.Config = config;
					yield return value;
				}
			}
		}

		public IEnumerable<IAnalyseResult> Analyse()
		{
			this.Source.Seek(0L, SeekOrigin.Begin);
			this.AssemblyDefinition = AssemblyDefinition.ReadAssembly(this.Source);

			foreach (var action in this.Actions)
			{
				foreach (var result in action.Analyse(this))
				{
					yield return result;
				}
			}
		}
	}
}
