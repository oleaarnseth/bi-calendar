using Xunit;
using Moq;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;
using BICalendar.Options;
using Microsoft.Extensions.Options;
using BICalendar.Models;
using BICalendar.Services.Interfaces;
using BICalendar.Services.Implementations;

namespace BICalendar.Tests
{
    public class BICalendarEventServiceTests
    {
        private readonly MemoryCache _memoryCache;
        private readonly Mock<IRequestService> _requestServiceMock;
        private readonly BICalendarEventService _service;
        private readonly Mock<IOptions<CalendarOptions>> _calendarOptionsMock;

        public BICalendarEventServiceTests()
        {
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            _requestServiceMock = new Mock<IRequestService>();
            _calendarOptionsMock = new Mock<IOptions<CalendarOptions>>();
            _calendarOptionsMock.Setup(o => o.Value)
                .Returns(
                    new CalendarOptions
                    {
                        CalendarApiBaseAddress = "https://bi.no",
                        CacheAbsoluteExpiryMinutes = 2
                    });
            _service = new BICalendarEventService(
                _requestServiceMock.Object,
                _memoryCache,
                _calendarOptionsMock.Object);
        }

        [Fact]
        public async Task GetCalendarEventsAsync_FetchesFromApi_AndCachesResult()
        {
            // Arrange
            var query = new CalendarEventsQuery { Take = 5, Language = "en" };

            var apiResponse = new List<CalendarEvent>
            {
                new CalendarEvent { title = "Test Event 1" },
                new CalendarEvent { title = "Test Event 2" }
            };

            _requestServiceMock
                .Setup(r => r.GetAsync("/api/calendar-events", query))
                .ReturnsAsync(JsonSerializer.Serialize(apiResponse));

            // Act - first call (should hit API)
            var result1 = await _service.GetCalendarEventsAsync(query);

            // Act - second call (should come from cache, not API)
            var result2 = await _service.GetCalendarEventsAsync(query);

            // Assert
            Assert.Equal(2, result1.Count());
            Assert.Equal(2, result2.Count());
            _requestServiceMock.Verify(r => r.GetAsync("/api/calendar-events", query), Times.Once); // only once!
        }
    }
}
