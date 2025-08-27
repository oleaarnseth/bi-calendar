using BICalendar.Models;

namespace BICalendar.Services.Interfaces
{
    public interface ICalendarEventService
    {
        Task<IEnumerable<CalendarEvent>> GetCalendarEventsAsync(CalendarEventsQuery calendarEventsQuery);
    }
}
