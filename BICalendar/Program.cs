using Azure.Identity;
using BICalendar;
using BICalendar.Options;
using BICalendar.Services;
using BICalendar.Endpoints;

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

//var keyvaultName = System.Environment.GetEnvironmentVariable("AZURE_KEYVAULT_NAME");

// Load secrets from Azure Key Vault
#if DEBUG
builder.Configuration.AddAzureKeyVault(
    new Uri($"https://ole-test-keyvault.vault.azure.net/"),
    new DefaultAzureCredential());
#else
builder.Configuration.AddAzureKeyVault(
    new Uri($"https://{keyvaultName}.vault.azure.net/"),
    new EnvironmentCredential());
#endif

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

app.MapCalendarEventEndpoints();

app.Run();
