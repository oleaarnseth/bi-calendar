using BICalendar.Endpoints;
using BICalendar.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Authentication and authorization
builder.Services.AddAzureAdAuthentication(builder.Configuration);

// Business services, Swagger, options
builder.Services.AddBICalendarServices(builder.Configuration);

// Azure Config and Key Vault
builder.Configuration.AddAzureConfiguration();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

// Swagger Config
app.UseConfiguredSwagger(app.Environment);

// API endpoints
app.MapCalendarEventEndpoints();

app.Run();