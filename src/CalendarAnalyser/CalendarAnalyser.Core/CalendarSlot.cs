using System;
using System.Collections.Generic;
using System.Linq;

namespace CalendarAnalyser.Core;

internal class CalendarSlot(TimeOnly startTime, TimeSpan length)
{
    public TimeOnly StartTime { get; } = startTime;
    public TimeOnly EndTime { get; } = startTime.Add(length);    
    public ICollection<string> Categories = new List<string> { Constants.FreeCategoryName };

    public bool IsMeetingInSlot(TimeOnly meetingStart, TimeOnly meetingEnd) => (StartTime < meetingStart && EndTime > meetingStart) ||
        (StartTime < meetingEnd && EndTime > meetingStart) || (StartTime > meetingStart && StartTime < meetingEnd && EndTime < meetingEnd && EndTime > meetingStart);

    public void AddCategory(string categoryName)
    {
        if (Categories.Count == 1 && Categories.First() == Constants.FreeCategoryName)
        {
            Categories.Clear();
        }

        if(Categories.Contains(categoryName))
        {
            return;
        }

        Categories.Add(categoryName);
    }
}
    