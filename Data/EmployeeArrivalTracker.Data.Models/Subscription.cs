namespace EmployeeArrivalTracker.Data.Models
{
    using System;
    using System.Text.Json;

    public class Subscription
    {
        public string Token { get; set; }

        public DateTime Expires { get; set; }

        public static Subscription FromJson(string json)
        {
            return JsonSerializer.Deserialize<Subscription>(json);
        }
    }
}
