using System;
using System.Collections.Generic;

namespace CalendarAnalyser.Core.Configuration;

public class CalendarAnalysisConfigurationBuilder 
{
    private TimeSpan timeResolution = TimeSpan.FromMinutes(30);
    private TimeSpan coreHoursStartTime = TimeSpan.FromHours(9);
    private TimeSpan coreHoursEndTime = TimeSpan.FromHours(15);
    private List<IAnalysisRule> rules = [];
    private bool onlyWorkingDays = true;
    private DateTime analysisStartDate;
    private DateTime analysisEndDate;
    private Action<string> logAction = _ => { };
    private bool filterOutAllDayEvents = false;
    private int focusSpotSlotsNumberLength = 4;
    private bool collectMeetingsPerCategory = false;

    public CalendarAnalysisConfigurationBuilder WithTimeResolution(TimeSpan timeResolution)
    {
        this.timeResolution = timeResolution;
        return this;
    }

    public CalendarAnalysisConfigurationBuilder WithCoreHoursStartAt(TimeSpan coreHoursStartAt)
    {
        this.coreHoursStartTime = coreHoursStartAt;
        return this;
    }

    public CalendarAnalysisConfigurationBuilder WithCoreHoursEndAt(TimeSpan coreHoursEndAt)
    {
        this.coreHoursEndTime = coreHoursEndAt;
        return this;
    }

    public CalendarAnalysisConfigurationBuilder WithRules(ICollection<IAnalysisRule> rules)
    {
        foreach (var rule in rules)
        {
            this.rules.Add(rule);
        }
        return this;
    }

    public CalendarAnalysisConfigurationBuilder WithOnlyWorkingDays()
    {
        this.onlyWorkingDays = true;
        return this;
    }

    public CalendarAnalysisConfigurationBuilder WithAnalysisDateRange(DateTime analysisStartDate, DateTime analysisEndDate)
    {
        this.analysisStartDate = analysisStartDate;
        this.analysisEndDate = analysisEndDate;

        return this;
    }

    public CalendarAnalysisConfigurationBuilder WithLogAction(Action<string> logAction)
    {
        this.logAction = logAction; 
        
        return this;
    }

    public CalendarAnalysisConfigurationBuilder WithoutAllDayEvents()
    {
        this.filterOutAllDayEvents = true;

        return this;
    }

    public CalendarAnalysisConfigurationBuilder WithCollectingMeetingsPerCategory()
    {
        this.collectMeetingsPerCategory = true;

        return this;
    }

    public CalendarAnalysisConfiguration Build()
    {
        return new CalendarAnalysisConfiguration(
            analysisStartDate,
            analysisEndDate,
            rules,
            coreHoursStartTime,
            coreHoursEndTime,
            onlyWorkingDays,
            timeResolution,
            filterOutAllDayEvents,
            focusSpotSlotsNumberLength,
            collectMeetingsPerCategory,
            logAction
            );
    }
}
