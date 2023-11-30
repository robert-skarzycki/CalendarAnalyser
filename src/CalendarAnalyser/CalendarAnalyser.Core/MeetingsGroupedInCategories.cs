using CalendarAnalyser.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CalendarAnalyser.Core;

public class MeetingsGroupedInCategories
{
    public IDictionary<string, List<Meeting>> Data { get; }

    public MeetingsGroupedInCategories(ICollection<Meeting> meetings, CalendarAnalysisConfiguration configuration)
    {
        var result = configuration.Rules.Select(r => r.Category).Distinct().ToDictionary(category => category, _ => new List<Meeting>());
        result.Add(Constants.OtherCategoryName, new List<Meeting>());

        foreach (var meeting in meetings)
        {
            var matchingRules = configuration.Rules.Where(r => r.IsMatch(meeting)).ToArray();

            if (matchingRules.Length > 1)
            {
                throw new InvalidOperationException("More than one rules apply");
            }
            else
            {
                var categoryName = matchingRules.Length == 0 ? Constants.OtherCategoryName : matchingRules.First().Category;

                result[categoryName].Add(meeting);
            }
        }

        Data = result;
    }
}
