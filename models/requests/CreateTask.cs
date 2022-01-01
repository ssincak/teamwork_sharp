using System;
using FullSerializer;

namespace TeamWorkSharp
{
    class CreateTask
    {
        public class Body
        {
            public string content { get; set; }
            public string description { get; set; }
            public string pendingFileAttachments = "";

            public string priority { get; set; }

            [fsProperty("start-date")]
            public string startDate { get; set; }

            [fsProperty("responsible-party-id")]
            public string responsiblePartyId = "-1";  // -1 == anyone

            public string commentFollowerIds = "";
            public string changeFollowerIds = "";

            public bool notify { get; set; }
        }

        [fsProperty("todo-item")]
        public Body task = new Body();
    }
}
