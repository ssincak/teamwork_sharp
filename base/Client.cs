using FullSerializer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace TeamWorkSharp
{
    public class Client
    {
        #region Events
        public delegate void OnProjectsReceivedDelegate(List<Project> projects);
        public event OnProjectsReceivedDelegate onProjectsReceived;

        public delegate void OnTasksListReceivedDelegate(List<TaskList> taskLists);
        public event OnTasksListReceivedDelegate onTasksListReceived;

        public delegate void OnTaskListReceivedDelegate(TaskList taskList);
        public event OnTaskListReceivedDelegate onTaskListReceived;

        public delegate void OnPeopleReceivedDelegate(List<Person> people);
        public event OnPeopleReceivedDelegate onPeopleReceived;

        public delegate void OnCurrentUserDetailsReceivedDelegate(Person me);
        public event OnCurrentUserDetailsReceivedDelegate onCurrentUserDetailsReceived;

        public delegate void OnTasksReceivedDelegate(List<Task> tasks);
        public event OnTasksReceivedDelegate onTasksReceived;

        public delegate void OnFileUploadedDelegate(PendingFile f);
        public event OnFileUploadedDelegate onFileUploaded;

        public delegate void OnTaskPostDelegate(string taskID);
        public event OnTaskPostDelegate onTaskPost;

        public delegate void OnCommentPostDelegate();
        public event OnCommentPostDelegate onCommentPost;

        public delegate void OnErrorDelegate(string descr);
        public event OnErrorDelegate onError;

        public delegate void OnTaskListCreated(string id);
        public event OnTaskListCreated onTaskListCreated;
        #endregion

        #region protected variables
        static int requestCounter;

        private fsSerializer m_serializer = new fsSerializer();
        #endregion

        #region API
        public Client(string domainName, string token)
        {
            DomainName = domainName;
            Token = token;
        }

        public void RequestProjects()
        {
            try
            {
                Interlocked.Increment(ref requestCounter);

                HttpWebRequest req = CreateWebRequest(GetSubDomain("projects.json"));

                isError = false;

                req.BeginGetResponse(new AsyncCallback(OnProjectsReceived), req);
            }
            catch(Exception ex)
            {
                isError = true;
                errorDesc = ex.Message;
            }
        }

        public void RequestTasksList(Project proj)
        {
            try
            {
                Interlocked.Increment(ref requestCounter);

                HttpWebRequest req = CreateWebRequest(GetSubDomain("projects/" + proj.id + "/tasklists.json"));

                isError = false;

                req.BeginGetResponse(new AsyncCallback(OnTaskListsReceived), req);
            }
            catch (Exception ex)
            {
                isError = true;
                errorDesc = ex.Message;
            }
        }

        public void RequestTaskList(string id)
        {
            try
            {
                Interlocked.Increment(ref requestCounter);

                HttpWebRequest req = CreateWebRequest(GetSubDomain("tasklists/" + id + ".json"));

                isError = false;

                req.BeginGetResponse(new AsyncCallback(OnTaskListReceived), req);
            }
            catch (Exception ex)
            {
                isError = true;
                errorDesc = ex.Message;
            }
        }

        public void RequestCurrentUserDetails()
        {
            try
            {
                Interlocked.Increment(ref requestCounter);

                HttpWebRequest req = CreateWebRequest(GetSubDomain("me.json"));

                isError = false;

                req.BeginGetResponse(new AsyncCallback(OnCurrentUserDetailsReceived), req);
            }
            catch (Exception ex)
            {
                isError = true;
                errorDesc = ex.Message;
            }
        }

        public void RequestPeople(Project proj)
        {
            try
            {
                Interlocked.Increment(ref requestCounter);

                HttpWebRequest req = CreateWebRequest(GetSubDomain("projects/" + proj.id + "/people.json"));

                isError = false;

                req.BeginGetResponse(new AsyncCallback(OnPeopleReceived), req);
            }
            catch (Exception ex)
            {
                isError = true;
                errorDesc = ex.Message;
            }
        }

        public void RequestTasks(Project proj)
        {
            try
            {
                Interlocked.Increment(ref requestCounter);

                HttpWebRequest req = CreateWebRequest(GetSubDomain("projects/" + proj.id + "/tasks.json"));

                isError = false;

                req.BeginGetResponse(new AsyncCallback(OnTasksReceived), req);
            }
            catch (Exception ex)
            {
                isError = true;
                errorDesc = ex.Message;
            }
        }

        public void PostTaskComment(string taskID, string comment)
        {
            try
            {
                Interlocked.Increment(ref requestCounter);

                HttpWebRequest req = CreatePostWebRequest(GetSubDomain("tasks/" + taskID + "/comments.json"));

                isError = false;

                CreateComment pc = new CreateComment();
                pc.comment = new Comment();
                pc.comment.body = comment;

                SerializeIntoRequest<CreateComment>(req, pc);

                req.BeginGetResponse(new AsyncCallback(OnCommentPosted), req);
            }
            catch (Exception ex)
            {
                isError = true;
                errorDesc = ex.Message;
            }
        }

        public void CreateTaskList(Project proj, string name, string description)
        {
            try
            {
                Interlocked.Increment(ref requestCounter);

                HttpWebRequest req = CreatePostWebRequest(GetSubDomain("projects/" + proj.id + "/tasklists.json"));

                isError = false;

                CreateTaskList t = new CreateTaskList();
                t.taskList.name = name;
                t.taskList.description = description;

                SerializeIntoRequest<CreateTaskList>(req, t);

                req.BeginGetResponse(new AsyncCallback(OnTaskListCreationSucceeded), req);
            }
            catch (Exception ex)
            {
                isError = true;
                errorDesc = ex.Message;
            }
        }

        public void PostTask(TaskList taskList, string name, string description, TeamWorkSharp.Task.Priority priority, PendingFile[] fileAttachments, Person[] assignTo, bool notify, Person[] watchers)
        {
            if (taskList == null)
                return;

            try
            {
                Interlocked.Increment(ref requestCounter);

                HttpWebRequest req = CreatePostWebRequest(GetSubDomain("tasklists/" + taskList.id + "/tasks.json"));

                isError = false;

                CreateTask ptr = new CreateTask();
                CreateTask.Body tsk = ptr.task;

                tsk.content = name;
                tsk.description = description;
                tsk.priority = priority.Value;
                tsk.startDate = DateTime.Now.ToString("yyyyMMdd");
                tsk.notify = notify;

                if (fileAttachments != null)
                {
                    foreach (var f in fileAttachments)
                    {
                        if (tsk.pendingFileAttachments.Length > 0)
                            tsk.pendingFileAttachments += ",";

                        tsk.pendingFileAttachments += f.refString;
                    }
                }

                // add assignees
                if (assignTo != null)
                {
                    foreach (var p in assignTo)
                    {
                        if (tsk.responsiblePartyId.Length > 0)
                            tsk.responsiblePartyId += ",";

                        tsk.responsiblePartyId += p.id;
                    }
                }

                // add followers
                if(watchers != null)
                {
                    foreach(var v in watchers)
                    {
                        if (tsk.commentFollowerIds.Length > 0)
                            tsk.commentFollowerIds += ",";
                        if (tsk.changeFollowerIds.Length > 0)
                            tsk.changeFollowerIds += ",";

                        tsk.commentFollowerIds += v.id;
                        tsk.changeFollowerIds += v.id;
                    }
                }

                SerializeIntoRequest<CreateTask>(req, ptr);

                req.BeginGetResponse(new AsyncCallback(OnTaskPosted), req);
            }
            catch (Exception ex)
            {
                isError = true;
                errorDesc = ex.Message;
            }
        }

        public void UploadFile(string file)
        {
            try
            {
                Interlocked.Increment(ref requestCounter);

                string contentType = GetContentType(file);

                string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
                byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

                string sdom = GetSubDomain("pendingfiles.json");

                isError = false;

                //Creation and specification of the request
                HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(sdom); //sVal is id for the webService
                wr.Method = "POST";
                wr.KeepAlive = true;
                wr.Accept = "*/*";

                var credentialBuffer = new UTF8Encoding().GetBytes(Token + ":xxxx");
                wr.Headers["Authorization"] = "Basic " + Convert.ToBase64String(credentialBuffer);

                wr.ContentType = "multipart/form-data; boundary=" + boundary;

                Stream rs = wr.GetRequestStream();

                string fileName = Path.GetFileName(file);

                //Writting of the file
                byte[] starter = System.Text.Encoding.ASCII.GetBytes("--" + boundary + "--\r\n");
                rs.Write(starter, 0, starter.Length);

                string headerTemplate = "Content-Disposition: form-data; name=\"file\"; filename=\"{0}\"\r\nContent-Type: {1}\r\n\r\n";
                string header = string.Format(headerTemplate, fileName, contentType);
                byte[] headerbytes = System.Text.Encoding.UTF8.GetBytes(header);
                rs.Write(headerbytes, 0, headerbytes.Length);

                FileStream fileStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                byte[] buffer = new byte[4096];
                int bytesRead = 0;
                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    rs.Write(buffer, 0, bytesRead);
                }
                fileStream.Close();

                byte[] trailer = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
                rs.Write(trailer, 0, trailer.Length);
                rs.Close();

                wr.BeginGetResponse(new AsyncCallback(OnFileUploaded), wr);
            }
            catch (Exception ex)
            {
                isError = true;
                errorDesc = ex.Message;
            }
        }

        public TaskList FindTaskList(string name, IEnumerable<TaskList> lists)
        {
            if (lists == null)
                return null;

            foreach(var t in lists)
            {
                if (t.name == name)
                    return t;
            }

            return null;
        }

        public Project FindProject(string name, IEnumerable<Project> projs)
        {
            if (projs == null)
                return null;

            foreach (var p in projs)
            {
                if (p.name == name)
                    return p;
            }

            return null;
        }

        public string DomainName
        {
            get;
            private set;
        }

        public string Token
        {
            get;
            private set;
        }

        public bool requestPending
        {
            get { return requestCounter > 0; }
            //private set;
        }

        public bool isError
        {
            get;
            private set;
        }

        public string errorDesc
        {
            get;
            private set;
        }

        public string FullDomain
        {
            get
            {
                return "http://" + DomainName + ".teamwork.com";
            }
        }
        #endregion

        #region responses
        private void OnProjectsReceived(IAsyncResult result)
        {
            try
            {
                ProjectsResponse deserialized = Deserialize<ProjectsResponse>(result);

                if (onProjectsReceived != null)
                    onProjectsReceived(deserialized.projects);
            }
            catch (Exception ex)
            {
                isError = true;
                errorDesc = ex.Message;

                if (onError != null)
                    onError(ex.Message);
            }
            finally
            {
                Interlocked.Decrement(ref requestCounter);
            }
        }

        private void OnTaskListsReceived(IAsyncResult result)
        {
            try
            {
                TasksListResponse deserialized = Deserialize<TasksListResponse>(result);

                if (onTasksListReceived != null)
                    onTasksListReceived(deserialized.tasklists);
            }
            catch (Exception ex)
            {
                isError = true;
                errorDesc = ex.Message;

                if (onError != null)
                    onError(ex.Message);
            }
            finally
            {
                Interlocked.Decrement(ref requestCounter);
            }
        }

        private void OnTaskListReceived(IAsyncResult result)
        {
            try
            {
                TaskListResponse deserialized = Deserialize<TaskListResponse>(result);

                if (onTaskListReceived != null)
                    onTaskListReceived(deserialized.taskList);
            }
            catch (Exception ex)
            {
                isError = true;
                errorDesc = ex.Message;

                if (onError != null)
                    onError(ex.Message);
            }
            finally
            {
                Interlocked.Decrement(ref requestCounter);
            }
        }

        private void OnCurrentUserDetailsReceived(IAsyncResult result)
        {
            try
            {
                CurrentUserResponse deserialized = Deserialize<CurrentUserResponse>(result);

                if (onCurrentUserDetailsReceived != null)
                    onCurrentUserDetailsReceived(deserialized.person);
            }
            catch (Exception ex)
            {
                isError = true;
                errorDesc = ex.Message;

                if (onError != null)
                    onError(ex.Message);
            }
            finally
            {
                Interlocked.Decrement(ref requestCounter);
            }
        }

        private void OnPeopleReceived(IAsyncResult result)
        {
            try
            {
                PeopleResponse deserialized = Deserialize<PeopleResponse>(result);

                if (onPeopleReceived != null)
                    onPeopleReceived(deserialized.people);
            }
            catch (Exception ex)
            {
                isError = true;
                errorDesc = ex.Message;

                if (onError != null)
                    onError(ex.Message);
            }
            finally
            {
                Interlocked.Decrement(ref requestCounter);
            }
        }

        private void OnTasksReceived(IAsyncResult result)
        {
            try
            {
                TasksResponse deserialized = Deserialize<TasksResponse>(result);

                if (onTasksReceived != null)
                    onTasksReceived(deserialized.tasks);
            }
            catch (Exception ex)
            {
                isError = true;
                errorDesc = ex.Message;

                if (onError != null)
                    onError(ex.Message);
            }
            finally
            {
                Interlocked.Decrement(ref requestCounter);
            }
        }

        private void OnFileUploaded(IAsyncResult result)
        {
            try
            {
                PendingFileResponse deserialized = Deserialize<PendingFileResponse>(result);

                if (onFileUploaded != null)
                    onFileUploaded(deserialized.pendingFile);
            }
            catch (Exception ex)
            {
                isError = true;
                errorDesc = ex.Message;

                if (onError != null)
                    onError(ex.Message);
            }
            finally
            {
                Interlocked.Decrement(ref requestCounter);
            }
        }

        private void OnTaskPosted(IAsyncResult result)
        {
            try
            {
                HttpWebResponse response = (result.AsyncState as HttpWebRequest).EndGetResponse(result) as HttpWebResponse;
                response.Close();

                string taskID = response.Headers["id"];

                if (onTaskPost != null)
                    onTaskPost(taskID);
            }
            catch (Exception ex)
            {
                isError = true;
                errorDesc = ex.Message;

                if (onError != null)
                    onError(ex.Message);
            }
            finally
            {
                Interlocked.Decrement(ref requestCounter);
            }
        }

        private void OnTaskListCreationSucceeded(IAsyncResult result)
        {
            try
            {
                HttpWebResponse response = (result.AsyncState as HttpWebRequest).EndGetResponse(result) as HttpWebResponse;
                response.Close();

                if (onTaskListCreated != null)
                    onTaskListCreated(response.Headers["id"]);
            }
            catch (Exception ex)
            {
                isError = true;
                errorDesc = ex.Message;

                if (onError != null)
                    onError(ex.Message);
            }
            finally
            {
                Interlocked.Decrement(ref requestCounter);
            }
        }

        private void OnCommentPosted(IAsyncResult result)
        {
            try
            {
                HttpWebResponse response = (result.AsyncState as HttpWebRequest).EndGetResponse(result) as HttpWebResponse;
                response.Close();

                if (onCommentPost != null)
                    onCommentPost();
            }
            catch (Exception ex)
            {
                isError = true;
                errorDesc = ex.Message;

                if (onError != null)
                    onError(ex.Message);
            }
            finally
            {
                Interlocked.Decrement(ref requestCounter);
            }
        }
        #endregion

        #region protected methods
        protected string GetSubDomain(string subDomain)
        {
            return FullDomain + "/" + subDomain;
        }

        protected HttpWebRequest CreateWebRequest(string requestUriString)
        {
            HttpWebRequest req = HttpWebRequest.Create(requestUriString) as HttpWebRequest;

            var credentialBuffer = new UTF8Encoding().GetBytes(Token + ":xxxx");
            req.Headers["Authorization"] = "Basic " + Convert.ToBase64String(credentialBuffer);

            req.ContentType = "application/json";

            return req;
        }

        protected HttpWebRequest CreatePostWebRequest(string requestUriString)
        {
            HttpWebRequest ret = CreateWebRequest(requestUriString);

            ret.Method = "POST";

            return ret;
        }

        class PostFile
        {
            public string file { get; set; }
        }

        protected string GetContentType(string fileName)
        {
            if (fileName.EndsWith(".txt") || fileName.EndsWith(".log"))
                return "text/plain";
            else if (fileName.EndsWith(".jpg"))
                return "image/jpeg";
            else if (fileName.EndsWith(".png"))
                return "image/png";
            else if (fileName.EndsWith(".zip"))
                return "application/zip";

            return "application/octet-stream";
        }

        private void SerializeIntoRequest<T>(HttpWebRequest req, T obj)
        {
            fsData data;
            m_serializer.TrySerialize<T>(obj, out data).AssertSuccessWithoutWarnings();

            string postData = fsJsonPrinter.CompressedJson(data);
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            req.ContentLength = byteArray.Length;

            using (var dataStream = req.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
            }
        }

        private T Deserialize<T>(HttpWebResponse response)
        {
            System.IO.StreamReader reader = new StreamReader(response.GetResponseStream());
            string str = reader.ReadToEnd();

            fsData data = fsJsonParser.Parse(str);
            T deserialized = default(T);
            m_serializer.TryDeserialize(data, ref deserialized).AssertSuccessWithoutWarnings();

            return deserialized;
        }

        private T Deserialize<T>(IAsyncResult result)
        {
            HttpWebResponse response = (result.AsyncState as HttpWebRequest).EndGetResponse(result) as HttpWebResponse;

            return Deserialize<T>(response);
        }
        #endregion
    }
}
