using CalendarAnalyser.Core;

namespace CalendarAnalyser.Extensions.Export.Excel.Tests
{
    public class CalendarAnalyserExcelExporterTests
    {
        private const string DummyCategory = "DummyCategory";
        [Fact]
        public async Task ShouldCreateFileCorrectly()
        {
            var day1 = new DateTime(2023, 7, 31);
            var day2 = new DateTime(2023, 8, 1);
            var result = new CalendarAnalysisResult
            {
                CategoriesAnalysis =
                    new CalendarCategoriesAnalysisResult(new Dictionary<string, TimeSpan>()
                    {
                        { DummyCategory, new TimeSpan (1, 30, 0) }
                    }, TimeSpan.FromHours(5)),
                CalendarSlots = new[]
                {
                    CalendarSlot(day1, 9, 0, Constants.FreeCategoryName),
                    CalendarSlot(day1, 9, 30, DummyCategory),
                    CalendarSlot(day1, 10, 0, DummyCategory),
                    CalendarSlot(day1, 10, 30, Constants.FreeCategoryName),
                    CalendarSlot(day1, 11, 0, Constants.FreeCategoryName),
                    CalendarSlot(day2, 9, 0, Constants.FreeCategoryName),
                    CalendarSlot(day2, 9, 30, Constants.FreeCategoryName),
                    CalendarSlot(day2, 10, 0, DummyCategory),
                    CalendarSlot(day2, 10, 30, Constants.FreeCategoryName),
                    CalendarSlot(day2, 11, 0, Constants.FreeCategoryName),
                }
            };

            await CalendarAnalyserExcelExporter.Export("test.xlsx", result);
        }

        private CalendarSlot CalendarSlot(DateTime date, int hours, int minutes, string category) => new(date.AddHours(hours).AddMinutes(minutes), category);
    }
}