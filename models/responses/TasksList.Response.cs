using FullSerializer;
using System;
using System.Collections.Generic;

namespace TeamWorkSharp
{
    class TasksListResponse
    {
        public List<TaskList> tasklists = null;

        [fsProperty("STATUS")]
        public string status { get; set; }
    }
}
