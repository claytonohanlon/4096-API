using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kolokAPI.Models
{
    public class SwipeData
    {
        public int velocity { get; set; }
        public int duration { get; set; }
        public bool liked { get; set; }
        public bool read { get; set; }
        public int uid { get; set; }
    }
}
