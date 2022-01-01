using FullSerializer;
using System;

namespace TeamWorkSharp
{
    public class Company
    {
        public string name { get; set; }

        [fsProperty("is-owner")]
        public int isOwner { get; set; }

        public int dd { get; set; }
    }
}
