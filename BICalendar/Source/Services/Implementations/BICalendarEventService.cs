using System.Text.Json;
using BICalendar.Models;
using BICalendar.Options;
using BICalendar.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace BICalendar.Services.Implementations
{
    public class BICalendarEventService : ICalendarEventService
    {
        private readonly IRequestService _requestService;
        private readonly IMemoryCache _cache;
        private readonly CalendarOptions _calendarOptions;

        public BICalendarEventService(IRequestService requestService,
            IMemoryCache cache,
            IOptions<CalendarOptions> calendarOptions)
        {
            _requestService = requestService;
            _cache = cache;
            _calendarOptions = calendarOptions.Value;
        }

        public async Task<IEnumerable<CalendarEvent>> GetCalendarEventsAsync(CalendarEventsQuery calendarEventsQuery)
        {
            string cacheKey = calendarEventsQuery.GetCacheKey();

            if (_cache.TryGetValue(cacheKey, out List<CalendarEvent> cachedEvents))
            {
                return cachedEvents;
            }

            string jsonData = await _requestService.GetAsync("/api/calendar-events", calendarEventsQuery);
            var result = JsonSerializer.Deserialize<List<CalendarEvent>>(jsonData);

            // Store result in cache:
            var cacheAbsoluteExpiry = _calendarOptions.CacheAbsoluteExpiryMinutes;
            _cache.Set(cacheKey, result, TimeSpan.FromMinutes(cacheAbsoluteExpiry));

            return result;
        }
    }
}
