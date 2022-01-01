using System;
using FullSerializer;

namespace TeamWorkSharp
{
    public class TaskListResponse
    {
        [fsProperty("todo-list")]
        public TaskList taskList { set; get; }

        public string STATUS { set; get; }
    }
}
