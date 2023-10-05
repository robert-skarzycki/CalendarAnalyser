using System;
using System.Collections.Generic;

namespace CalendarAnalyser.Core.Configuration;

public class CalendarAnalysisConfigurationBuilder 
{
    private readonly CalendarAnalysisConfiguration configuration = new();

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
        configuration.Rules = rules;
        return this;
    }

    public CalendarAnalysisConfiguration Build()
    {
        return configuration;
    }
}
