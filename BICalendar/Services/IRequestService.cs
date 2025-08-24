namespace BICalendar.Services
{
    public interface IRequestService
    {
        Task<string> GetAsync(string apiEndpoint, object requestObject);
    }
}
