using Mono.Cecil;
using PluginAnalyser.Lib.Analysers;
using PluginAnalyser.Lib.Extensions;
using PluginAnalyser.Lib.Results;
using System;
using System.Collections.Generic;

namespace PluginAnalyser.Lib.Actions
{
	public class AssemblyNameSpaceUsage : IAssemblyAction
	{
		public AnalyserConfig Config { get; set; }
		public string Id { get; set; }
		public string DisplayMessage { get; set; }
		public ActionSeverity Severity { get; set; }
		public string QualifiedNameSpace { get; private set; }

		public AssemblyNameSpaceUsage(string qualifiedNameSpace)
		{
			this.QualifiedNameSpace = qualifiedNameSpace;
		}

		public IEnumerable<IAnalyseResult> Analyse(AssemblyAnalyser analyser)
		{
			List<IAnalyseResult> results = new List<IAnalyseResult>();

			analyser.AssemblyDefinition.ForEachInstruction((method, instruction) =>
			{
				var methodRef = instruction.Operand as MethodReference;

				if (methodRef != null)
				{
					if (methodRef.DeclaringType.Namespace.Equals
					(
						this.QualifiedNameSpace, 
						StringComparison.CurrentCultureIgnoreCase
					))
					{
						results.Add(new MatchedResult()
						{
							Action = this,
							Match = this.QualifiedNameSpace,
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
