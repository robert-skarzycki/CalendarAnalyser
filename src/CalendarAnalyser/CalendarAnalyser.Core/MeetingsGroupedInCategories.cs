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
            if(configuration.FilterOutAllDayEvents && meeting.IsAllDay) { continue; }

            var matchingRules = configuration.Rules.Where(r => r.IsMatch(meeting)).ToArray();
            var matchingCategories = matchingRules.Select(r => r.Category).Distinct().ToArray();

            if (matchingCategories.Length > 1)
            {
                configuration.LogAction($"More than one rules apply: ${string.Join(", ", matchingCategories)} for \"${meeting.Subject}\" on ${meeting.StartDateTime:o}");
            }            
            
            var categoryName = matchingCategories.Length == 0 ? Constants.OtherCategoryName : matchingCategories.First(); // TODO: Handle multiple categories matching

            result[categoryName].Add(meeting);            
        }

        Data = result;
    }
}
