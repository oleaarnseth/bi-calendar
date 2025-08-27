using Azure.Identity;
using BICalendar.Options;
using BICalendar.Services;
using BICalendar.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddHttpClient<IRequestService, BIRequestService>();
builder.Services.AddScoped<ICalendarEventService, BICalendarEventService>();
builder.Services.AddMemoryCache();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<CalendarOptions>(
    builder.Configuration.GetSection("Calendar"));

// Use default credential if debug, and managed identity for release
#if DEBUG
var azureCredential = new DefaultAzureCredential();
#else
var azureCredential = new ManagedIdentityCredential();
#endif

// Add Azure App Configuration 
var appConfigUri = System.Environment.GetEnvironmentVariable("AZURE_APPCONFIG_URI");
builder.Configuration.AddAzureAppConfiguration(options =>
{
    options.Connect(new Uri(appConfigUri), azureCredential);
});

// Add Azure Key Vault
var keyVaultUri = System.Environment.GetEnvironmentVariable("AZURE_KEYVAULT_URI");
builder.Configuration.AddAzureKeyVault(
    new Uri(keyVaultUri),
    azureCredential
);

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

// Map API endpoints
app.MapCalendarEventEndpoints();

app.Run();