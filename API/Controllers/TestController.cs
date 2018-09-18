using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using kolokAPI.Models;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace kolokAPI.Controllers
{
    //[Route("api/[controller]")]
    public class TestController : Controller
    {

        [HttpGet]
        public string Index()
        {
            return "Hello World!";
        }

        [HttpGet]
        public SwipeData getSwipeData()
        {
            SwipeData testdata = new SwipeData
            {
                velocity = 2,
                duration = 52,
                liked = true,
                read = false,
                uid = 1885928
            };

            return testdata;
        }

        [HttpPost]
        public string postSwipeData([FromBody] SwipeData input)
        {
            /*
             * This controller will post swipe data to the database
             */ 
            return "test";
        }
    }
}
