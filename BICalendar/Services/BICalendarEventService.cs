using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;

namespace BICalendar.Services
{
    public class BICalendarEventService : ICalendarEventService
    {
        private readonly IRequestService _requestService;
        private readonly IMemoryCache _cache;

        public BICalendarEventService(IRequestService requestService,
            IMemoryCache cache)
        {
            _requestService = requestService;
            _cache = cache;
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

            _cache.Set(cacheKey, result, TimeSpan.FromMinutes(5));

            return result;
        }
    }
}
