using PluginAnalyser.Lib.Actions;
using System;
using System.Collections.Generic;

namespace PluginAnalyser.Lib.Results
{
	public class MatchedResult : IAnalyseActionResult, ILocation
	{
		public IAssemblyAction Action { get; set; }

		public string Match { get; set; }
		public string DeclaringMethod { get; set; }

		public string DeclaringType { get; set; }
		public IEnumerable<String> Locations { get; set; }
	}
}
