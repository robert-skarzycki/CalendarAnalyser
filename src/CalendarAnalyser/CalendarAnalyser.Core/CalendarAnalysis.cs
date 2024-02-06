using CalendarAnalyser.Core.Configuration;
using CalendarAnalyser.Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CalendarAnalyser.Core;

public class CalendarAnalysis
{
    private readonly CalendarAnalysisConfiguration configuration;
    private readonly List<WorkingDay> workingDays;

    public CalendarAnalysis(CalendarAnalysisConfiguration configuration, ICollection<Meeting> meetings)
    {
        this.configuration = configuration;

        if (meetings == null) { throw new ArgumentNullException(nameof(meetings)); }

        var filteredMeetings = meetings.Where(ShouldMeetingBeIncluded).ToList();

        workingDays = BuildWorkingDays().ToList();

        AddMeetingsToDays(filteredMeetings);
    }
    public Dictionary<DateOnly, IEnumerable<ICalendarResultSlot>> BuildCalendarSlotsPerDay()
    {
        var resultsSlotsPerWorkingDay = workingDays.ToDictionary(wd => wd.Date, wd => wd.Slots.Select(s => new CalendarResultSlot(s.StartTime, string.Join(";", s.Categories)) as ICalendarResultSlot));

        return resultsSlotsPerWorkingDay;
    }

    public ICalendarCategoriesAnalysisResult AnalyzeCategories()
    {
        var totalDurationPerCategory = CalculateTotalDurationPerCategory(workingDays);

        var categoriesAnalysisResult = new CalendarCategoriesAnalysisResult(totalDurationPerCategory);

        return categoriesAnalysisResult;
    }

    private bool ShouldMeetingBeIncluded(Meeting meeting)
    {
        if (configuration.FilterOutAllDayEvents && meeting.IsAllDay) { 
            return false; 
        }
        
        var isLastingMoreThanOneDay = meeting.StartDateTime.Date != meeting.EndDateTime.Date;
        
        if (isLastingMoreThanOneDay) { 
            return false;
        }

        return true;
    }

    private void AddMeetingsToDays(ICollection<Meeting> meetings)
    {
        foreach (var meeting in meetings)
        {
            var category = MeetingsCategoryMatcher.Match(meeting, configuration);
            
            var meetingWorkingDay = workingDays.FirstOrDefault(wd => wd.Date == DateOnly.FromDateTime(meeting.StartDateTime));
            meetingWorkingDay?.AddMeetingWithCategory(meeting, category);
        }
    }

    private IEnumerable<WorkingDay> BuildWorkingDays()
    {
        static bool IsWeekend(DateTime date) => date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;

        var currentDate = configuration.AnalysisStartDate.Date;

        while(currentDate < configuration.AnalysisEndDate.Date)
        {
            if(!configuration.OnlyWorkingDays || !IsWeekend(currentDate))
            {
                yield return new WorkingDay(DateOnly.FromDateTime(currentDate), configuration.TimeResolution, configuration.CoreHoursStartTime, configuration.CoreHoursEndTime);                               
            }

            currentDate = currentDate.AddDays(1);
        }
    }

    private Dictionary<string, TimeSpan> CalculateTotalDurationPerCategory(ICollection<WorkingDay> workingDays)
    {
        var result = configuration.Rules.Select(r => r.Category).Distinct().ToDictionary(category => category, _ => TimeSpan.Zero);
        result.Add(Constants.OtherCategoryName, TimeSpan.Zero);
        result.Add(Constants.FreeCategoryName, TimeSpan.Zero);

        void AddToCategory(string category, TimeSpan duration)
        {
            result[category] = result[category].Add(duration);
        }

        foreach (var workingDay in workingDays)
        {
            foreach (var slot in workingDay.Slots)
            {
                if (slot.Categories.Count == 1)
                {
                    AddToCategory(slot.Categories.First(), configuration.TimeResolution);
                }
                else
                {
                    var durationSplit = configuration.TimeResolution / slot.Categories.Count;
                    foreach (var categoryInSlot in slot.Categories)
                    {
                        AddToCategory(categoryInSlot, durationSplit);
                    }
                }
            }
        }

        return result;
    }
}
