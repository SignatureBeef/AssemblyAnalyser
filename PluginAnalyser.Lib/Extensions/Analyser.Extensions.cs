using PluginAnalyser.Lib.Analysers;
using PluginAnalyser.Lib.Analysers.Sources;

namespace PluginAnalyser.Lib.Extensions
{
	public static class AnalyserExtensions
	{
		public static Analyser AddFile(this Analyser analyser, string path)
		{
			analyser.Sources.Add(new AnalyserFile(analyser.Config, path));
			return analyser;
		}
	}
}
