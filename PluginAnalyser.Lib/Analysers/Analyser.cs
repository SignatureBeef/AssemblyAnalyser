using PluginAnalyser.Lib.Analysers.Sources;
using PluginAnalyser.Lib.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PluginAnalyser.Lib.Analysers
{
	public class Analyser
	{
		public AnalyserConfig Config { get; private set; }
		public List<IAnalyserSource> Sources { get; private set; } = new List<IAnalyserSource>();

		public Analyser(AnalyserConfig config)
		{
			this.Config = config;
		}

		private AnalysedResult runPostAnalysers(IEnumerable<AnalysedResult> analysed)
		{
			var results = new List<IAnalyseResult>();
			var res = new AnalysedResult()
			{
				Results = results,
				Source = new PostAnalyserSource()
			};

			foreach (var analyser in this.Config.PostAnalysers)
			{
				Dictionary<string, int> passed = new Dictionary<string, int>();

				var locations = new List<String>();
				foreach (var id in analyser.When)
				{
					foreach (var sourceItem in analysed)
					{
						if (sourceItem.Results != null)
						{
							foreach (var result in sourceItem.Results)
							{
								var actionResult = result as IAnalyseActionResult;
								if (actionResult != null)
								{
									if (actionResult.Action.Id == id)
									{
										if (passed.ContainsKey(id))
										{
											passed[id]++;
										}
										else
										{
											passed.Add(id, 1);
										}

										var location = result as ILocation;
										if (location != null)
										{
											locations.AddRange(location.Locations);
										}
									}
								}
							}
						}
					}
				}
				if (analyser.When.Length > 0 && analyser.When.Length == passed.Keys.Count)
				{
					results.Add(new PostAnalyseResult()
					{
						DisplayMessage = analyser.Then.DisplayMessage,
						Severity = analyser.Then.Severity,
						Locations = locations
					});
				}
			}

			return res;
		}

		public IEnumerable<AnalysedResult> RunAnalysers()
		{
			foreach (var source in this.Sources)
			{
				var sourceResults = source.Analyse();
				yield return new AnalysedResult()
				{
					Source = source,
					Results = sourceResults
				};
			}
		}

		public IEnumerable<AnalysedResult> Analyse()
		{
			var results = new List<AnalysedResult>();

			var analysed = RunAnalysers();
			results.AddRange(analysed);

			var post = runPostAnalysers(analysed);
			if (post.Results != null) //.Count() > 0)
				results.Add(post);

			return results;
		}
	}
}
