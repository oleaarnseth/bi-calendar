using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;

namespace BICalendar.Configuration;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddAzureAdAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        // Add JWT + Microsoft Identity
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(
                options =>
                {
                    configuration.Bind("AzureAd", options);
                    options.TokenValidationParameters.NameClaimType = "name";
                },
                options => { configuration.Bind("AzureAd", options); });

        // Add Authorization policy
        services.AddAuthorization(config =>
        {
            config.AddPolicy("AuthZPolicy", policyBuilder =>
                policyBuilder.Requirements.Add(new ScopeAuthorizationRequirement()
                {
                    RequiredScopesConfigurationKey = "AzureAd:Scopes"
                }));
        });

        return services;
    }
}
