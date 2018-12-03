using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Models.QueryModels;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    public class ArticleController : Controller
    {
        [HttpPost]
        public ToReturn GetArticles([FromBody] QueryModel data)
        {
            //if no parameters are submitted, set default values
            var defaultData = new QueryModel();
            if (data.num == 0 || data.not == null)
            {
                defaultData.username = data.username;
                defaultData.num = 3;
                defaultData.not = new List<string>();
            }
            else
            {
                defaultData.username = data.username;
                defaultData.num = data.num;
                defaultData.not = data.not;
            }

            MySqlConnection conn = new MySqlConnection("server=142.93.63.223; " +
                                           "user id=user; " +
                                           "password=gift?FINISH?finland?45; " +
                                           "database=CORE; " +
                                           "persistsecurityinfo=True; " +
                                           "SslMode=none");


            //Query the KeywordWeights table for the keyword count for the specific user.
            string checkForKeywords = "SELECT COUNT(keyword) FROM KeywordWeights WHERE userName = '"+defaultData.username+"'";
            MySqlCommand cmd_checkForKeywords = new MySqlCommand(checkForKeywords, conn);
            conn.Open();
            var checkKeyword_reader = cmd_checkForKeywords.ExecuteReader();
            checkKeyword_reader.Read();
            int checkKeyword_result = (int)(long)checkKeyword_reader[0];


            //If the user doesn't have sufficient keyword data (less than 20 keywords), 
            //return 10 random articles, in english, that are published within the past 8 days
            string query = string.Empty;
            ArticleModel def_random = new ArticleModel();
            ToReturn to_return = new ToReturn() { ret = new List<ArticleModel>() };

            if (checkKeyword_result < 20)
            {
                query = "https://api.newsapi.aylien.com/api/v1/stories?language=en&published_at.start=NOW-8DAYS%2FDAY&published_at.end=NOW&sort_by=recency&per_page="+defaultData.num;
                def_random.topic = "random";
                def_random.query = query;

                to_return.ret.Add(def_random);
                
                /*SAMPLE RESPONSE FOR A RANDOM ARTICLE
                 *{
                 * "ret":
                 *   [
                     * {
                     * "topic":"random",
                     * "query":"https://api.newsapi.aylien.com/api/v1/stories?language=en&published_at.start=NOW-8DAYS%2FDAY&published_at.end=NOW&sort_by=recency"
                     * }
                 *   ]
                 * }
                 */

            }
            else
            {
                /*
                 * logic here for searching keywords in the database
                 * and constructing the query appropriately.
                 * Make sure to add in the number of articles to return
                 */
            }

            //string testOutput = JsonConvert.SerializeObject(to_return);
            //System.Diagnostics.Debug.WriteLine(testOutput);

            return to_return;
        }

        [HttpPost]
        public List<ArticleHistory> GetHistory([FromBody] RequestHistory toSearch)
        {
            List<ArticleHistory> ret = new List<ArticleHistory>();
            MySqlConnection conn = new MySqlConnection("server=142.93.63.223; " +
                                          "user id=user; " +
                                          "password=gift?FINISH?finland?45; " +
                                          "database=CORE; " +
                                          "persistsecurityinfo=True; " +
                                          "SslMode=none");

            MySqlCommand fetchHist = conn.CreateCommand();

            fetchHist.CommandText = "SELECT articleID, timeStamp FROM ArticleHistory WHERE userName=@username";
            fetchHist.Parameters.AddWithValue("@username", toSearch.username);

            conn.Open();

            var dataReader = fetchHist.ExecuteReader();

            if(dataReader.HasRows)
            {
                while(dataReader.Read())
                {
                    ret.Add(new ArticleHistory { articleID = dataReader.GetString(0), title = "placeholder" });
                }
            }
            else
            {
                dataReader.Close();
                ret.Add(new ArticleHistory { articleID = "No rows returned" });
            }

            return ret;
        }


        /*
        public string resultText { get; set; }

        public async Task GetRandom()
        {
            string URL = "https://api.newsapi.aylien.com/api/v1/stories";
            string AppID = "e8e4d527";
            string AppKey = "94645f99e3810d0aa85fc6f5a3ba8726";
            var language = new List<string>() { "en" };


            HttpClient client = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Get, URL);
            request.Headers.Add("X-AYLIEN-NewsAPI-Application-ID", AppID);
            request.Headers.Add("X-AYLIEN-NewsAPI-Application-Key", AppKey);

            var response = await client.SendAsync(request);

            if(response.IsSuccessStatusCode)
            {
                resultText = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine("SUCCESS");
            }
            else
            {
                resultText = "An error has occured..";
                System.Diagnostics.Debug.WriteLine("FAIL");
            }

            System.Diagnostics.Debug.WriteLine("\nTHIS IS THE DATA\n");
            System.Diagnostics.Debug.WriteLine(resultText);
        }
        */
    }
}
