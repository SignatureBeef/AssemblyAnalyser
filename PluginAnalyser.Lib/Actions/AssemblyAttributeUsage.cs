using PluginAnalyser.Lib.Analysers;
using PluginAnalyser.Lib.Extensions;
using PluginAnalyser.Lib.Results;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace PluginAnalyser.Lib.Actions
{
	public class AssemblyAttributeUsage : IAssemblyAction
	{
		public AnalyserConfig Config { get; set; }
		public string Id { get; set; }
		public string DisplayMessage { get; set; }
		public ActionSeverity Severity { get; set; }
		public string QualifiedAttributeName { get; private set; }

		public AssemblyAttributeUsage(string qualifiedAttributeName)
		{
			this.QualifiedAttributeName = qualifiedAttributeName;
		}

		public IEnumerable<IAnalyseResult> Analyse(AssemblyAnalyser analyser)
		{
			List<IAnalyseResult> results = new List<IAnalyseResult>();

			analyser.AssemblyDefinition.ForEachType(type =>
			{
				foreach (var attr in type.CustomAttributes)
				{
					var qualifiedName = attr.AttributeType.FullName;
					if (qualifiedName.Equals
					(
						this.QualifiedAttributeName, 
						StringComparison.CurrentCultureIgnoreCase
					))
					{
						results.Add(new MatchedResult()
						{
							Action = this,
							Match = this.QualifiedAttributeName,
							DeclaringType = type.FullName,
							Locations = new[] { analyser.AssemblyDefinition.FullName }
						});
					}
				}
			});

			analyser.AssemblyDefinition.ForEachMethod((method) =>
			{
				if (this.QualifiedAttributeName == typeof(DllImportAttribute).FullName)
				{
					if (method.IsPInvokeImpl)
					{
						results.Add(new MatchedResult()
						{
							Action = this,
							Match = this.QualifiedAttributeName,
							DeclaringMethod = method.FullName,
							DeclaringType = method.DeclaringType.FullName,
							Locations = new[] { analyser.AssemblyDefinition.FullName }
						});
					}
				}
				else if (method.HasCustomAttributes)
				{
					foreach (var attr in method.CustomAttributes)
					{
						var qualifiedName = attr.AttributeType.FullName;
						if (qualifiedName.Equals
						(
							this.QualifiedAttributeName,
							StringComparison.CurrentCultureIgnoreCase
						))
						{
							results.Add(new MatchedResult()
							{
								Action = this,
								Match = this.QualifiedAttributeName,
								DeclaringMethod = method.FullName,
								DeclaringType = method.DeclaringType.FullName,
								Locations = new[] { analyser.AssemblyDefinition.FullName }
							});
						}
					}
				}
			}, false);

			return results;
		}
	}
}
