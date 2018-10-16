using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.QueryModels
{

    public class QueryModel
    {
        public string username { get; set; }
        public int num { get; set; }
        public List<string> not { get; set; }
    }


    public class ArticleModel
    {
        public string topic { get; set; }
        public string query { get; set; }
    }

    public class ToReturn
    {
        public List<ArticleModel> ret { get; set; }
    }


}
