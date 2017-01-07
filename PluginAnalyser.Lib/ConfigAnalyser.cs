using PluginAnalyser.Lib.Actions;
using System;
using System.Linq;

namespace PluginAnalyser.Lib
{
	public class ConfigAnalyser
	{
		public string Id { get; set; }
		public string Type { get; set; }
		public string[] Args { get; set; }

		public string DisplayMessage { get; set; }
		public ActionSeverity Severity { get; set; }

		private object _cache;

		public TInstance Resolve<TInstance>() where TInstance : class
		{
			if (_cache != null && !_cache.Equals(default(TInstance)))
				return _cache as TInstance;

			var type = this.GetType().Assembly.ExportedTypes.SingleOrDefault(x => x.Name == this.Type);

			if (type == null)
				return null;

			_cache = Activator.CreateInstance(type, this.Args);

			return _cache as TInstance;
		}
	}
}
