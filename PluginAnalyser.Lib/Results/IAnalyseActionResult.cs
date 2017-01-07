using PluginAnalyser.Lib.Actions;

namespace PluginAnalyser.Lib.Results
{
	public interface IAnalyseActionResult : IAnalyseResult
	{
		IAssemblyAction Action { get; set; }
	}
}
