using System.Collections.Generic;

namespace ProtoApp.Objects
{
    public class PrivateChat : ObjectBase
    {
        public int NotificationID { get; set; }

        public Meep LastMeep { get; set; }

        public string LastMeepDate { get; set; }

        public int CurrentMeepNumber { get; set; }

        public string MeepsUrl { get; set; }

        public string SubsciptionsURL { get; set; }

        public int NotificationsCount { get; set; }

        public List<User> OtherUsers { get; set; }
    }
}