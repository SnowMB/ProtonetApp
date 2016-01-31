using Newtonsoft.Json;

namespace ProtoApp.Objects
{ 
    public class TokenResponse : UpdateableObject
    {
        public string Token { get; set; }

        [JsonProperty(PropertyName = "user_id")]
        public int UserID { get; set; }

        [JsonProperty(PropertyName = "owner_id")]
        public int OwnerID { get; set; }

        [JsonProperty(PropertyName = "owner_type")]
        public string OwnerType { get; set; }

        public string Comment { get; set; }


    }
}