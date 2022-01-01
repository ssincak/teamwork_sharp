using System;
using FullSerializer;

namespace TeamWorkSharp
{
    public class CreateTaskList
    {
        public class Body
        {
            public string name { set; get; }

            [fsProperty("private")]
            public bool isPrivate { set; get; }

            public bool pinned = true;

            [fsProperty("milestone-id")]
            public string milestoneId { set; get; }

            public string description { set; get; }

            [fsProperty("todo-list-template-id")]
            public string todoListTemplateId { set; get; }
        }

        [fsProperty("todo-list")]
        public Body taskList = new Body();
    }
}
