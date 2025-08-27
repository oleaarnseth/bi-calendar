using BICalendar.Options;
using BICalendar.Services.Implementations;
using BICalendar.Services.Interfaces;

namespace BICalendar.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBICalendarServices(this IServiceCollection services, IConfiguration configuration)
    {
        // App services
        services.AddHttpClient<IRequestService, BIRequestService>();
        services.AddScoped<ICalendarEventService, BICalendarEventService>();
        services.AddMemoryCache();

        // Swagger & API Explorer
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        // Options
        services.Configure<CalendarOptions>(configuration.GetSection("Calendar"));

        return services;
    }
}
