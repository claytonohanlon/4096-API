using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using kolokAPI.UserModels;
using MySql.Data.MySqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace kolokAPI.Controllers
{
    public class UserController : Controller
    {
        [HttpPost]
        public IActionResult registerUser([FromBody] UserRegistration input)
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
            string insertUser = "INSERT IGNORE INTO Users (userID, userName, password) " +
                               "VALUES (" + input.uid + ",'" + input.username + "','" + input.password + "')";

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

            System.Diagnostics.Debug.WriteLine("POSTED VALUES:");
            System.Diagnostics.Debug.WriteLine(input.username);
            System.Diagnostics.Debug.WriteLine(input.password);
            System.Diagnostics.Debug.WriteLine(input.uid);

            return Ok();
        }

        [HttpGet]
        public IActionResult loginUser([FromBody] LoginUser input)
        {
            /*
             * This controller will take the submitted login data, check whether the
             * username and password match in the database. If they do, return OK.
             * If not, return respective error code and have front end prompt again.
             */
            System.Diagnostics.Debug.WriteLine(input.username);
            System.Diagnostics.Debug.WriteLine(input.password);
            return Ok();
        }
    }
}
