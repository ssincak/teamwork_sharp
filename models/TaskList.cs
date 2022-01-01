using FullSerializer;
using System;

namespace TeamWorkSharp
{
    public class TaskList
    {
        public string projectid { get; set; }
        public string name { get; set; }
        public string description { get; set; }

        [fsProperty("milestone-id")]
        public string milestoneId { get; set; }

        [fsProperty("uncompleted-count")]
        public int uncompletedCount { get; set; }

        public bool complete { get; set; }

        [fsProperty("private")]
        public bool isPrivate { get; set; }

        [fsProperty("overdue-count")]
        public int overdueCount { get; set; }

        [fsProperty("project-name")]
        public string projectName { get; set; }

        public bool pinned { get; set; }

        public string id { get; set; }

        public int position { get; set; }

        [fsProperty("completed-count")]
        public int completedCount { get; set; }
    }
}
