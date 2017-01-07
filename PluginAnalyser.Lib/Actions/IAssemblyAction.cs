using PluginAnalyser.Lib.Analysers;
using PluginAnalyser.Lib.Results;
using System.Collections.Generic;

namespace PluginAnalyser.Lib.Actions
{
	public interface IAssemblyAction
	{
		AnalyserConfig Config { get; set; }
		string DisplayMessage { get; set; }
		string Id { get; set; }
		ActionSeverity Severity { get; set; }

		IEnumerable<IAnalyseResult> Analyse(AssemblyAnalyser analyser);
	}
}
