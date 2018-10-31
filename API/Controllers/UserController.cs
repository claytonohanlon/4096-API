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
    public class UserController : Controller
    {
        [HttpPost]
        public IActionResult RegisterUser([FromBody] User input)
        {
            MySqlConnection conn = new MySqlConnection("server=142.93.63.223; " +
                                                       "user id=user; " +
                                                       "password=gift?FINISH?finland?45; " +
                                                       "database=CORE; " +
                                                       "persistsecurityinfo=True; " +
                                                       "SslMode=none");

            MySqlCommand checkUN = conn.CreateCommand();

            checkUN.CommandText = "SELECT 1 FROM Users WHERE userName = @userName";
            checkUN.Parameters.AddWithValue("@userName", input.username);

            MySqlCommand insertUser = conn.CreateCommand();

            insertUser.CommandText = "INSERT IGNORE INTO Users (userName, password) VALUES (@userName, @password)";
            insertUser.Parameters.AddWithValue("@userName", input.username);
            insertUser.Parameters.AddWithValue("@password", input.password);

            conn.Open();

            //execute check for username already in database
            var dataReader_chk = checkUN.ExecuteReader();

            if (dataReader_chk.HasRows)
            {
                //close the check data reader
                dataReader_chk.Close();

                //return appropriate HTTP error code to front end.
                System.Diagnostics.Debug.WriteLine("Value already exists. Closing connection.");
                return BadRequest();
            }
            else
            {
                //close previous data reader
                dataReader_chk.Close();

                //open up a new data reader to insert the new user into the database
                System.Diagnostics.Debug.WriteLine("Value " + input.username + " not found in Users, inserting new entry");
                insertUser.ExecuteNonQuery();
                System.Diagnostics.Debug.WriteLine("entry added.. closing reader");
            }

            conn.Close();

            return Ok();
        }

        [HttpPost]
        public IActionResult LoginUser([FromBody] User input)
        {
            /*
             * This controller will take the submitted login data, check whether the
             * username and password match in the database. If they do, return OK.
             * If not, return respective error code and have front end prompt again.
             */

            MySqlConnection conn = new MySqlConnection("server=142.93.63.223; " +
                                                       "user id=user; " +
                                                       "password=gift?FINISH?finland?45; " +
                                                       "database=CORE; " +
                                                       "persistsecurityinfo=True; " +
                                                       "SslMode=none");

            MySqlCommand checkLogin = conn.CreateCommand();
            checkLogin.CommandText = "SELECT EXISTS (SELECT * FROM Users WHERE userName = @userName AND password = @password)";
            checkLogin.Parameters.AddWithValue("@userName", input.username);
            checkLogin.Parameters.AddWithValue("@password", input.password);

            conn.Open();

            //execute SQL to verify existence of username and password combination
            var login = checkLogin.ExecuteReader();
            login.Read();
            string result = login[0].ToString();

            login.Close();
            conn.Close();

            if (result == "0")
            {
                //username and password combo DOES NOT exist
                //return HTTP/1.1 404 Not Found
                System.Diagnostics.Debug.WriteLine("NOT FOUND");
                return NotFound();
            }
            else if(result == "1")
            {
                //username and password combo DOES exist
                //return HTTP/1.1 200 OK
                System.Diagnostics.Debug.WriteLine("FOUND");
                return Ok();
            }
            else
            {
                //There is an issue with the request sent to the server
                //return HTTP/1.1 400 Bad Request
                System.Diagnostics.Debug.WriteLine("ERROR");
                return BadRequest();
            }
        }
    }
}
