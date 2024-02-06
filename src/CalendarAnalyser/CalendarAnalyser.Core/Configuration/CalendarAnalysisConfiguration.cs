using System;
using System.Collections.Generic;

namespace CalendarAnalyser.Core.Configuration;

public class CalendarAnalysisConfiguration
{
    public CalendarAnalysisConfiguration(
        DateTime analysisStartDate,
        DateTime analysisEndDate,
        ICollection<IAnalysisRule> rules,
        TimeSpan coreHoursStartTime,
        TimeSpan coreHoursEndTime,
        bool onlyWorkingDays,
        TimeSpan timeResolution,
        bool filterOutAllDayEvents,
        int focusSpotSlotsNumberLength,
        bool collectMeetingsPerCategory,
        Action<string> logAction = null
        )
    {
        ValidateStartEndDate(analysisStartDate, analysisEndDate);
        AnalysisStartDate = analysisStartDate;
        AnalysisEndDate = analysisEndDate;

        Rules = rules;

        ValidateCoreHours(coreHoursStartTime, coreHoursEndTime);
        CoreHoursStartTime = coreHoursStartTime;        
        CoreHoursEndTime = coreHoursEndTime;

        OnlyWorkingDays = onlyWorkingDays;

        TimeResolution = timeResolution;

        FilterOutAllDayEvents = filterOutAllDayEvents;

        FocusSpotSlotsNumberLength = focusSpotSlotsNumberLength;

        CollectMeetingsPerCategory = collectMeetingsPerCategory;

        LogAction = logAction;
    }

    private void ValidateStartEndDate(DateTime analysisStartDate, DateTime analysisEndDate)
    {
        if(analysisEndDate < analysisStartDate)
        {
            throw new ArgumentOutOfRangeException(nameof(analysisStartDate), "Analysis start date cannot be greater then end date.");
        }
    }

    private void ValidateCoreHours(TimeSpan coreHoursStartTime, TimeSpan coreHoursEndTime)
    {
        if (coreHoursStartTime < TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(coreHoursStartTime), "Core hours start time cannot be lower than 0:00.");
        }
        if (coreHoursStartTime > new TimeSpan(23, 59, 59))
        {
            throw new ArgumentOutOfRangeException(nameof(coreHoursStartTime), "Core hours star time cannot be greater than 23:59:59");
        }
        if (coreHoursStartTime > coreHoursEndTime)
        {
            throw new ArgumentOutOfRangeException(nameof(coreHoursStartTime), $"Core hours start time cannot be greater than core hours end time which is {coreHoursEndTime}");
        }

        if (coreHoursEndTime < TimeSpan.Zero)
        {
            throw new ArgumentOutOfRangeException(nameof(coreHoursEndTime), "Core hours end time cannot be lower than 0:00.");
        }
        if (coreHoursEndTime > new TimeSpan(23, 59, 59))
        {
            throw new ArgumentOutOfRangeException(nameof(coreHoursEndTime), "Core hours end time cannot be greater than 23:59:59");
        }
        if (coreHoursStartTime > coreHoursEndTime)
        {
            throw new ArgumentOutOfRangeException(nameof(coreHoursEndTime), $"Core hours end time cannot be lower than core hours start time which is {coreHoursStartTime}");
        }
    }  
    
    public DateTime AnalysisStartDate { get; init; }
    public DateTime AnalysisEndDate { get; init; }
    public ICollection<IAnalysisRule> Rules { get; init; }
    public TimeSpan CoreHoursStartTime { get; init; }
    public TimeSpan CoreHoursEndTime { get; init; }
    public bool OnlyWorkingDays { get; init; }
    public TimeSpan TimeResolution { get; init; }
    public bool FilterOutAllDayEvents { get; init; }
    public int FocusSpotSlotsNumberLength { get; init; }
    public bool CollectMeetingsPerCategory { get; init; }
    public Action<string> LogAction { get; init; }    
}
