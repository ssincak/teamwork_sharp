using System;
using FullSerializer;

namespace TeamWorkSharp
{
    public class Comment
    {
        public string body { get; set; }

        public string notify = "";

        public bool isprivate { get; set; }
        public string pendingFileAttachments { get; set; }

        [fsProperty("content-type")]
        public string content_type = "TEXT";
    }
}
