using Azure.Identity;

namespace BICalendar.Configuration;

public static class ConfigurationExtensions
{
    public static void AddAzureConfiguration(this IConfigurationManager configuration)
    {
        // Use default credential if debug, and managed identity for release
        #if DEBUG
        var azureCredential = new DefaultAzureCredential();
        #else
        var azureCredential = new ManagedIdentityCredential();
        #endif

        // Add Azure App Configuration
        var appConfigUri = Environment.GetEnvironmentVariable("AZURE_APPCONFIG_URI");
        if (!string.IsNullOrEmpty(appConfigUri))
        {
            configuration.AddAzureAppConfiguration(options =>
            {
                options.Connect(new Uri(appConfigUri), azureCredential);
            });
        }

        // Add Azure Key Vault
        var keyVaultUri = Environment.GetEnvironmentVariable("AZURE_KEYVAULT_URI");
        if (!string.IsNullOrEmpty(keyVaultUri))
        {
            configuration.AddAzureKeyVault(new Uri(keyVaultUri), azureCredential);
        }
    }
}
