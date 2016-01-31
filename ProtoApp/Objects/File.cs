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

        [OnDeserialized]
        internal void OnDeserialized (StreamingContext context) //Angaben der Bilder sind sonst falsch!!
        {
            var relativeUri = "api/v1/";
            Url = Url.Replace(relativeUri, "");
            ThumbnailUrl = ThumbnailUrl.Replace(relativeUri, "");
        }

    }
}
