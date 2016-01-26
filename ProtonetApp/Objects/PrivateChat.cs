using System.Collections.Generic;

namespace ProtonetApp.Objects
{
    public class PrivateChat : ObjectBase
    {
        public string NotificationID { get; set; }

        public Meep LastMeep { get; set; }

        public string LastMeepDate { get; set; }

        public int CurrentMeepNumber { get; set; }

        public string MeepsUrl { get; set; }

        public string SubsciptionsURL { get; set; }

        public string NotificationsCount { get; set; }

        public List<User> OtherUsers { get; set; }
    }
}