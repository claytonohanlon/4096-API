using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kolokAPI.Models
{
    public class SwipeData
    {
        public string userName{ get; set; }
        //May need to change Data Type 
        public long articleID { get; set; }
        public string title { get; set; }
        public string body{ get; set; }
        public int swipe { get; set; }
        public float sentiment { get; set; }
    }
}
