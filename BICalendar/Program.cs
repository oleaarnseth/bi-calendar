using Azure.Identity;
using BICalendar;
using BICalendar.Options;
using BICalendar.Services;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Register services:
builder.Services.AddHttpClient<IRequestService, BIRequestService>();
builder.Services.AddScoped<ICalendarEventService, BICalendarEventService>();
builder.Services.AddMemoryCache();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<CalendarOptions>(
    builder.Configuration.GetSection("Calendar"));

// Load configuration from Azure App Configuration 
builder.Configuration.AddAzureAppConfiguration(options =>
{
    options.Connect(new Uri("https://ole-test.azconfig.io"), new DefaultAzureCredential());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty; // Use swagger UI as start page when debugging
    });
}

//app.MapGet("/", () => "Hello World!");

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
})
.WithName("GetBICalendarEvents") // operationId in Swagger
.WithTags("Calendar Events")   // groups endpoints in Swagger
.Accepts<CalendarEventsQuery>("application/json")
.Produces<IEnumerable<CalendarEvent>>(StatusCodes.Status200OK)
.Produces(StatusCodes.Status400BadRequest)
.WithOpenApi();

app.Run();
