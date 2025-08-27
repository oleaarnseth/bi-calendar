using BICalendar.Models;
using BICalendar.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace BICalendar.Endpoints
{
    public static class CalendarEventEndpoints
    {
        public static IEndpointRouteBuilder MapCalendarEventEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api");

            group.MapPost("/bi-calendar-events", [Authorize(Policy = "AuthZPolicy")] async (CalendarEventsQuery calendarEventsQuery,
                ICalendarEventService calendarEventService) =>
            {
                // Send request
                var result = await calendarEventService.GetCalendarEventsAsync(calendarEventsQuery);

                // Return result
                return Results.Ok(result);
            })
            .WithName("GetBICalendarEvents") // operationId in Swagger
            .WithTags("Calendar Events")   // groups endpoints in Swagger
            .Accepts<CalendarEventsQuery>("application/json")
            .Produces<IEnumerable<CalendarEvent>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi();

            return routes;
        }
    }
}
