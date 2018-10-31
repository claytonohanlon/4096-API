using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.SwipeModels
{
    public class SwipeData
    {
        public string userName { get; set; }
        public long articleID { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public int swipe { get; set; }
        public float sentiment { get; set; }
        //public List<string> keywords { get; set; }
    }

    public class RootSwipeObject
    {
        public List<SwipeData> rootObject { get; set; }
    }
}
