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
            /*
             * This controller will grab the posted user data, check if username
             * already exists, if so, return error, if not, make the entry in the database
             * and return back the User ID for that new entry
             */

            MySqlConnection conn = new MySqlConnection("server=142.93.63.223; " +
                                                       "user id=user; " +
                                                       "password=gift?FINISH?finland?45; " +
                                                       "database=CORE; " +
                                                       "persistsecurityinfo=True; " +
                                                       "SslMode=none");

            string checkUN = "SELECT 1 FROM Users WHERE userName='"+input.username+"'";
            string insertUser = "INSERT IGNORE INTO Users (userName, password) " +
                               "VALUES ('" + input.username + "','" + input.password + "')";

            MySqlCommand cmd_checkUN = new MySqlCommand(checkUN, conn);
            MySqlCommand cmd_insertUser = new MySqlCommand(insertUser, conn);

            conn.Open();

            //execute check for username already in database
            var dataReader_chk = cmd_checkUN.ExecuteReader();

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
                var dataReader_ins = cmd_insertUser.ExecuteReader();
                System.Diagnostics.Debug.WriteLine("entry added.. closing reader");
                dataReader_ins.Close();
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

            string checkCred = "SELECT EXISTS (" +
                                 "SELECT * FROM Users WHERE userName = '"+input.username+"' AND password = '"+input.password+"'"+
                               ")";

            MySqlCommand checkLogin = new MySqlCommand(checkCred, conn);

            conn.Open();

            //execute SQL to verify existence of username and password combination
            var dataReader_login = checkLogin.ExecuteReader();

            //read in query result and close respective connections
            dataReader_login.Read();
            string result = dataReader_login[0].ToString();

            System.Diagnostics.Debug.WriteLine("RESULT: " + result);

            dataReader_login.Close();
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
