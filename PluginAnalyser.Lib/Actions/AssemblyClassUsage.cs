using Mono.Cecil;
using PluginAnalyser.Lib.Analysers;
using PluginAnalyser.Lib.Extensions;
using PluginAnalyser.Lib.Results;
using System;
using System.Collections.Generic;

namespace PluginAnalyser.Lib.Actions
{
	/// <summary>
	/// Searches in a specified assembly for usages of anything for the given class
	/// </summary>
	public class AssemblyClassUsage : IAssemblyAction
	{
		public AnalyserConfig Config { get; set; }
		public string Id { get; set; }
		public string DisplayMessage { get; set; }
		public ActionSeverity Severity { get; set; }
		public string QualifiedClassName { get; private set; }

		public AssemblyClassUsage(string qualifiedClassName)
		{
			this.QualifiedClassName = qualifiedClassName;
		}

		public IEnumerable<IAnalyseResult> Analyse(AssemblyAnalyser analyser)
		{
			List<IAnalyseResult> results = new List<IAnalyseResult>();

			analyser.AssemblyDefinition.ForEachInstruction((method, instruction) =>
			{
				var methodRef = instruction.Operand as MethodReference;

				if (methodRef != null)
				{
					if (methodRef.DeclaringType.FullName.Equals
					(
						this.QualifiedClassName, 
						StringComparison.CurrentCultureIgnoreCase
					))
					{
						results.Add(new MatchedResult()
						{
							Action = this,
							Match = this.QualifiedClassName,
							DeclaringMethod = method.FullName,
							DeclaringType = method.DeclaringType.FullName,
							Locations = new[] { analyser.AssemblyDefinition.FullName }
						});
					}
				}
			});

			return results;
		}
	}
}
