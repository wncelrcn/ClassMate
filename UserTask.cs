using Android.Content;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;
using Android.App;
using AndroidX.AppCompat.App;
using System.Threading.Tasks;
using System.Globalization;
using Android.Views;

namespace IT123P_FinalMP
{
    internal class UserTask
    {
        // HttpWebResponse and HttpWebRequest for REST API
        HttpWebResponse response;
        HttpWebRequest request;
        string result;
        string url = "http://192.168.100.11/IT123P_FinalMP/REST";
        LinearLayout currLayout;
        private Context context;

        // Constructor
        public UserTask(Context context)
        {
            this.context = context;
        }

        // Constructor with LinearLayout parameter
        public UserTask(Context context, LinearLayout currLayout)
        {
            this.context = context;
            this.currLayout = currLayout;
        }

        // Function to Get Tasks Per Class
        public async Task GetTaskPerClass(string username, string classCode, string className)
        {
            try
            {
                // Fetch the tasks
                await FetchTasks(username, classCode, className);
            }
            catch (Exception ex)
            {
                Toast.MakeText(context, "Error fetching tasks, please try again.", ToastLength.Short).Show();
            }
        }

        // Function to Fetch Tasks
        private async Task FetchTasks(string username, string taskClass, string className)
        {

            // URL to get the tasks per class
            string requestUrl = $"{url}/get_TaskPerClass.php?username={username}&taskClass={taskClass}";

            try
            {
                // Create a new HttpWebRequest
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);
                request.Method = "GET";

                using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    var result = await reader.ReadToEndAsync();
                    
                    // Debug API response
                    using JsonDocument doc = JsonDocument.Parse(result);
                    JsonElement root = doc.RootElement;

                    // Check if the JSON is an array
                    if (root.ValueKind == JsonValueKind.Array)
                    {

                        // Create a list of tasks
                        var userTasks = new List<Dictionary<string, string>>();
                        foreach (JsonElement item in root.EnumerateArray())
                        {
                            // Add the task to the list
                            var task = new Dictionary<string, string>
                            {
                                { "taskName", item.GetProperty("taskName").GetString() },
                                { "taskDesc", item.GetProperty("taskDesc").GetString() },
                                { "isDone", item.GetProperty("isDone").GetString() },
                                { "toDoDate", item.GetProperty("toDoDate").GetString() },
                                { "dueDate", item.GetProperty("dueDate").GetString() },
                                { "taskClass", item.GetProperty("taskClass").GetString() },
                                {"username", item.GetProperty("uname").GetString() },
                                {"classCode", taskClass },
                                {"className", className }


                            };
                            userTasks.Add(task);
                        }
                        // Create the task layout
                        ((Activity)context).RunOnUiThread(() => CreateTaskLayout(userTasks, "class"));
                    }
                    else
                    {
                        throw new InvalidOperationException("The JSON is not an array.");
                    }
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(context, "Error fetching tasks, please try again.", ToastLength.Short).Show();
            }
        }

        // Function to Get Tasks Per Date
        public async Task GetTasksPerDate(string username, string date)
        {
            try
            {
                // Check if the date is valid
                if (IsValidDate(date, out DateTime validDate))
                {
                    await FetchTasks(username, validDate);
                }
                else
                {
                    // Attempt to reformat the date if invalid
                    date = ReformatDate(date);

                    // Check if the date is valid
                    if (IsValidDate(date, out validDate))
                    {
                        await FetchTasks(username, validDate);
                    }
                    else
                    {
                        Toast.MakeText(context, "Invalid date format.", ToastLength.Short).Show();
                    }
                }
            }
            catch (Exception ex)
            {
                
                Toast.MakeText(context, "Error fetching tasks, please try again.", ToastLength.Short).Show();
            }
        }

        // Function to Fetch Tasks
        private async Task FetchTasks(string username, DateTime date)
        {
            // Convert the date to string
            string convertedDateStr = date.ToString("yyyy-MM-dd");
            string requestUrl = $"{url}/get_TaskPerDate.php?username={username}&date={convertedDateStr}";

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);
                request.Method = "GET";

