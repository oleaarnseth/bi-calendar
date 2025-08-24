using BICalendar.Helpers;
using BICalendar.Options;
using Microsoft.Extensions.Options;

namespace BICalendar.Services
{
    public class BIRequestService : IRequestService
    {
        private readonly HttpClient _httpClient;
        private readonly CalendarOptions _calendarOptions;

        public BIRequestService(HttpClient httpClient,
            IOptions<CalendarOptions> calendarOptions)
        {
            _httpClient = httpClient;
            _calendarOptions = calendarOptions.Value;
        }

        public async Task<string> GetAsync(string apiEndpoint, object requestObject)
        {
            _httpClient.BaseAddress = new Uri(_calendarOptions.CalendarApiBaseAddress);

            var requestUri = new Uri($"{apiEndpoint}?{QueryStringHelper.ToQueryString(requestObject)}", UriKind.Relative);
            var response = await _httpClient.GetAsync(requestUri);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}