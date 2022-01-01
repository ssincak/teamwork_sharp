using FullSerializer;
using System;

namespace TeamWorkSharp
{
    class CurrentUserResponse
    {
        public Person person { get; set; }

        [fsProperty("STATUS")]
        public string status { get; set; }
    }
}
