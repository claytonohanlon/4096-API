﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using kolokAPI.Models;

namespace kolokAPI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public SwipeData getSwipeData()
        {
            loadJSON(#Filestring_to_json_storage)
            SwipeData articleData = new SwipeData
            {
                articleID = articleData.ToProcess.articleID,
                title = articleData.ToProcess.title,
                body = articleData.ToProcess.body,
                swipeNumber = articleData.ToProcess.swipe
            };

            return articleData;
        }


    }
}
