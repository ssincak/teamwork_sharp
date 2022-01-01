using FullSerializer;
using System;

namespace TeamWorkSharp
{
    public class Person
    {
        public bool administrator { get; set; }

        [fsProperty("site-owner")]
        public bool siteOwner { get; set; }

        [fsProperty("last-name")]
        public string lastName { get; set; }

        [fsProperty("first-name")]
        public string firstName { get; set; }

        [fsProperty("user-name")]
        public string userName { get; set; }

        public string id { get; set; }

        public string FullName
        {
            get
            {
                return firstName + " " + lastName;
            }
        }
    }
}
