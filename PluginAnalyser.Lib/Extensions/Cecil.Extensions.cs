using Mono.Cecil;
using System;

namespace PluginAnalyser.Lib.Extensions
{
	public static class Cecil
	{
		public static void ForEachInstruction
		(
			this AssemblyDefinition assembly,
			Action<MethodDefinition, Mono.Cecil.Cil.Instruction> callback
		)
		{
			foreach (var module in assembly.Modules)
			{
				module.ForEachMethod(method =>
				{
					if (method.HasBody)
					{
						foreach (var ins in method.Body.Instructions.ToArray())
							callback.Invoke(method, ins);
					}
				});
			}
		}

		public static void ForEachMethod
		(
			this AssemblyDefinition assembly, 
			Action<MethodDefinition> callback, 
			bool hasBody = true
		)
		{
			foreach (var module in assembly.Modules)
			{
				module.ForEachMethod(method =>
				{
					if (method.HasBody || !hasBody)
					{
						callback.Invoke(method);
					}
				});
			}
		}

		public static void ForEachType(this AssemblyDefinition assembly, Action<TypeDefinition> callback)
		{
			foreach (var module in assembly.Modules)
			{
				module.ForEachType(callback);
			}
		}

		/// <summary>
		/// Enumerates over each type in the assembly, including nested types
		/// </summary>
		public static void ForEachType(this ModuleDefinition module, Action<TypeDefinition> callback)
		{
			foreach (var type in module.Types)
			{
				callback(type);

				//Enumerate nested types
				type.ForEachNestedType(callback);
			}
		}

		/// <summary>
		/// Enumerates all methods in the current module
		/// </summary>
		public static void ForEachMethod(this ModuleDefinition module, Action<MethodDefinition> callback)
		{
			module.ForEachType(type =>
			{
				foreach (var mth in type.Methods)
				{
					callback.Invoke(mth);
				}
			});
		}

		public static void ForEachNestedType(this TypeDefinition parent, Action<TypeDefinition> callback)
		{
			foreach (var type in parent.NestedTypes)
			{
				callback(type);

				type.ForEachNestedType(callback);
			}
		}
	}
}
