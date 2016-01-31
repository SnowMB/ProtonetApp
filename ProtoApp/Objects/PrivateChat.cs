using Newtonsoft.Json;
using System.Collections.Generic;

namespace ProtoApp.Objects
{
    public class PrivateChat : UpdateableObject
    {
        [JsonProperty(PropertyName = "notification_id")]
        public int NotificationID { get; set; }

        [JsonProperty(PropertyName = "last_meep")]
        public Meep LastMeep { get; set; }

        [JsonProperty(PropertyName = "last_meep_date")]
        public string LastMeepDate { get; set; }

        [JsonProperty(PropertyName = "current_meep_number")]
        public int CurrentMeepNumber { get; set; }

        [JsonProperty(PropertyName = "meeps_url")]
        public string MeepsUrl { get; set; }

        //[JsonProperty(PropertyName = "subscription_url")]
        //public string SubsciptionsURL { get; set; }

        [JsonProperty(PropertyName = "other_users")]
        public List<User> OtherUsers { get; set; }
    }
}