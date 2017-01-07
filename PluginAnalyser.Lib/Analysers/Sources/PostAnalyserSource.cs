
using PluginAnalyser.Lib.Results;
using System;
using System.Collections.Generic;

namespace PluginAnalyser.Lib.Analysers.Sources
{
	public class PostAnalyserSource : IAnalyserSource
	{
		public string Name { get; set; } = "post analyse";

		public IEnumerable<IAnalyseResult> Analyse()
		{
			throw new NotImplementedException();
		}
	}
}
