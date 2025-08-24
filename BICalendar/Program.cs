using BICalendar;
using BICalendar.Services;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Register services:
builder.Services.AddHttpClient<IRequestService, BIRequestService>();
builder.Services.AddScoped<ICalendarEventService, BICalendarEventService>();
builder.Services.AddMemoryCache();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

//app.MapGet("/runtime", async () =>
//{
//    // Resolve service via DI
//    var requestService = app.Services.GetRequiredService<IRequestService>();
//    string url = "https://api.github.com/repos/dotnet/runtime";
//    string result = await requestService.GetAsync(url);

//    return result;
//});

app.MapPost("/api/bi-calendar-events", async (CalendarEventsQuery calendarEventsQuery,
    [FromServices] ICalendarEventService calendarEventService) =>
{
    // Send request
    var result = await calendarEventService.GetCalendarEventsAsync(calendarEventsQuery);

    // Return result
    return Results.Ok(result);
});

app.Run();
