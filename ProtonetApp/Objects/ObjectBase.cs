using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProtonetApp.Objects
{
    public class ObjectBase
    {
        public string Url { get; set; }

        public int ID { get; set; }

        public string CreatedAt { get; set; }

        public string UpdatedAt { get; set; }
    }
}
