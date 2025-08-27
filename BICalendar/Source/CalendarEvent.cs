namespace BICalendar
{
    public class CalendarEvent
    {
        public string id { get; set; }
        public string language { get; set; }
        public string title { get; set; }
        public string location { get; set; }
        public string filterList { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public string url { get; set; }
        public string imageUrl { get; set; }
        public string imageText { get; set; }
        public bool bothLanguages { get; set; }
    }
}