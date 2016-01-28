using Newtonsoft.Json;
using System.Collections.Generic;

namespace ProtoApp.Objects
{
    public class PrivateChatsContainer
    {
        [JsonProperty(PropertyName = "private_chats")]
        public List<PrivateChat> Chats { get; set; }
    }
}