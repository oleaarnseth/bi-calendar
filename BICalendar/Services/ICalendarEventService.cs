namespace BICalendar.Services
{
    public interface ICalendarEventService
    {
        Task<IEnumerable<CalendarEvent>> GetCalendarEventsAsync(CalendarEventsQuery calendarEventsQuery);
    }
}
