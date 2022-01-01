using System;
using System.Collections.Generic;
using TeamWorkSharp;

namespace Application
{
    class Program
    {
        static string DomainName = "";  // your_domain_name.teamwork.com
        static string UserToken = "";  // your token

        static bool running = true;
        static bool error = false;

        static Client m_client = null;
        static Project m_project = null;
        static TaskList m_generalTasks = null;

        static void Main(string[] args)
        {
            Log("Test started...");

            m_client = new Client(DomainName, UserToken);

            m_client.onProjectsReceived += OnProjectsReceived;
            m_client.onTasksListReceived += OnTasksListReceived;
            m_client.onTaskListReceived += OnTaskListReceived;
            m_client.onPeopleReceived += OnPeopleReceived;
            m_client.onTaskPost += OnTaskPost;
            m_client.onError += OnError;
            m_client.onCurrentUserDetailsReceived += OnCurrentUserDetailsReceived;
            m_client.onCommentPost += OnCommentPost;
            m_client.onTaskListCreated += OnTaskListCreated;

            m_client.RequestCurrentUserDetails();

            while(running)
            {
                System.Threading.Thread.Sleep(100);
            }

            System.Threading.Thread.Sleep(error ? 5000:1000);
        }

        static void OnError(string descr)
        {
            Log("Error: " + descr);

            error = true;
            running = false;
        }

        static void OnCurrentUserDetailsReceived(Person me)
        {
            Log("Current user received: " + me.firstName + " " + me.lastName);

            m_client.RequestProjects();
        }

        static void OnProjectsReceived(List<Project> projects)
        {
            Log("Projects received...");

            m_project = m_client.FindProject("Test Project", projects);

            m_client.RequestTasksList(m_project);
        }

        static void OnTasksListReceived(List<TaskList> taskLists)
        {
            Log("Task lists received...");

            string reqTaskListName = "General tasks 2";

            m_generalTasks = m_client.FindTaskList(reqTaskListName, taskLists);

            if (m_generalTasks == null)
            {
                m_client.CreateTaskList(m_project, reqTaskListName, "New task list");
            }
            else
            {
                m_client.RequestPeople(m_project);
            }
        }

        static void OnTaskListCreated(string id)
        {
            m_client.RequestTaskList(id);
        }

        static void OnTaskListReceived(TaskList taskList)
        {
            m_generalTasks = taskList;

            m_client.RequestPeople(m_project);
        }

        static void OnPeopleReceived(List<Person> people)
        {
            Log("People received...");

            //var logFile = c.UploadFile("test.txt");
            //var screenshotFile = c.UploadFile("screenshot.png");
            m_client.PostTask(m_generalTasks, "Test N1", "With attachments", TeamWorkSharp.Task.Priority.None, null, null, false, null); //new PendingFile[] { logFile });
        }

        static void OnTaskPost(string taskID)
        {
            Log("Task posted, id: " + taskID);

            m_client.PostTaskComment(taskID, "Testing comment.");
        }

        static void OnCommentPost()
        {
            Log("Task comment posted.");

            running = false;
        }

        static void Log(string msg)
        {
            Console.WriteLine(msg);
        }
    }
}
