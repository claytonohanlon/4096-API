using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.QueryModels
{
    public class ArticleHistory
    {
        public string articleID { get; set; }
        public string title { get; set; }
    }

    public class RequestHistory
    {
        public string username { get; set; }
    }
}
