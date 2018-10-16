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
        public List<string> keywords { get; set; }
    }

    public class RootSwipeObject
    {
        public List<SwipeData> rootObject { get; set; }
    }
}

/*

{
   "num":15,
   "not":
    [
        "Trump",
        "Elon Musk",
        "Fax Machines"
    ]
}

``````````````````````````````````````

[
    {
        "topic":"Trump",
        "query":"https://api.newsapi.aylien.com/api/v1/stories?body=Kavanaugh+OR+hurricane&published_at.start=NOW-8DAYS%2FDAY&published_at.end=NOW&categories.id%5B%5D=IAB11-4&categories.taxonomy=iab-qag&language=en&sort_by=recency"
    },
    {
        "topic":"Trump",
        "query":"https://api.newsapi.aylien.com/api/v1/stories?body=Kavanaugh+OR+hurricane&published_at.start=NOW-8DAYS%2FDAY&published_at.end=NOW&categories.id%5B%5D=IAB11-4&categories.taxonomy=iab-qag&language=en&sort_by=recency"
    },
    {
        "topic":"Trump",
        "query":"https://api.newsapi.aylien.com/api/v1/stories?body=Kavanaugh+OR+hurricane&published_at.start=NOW-8DAYS%2FDAY&published_at.end=NOW&categories.id%5B%5D=IAB11-4&categories.taxonomy=iab-qag&language=en&sort_by=recency"
    }
]


*/
