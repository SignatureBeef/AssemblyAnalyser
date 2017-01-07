using PluginAnalyser.Lib.Actions;
using System;
using System.Collections.Generic;

namespace PluginAnalyser.Lib.Results
{
	public class PostAnalyseResult : IAnalyseResult
	{
		public string DisplayMessage { get; set; }
		public ActionSeverity Severity { get; set; }

		public List<String> Locations { get; set; }
	}
}
