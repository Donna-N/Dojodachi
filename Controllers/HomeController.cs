using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using Dojodachi.Models;

namespace Dojodachi.Controllers
{
    public static class SessionExtensions
    {
        // We can call ".SetObjectAsJson" just like our other session set methods, by passing a key and a value
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            // This helper function simply serializes the object to JSON and stores it as a string in session
            session.SetString(key, JsonSerializer.Serialize(value));
        }
        
        // generic type T is a stand-in indicating that we need to specify the type on retrieval
        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            string value = session.GetString(key);
            // Upon retrieval the object is deserialized based on the type we specified
            return value == null ? default(T) : JsonSerializer.Deserialize<T>(value);
        }
    }
    public class HomeController : Controller
    {

        [HttpGet("")]
        public IActionResult Index()
        {
            if(HttpContext.Session.GetObjectFromJson<NewDojodachi>("MyDojodachi")==null)
            {
                NewDojodachi kimchi = new NewDojodachi();
                HttpContext.Session.SetObjectAsJson("MyDojodachi", kimchi);
                ViewBag.Happiness = kimchi.Happiness;
                ViewBag.Fullness = kimchi.Fullness;
                ViewBag.Energy = kimchi.Energy;
                ViewBag.Meals = kimchi.Meals;
                ViewBag.Response = kimchi.Response;
                ViewBag.Image = kimchi.Image;
            }
            else
            {
                ViewBag.Happiness = HttpContext.Session.GetObjectFromJson<NewDojodachi>("MyDojodachi").Happiness;
                ViewBag.Fullness = HttpContext.Session.GetObjectFromJson<NewDojodachi>("MyDojodachi").Fullness;
                ViewBag.Energy = HttpContext.Session.GetObjectFromJson<NewDojodachi>("MyDojodachi").Energy;
                ViewBag.Meals = HttpContext.Session.GetObjectFromJson<NewDojodachi>("MyDojodachi").Meals;
                ViewBag.Response = HttpContext.Session.GetObjectFromJson<NewDojodachi>("MyDojodachi").Response;
                ViewBag.Image = HttpContext.Session.GetObjectFromJson<NewDojodachi>("MyDojodachi").Image;
            }
            if (HttpContext.Session.GetObjectFromJson<NewDojodachi>("MyDojodachi").Energy > 100 && HttpContext.Session.GetObjectFromJson<NewDojodachi>("MyDojodachi").Fullness > 100 && HttpContext.Session.GetObjectFromJson<NewDojodachi>("MyDojodachi").Happiness > 100 )
            {
                return RedirectToAction("Winner");
            }
            if (HttpContext.Session.GetObjectFromJson<NewDojodachi>("MyDojodachi").Fullness <=0 || HttpContext.Session.GetObjectFromJson<NewDojodachi>("MyDojodachi").Happiness <= 0 )
            {
                return RedirectToAction("Loser");
            }
            return View();
        }

        [HttpPost("Feed")]
        public IActionResult Feed()
        {
            NewDojodachi kimchi = HttpContext.Session.GetObjectFromJson<NewDojodachi>("MyDojodachi");
            if (kimchi.Meals >0)
            {
                kimchi.Meals -= 1;
                Random rand = new Random();
                if (rand.Next(1,5) != 1)
                {
                    var feeding = rand.Next(5,11);
                    kimchi.Fullness += rand.Next(5,11);
                    kimchi.Response = $"You fed your Dojodachi and it increased in fullness by {feeding}!";
                    kimchi.Image = @"/Images/eating.jpeg";
                }
                else 
                {
                    kimchi.Response = "Your Dojodachi did not like that meal. Fullness did not increase."; 
                    kimchi.Image = @"/Images/sad.jpeg";   
                }
            }
            else 
            {
                kimchi.Response = "Sorry, you're out of food. You need more meals to feed your Dojodachi.";
                kimchi.Image = @"/Images/sad.jpeg"; 
            }
            HttpContext.Session.SetObjectAsJson("MyDojodachi", kimchi);
            return RedirectToAction("Index");
        }

        [HttpPost("Play")]
        public IActionResult Play()
        {
            NewDojodachi kimchi = HttpContext.Session.GetObjectFromJson<NewDojodachi>("MyDojodachi");
            if (kimchi.Energy>=5)
            {
                kimchi.Energy -=5;
                Random rand = new Random();
                if (rand.Next(1,5) != 1)
                {
                    var makeHappy = rand.Next(5,11);
                    kimchi.Happiness += makeHappy;
                    kimchi.Response = $"You played with your Dojodachi and it increased in happiness by {makeHappy}!";
                    kimchi.Image = @"/Images/playing.png";
                }
                else 
                {
                    kimchi.Response = "Your Dojodachi did not like that game. Happiness did not increase."; 
                    kimchi.Image = @"/Images/sad.jpeg";   
                }
            }
            else
            {
                kimchi.Response = "You don't have enough energy left to play today.";
                kimchi.Image = @"/Images/sad.jpeg"; 
            }
            HttpContext.Session.SetObjectAsJson("MyDojodachi", kimchi);
            return RedirectToAction("Index");
        }

        [HttpPost("Work")]
        public IActionResult Work()
        {            
            NewDojodachi kimchi = HttpContext.Session.GetObjectFromJson<NewDojodachi>("MyDojodachi");
            if (kimchi.Energy>=5)
            {
                kimchi.Energy -=5;
                Random rand = new Random();
                var plusMeal = rand.Next(1,4);
                kimchi.Meals += plusMeal;
                if (plusMeal == 1)
                {
                    kimchi.Response = $"Your Dojodachi worked and earned {plusMeal} meal!";
                }
                else 
                {
                    kimchi.Response = $"Your Dojodachi worked and earned {plusMeal} meals!";
                }
                kimchi.Image = @"/Images/working.jpeg";
            }
            else 
            {
                kimchi.Response = "You don't have enough energy left to work today.";
                kimchi.Image = @"/Images/sad.jpeg"; 
            }
            HttpContext.Session.SetObjectAsJson("MyDojodachi", kimchi);
            return RedirectToAction("Index");
        }

        [HttpPost("Sleep")]
        public IActionResult Sleep()
        {            
            NewDojodachi kimchi = HttpContext.Session.GetObjectFromJson<NewDojodachi>("MyDojodachi");
            kimchi.Energy +=15;
            kimchi.Fullness -= 5;
            kimchi.Happiness -=5;
            kimchi.Response = "You slept and earned 15 energy!";
            kimchi.Image = @"/Images/sleeping.jpeg";
            HttpContext.Session.SetObjectAsJson("MyDojodachi", kimchi);
            return RedirectToAction("Index");
        }

        [HttpGet("result")]
        public IActionResult Result()
        {
                NewDojodachi kimchi = HttpContext.Session.GetObjectFromJson<NewDojodachi>("MyDojodachi");
                ViewBag.Happiness = HttpContext.Session.GetObjectFromJson<NewDojodachi>("MyDojodachi").Happiness;
                ViewBag.Fullness = HttpContext.Session.GetObjectFromJson<NewDojodachi>("MyDojodachi").Fullness;
                ViewBag.Energy = HttpContext.Session.GetObjectFromJson<NewDojodachi>("MyDojodachi").Energy;
                ViewBag.Meals = HttpContext.Session.GetObjectFromJson<NewDojodachi>("MyDojodachi").Meals;
                ViewBag.Response = HttpContext.Session.GetObjectFromJson<NewDojodachi>("MyDojodachi").Response;
                ViewBag.Image = HttpContext.Session.GetObjectFromJson<NewDojodachi>("MyDojodachi").Image;
                return View();
        }

        [HttpGet("winner")]
        public IActionResult Winner()
        {
            NewDojodachi kimchi = HttpContext.Session.GetObjectFromJson<NewDojodachi>("MyDojodachi");
            kimchi.Response = "Congratulations! You won!";
            kimchi.Image = @"Images/happy.png";
            HttpContext.Session.SetObjectAsJson("MyDojodachi", kimchi);
            return RedirectToAction("Result");
        }
        [HttpGet("loser")]
        public IActionResult Loser()
        {
            NewDojodachi kimchi = HttpContext.Session.GetObjectFromJson<NewDojodachi>("MyDojodachi");
            kimchi.Response = "Your Dojodachi has passed away";
            kimchi.Image = @"Images/dead.jpeg";           
            HttpContext.Session.SetObjectAsJson("MyDojodachi", kimchi);
            return RedirectToAction("Result");
        }

        [HttpPost("restart")]
        public IActionResult Restart()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

    }
}
