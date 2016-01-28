using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoApp.Objects
{
    public class PrivateChatContainer
    {
        [JsonProperty(PropertyName = "private_chat")]
        public PrivateChat Chat { get; set; }
    }
}
