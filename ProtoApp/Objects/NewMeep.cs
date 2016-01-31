using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoApp.Objects
{
    public class NewMeep
    {
        [JsonProperty(PropertyName="message")]
        public string Message { get; set; }


        [JsonProperty(PropertyName = "files")]
        public List<FileObject> Files { get; set; }
    }
}
