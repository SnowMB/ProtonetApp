using Newtonsoft.Json;

namespace ProtoApp.Objects
{
    public class User : UpdateableObject
    {
        [JsonProperty(PropertyName = "first_name")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName = "last_name")]
        public string LastName { get; set; }

        public string Email { get; set; }

        public string Avatar { get; set; }

        public string Role { get; set; }

        public bool Deactivated { get; set; }

        public string UserName { get; set; }

        [JsonProperty(PropertyName = "last_active_at")]
        public string LastActiveAt { get; set; }

        public bool Online { get; set; }
    }
}