using PluginAnalyser.Lib.Results;
using System;

namespace PluginAnalyser.Lib.Analysers
{
	public class AssemblyAnalyserNotice : Exception
	{
		public IAnalyseResult Result { get; set; }

		public AssemblyAnalyserNotice(IAnalyseResult result)
		{
			this.Result = result;
		}
	}
}
