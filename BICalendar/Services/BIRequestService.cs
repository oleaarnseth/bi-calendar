using BICalendar.Helpers;

namespace BICalendar.Services
{
    public class BIRequestService : IRequestService
    {
        private readonly HttpClient _httpClient;

        public BIRequestService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://bi.no");
        }

        public async Task<string> GetAsync(string apiEndpoint, object requestObject)
        {
            var requestUri = new Uri($"{apiEndpoint}?{QueryStringHelper.ToQueryString(requestObject)}", UriKind.Relative);
            var response = await _httpClient.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}