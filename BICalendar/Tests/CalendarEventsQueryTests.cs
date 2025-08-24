using Xunit;

namespace BICalendar.Tests
{
    public class CalendarEventsQueryTests
    {
        [Theory]
        [InlineData(5, "en", "Main", "Staff", "CalendarEvents_take:5_lang:en_campus:main_aud:staff")]
        [InlineData(10, "EN", " main ", null, "CalendarEvents_take:10_lang:en_campus:main_aud:none")]
        [InlineData(3, null, null, null, "CalendarEvents_take:3_lang:none_campus:none_aud:none")]
        public void GetCacheKey_ReturnsExpectedKey(
            int take, string language, string campus, string audience, string expectedKey)
        {
            // Arrange
            var query = new CalendarEventsQuery
            {
                Take = take,
                Language = language,
                Campus = campus,
                Audience = audience
            };

            // Act
            string actualKey = query.GetCacheKey();

            // Assert
            Assert.Equal(expectedKey, actualKey);
        }
    }
}
