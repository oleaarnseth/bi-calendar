namespace BICalendar.Services.Interfaces
{
    public interface IRequestService
    {
        Task<string> GetAsync(string apiEndpoint, object requestObject);
    }
}
