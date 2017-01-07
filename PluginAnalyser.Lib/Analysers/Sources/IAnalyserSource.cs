using PluginAnalyser.Lib.Results;
using System.Collections.Generic;

namespace PluginAnalyser.Lib.Analysers.Sources
{
	public interface IAnalyserSource
	{
		string Name { get; set; }

		IEnumerable<IAnalyseResult> Analyse();
	}
}