                using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    var result = await reader.ReadToEndAsync();
                    
                    // Debug API response
                    using JsonDocument doc = JsonDocument.Parse(result);
                    JsonElement root = doc.RootElement;


                    if (root.ValueKind == JsonValueKind.Array)
                    {
                        // Create a list of tasks
                        var userTasks = new List<Dictionary<string, string>>();
                        foreach (JsonElement item in root.EnumerateArray())
                        {
                            // Add the task to the list
                            var task = new Dictionary<string, string>
                            {
                                { "taskName", item.GetProperty("taskName").GetString() },
                                { "taskDesc", item.GetProperty("taskDesc").GetString() },
                                { "isDone", item.GetProperty("isDone").GetString() },
                                { "toDoDate", item.GetProperty("toDoDate").GetString() },
                                { "dueDate", item.GetProperty("dueDate").GetString() },
                                { "taskClass", item.GetProperty("taskClass").GetString() },
                                {"username", item.GetProperty("uname").GetString() },
                            };
                            userTasks.Add(task);
                        }

                        // Create the task layout
                        ((Activity)context).RunOnUiThread(() => CreateTaskLayout(userTasks, "dashboard"));
                    }
                    else
                    {
                        throw new InvalidOperationException("The JSON is not an array.");
                    }
                }
            }
            catch (Exception ex)
            {
                
                Toast.MakeText(context, "Error fetching tasks, please try again.", ToastLength.Short).Show();
            }
        }

        // Function to Create Task Layout
        public void CreateTaskLayout(List<Dictionary<string, string>> tasks, string layout)
        {
            try
            {
                // Clear the current layout
                currLayout.RemoveAllViews();

                // Initialize your FontHandler instances
                FontHandler boldFont = new FontHandler(context, "Raleway-Bold.ttf");
                FontHandler mediumFont = new FontHandler(context, "Raleway-Medium.ttf");
                FontHandler regularFont = new FontHandler(context, "Raleway-Regular.ttf");
                FontHandler semiBoldFont = new FontHandler(context, "Raleway-Semibold.ttf");

                // Check if there are no tasks
                if (tasks.Count == 0)
                {
                    // Create a TextView to display the "no tasks" message
                    TextView noTasksTextView = new TextView(context)
                    {
                        Text = "You currently have no tasks."
                    };
                    noTasksTextView.SetTextColor(Android.Graphics.Color.Black);
                    noTasksTextView.SetTextSize(Android.Util.ComplexUnitType.Sp, 20); 
                    mediumFont.SetFont(noTasksTextView);

                    // Create layout parameters with top margin
                    LinearLayout.LayoutParams noTasksLayoutParams = new LinearLayout.LayoutParams(
                        LinearLayout.LayoutParams.WrapContent,
                        LinearLayout.LayoutParams.WrapContent
                    );
                    noTasksLayoutParams.SetMargins(0, 70, 0, 0);
                    noTasksLayoutParams.Gravity = GravityFlags.Center;

                    
                    noTasksTextView.LayoutParameters = noTasksLayoutParams;

                    
                    currLayout.AddView(noTasksTextView);

                    
                    return;
                }


                foreach (var task in tasks)
                {
                    // Parse the due date
                    DateTime dueDate = DateTime.Parse(task["dueDate"]);
                    DateTime currentDate = DateTime.Now;
                    int daysUntilDue = (dueDate - currentDate).Days;

                    // Determine the background color based on the due date
                    string backgroundColor;
                    if (daysUntilDue <= 1)
                    {
                        backgroundColor = "#fce1e4"; // Red
                    }
                    else if (daysUntilDue <= 3)
                    {
                        backgroundColor = "#fcf4dd"; // Yellow
                    }
                    else
                    {
                        backgroundColor = "#daeaf6"; // Blue
                    }
                    // Create a new LinearLayout
                    LinearLayout linearLayout = new LinearLayout(context)
                    {
                        Orientation = Orientation.Vertical
                    };

                    linearLayout.SetBackgroundColor(Android.Graphics.Color.ParseColor(backgroundColor));

                    // Create layout parameters
                    LinearLayout.LayoutParams layoutParams = new LinearLayout.LayoutParams(
                        LinearLayout.LayoutParams.MatchParent,
                        LinearLayout.LayoutParams.WrapContent
                    );
                    layoutParams.SetMargins(0, 0, 0, 50);

                    
                    linearLayout.LayoutParameters = layoutParams;
                    linearLayout.SetPadding(20, 20, 20, 20);

                    // Create a TextView for the task name
                    TextView textViewName = new TextView(context)
                    {
                        Text = task["taskName"] 
                    };
                    textViewName.SetPadding(10, 10, 0, 10);
                    textViewName.SetTextColor(Android.Graphics.Color.Black);
                    textViewName.SetTextSize(Android.Util.ComplexUnitType.Sp, 24);
                    semiBoldFont.SetFont(textViewName);

                    // Add the task name TextView to the LinearLayout
                    linearLayout.AddView(textViewName);

                    // Create a TextView for the task course code
                    TextView textViewCourse = new TextView(context)
                    {
                        Text = task["taskClass"]
                    };
                    textViewCourse.SetPadding(10, 0, 0, 10);
                    textViewCourse.SetTextColor(Android.Graphics.Color.Gray);
                    textViewCourse.SetTextSize(Android.Util.ComplexUnitType.Sp, 18);
                    mediumFont.SetFont(textViewCourse);

                    // Add the task course code TextView to the LinearLayout
                    linearLayout.AddView(textViewCourse);

                    
                    TextView textViewDueDate = new TextView(context)
                    {
                        Text = "Due Date: " + task["dueDate"]
                    };
                    textViewDueDate.SetPadding(10, 0, 0, 10);
                    textViewDueDate.SetTextColor(Android.Graphics.Color.Gray);
                    textViewDueDate.SetTextSize(Android.Util.ComplexUnitType.Sp, 18);
                    regularFont.SetFont(textViewDueDate); 

                    
                    linearLayout.AddView(textViewDueDate);

                    
                    currLayout.AddView(linearLayout);

                    
                    linearLayout.Click += (sender, e) =>
                    {
                        NextActivityHandler nextActivity = new NextActivityHandler(context, typeof(TaskView));

                        nextActivity.PassDataToNextActivity("taskName", task["taskName"]);
                        nextActivity.PassDataToNextActivity("taskClass", task["taskClass"]);
                        nextActivity.PassDataToNextActivity("taskDesc", task["taskDesc"]);
                        nextActivity.PassDataToNextActivity("toDoDate", task["toDoDate"]);
                        nextActivity.PassDataToNextActivity("dueDate", task["dueDate"]);
                        nextActivity.PassDataToNextActivity("username", task["username"]);

                        if (layout == "class")
                        {
                            nextActivity.PassDataToNextActivity("classCode", task["classCode"]);
                            nextActivity.PassDataToNextActivity("className", task["className"]);
                        }

                        nextActivity.PassDataToNextActivity("layout", layout);
                        nextActivity.NavigateToNextActivity(context);
                    };
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(context, "Error creating task layout, please try again.", ToastLength.Short).Show();
            }
        }

        // Function to Insert Task
        public void InsertTask(string username, string taskName, string taskDesc, bool isDone, string toDoDate, string dueDate, string taskClass)
        {
            try
            {
                // Check if the date is valid
                if (IsValidDate(toDoDate, out DateTime validToDoDate) && IsValidDate(dueDate, out DateTime validDueDate))
                {
                    // Convert the dates to string
                    string convertedDateStrTDD = validToDoDate.ToString("yyyy-MM-dd");
                    string convertedDateStrDD = validDueDate.ToString("yyyy-MM-dd");

                    // URL to add a task
                    string requestUrl = $"{url}/add_task.php?username={username}&taskName={taskName}&taskDesc={taskDesc}&isDone={isDone}&toDoDate={convertedDateStrTDD}&dueDate={convertedDateStrDD}&taskClass={taskClass}";

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string result = reader.ReadToEnd();

                        // If the result contains "OK!", show a toast message that the task is added
                        if (result.Contains("OK!"))
                        {
                            Toast.MakeText(context, "Task Added.", ToastLength.Short).Show();
                        }
                        else
                        {
                            Toast.MakeText(context, "Task is not Added.", ToastLength.Short).Show();
                        }
                    }
                }
                else
                {
                    // Attempt to reformat the dates if invalid
                    toDoDate = ReformatDate(toDoDate);
                    dueDate = ReformatDate(dueDate);

                    // Check if the date is valid
                    if (IsValidDate(toDoDate, out validToDoDate) && IsValidDate(dueDate, out validDueDate))
                    {
                        // Convert the dates to string
                        string convertedDateStrTDD = validToDoDate.ToString("yyyy-MM-dd");
                        string convertedDateStrDD = validDueDate.ToString("yyyy-MM-dd");

                        string requestUrl = $"{url}/add_task.php?username={username}&taskName={taskName}&taskDesc={taskDesc}&isDone={isDone}&toDoDate={convertedDateStrTDD}&dueDate={convertedDateStrDD}&taskClass={taskClass}";

                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            // If the result contains "OK!", show a toast message that the task is added
                            string result = reader.ReadToEnd();
                            if (result.Contains("OK!"))
                            {
                                Toast.MakeText(context, "Task Added.", ToastLength.Short).Show();
                            }
                            else
                            {
                                Toast.MakeText(context, "Task not Added.", ToastLength.Short).Show();
                            }
                        }
                    }
                    else
                    {
                        Toast.MakeText(context, "Invalid Date Format.", ToastLength.Short).Show();
                    }
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(context, $"Error adding task, please try again. {ex.Message}", ToastLength.Short).Show();
            }
        }

        // Function to Update Task if it is Done already
        public void TaskDone(string username, string taskName, string taskClass)
        {
            try
            {
                string requestUrl = $"{url}/task_IsDone.php?username={username}&taskName={taskName}&taskClass={taskClass}";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    // If the result contains "OK!", show a toast message that the task is marked as done
                    string result = reader.ReadToEnd();
                    if (result.Contains("OK!"))
                    {
                        Toast.MakeText(context, "Task Marked as Done.", ToastLength.Short).Show();
                    }
                    else
                    {
                        Toast.MakeText(context, "Task not Marked as Done.", ToastLength.Short).Show();
                    }
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(context, "Error marking task as done, please try again.", ToastLength.Short).Show();
            }
        }

        // Function to Delete Task
        public void DeleteTask(string username, string taskName, string taskClass)
        {
            string requestUrl = $"{url}/delete_task.php?username={username}&taskName={taskName}&taskClass={taskClass}";

            request = (HttpWebRequest)WebRequest.Create(requestUrl);
            response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());

            var result = reader.ReadToEnd();
            
            // If the result contains "OK!", show a toast message that the task is deleted
            if (result.Contains("OK!"))
            {
                Toast.MakeText(context, "Task Deleted.", ToastLength.Short).Show();

                NextActivityHandler nextActivityHandler = new NextActivityHandler(context, "", typeof(Dashboard));
                nextActivityHandler.PassDataToNextActivity("username", username);
                nextActivityHandler.NavigateToNextActivity(context);
            }
            else
            {
                Toast.MakeText(context, "Task Not Deleted.", ToastLength.Short).Show();
            }


        }

        // Function to Check if the Date is Valid
        static bool IsValidDate(string date, out DateTime validDate)
        {
            // Array of date formats
            string[] formats = { "d/M/yyyy", "dd/M/yyyy", "d/MM/yyyy", "dd/MM/yyyy" };

            // Check if the date is valid
            bool isValid = DateTime.TryParseExact(date, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out validDate);
            return isValid;
        }
        // Function to Reformat the Date
        static string ReformatDate(string date)
        {
            try
            {
                // Attempt to parse the date
                DateTime parsedDate = DateTime.ParseExact(date, "d/M/yyyy", CultureInfo.InvariantCulture);
                return parsedDate.ToString("dd/MM/yyyy");
            }
            catch
            {
                return date;
            }
        }

    }
}
