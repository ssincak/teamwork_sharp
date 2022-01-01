using FullSerializer;
using System;
using System.Collections.Generic;

namespace TeamWorkSharp
{
    class TasksResponse
    {
        [fsProperty("STATUS")]
        public string status { get; set; }

        [fsProperty("todo-items")]
        public List<Task> tasks = null;
    }
}
