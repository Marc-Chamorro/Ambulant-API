using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class Person
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Hash { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTime Creation { get; set; }
        public int Discount { get; set; }
    }

    public class PersonReduced
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class PersonSignUp
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}