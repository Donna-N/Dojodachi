using System;

namespace Dojodachi.Models
{
    public class NewDojodachi
    {
        public int Happiness{get; set;} = 20;
        public int Fullness {get; set;} = 20;
        public int Energy {get; set;} = 50;
        public int Meals {get;set;} = 3;
        public string Response {get;set;} = "Interact with your Dojodachi using the buttons below!";
        public string Image {get; set;} = @"Images/hello.png";

        

        
    }
}