# bi-calendar

## About
This is a ASP.NET Core minimal API app implementation of the case. Also includes configuration from Azure App Configuration and Azure Key Vault, using managed identity for Release and Default Azure Credentials for Debug. The API is also protected by the Microsoft Identity platform.

## How to run
The app is containerized and can by built by navigating to the folder "\BICalendar\Source" and running the following docker command:

docker build -f Dockerfile -t myazureregistry.azurecr.io/bicalender:test "..\"

## Deploying to a container environment
For the app to work, the following environment settings must be set for the container:

| Env setting name | Description |
| -------- | ------- |
| AZURE_KEYVAULT_URI  | URI to Azure Key Vault  |
| AZURE_APPCONFIG_URI | URI to Azure App Config |

In the Azure App Config or Key Vault, the following settings must be present:

| App Config name | Key Vault name | Description |
| -------- | ------- | ------- |
| AzureAd:ClientId  | AzureAd__ClientId  | Client id of Entra App Registration |
| AzureAd:Instance | AzureAd__Instance | Microsoft Entra authentication endpoint, should be "https://login.microsoftonline.com" for non-US and non-China |
| AzureAd:Scopes | AzureAd__Scopes | The scope that is used to request access to the application |
| AzureAd:TenantId | AzureAd__TenantId | The id of the tenant where the application is registered |
| Calendar:CacheAbsoluteExpiryMinutes | Calendar__CacheAbsoluteExpiryMinutes | Absolute expiry of memory cache in minutes |
| Calendar:CalendarApiBaseAddress | Calendar__CalendarApiBaseAddress | Base address for the BI endpoint, ie. "https://bi.no" |
