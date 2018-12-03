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
            if (data.not == null)
            {
                defaultData.not = new List<string>();
                if (data.num == 0)
                    defaultData.num = 3;
                else
                    defaultData.num = data.num;

                defaultData.username = data.username;
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
            MySqlCommand checkForKeywords = conn.CreateCommand();
            checkForKeywords.CommandText = "SELECT COUNT(keyword) FROM KeywordWeights WHERE userName = @userName";
            checkForKeywords.Parameters.AddWithValue("@userName", defaultData.username);

            conn.Open();
            var checkKeyword_reader = checkForKeywords.ExecuteReader();
            checkKeyword_reader.Read();
            int checkKeyword_result = (int)(long)checkKeyword_reader[0];
            checkKeyword_reader.Close();


            //If the user doesn't have sufficient keyword data (less than 100 keywords), 
            //return x(num) random articles, in english, that are published within the past 8 days
            string query = string.Empty;
            ArticleModel result = new ArticleModel();
            ToReturn to_return = new ToReturn() { ret = new List<ArticleModel>() };

            if (checkKeyword_result < 100)
            {
                query = "https://api.newsapi.aylien.com/api/v1/stories?language=en&published_at.start=NOW-8DAYS%2FDAY&published_at.end=NOW&sort_by=recency&per_page="+defaultData.num;
                result.topic = "random";
                result.query = query;

                to_return.ret.Add(result);
                
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

                //grab random keyword from the top 15
                MySqlCommand getTop = conn.CreateCommand();
                getTop.CommandText = "SELECT keyword FROM (" +
                                        "SELECT * FROM KeywordWeights " +
                                         "WHERE userName = @userName " +
                                         "ORDER BY weight DESC " +
                                         "LIMIT 25) _t " +
                                     "ORDER BY RAND() " +
                                     "LIMIT 1";
                getTop.Parameters.AddWithValue("@userName", defaultData.username);

                var topResult = getTop.ExecuteReader();
                topResult.Read();
                string topKeyword = topResult.GetString(0);
                topResult.Close();

                string topQuery = "https://api.newsapi.aylien.com/api/v1/stories?language=en&published_at.start=NOW-8DAYS%2FDAY&published_at.end=NOW&sort_by=recency&per_page=" + 
                         "3" + "&text="+ topKeyword;
                result.topic = topKeyword;
                result.query = topQuery;

                to_return.ret.Add(result);
                result = new ArticleModel();


                //get random keyword for 'neutral' status
                MySqlCommand getN = conn.CreateCommand();
                getN.CommandText = "SELECT keyword FROM CORE.KeywordWeights WHERE userName = @userName ORDER BY RAND() LIMIT 1";
                getN.Parameters.AddWithValue("@userName", defaultData.username);

                var nResult = getN.ExecuteReader();
                nResult.Read();
                string nKeyword = nResult.GetString(0);
                nResult.Close();

                string nQuery = "https://api.newsapi.aylien.com/api/v1/stories?language=en&published_at.start=NOW-8DAYS%2FDAY&published_at.end=NOW&sort_by=recency&per_page=" +
                         "3" + "&text=" + nKeyword;
                result.topic = nKeyword;
                result.query = nQuery;

                to_return.ret.Add(result);
                result = new ArticleModel();


                //grab random keyword from the bottom 15
                MySqlCommand getBot = conn.CreateCommand();
                getBot.CommandText = "SELECT keyword FROM (" +
                                        "SELECT * FROM KeywordWeights " +
                                         "WHERE userName = @userName " +
                                         "ORDER BY weight ASC " +
                                         "LIMIT 25) _t " +
                                     "ORDER BY RAND() " +
                                     "LIMIT 1";
                getBot.Parameters.AddWithValue("@userName", defaultData.username);

                var botResult = getBot.ExecuteReader();
                botResult.Read();
                string botKeyword = botResult.GetString(0);
                botResult.Close();

                string botQuery = "https://api.newsapi.aylien.com/api/v1/stories?language=en&published_at.start=NOW-8DAYS%2FDAY&published_at.end=NOW&sort_by=recency&per_page=" +
                         "3" + "&text=" + botKeyword;
                result.topic = botKeyword;
                result.query = botQuery;

                to_return.ret.Add(result);
                result = new ArticleModel();

            }
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

            fetchHist.CommandText = "SELECT articleID, timeStamp, title FROM ArticleHistory WHERE userName=@username";
            fetchHist.Parameters.AddWithValue("@username", toSearch.username);

            conn.Open();

            var dataReader = fetchHist.ExecuteReader();

            if(dataReader.HasRows)
            {
                while(dataReader.Read())
                {
                    ArticleHistory data = new ArticleHistory();
                    if(dataReader.IsDBNull(2))
                    {
                        data.articleID = dataReader.GetString(0);
                        data.timeStamp = dataReader.GetString(1);
                        data.title = "No Title Found";
                    }
                    else
                    {
                        data.articleID = dataReader.GetString(0);
                        data.timeStamp = dataReader.GetString(1);
                        data.title = dataReader.GetString(2);
                    }
                    ret.Add(data);
                }
            }
            else
            {
                dataReader.Close();
                ret.Add(new ArticleHistory { articleID = "No rows returned" });
            }

            return ret;
        }
    }
}
