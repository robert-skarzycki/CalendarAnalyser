using System;
using System.Collections.Generic;
using System.Linq;

namespace CalendarAnalyser.Core;

internal class CalendarSlot(TimeOnly startTime, TimeSpan length)
{
    public TimeOnly StartTime { get; } = startTime;
    public TimeOnly EndTime { get; } = startTime.Add(length);    
    private readonly ICollection<string> categories = new[] { Constants.FreeCategoryName };

    public bool IsMeetingInSlot(TimeOnly meetingStart, TimeOnly meetingEnd) => (StartTime < meetingStart && EndTime > meetingStart) ||
        (StartTime < meetingEnd && EndTime > meetingStart) || (StartTime > meetingStart && StartTime < meetingEnd && EndTime < meetingEnd && EndTime > meetingStart);

    public void AddCategory(string categoryName)
    {
        if (categories.Count == 1 && categories.First() == Constants.FreeCategoryName)
        {
            categories.Clear();
        }

        categories.Add(categoryName);
    }
}
    