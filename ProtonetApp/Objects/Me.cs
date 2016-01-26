using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtonetApp.Objects
{
    public class Me : User
    {
        public string PrivateChatsUrl { get; set; }

        public string ProjectsUrl { get; set; }

        public string UserUrl { get; set; }

        public string DevicesUrl { get; set; }
    }
}
