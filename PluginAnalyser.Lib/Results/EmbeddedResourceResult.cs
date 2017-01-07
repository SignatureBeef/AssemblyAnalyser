using PluginAnalyser.Lib.Actions;
using System;
using System.Collections.Generic;

namespace PluginAnalyser.Lib.Results
{
	public class EmbeddedResourceResult : IAnalyseActionResult, ILocation
	{
		public IAssemblyAction Action { get; set; }

		public string Match { get; set; }
		public IEnumerable<String> Locations { get; set; }
	}
}
