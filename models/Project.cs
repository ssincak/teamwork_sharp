using FullSerializer;
using System;

namespace TeamWorkSharp
{
    public class Project
    {
        public bool starred { get; set; }
        public string name { get; set; }

        [fsProperty("show-announcement")]
        public bool showAnnouncement { get; set; }

        public string announcement { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public bool isProjectAdmin { get; set; }

        [fsProperty("created-on")]
        public string createdOn { get; set; }

        [fsProperty("start-page")]
        public string startPage { get; set; }

        public string startDate { get; set; }

        public string logo { get; set; }
        public bool notifyeveryone { get; set; }
        public string id { get; set; }

        [fsProperty("last-changed-on")]
        public string lastChangedOn { get; set; }

        public string endDate { get; set; }
    }
}
