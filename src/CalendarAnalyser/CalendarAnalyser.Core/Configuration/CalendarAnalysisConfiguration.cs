using System;
using System.Collections.Generic;

namespace CalendarAnalyser.Core.Configuration;

public class CalendarAnalysisConfiguration
{
    private TimeSpan coreHoursStartTime;
    private TimeSpan coreHoursEndTime;

    public bool OnlyWorkingDays { get; set; }
    public TimeSpan TimeResolution { get; set; }
    public TimeSpan CoreHoursStartTime
    {
        get { return coreHoursStartTime; }
        set
        {
            //if (value < TimeSpan.Zero)
            //{
            //    throw new ArgumentOutOfRangeException("Core hours start time cannot be lower than 0:00.");
            //}
            //if (value > new TimeSpan(23, 59, 59))
            //{
            //    throw new ArgumentOutOfRangeException("Core hours star time cannot be greater than 23:59:59");
            //}
            //if (value > coreHoursEndTime)
            //{
            //    throw new ArgumentOutOfRangeException($"Core hours start time cannot be greater than core hours end time which is {coreHoursEndTime}");
            //}

            coreHoursStartTime = value;
        }
    }

    public TimeSpan CoreHoursEndTime
    {
        get { return coreHoursEndTime; }
        set
        {
            //if (value < TimeSpan.Zero)
            //{
            //    throw new ArgumentOutOfRangeException("Core hours end time cannot be lower than 0:00.");
            //}
            //if (value > new TimeSpan(23, 59, 59))
            //{
            //    throw new ArgumentOutOfRangeException("Core hours end time cannot be greater than 23:59:59");
            //}
            //if (value < coreHoursEndTime)
            //{
            //    throw new ArgumentOutOfRangeException($"Core hours end time cannot be lower than core hours start time which is {coreHoursStartTime}");
            //}

            coreHoursEndTime = value;
        }
    }

    public ICollection<IAnalysisRule> Rules { get; set; }

    public DateTime AnalysisStartDate { get; set; }
    public DateTime AnalysisEndDate { get; set; }

    public Action<string> LogAction { get; set; }
    public bool FilterOutAllDayEvents { get; set; }

    public int FocusSpotSlotsNumberLength { get; set; }

    public bool CollectMeetingsPerCategory { get; set; }
}
