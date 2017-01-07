namespace PluginAnalyser.Lib
{
	public class ConfigPostAnalyser
	{
		/// <summary>
		/// Id's of <see cref="ConfigAnalyser"/>'s
		/// </summary>
		public string[] When { get; set; }

		public ConfigPostAnalyserAction Then { get; set; }
	}
}
