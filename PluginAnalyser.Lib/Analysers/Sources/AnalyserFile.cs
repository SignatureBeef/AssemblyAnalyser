using System.IO;

namespace PluginAnalyser.Lib.Analysers.Sources
{
	public class AnalyserFile : AnalyserFileStream
	{
		static Stream TryGetStream(string path)
		{
			if (File.Exists(path))
				return File.OpenRead(path);

			return null;
		}

		public AnalyserFile(AnalyserConfig config, string path)
			: base(config, path, TryGetStream(path))
		{

		}
	}
}
