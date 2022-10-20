using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flow.Launcher.Plugin.Raindrop.Core
{
    public class RaindropResponse
    {
        public bool Result { get; set; }
        public Raindrop[] Items { get; set; }
        public int Count { get; set; }
        public int CollectionId { get; set; }
    }
}
