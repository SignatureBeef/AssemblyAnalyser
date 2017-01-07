using System;
using System.Collections.Generic;

namespace PluginAnalyser.Lib.Results
{
	public interface ILocation
	{
		IEnumerable<String> Locations { get; set; }
	}
}
