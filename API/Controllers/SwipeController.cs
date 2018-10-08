using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using API.Models;
using kolokAPI.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    public class SwipeController : Controller
    {
        [HttpPost]
        public IActionResult PostSwipeData([FromBody] SwipeData articleData)
        {
            MySqlConnection conn = new MySqlConnection("server=142.93.63.223; " +
                                           "user id=user; " +
                                           "password=gift?FINISH?finland?45; " +
                                           "database=CORE; " +
                                           "persistsecurityinfo=True; " +
                                           "SslMode=none");
            //May need to add a semicolon at the end of the query if it bars out
            string insertData = "INSERT INTO ToProcess VALUES(" + "'" + articleData.userName + "'," +
                                                                articleData.articleID + "," +
                                                                "'" + articleData.title + "','" +
                                                                articleData.body + "'," +
                                                                articleData.swipe + "," +
                                                                articleData.sentiment +
                                                            ")";
            string insertHistory = "INSERT INTO ArticleHistory (userName, articleID) VALUES(" + "'" + articleData.userName + "'," +
                                                                articleData.articleID + ")";
            MySqlCommand cmd_insertSwipeData = new MySqlCommand(insertData, conn);
            MySqlCommand cmd_insertSwipeHisrory = new MySqlCommand(insertHistory, conn);
            //Checks if Primary Key (User ID and article ID) is null, if so there is an invalid request so return a bad one
            System.Diagnostics.Debug.WriteLine("Name:" + articleData.userName +",ArtID:" +articleData.articleID+ 
                ",Title:"+ articleData.title + ",Body:" + articleData.body +
                ",Swipe:" + articleData.swipe + ",Sentiment:" + articleData.sentiment);
            if (articleData == null)
            {
                return BadRequest();
            }
            conn.Open();
            if (articleData.userName == null || articleData.articleID == 0)
            {
                return BadRequest();
            }
            //If the primary key is valid, add to insertData string into database 
            var dataReader_swipe = cmd_insertSwipeData.ExecuteReader();
            dataReader_swipe.Close();
            var dataReader_history = cmd_insertSwipeHisrory.ExecuteReader();
            dataReader_history.Close();
            conn.Close();
            return Ok();
        }
    }
}
