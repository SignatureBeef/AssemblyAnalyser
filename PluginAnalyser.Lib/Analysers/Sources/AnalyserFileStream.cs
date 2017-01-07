using PluginAnalyser.Lib.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PluginAnalyser.Lib.Analysers.Sources
{
	public class AnalyserFileStream : IAnalyserSource
	{
		public AnalyserConfig Config { get; private set; }

		protected Stream Source { get; set; }

		//public string Name => System.IO.Path.GetFileName(this.Path);
		public string Name { get; set; }

		private Lazy<string> _magicHeader;

		public AnalyserFileStream(AnalyserConfig config, string name, Stream source)
		{
			this.Config = config;
			this.Name = name;
			this.Source = source;

			this._magicHeader = new Lazy<string>(() => GetMagicHeader());
		}

		private string GetMagicHeader()
		{
			if (this.Source != null && this.Source.Length >= 2)
			{
				this.Source.Seek(0L, SeekOrigin.Begin);

				byte[] magic = new byte[2];
				var length = this.Source.Read(magic, 0, magic.Length);
				if (length == magic.Length)
				{
					return System.Text.Encoding.Default.GetString(magic);
				}
			}

			return null;
		}

		/// <summary>
		/// Determines if the file is a DOS executable item (exe,dll)
		/// </summary>
		/// <see cref="https://en.wikipedia.org/wiki/DOS_MZ_executable"/> 
		/// <returns>True when a DOS exe</returns>
		public bool IsDosExec()
		{
			return _magicHeader.Value == "MZ" || _magicHeader.Value == "ZM";
		}

		private IEnumerable<IAnalyseResult> AnalyseAssembly()
		{
			this.Source.Seek(0L, SeekOrigin.Begin);

			var assemblyAnalyser = new AssemblyAnalyser(this.Config, this.Source);
			return assemblyAnalyser.Analyse();
		}

		public IEnumerable<IAnalyseResult> Analyse()
		{
			if (IsDosExec())
			{
				return AnalyseAssembly();
			}

			return Enumerable.Empty<IAnalyseResult>();
		}
	}
}
