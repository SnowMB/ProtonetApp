using Newtonsoft.Json;
using System.Collections.Generic;

namespace ProtoApp.Objects
{
    public class Meep : UpdateableObject
    {
        public string Type { get; set; }

        [JsonProperty(PropertyName = "no")]
        public int Number { get; set; }

        public string Message { get; set; }

        public User User { get; set; }

        public List<FileObject> Files { get; set; }
    }
}