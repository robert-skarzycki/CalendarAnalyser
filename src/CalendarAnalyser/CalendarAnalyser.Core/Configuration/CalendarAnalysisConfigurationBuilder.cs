using System;
using System.Collections.Generic;

namespace CalendarAnalyser.Core.Configuration;

public class CalendarAnalysisConfigurationBuilder 
{
    internal readonly CalendarAnalysisConfiguration configuration = new()
    {
        CoreHoursStartTime = TimeSpan.FromHours(9),
        CoreHoursEndTime = TimeSpan.FromHours(15),
        OnlyWorkingDays = true,
        TimeResolution = TimeSpan.FromMinutes(30),
        Rules = new List<IAnalysisRule>(),
        LogAction = _ => { },
        FilterOutAllDayEvents = false,
        FocusSpotSlotsNumberLength = 4
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

    public CalendarAnalysisConfigurationBuilder WithOnlyWorkingDays()
    {
        configuration.OnlyWorkingDays = true;
        return this;
    }

    public CalendarAnalysisConfigurationBuilder WithAnalysisDateRange(DateTime analysisStartDate, DateTime analysisEndDate)
    {
        configuration.AnalysisStartDate = analysisStartDate;
        configuration.AnalysisEndDate = analysisEndDate;

        return this;
    }

    public CalendarAnalysisConfigurationBuilder WithLogAction(Action<string> logAction)
    {
        configuration.LogAction = logAction; 
        
        return this;
    }

    public CalendarAnalysisConfigurationBuilder WithoutAllDayEvents()
    {
        configuration.FilterOutAllDayEvents = true;

        return this;
    }

    public CalendarAnalysisConfiguration Build()
    {
        return configuration;
    }
}
