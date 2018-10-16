using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Models.SwipeModels;
using MySql.Data.MySqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    public class SwipeController : Controller
    {
        [HttpPost]
        public IActionResult PostSwipeData([FromBody] RootSwipeObject articleData)
        {
            MySqlConnection conn = new MySqlConnection("server=142.93.63.223; " +
                                           "user id=user; " +
                                           "password=gift?FINISH?finland?45; " +
                                           "database=CORE; " +
                                           "persistsecurityinfo=True; " +
                                           "SslMode=none");

            conn.Open();
            foreach (SwipeData item in articleData.rootObject)
            {
                //System.Diagnostics.Debug.WriteLine("{0} {1} {2} {3} {4} {5}\n", item.userName, item.articleID, item.title, item.body, item.swipe, item.sentiment);
                string _userName = item.userName;
                long _articleID = item.articleID;
                string _title = item.title;
                string _body = item.body;
                int _swipe = item.swipe;
                float _sentiment = item.sentiment;

                //SQL query to insert one SwipeData object into the database
                //This query will be called multiple times for each article object sent in articleData
                string insertData = "INSERT INTO ToProcess VALUES(" + "'" + _userName + "'," +
                                                                        _articleID + "," +
                                                                  "'" + _title + "','" +
                                                                        _body + "'," +
                                                                        _swipe + "," +
                                                                        _sentiment +
                                                                 ")";

                //SQL query to insert the above entry into the historical table
                string insertHistory = "INSERT INTO ArticleHistory (userName, articleID) VALUES(" + "'" + _userName + "'," +
                                                                                                          _articleID + ")";

                MySqlCommand cmd_insertSwipeData = new MySqlCommand(insertData, conn);
                MySqlCommand cmd_insertSwipeHisrory = new MySqlCommand(insertHistory, conn);

                cmd_insertSwipeData.ExecuteNonQuery();
                cmd_insertSwipeHisrory.ExecuteNonQuery();

                foreach(string keyword in item.keywords)
                {
                    string _keyword = keyword;
                    string insertKeyword = "INSERT INTO ToProcessKeywords VALUES('" + _userName + "','" + _articleID + "','" + _keyword + "')";

                    MySqlCommand cmd_insertKeyword = new MySqlCommand(insertKeyword, conn);
                    cmd_insertKeyword.ExecuteNonQuery();
                }
            }
            conn.Close();

            return Ok();
        }
    }
}
