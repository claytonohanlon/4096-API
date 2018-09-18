using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kolokAPI.Models
{
    public class Summary
    {
        public List<string> sentences { get; set; }
    }

    public class Location
    {
        public string country { get; set; }
        public string state { get; set; }
        public string city { get; set; }
    }

    public class Scope
    {
        public string country { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string level { get; set; }
    }

    public class Alexa
    {
        public int rank { get; set; }
        public DateTime fetched_at { get; set; }
        public string country { get; set; }
    }

    public class Rankings
    {
        public List<Alexa> alexa { get; set; }
    }

    public class Source
    {
        public int id { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int links_in_count { get; set; }
        public string home_page_url { get; set; }
        public string domain { get; set; }
        public List<Location> locations { get; set; }
        public List<Scope> scopes { get; set; }
        public Rankings rankings { get; set; }
    }

    public class Author
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Title
    {
        public string text { get; set; }
        public List<string> types { get; set; }
        public List<List<int>> indices { get; set; }
    }

    public class Links
    {
        public string dbpedia { get; set; }
    }

    public class Body
    {
        public string text { get; set; }
        public double score { get; set; }
        public List<string> types { get; set; }
        public Links links { get; set; }
        public List<List<int>> indices { get; set; }
    }

    public class Entities
    {
        public List<Title> title { get; set; }
        public List<Body> body { get; set; }
    }

    public class Links2
    {
        public string self { get; set; }
        public string parent { get; set; }
    }

    public class Category
    {
        public string id { get; set; }
        public string taxonomy { get; set; }
        public int level { get; set; }
        public double score { get; set; }
        public bool confident { get; set; }
        public Links2 links { get; set; }
    }

    public class SocialSharesCount
    {
        public List<object> facebook { get; set; }
        public List<object> google_plus { get; set; }
        public List<object> linkedin { get; set; }
        public List<object> reddit { get; set; }
    }

    public class Title2
    {
        public string polarity { get; set; }
        public double score { get; set; }
    }

    public class Body2
    {
        public string polarity { get; set; }
        public double score { get; set; }
    }

    public class Sentiment
    {
        public Title2 title { get; set; }
        public Body2 body { get; set; }
    }

    public class Links3
    {
        public string permalink { get; set; }
        public string related_stories { get; set; }
        public string coverages { get; set; }
    }

    public class RootObject
    {
        public int id { get; set; }
        public string title { get; set; }
        public string body { get; set; }
        public Summary summary { get; set; }
        public Source source { get; set; }
        public Author author { get; set; }
        public Entities entities { get; set; }
        public List<string> keywords { get; set; }
        public List<string> hashtags { get; set; }
        public int characters_count { get; set; }
        public int words_count { get; set; }
        public int sentences_count { get; set; }
        public int paragraphs_count { get; set; }
        public List<Category> categories { get; set; }
        public SocialSharesCount social_shares_count { get; set; }
        public List<object> media { get; set; }
        public Sentiment sentiment { get; set; }
        public string language { get; set; }
        public DateTime published_at { get; set; }
        public Links3 links { get; set; }
    }
}
