using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtoApp.Objects
{
    public class ObjectBase
    {
        public string Url { get; set; }

        public int ID { get; set; }

        [JsonProperty(PropertyName = "created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty(PropertyName = "updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
