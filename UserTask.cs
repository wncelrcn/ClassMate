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
        HttpWebResponse response;
        HttpWebRequest request;
        string result;
        string url = "http://192.168.1.66:8080/IT123P_FinalMP/REST";
        LinearLayout currLayout;
        private Context context;


        public UserTask(Context context)
        {
            this.context = context;
        }

        public UserTask(Context context, LinearLayout currLayout)
        {
            this.context = context;
            this.currLayout = currLayout;
        }

        public async Task GetTaskPerClass(string username, string classCode, string className)
        {
            try
            {
                await FetchTasks(username, classCode, className);
            }
            catch (Exception ex)
            {
                Toast.MakeText(context, "Error fetching tasks. Please try again.", ToastLength.Short).Show();
            }
        }

        private async Task FetchTasks(string username, string taskClass, string className)
        {
            string requestUrl = $"{url}/get_TaskPerClass.php?username={username}&taskClass={taskClass}";

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);
                request.Method = "GET";

                using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    var result = await reader.ReadToEndAsync();
                    Console.WriteLine($"API Response: {result}"); // Debug API response

                    using JsonDocument doc = JsonDocument.Parse(result);
                    JsonElement root = doc.RootElement;

                    if (root.ValueKind == JsonValueKind.Array)
                    {
                        var userTasks = new List<Dictionary<string, string>>();
                        foreach (JsonElement item in root.EnumerateArray())
                        {
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
                Toast.MakeText(context, "Error fetching tasks. Please try again.", ToastLength.Short).Show();
            }
        }


        public async Task GetTasksPerDate(string username, string date)
        {
            try
            {
                if (IsValidDate(date, out DateTime validDate))
                {
                    await FetchTasks(username, validDate);
                }
                else
                {
                    // Attempt to reformat the date if invalid
                    date = ReformatDate(date);

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
                
                Toast.MakeText(context, "Error fetching tasks. Please try again.", ToastLength.Short).Show();
            }
        }

        private async Task FetchTasks(string username, DateTime date)
        {
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
                    Console.WriteLine($"API Response: {result}"); // Debug API response

                    using JsonDocument doc = JsonDocument.Parse(result);
                    JsonElement root = doc.RootElement;

                    if (root.ValueKind == JsonValueKind.Array)
                    {
                        var userTasks = new List<Dictionary<string, string>>();
                        foreach (JsonElement item in root.EnumerateArray())
                        {
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
                
                Toast.MakeText(context, "Error fetching tasks. Please try again.", ToastLength.Short).Show();
            }
        }

        public void CreateTaskLayout(List<Dictionary<string, string>> tasks, string layout)
        {
            try
            {
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
                    noTasksTextView.SetTextSize(Android.Util.ComplexUnitType.Sp, 20); // Set text size to 18sp
                    mediumFont.SetFont(noTasksTextView); // Set the medium font

                    // Create layout parameters with top margin
                    LinearLayout.LayoutParams noTasksLayoutParams = new LinearLayout.LayoutParams(
                        LinearLayout.LayoutParams.WrapContent,
                        LinearLayout.LayoutParams.WrapContent
                    );
                    noTasksLayoutParams.SetMargins(0, 70, 0, 0); // Add top margin of 50px
                    noTasksLayoutParams.Gravity = GravityFlags.Center;

                    // Apply the layout parameters to the TextView
                    noTasksTextView.LayoutParameters = noTasksLayoutParams;

                    // Add the TextView to the parent container
                    currLayout.AddView(noTasksTextView);

                    // Exit the method early since there are no tasks to display
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

                    linearLayout.SetBackgroundColor(Android.Graphics.Color.ParseColor(backgroundColor)); // Set background color

                    // Create layout parameters with margins
                    LinearLayout.LayoutParams layoutParams = new LinearLayout.LayoutParams(
                        LinearLayout.LayoutParams.MatchParent,
                        LinearLayout.LayoutParams.WrapContent
                    );
                    layoutParams.SetMargins(0, 0, 0, 50);

                    // Apply the layout parameters to the LinearLayout
                    linearLayout.LayoutParameters = layoutParams;
                    linearLayout.SetPadding(20, 20, 20, 20); // Adding padding

                    // Create a TextView for the task name
                    TextView textViewName = new TextView(context)
                    {
                        Text = task["taskName"] // Ensure it's converted to string
                    };
                    textViewName.SetPadding(10, 10, 0, 10); // Adding padding below the text
                    textViewName.SetTextColor(Android.Graphics.Color.Black);
                    textViewName.SetTextSize(Android.Util.ComplexUnitType.Sp, 24); // Set text size to 24sp
                    semiBoldFont.SetFont(textViewName); // Set the semibold font

                    // Add the task name TextView to the LinearLayout
                    linearLayout.AddView(textViewName);

                    // Create a TextView for the task course code
                    TextView textViewCourse = new TextView(context)
                    {
                        Text = task["taskClass"] // Ensure it's converted to string
                    };
                    textViewCourse.SetPadding(10, 0, 0, 10); // Adding padding below the text
                    textViewCourse.SetTextColor(Android.Graphics.Color.Gray);
                    textViewCourse.SetTextSize(Android.Util.ComplexUnitType.Sp, 18); // Set text size to 18sp
                    mediumFont.SetFont(textViewCourse); // Set the medium font

                    // Add the task course code TextView to the LinearLayout
                    linearLayout.AddView(textViewCourse);

                    // Create a TextView for the due date
                    TextView textViewDueDate = new TextView(context)
                    {
                        Text = "Due Date: " + task["dueDate"] // Ensure it's converted to string
                    };
                    textViewDueDate.SetPadding(10, 0, 0, 10); // Adding padding below the text
                    textViewDueDate.SetTextColor(Android.Graphics.Color.Gray);
                    textViewDueDate.SetTextSize(Android.Util.ComplexUnitType.Sp, 18); // Set text size to 18sp
                    regularFont.SetFont(textViewDueDate); // Set the regular font

                    // Add the due date TextView to the LinearLayout
                    linearLayout.AddView(textViewDueDate);

                    // Add the LinearLayout to the parent container
                    currLayout.AddView(linearLayout);

                    // Attach click event to the LinearLayout if you want to handle clicks
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
                Toast.MakeText(context, "Error creating task layout. Please try again.", ToastLength.Short).Show();
            }
        }


        public void InsertTask(string username, string taskName, string taskDesc, bool isDone, string toDoDate, string dueDate, string taskClass)
        {
            try
            {
                if (IsValidDate(toDoDate, out DateTime validToDoDate) && IsValidDate(dueDate, out DateTime validDueDate))
                {
                    string convertedDateStrTDD = validToDoDate.ToString("yyyy-MM-dd");
                    string convertedDateStrDD = validDueDate.ToString("yyyy-MM-dd");

                    string requestUrl = $"{url}/add_task.php?username={username}&taskName={taskName}&taskDesc={taskDesc}&isDone={isDone}&toDoDate={convertedDateStrTDD}&dueDate={convertedDateStrDD}&taskClass={taskClass}";

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string result = reader.ReadToEnd();
                        if (result.Contains("OK!"))
                        {
                            Toast.MakeText(context, "Task Added", ToastLength.Short).Show();
                        }
                        else
                        {
                            Toast.MakeText(context, "Task not Added", ToastLength.Short).Show();
                        }
                    }
                }
                else
                {
                    // Attempt to reformat the dates if invalid
                    toDoDate = ReformatDate(toDoDate);
                    dueDate = ReformatDate(dueDate);

                    if (IsValidDate(toDoDate, out validToDoDate) && IsValidDate(dueDate, out validDueDate))
                    {
                        string convertedDateStrTDD = validToDoDate.ToString("yyyy-MM-dd");
                        string convertedDateStrDD = validDueDate.ToString("yyyy-MM-dd");

                        string requestUrl = $"{url}/add_task.php?username={username}&taskName={taskName}&taskDesc={taskDesc}&isDone={isDone}&toDoDate={convertedDateStrTDD}&dueDate={convertedDateStrDD}&taskClass={taskClass}";

                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            string result = reader.ReadToEnd();
                            if (result.Contains("OK!"))
                            {
                                Toast.MakeText(context, "Task Added", ToastLength.Short).Show();
                            }
                            else
                            {
                                Toast.MakeText(context, "Task not Added", ToastLength.Short).Show();
                            }
                        }
                    }
                    else
                    {
                        Toast.MakeText(context, "Invalid Date Format", ToastLength.Short).Show();
                    }
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(context, $"Error adding task. Please try again. {ex.Message}", ToastLength.Short).Show();
            }
        }


        public void TaskDone(string username, string taskName, string taskClass)
        {
            try
            {
                string requestUrl = $"{url}/task_IsDone.php?username={username}&taskName={taskName}&taskClass={taskClass}";

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string result = reader.ReadToEnd();
                    if (result.Contains("OK!"))
                    {
                        Toast.MakeText(context, "Task Marked as Done", ToastLength.Short).Show();
                    }
                    else
                    {
                        Toast.MakeText(context, "Task not Marked as Done", ToastLength.Short).Show();
                    }
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(context, "Error marking task as done. Please try again.", ToastLength.Short).Show();
            }
        }


        public void DeleteTask(string username, string taskName, string taskClass)
        {
            string requestUrl = $"{url}/delete_task.php?username={username}&taskName={taskName}&taskClass={taskClass}";

            request = (HttpWebRequest)WebRequest.Create(requestUrl);
            response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());

            var result = reader.ReadToEnd();

            if (result.Contains("OK!"))
            {
                Toast.MakeText(context, "Task Deleted", ToastLength.Short).Show();

                NextActivityHandler nextActivityHandler = new NextActivityHandler(context, "", typeof(Dashboard));
                nextActivityHandler.PassDataToNextActivity("username", username);
                nextActivityHandler.NavigateToNextActivity(context);
            }
            else
            {
                Toast.MakeText(context, "Task Not Deleted", ToastLength.Short).Show();
            }


        }

        static bool IsValidDate(string date, out DateTime validDate)
        {
            string[] formats = { "d/M/yyyy", "dd/M/yyyy", "d/MM/yyyy", "dd/MM/yyyy" };
            bool isValid = DateTime.TryParseExact(date, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out validDate);
            return isValid;
        }

        static string ReformatDate(string date)
        {
            try
            {
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
