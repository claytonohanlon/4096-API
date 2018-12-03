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
            try
            {
                foreach (SwipeData item in articleData.rootObject)
                {
                    MySqlCommand insertSwipe = conn.CreateCommand();

                    insertSwipe.CommandText = "INSERT INTO ToProcess VALUES(@userName, @articleID, @title, @body, @swipe, @sentiment)";
                    insertSwipe.Parameters.AddWithValue("@userName", item.userName);
                    insertSwipe.Parameters.AddWithValue("@articleID", item.articleID);
                    insertSwipe.Parameters.AddWithValue("@title", item.title);
                    insertSwipe.Parameters.AddWithValue("@body", item.body);
                    insertSwipe.Parameters.AddWithValue("@swipe", item.swipe);
                    insertSwipe.Parameters.AddWithValue("@sentiment", item.sentiment);

                    MySqlCommand insertHist = conn.CreateCommand();

                    insertHist.CommandText = "INSERT INTO ArticleHistory (userName, articleID, title) VALUES (@userName, @articleID, @title)";
                    insertHist.Parameters.AddWithValue("@userName", item.userName);
                    insertHist.Parameters.AddWithValue("@articleID", item.articleID);
                    insertHist.Parameters.AddWithValue("@title", item.title);

                    insertSwipe.ExecuteNonQuery();
                    insertHist.ExecuteNonQuery();

                    insertSwipe.Parameters.Clear();
                    insertHist.Parameters.Clear();
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            conn.Close();
            return Ok();
        }
    }
}
