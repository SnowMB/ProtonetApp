using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoApp.Objects
{
    public class DeviceMessage
    {
        [JsonProperty(PropertyName = "user_id")]
        public int UserID { get; set; }

        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }

        [JsonProperty(PropertyName = "platform")]
        public string Platform { get; set; }

        [JsonProperty(PropertyName = "uuid")]
        public Guid Uuid { get; set; }

        [JsonProperty(PropertyName = "model")]
        public string Model { get; set; }

        [JsonProperty(PropertyName = "app_name")]
        public string AppName { get; set; }

        [JsonProperty(PropertyName = "app_version")]
        public string AppVersion { get; set; }
    }
}
