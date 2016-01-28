﻿using Newtonsoft.Json;

namespace ProtoApp.Objects
{
    public class Meep : ObjectBase
    {
        public string Type { get; set; }

        [JsonProperty(PropertyName = "no")]
        public int Number { get; set; }

        public string Message { get; set; }

        public User User { get; set; }
    }
}