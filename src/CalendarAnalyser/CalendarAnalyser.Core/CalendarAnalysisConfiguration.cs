using System;
using System.Collections.Generic;

namespace CalendarAnalyser.Core;

public class CalendarAnalysisConfiguration
{
    private TimeSpan coreHoursStartTime = TimeSpan.FromHours(9);
    private TimeSpan coreHoursEndTime = TimeSpan.FromHours(15);

    public bool OnlyWorkingDays { get; set; } = true;
    public TimeSpan TimeResolution { get; set; } = TimeSpan.FromMinutes(30);
    public TimeSpan CoreHoursStartTime
    {
        get { return coreHoursStartTime; }
        set
        {
            if (value < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException("Core hours start time cannot be lower than 0:00.");
            }
            if (value > new TimeSpan(23, 59, 59))
            {
                throw new ArgumentOutOfRangeException("Core hours star time cannot be greater than 23:59:59");
            }
            if(value > coreHoursEndTime)
            {
                throw new ArgumentOutOfRangeException($"Core hours start time cannot be greater than core hours end time which is {coreHoursEndTime}");
            }

            coreHoursStartTime = value;
        }
    }

    public TimeSpan CoreHoursEndTime
    {
        get { return coreHoursEndTime; }
        set
        {
            if (value < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException("Core hours end time cannot be lower than 0:00.");
            }
            if (value > new TimeSpan(23, 59, 59))
            {
                throw new ArgumentOutOfRangeException("Core hours end time cannot be greater than 23:59:59");
            }
            if (value < coreHoursEndTime)
            {
                throw new ArgumentOutOfRangeException($"Core hours end time cannot be lower than core hours start time which is {coreHoursStartTime}");
            }

            coreHoursEndTime = value;
        }
    }

    public ICollection<IAnalysisRule> Rules { get; set; }
}
