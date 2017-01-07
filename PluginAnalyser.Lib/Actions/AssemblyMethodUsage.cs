using Mono.Cecil;
using PluginAnalyser.Lib.Analysers;
using PluginAnalyser.Lib.Extensions;
using PluginAnalyser.Lib.Results;
using System;
using System.Collections.Generic;

namespace PluginAnalyser.Lib.Actions
{
	/// <summary>
	/// Searches in a specified assembly for a matching method 
	/// </summary>
	public class AssemblyMethodUsage : IAssemblyAction
	{
		public AnalyserConfig Config { get; set; }
		public string Id { get; set; }
		public string DisplayMessage { get; set; }
		public ActionSeverity Severity { get; set; }
		public string QualifiedMethodName { get; private set; }

		public AssemblyMethodUsage(string qualifiedMethodName)
		{
			this.QualifiedMethodName = qualifiedMethodName;
		}

		public IEnumerable<IAnalyseResult> Analyse(AssemblyAnalyser analyser)
		{
			List<IAnalyseResult> results = new List<IAnalyseResult>();

			analyser.AssemblyDefinition.ForEachInstruction((method, instruction) =>
			{
				var methodRef = instruction.Operand as MethodReference;

				if (methodRef != null)
				{
					var qualifiedName = methodRef.DeclaringType.FullName + '.' + methodRef.Name;
					if (qualifiedName.Equals
					(
						this.QualifiedMethodName,
						StringComparison.CurrentCultureIgnoreCase
					))
					{
						results.Add(new MatchedResult()
						{
							Action = this,
							Match = this.QualifiedMethodName,
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
