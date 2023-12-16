using CalendarAnalyser.Core;

namespace CalendarAnalyser.Extensions.Export.Excel.Tests
{
    public class CalendarAnalyserExcelExporterTests
    {
        private const string DummyCategory = "DummyCategory";
        [Fact]
        public async Task ShouldCreateFileCorrectly()
        {
            var day1 = new DateOnly(2023, 7, 31);
            var day2 = new DateOnly(2023, 8, 1);

            var categoriesResult = new CalendarCategoriesAnalysisResult(new Dictionary<string, TimeSpan>()
                    {
                        { DummyCategory, new TimeSpan (1, 30, 0) },
                        { Constants.FreeCategoryName, new TimeSpan (3, 30, 0) }
                    });


            var result = new CalendarAnalysisResult(
            categoriesResult,new Dictionary<DateOnly, IEnumerable<Core.Results.ICalendarResultSlot>>{
                {
                    day1,
                    new[] {
                    CalendarSlot(9, 0, Constants.FreeCategoryName),
                    CalendarSlot(9, 30, DummyCategory),
                    CalendarSlot(10, 0, DummyCategory),
                    CalendarSlot(10, 30, Constants.FreeCategoryName),
                    CalendarSlot(11, 0, Constants.FreeCategoryName)
                    }},
                    {
                    day2,
                    new[] {
                    CalendarSlot(9, 0, Constants.FreeCategoryName),
                    CalendarSlot(9, 30, Constants.FreeCategoryName),
                    CalendarSlot(10, 0, DummyCategory),
                    CalendarSlot(10, 30, Constants.FreeCategoryName),
                    CalendarSlot(11, 0, Constants.FreeCategoryName)
                    } }
                });

            await CalendarAnalyserExcelExporter.Export("test.xlsx", result);
        }

        private CalendarResultSlot CalendarSlot(int hours, int minutes, string category) => new(new TimeOnly(hours, minutes), category);
    }
}