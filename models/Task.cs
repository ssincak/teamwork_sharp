using FullSerializer;
using System;

namespace TeamWorkSharp
{
    public class Task
    {
        public class Priority
        {
            private Priority(string value) { Value = value; }
            public string Value { get; set; }

            public static Priority None { get { return new Priority(""); } }
            public static Priority Low { get { return new Priority("low"); } }
            public static Priority Medium { get { return new Priority("medium"); } }
            public static Priority High { get { return new Priority("high"); } }
        }

        [fsProperty("project-id")]
        public Int64 projectId { get; set; }

        public int order { get; set; }

        [fsProperty("comments-count")]
        public int commentsCount { get; set; }

        [fsProperty("created-on")]
        public string createdOn { get; set; }

        public bool canEdit { get; set; }

        [fsProperty("has-predecessors")]
        public int hasPredecessors { get; set; }

        public Int64 id { get; set; }

        public bool completed { get; set; }

        public int position { get; set; }

        [fsProperty("estimated-minutes")]
        public Int64 estimatedMinutes { get; set; }

        public string description { get; set; }

        public int progress { get; set; }

        [fsProperty("harvest-enabled")]
        public bool harvestEnabled { get; set; }

        [fsProperty("responsible-party-lastname")]
        public string responsiblePartyLastName { get; set; }

        public string parentTaskId { get; set; }

        public string companyId { get; set; }

        [fsProperty("creator-avatar-url")]
        public string creatorAvatarUrl { get; set; }

        public string creatorId { get; set; }

        public string projectName { get; set; }

        public string startDate { get; set; }

        [fsProperty("tasklist-private")]
        public bool taskListPrivate { get; set; }

        public string lockdownId { get; set; }

        public bool canComplete { get; set; }

        [fsProperty("responsible-party-id")]
        public string responsiblePartyId { get; set; }

        [fsProperty("creator-lastname")]
        public string creatorLastName { get; set; }

        [fsProperty("has-reminders")]
        public bool hasReminders { get; set; }

        [fsProperty("todo-list-name")]
        public string todoListName { get; set; }

        [fsProperty("has-unread-comments")]
        public bool hasUnreadComments { get; set; }

        public string status { get; set; }
        public string content { get; set; }

        [fsProperty("creator-firstname")]
        public string creatorFirstName { get; set; }

        public string priority { get; set; }


    }
}
