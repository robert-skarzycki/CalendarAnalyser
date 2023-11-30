using CalendarAnalyser.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CalendarAnalyser.Core;

public class CalendarAnalysisEngine
{
    private readonly CalendarAnalysisConfiguration configuration;

    public CalendarAnalysisEngine(CalendarAnalysisConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public CalendarAnalysisResult Analyze(ICollection<Meeting> meetings)
    {
        if (meetings == null) { throw new ArgumentNullException(nameof(meetings)); }

        var totalWorkingTime = CalculateTotalWorkingTime();

        var meetingsGrouped = new MeetingsGroupedInCategories(meetings, configuration);

        var totalDurationPerCategory = CalculateTotalDurationPerCategory(meetingsGrouped.Data);

        var calendarSlots = BuildCalendarSlots(meetingsGrouped.Data);

        var result = new CalendarAnalysisResult
        {
            CategoriesAnalysis = new CalendarCategoriesAnalysisResult(totalDurationPerCategory, totalWorkingTime),
            CalendarSlots = calendarSlots
        };

        return result;
    }

    private IEnumerable<CalendarSlot> BuildCalendarSlots(IDictionary<string, List<Meeting>> meetingGroups)
    {
        var currentDate = configuration.AnalysisStartDate.Date;

        var coreHoursLength = configuration.CoreHoursEndTime - configuration.CoreHoursStartTime;
        var slotsPerDay = Math.Ceiling(coreHoursLength.TotalMinutes / configuration.TimeResolution.TotalMinutes);

        while(currentDate < configuration.AnalysisEndDate.Date)
        {
            if(!configuration.OnlyWorkingDays || !IsWeekend(currentDate))
            {
                for(var i=0;i< slotsPerDay; i++)
                {
                    var slotStartDate = currentDate.Add(configuration.CoreHoursStartTime).AddMinutes(i * configuration.TimeResolution.TotalMinutes);
                    var slotEndDate = slotStartDate.Add(configuration.TimeResolution);

                    var categories = meetingGroups.Where(pair => pair.Value.Any(meeting => meeting.StartDateTime < slotEndDate && meeting.EndDateTime > slotStartDate)).ToArray();

                    if (categories.Any())
                    {
                        yield return new CalendarSlot(slotStartDate, string.Join("_", categories.Select(pair => pair.Key)));
                    }
                    else
                    {
                        yield return new CalendarSlot(slotStartDate, Constants.FreeCategoryName);
                    }
                }
                
            }

            currentDate = currentDate.AddDays(1);
        }
    }

    private TimeSpan CalculateTotalWorkingTime()
    {
        var totalDays = 0.0;
        if (!configuration.OnlyWorkingDays)
        {
            totalDays = (configuration.AnalysisEndDate.Date - configuration.AnalysisStartDate.Date).TotalDays + 1.0;
        }
        else
        {

            var currentDate = configuration.AnalysisStartDate.Date;
            while (currentDate <= configuration.AnalysisEndDate)
            {
                if (!IsWeekend(currentDate))
                {
                    totalDays += 1.0;
                }
                currentDate = currentDate.AddDays(1);
            }
        }

        return TimeSpan.FromMinutes(totalDays * (configuration.CoreHoursEndTime - configuration.CoreHoursStartTime).TotalMinutes);
    }

    private Dictionary<string, TimeSpan> CalculateTotalDurationPerCategory(IDictionary<string, List<Meeting>> meetingGroups)
    {
        var result = new Dictionary<string, TimeSpan>();

        foreach(var pair in meetingGroups)
        {
            var sumOfMeetingsDuration = TimeSpan.Zero;

            foreach (var meeting in pair.Value)
            {
                var coreHoursStart = meeting.StartDateTime.Date.Add(configuration.CoreHoursStartTime);
                var trimmedMeetingStartDateTime = meeting.StartDateTime < coreHoursStart ? coreHoursStart : meeting.StartDateTime;
                var coreHoursEnd = meeting.StartDateTime.Date.Add(configuration.CoreHoursEndTime);
                var trimmedMeetingEndDateTime = meeting.EndDateTime > coreHoursEnd ? coreHoursEnd : meeting.EndDateTime;

                var meetingDuration = trimmedMeetingEndDateTime - trimmedMeetingStartDateTime;

                sumOfMeetingsDuration += meetingDuration;
            }

            result.Add(pair.Key, sumOfMeetingsDuration);
        }

        return result;
    }

    private bool IsWeekend(DateTime date) => date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
}
