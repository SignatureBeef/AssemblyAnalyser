using System.IO;

namespace PluginAnalyser.Lib.Analysers
{
	public class AssemblyFileAnalyser : AssemblyAnalyser
	{
		public string Path { get; private set; }

		public AssemblyFileAnalyser(AnalyserConfig config, string path)
		{
			this.Path = path;

			this.Source = File.OpenRead(path);
		}
	}
}
