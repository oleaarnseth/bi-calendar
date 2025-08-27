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

        [Fact]
        public async Task GetCalendarEventsAsync_ReturnsEmpty_WhenApiReturnsEmpty()
        {
            // Arrange
            var query = new CalendarEventsQuery { Take = 5, Language = "en" };
            _requestServiceMock
                .Setup(r => r.GetAsync("/api/calendar-events", query))
                .ReturnsAsync("[]");

            // Act
            var result = await _service.GetCalendarEventsAsync(query);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetCalendarEventsAsync_DifferentQueries_AreCachedSeparately()
        {
            // Arrange
            var query1 = new CalendarEventsQuery { Take = 5, Language = "en" };
            var query2 = new CalendarEventsQuery { Take = 10, Language = "no" };

            var apiResponse1 = new List<CalendarEvent> { new CalendarEvent { title = "Event 1" } };
            var apiResponse2 = new List<CalendarEvent> { new CalendarEvent { title = "Event 2" } };

            _requestServiceMock
                .Setup(r => r.GetAsync("/api/calendar-events", query1))
                .ReturnsAsync(JsonSerializer.Serialize(apiResponse1));

            _requestServiceMock
                .Setup(r => r.GetAsync("/api/calendar-events", query2))
                .ReturnsAsync(JsonSerializer.Serialize(apiResponse2));

            // Act
            var result1 = await _service.GetCalendarEventsAsync(query1);
            var result2 = await _service.GetCalendarEventsAsync(query2);

            // Assert
            Assert.Single(result1);
            Assert.Single(result2);
            Assert.NotEqual(result1.First().title, result2.First().title);
        }

        [Fact]
        public async Task GetCalendarEventsAsync_CacheExpires_AllowsRefetch()
        {
            // Arrange
            var query = new CalendarEventsQuery { Take = 5, Language = "en" };
            var apiResponse = new List<CalendarEvent> { new CalendarEvent { title = "Event" } };

            _requestServiceMock
                .Setup(r => r.GetAsync("/api/calendar-events", query))
                .ReturnsAsync(JsonSerializer.Serialize(apiResponse));

            // Act - First fetch (cached)
            var result1 = await _service.GetCalendarEventsAsync(query);

            // Simulate cache expiry by clearing it
            _memoryCache.Compact(1.0);

            // Fetch again (should call API again)
            var result2 = await _service.GetCalendarEventsAsync(query);

            // Assert
            _requestServiceMock.Verify(r => r.GetAsync("/api/calendar-events", query), Times.Exactly(2));
        }
    }
}
