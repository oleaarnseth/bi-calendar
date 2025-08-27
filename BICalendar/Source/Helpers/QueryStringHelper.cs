using System.Reflection;
using System.Web;

namespace BICalendar.Helpers
{
    public static class QueryStringHelper
    {
        public static string ToQueryString(object obj)
        {
            if (obj == null) return string.Empty;

            var properties = obj.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead)
                .Where(p => p.GetValue(obj, null) != null) // skip null values
                .ToDictionary(
                    p => p.Name.Substring(0, 1).ToLower() + p.Name.Substring(1), // camelCase
                    p => p.GetValue(obj, null)?.ToString()
                );

            var query = HttpUtility.ParseQueryString(string.Empty);
            foreach (var kvp in properties)
            {
                if (!string.IsNullOrWhiteSpace(kvp.Value))
                    query[kvp.Key] = kvp.Value;
            }

            return query.ToString();
        }
    }
}
