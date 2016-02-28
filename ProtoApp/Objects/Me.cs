using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoApp.Objects
{
    public class Me : User
    {
        [JsonProperty(PropertyName = "private_chats_url")]
        public string PrivateChatsUrl { get; set; }

        [JsonProperty(PropertyName = "projects_url")]
        public string ProjectsUrl { get; set; }

        [JsonProperty(PropertyName = "user_url")]
        public string UserUrl { get; set; }

        [JsonProperty(PropertyName = "devices_url")]
        public string DevicesUrl { get; set; }
    }
}
