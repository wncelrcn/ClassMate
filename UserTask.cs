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

namespace IT123P_FinalMP
{
    internal class UserTask
    {
        HttpWebResponse response;
        HttpWebRequest request;
        string result;
        string url = "http://192.168.1.89/IT123P_FinalMP/REST";
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
                Console.WriteLine($"Error in GetTasksPerDate: {ex.Message}");
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
                                { "taskClass", item.GetProperty("taskClass").GetString() }
                            };
                            userTasks.Add(task);
                        }
                        ((Activity)context).RunOnUiThread(() => CreateTaskLayout(userTasks));
                    }
                    else
                    {
                        throw new InvalidOperationException("The JSON is not an array.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching JSON: " + ex.Message);
                Toast.MakeText(context, "Error fetching tasks. Please try again.", ToastLength.Short).Show();
            }
        }

        public void CreateTaskLayout(List<Dictionary<string, string>> tasks)
        {
            try
            {
                currLayout.RemoveAllViews();

                // Initialize your FontHandler instances
                FontHandler boldFont = new FontHandler(context, "Raleway-Bold.ttf");
                FontHandler mediumFont = new FontHandler(context, "Raleway-Medium.ttf");
                FontHandler regularFont = new FontHandler(context, "Raleway-Regular.ttf");
                FontHandler semiBoldFont = new FontHandler(context, "Raleway-Semibold.ttf");

                foreach (var task in tasks)
                {
                    // Create a new LinearLayout
                    LinearLayout linearLayout = new LinearLayout(context)
                    {
                        Orientation = Orientation.Vertical
                    };

                    linearLayout.SetBackgroundColor(Android.Graphics.Color.ParseColor("#EDE7F6")); // Light purple background

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
                    boldFont.SetFont(textViewName); // Set the bold font

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
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CreateTaskLayout: {ex.Message}");
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
                    }

                    if (result.Contains("OK!"))
                    {
                        Toast.MakeText(context, "Task Added", ToastLength.Short).Show();
                    }
                    else
                    {
                        Toast.MakeText(context, "Task not Added", ToastLength.Short).Show();
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
                        }

                        if (result.Contains("OK!"))
                        {
                            Toast.MakeText(context, "Task Added", ToastLength.Short).Show();
                        }
                        else
                        {
                            Toast.MakeText(context, "Task not Added", ToastLength.Short).Show();
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
                Console.WriteLine($"Error in InsertTask: {ex.Message}");
                Toast.MakeText(context, "Error adding task. Please try again.", ToastLength.Short).Show();
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
                // If parsing fails, return the original date string
                return date;
            }
        }
    }
}
