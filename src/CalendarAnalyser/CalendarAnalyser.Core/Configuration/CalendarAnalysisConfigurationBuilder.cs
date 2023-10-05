using System;
using System.Collections.Generic;

namespace CalendarAnalyser.Core.Configuration;

public class CalendarAnalysisConfigurationBuilder 
{
    private readonly CalendarAnalysisConfiguration configuration = new()
    {
        CoreHoursStartTime = TimeSpan.FromHours(9),
        CoreHoursEndTime = TimeSpan.FromHours(15),
        OnlyWorkingDays = true,
        TimeResolution = TimeSpan.FromMinutes(30),
        Rules = new List<IAnalysisRule>()
    };

    public CalendarAnalysisConfigurationBuilder WithTimeResolution(TimeSpan timeResolution)
    {
        configuration.TimeResolution = timeResolution;
        return this;
    }

    public CalendarAnalysisConfigurationBuilder WithCoreHoursStartAt(TimeSpan coreHoursStartAt)
    {
        configuration.CoreHoursStartTime = coreHoursStartAt;
        return this;
    }

    public CalendarAnalysisConfigurationBuilder WithCoreHoursEndAt(TimeSpan coreHoursEndAt)
    {
        configuration.CoreHoursEndTime = coreHoursEndAt;
        return this;
    }

    public CalendarAnalysisConfigurationBuilder WithRules(ICollection<IAnalysisRule> rules)
    {
        foreach (var rule in rules)
        {
            configuration.Rules.Add(rule);
        }
        return this;
    }

    public CalendarAnalysisConfiguration Build()
    {
        return configuration;
    }
}
