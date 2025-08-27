namespace BICalendar
{
    public class CalendarEventsQuery
    {
        public int Take { get; set; } = 5;
        public string Language { get; set; } = "all";
        public string Campus { get; set; }
        public string Audience { get; set; }

        public string GetCacheKey()
        {
            // Normalize string properties (null -> "none", trim, lowercase)
            string Normalize(string? value) =>
                string.IsNullOrWhiteSpace(value) ? "none" : value.Trim().ToLowerInvariant();

            return $"CalendarEvents_" +
                   $"take:{Take}_" +
                   $"lang:{Normalize(Language)}_" +
                   $"campus:{Normalize(Campus)}_" +
                   $"aud:{Normalize(Audience)}";
        }
    }
}
