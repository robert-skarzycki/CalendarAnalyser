using System;
using System.Collections.Generic;
using System.Linq;

namespace CalendarAnalyser.Core;

internal class WorkingDay(DateOnly date, TimeSpan slotResolution, TimeSpan coreHoursStart, TimeSpan coreHoursEnd)
{
    public DateOnly Date { get; } = date;
    private readonly ICollection<CalendarSlot> slots = BuildSlots(slotResolution, coreHoursStart, coreHoursEnd);
    public TimeSpan WorkingTime => slots.Count * slotResolution;

    private static ICollection<CalendarSlot> BuildSlots(TimeSpan timeResolution, TimeSpan coreHoursStartTime, TimeSpan coreHoursEndTime)
    {
        var coreHoursLength = coreHoursEndTime - coreHoursStartTime;
        var slotsPerDay = (int)Math.Ceiling(coreHoursLength.TotalMinutes / timeResolution.TotalMinutes);

        var slots = Enumerable.Range(0, slotsPerDay).Select(i => new CalendarSlot(TimeOnly.FromTimeSpan(coreHoursStartTime.Add(TimeSpan.FromMinutes(i * timeResolution.TotalMinutes))), timeResolution)).ToArray();

        return slots;
    }

    public void AddMeetingWithCategory(Meeting meeting, string category)
    {        
        if(DateOnly.FromDateTime(meeting.StartDateTime.Date) != Date || DateOnly.FromDateTime(meeting.EndDateTime.Date) != Date)
        {
            throw new InvalidOperationException($"Provided meeting is outside given working day - {Date}");
        }

        var meetingStartTime = TimeOnly.FromDateTime(meeting.StartDateTime);
        var meetingEndTime = TimeOnly.FromDateTime(meeting.EndDateTime);

        foreach (var slot in slots)
        {
            if(slot.IsMeetingInSlot(meetingStartTime, meetingEndTime))
            {
                slot.AddCategory(category);
            }
        }
    }
}
    