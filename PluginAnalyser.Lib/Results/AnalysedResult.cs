using PluginAnalyser.Lib.Analysers.Sources;
using System.Collections.Generic;

namespace PluginAnalyser.Lib.Results
{
	public class AnalysedResult
	{
		public IAnalyserSource Source { get; set; }
		public IEnumerable<IAnalyseResult> Results { get; set; }
	}
}
