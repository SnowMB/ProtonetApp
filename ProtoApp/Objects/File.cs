using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ProtoApp.Objects
{
    public class FileObject : ProtonetObject
    {
        public string Url { get; set; }

        [JsonProperty(PropertyName = "thumbnail_url")]
        public string ThumbnailUrl { get; set; }


        public string Type { get; set; }

    }
}
