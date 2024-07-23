using Android.Content;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;
using Android.App;
using System.Threading.Tasks;

namespace IT123P_FinalMP
{
    internal class UserClass
    {
        // Initialize the variables
        string url = "http://192.168.100.11/IT123P_FinalMP/REST";
        List<Dictionary<string, string>> userClasses = new List<Dictionary<string, string>>();
        LinearLayout currLayout;

        private Context context;

        // Constructor
        public UserClass(Context context)
        {
            this.context = context;
        }

        // Constructor with LinearLayout parameter
        public UserClass(Context context, LinearLayout currLayout)
        {
            this.context = context;
            this.currLayout = currLayout;
        }

        // Function to Add Class
        public void UserAddClass(string username, string classCode, string className)
        {
            // Concatenate the URL
            string requestUrl = $"{url}/add_class.php?username={username}&className={className}&classCode={classCode}";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string result = reader.ReadToEnd();

                    if (result.Contains("OK!"))
                    {
                        Toast.MakeText(context, "Class Added.", ToastLength.Short).Show();
                        NextActivityHandler nextActivityHandler = new NextActivityHandler(context, "Next...", typeof(ClassesMainView));
                        nextActivityHandler.PassDataToNextActivity("username", username);
                        nextActivityHandler.NavigateToNextActivity(context);
                    }
                    else
                    {
                        Toast.MakeText(context, "Class is not Added.", ToastLength.Short).Show();
                    }
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(context, $"Error: {ex.Message}", ToastLength.Short).Show();
            }
        }

        // Function to Delete Class
        public void UserDeleteClass(string username, string classCode)
        {
            // Concatenate the URL
            string requestUrl = $"{url}/delete_class.php?username={username}&classCode={classCode}";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string result = reader.ReadToEnd();

                    if (result.Contains("OK!"))
                    {
                        Toast.MakeText(context, "Class is Deleted.", ToastLength.Short).Show();
                        NextActivityHandler nextActivityHandler = new NextActivityHandler(context, "Next...", typeof(ClassesMainView));
                        nextActivityHandler.PassDataToNextActivity("username", username);
                        nextActivityHandler.NavigateToNextActivity(context);
                    }
                    else
                    {
                        Toast.MakeText(context, "Class is not Deleted.", ToastLength.Short).Show();
                    }
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(context, $"Error: {ex.Message}", ToastLength.Short).Show();
            }
        }

        // Function to Get Current Student Classes
        public async Task GetCurrStudClasses(string username)
        {
            // Concatenate the URL
            string requestUrl = $"{url}/get_StudClasses.php?username={username}";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);
            request.Method = "GET";

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    var result = await reader.ReadToEndAsync();
                    using JsonDocument doc = JsonDocument.Parse(result);
                    JsonElement root = doc.RootElement;

                    if (root.ValueKind == JsonValueKind.Array)
                    {
                        userClasses.Clear();
                        foreach (JsonElement item in root.EnumerateArray())
                        {
                            if (item.TryGetProperty("className", out JsonElement className) && item.TryGetProperty("classCode", out JsonElement classCode))
                            {
                                var user = new Dictionary<string, string>
                                {
                                    { "classCode", classCode.GetString() },
                                    { "className", className.GetString() }
                                };
                                userClasses.Add(user);
                            }
                        }
                        ((Activity)context).RunOnUiThread(() => CreateClassLayout(username));
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
            }
        }

        // Function to Fetch and Populate Classes
        public async Task FetchAndPopulateClasses(string username, Spinner classSpinner)
        {
            // Concatenate the URL
            string requestUrl = $"{url}/get_StudClasses.php?username={username}";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);
            request.Method = "GET";

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    var result = await reader.ReadToEndAsync();
                    using JsonDocument doc = JsonDocument.Parse(result);
                    JsonElement root = doc.RootElement;

                    if (root.ValueKind == JsonValueKind.Array)
                    {
                        userClasses.Clear();
                        foreach (JsonElement item in root.EnumerateArray())
                        {
                            if (item.TryGetProperty("className", out JsonElement className) && item.TryGetProperty("classCode", out JsonElement classCode))
                            {
                                var user = new Dictionary<string, string>
                                {
                                    { "classCode", classCode.GetString() },
                                    { "className", className.GetString() }
                                };
                                userClasses.Add(user);
                            }
                        }
                        var classCodes = new List<string>();
                        foreach (var userClass in userClasses)
                        {
                            if (userClass.TryGetValue("classCode", out var classCode))
                            {
                                classCodes.Add(classCode);
                            }
                        }
                        ((Activity)context).RunOnUiThread(() =>
                        {
                            var adapter = new ArrayAdapter<string>(context, Android.Resource.Layout.SimpleSpinnerItem, classCodes);
                            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                            classSpinner.Adapter = adapter;
                        });
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
            }
        }

        // Function to Create Class Layout
        public void CreateClassLayout(string username)
        {
            currLayout.RemoveAllViews();

            FontHandler boldFont = new FontHandler(context, "Raleway-Bold.ttf");
            FontHandler mediumFont = new FontHandler(context, "Raleway-Medium.ttf");
            FontHandler regularFont = new FontHandler(context, "Raleway-Regular.ttf");
            FontHandler semiBoldFont = new FontHandler(context, "Raleway-Semibold.ttf");

            foreach (var classes in userClasses)
            {
                LinearLayout linearLayout = new LinearLayout(context)
                {
                    Orientation = Orientation.Vertical
                };

                linearLayout.SetBackgroundColor(Android.Graphics.Color.ParseColor("#DDEDEA"));

                LinearLayout.LayoutParams layoutParams = new LinearLayout.LayoutParams(
                    LinearLayout.LayoutParams.MatchParent,
                    LinearLayout.LayoutParams.WrapContent
                );
                layoutParams.SetMargins(0, 0, 0, 50);

                linearLayout.LayoutParameters = layoutParams;
                linearLayout.SetPadding(20, 20, 20, 20);

                TextView textViewCode = new TextView(context)
                {
                    Text = classes["classCode"].ToString(),
                };
                textViewCode.SetPadding(10, 30, 0, 50);
                textViewCode.SetTextColor(Android.Graphics.Color.Black);
                textViewCode.SetTextSize(Android.Util.ComplexUnitType.Sp, 24);
                boldFont.SetFont(textViewCode);

                linearLayout.AddView(textViewCode);

                TextView textViewName = new TextView(context)
                {
                    Text = classes["className"].ToString(),
                };
                textViewName.SetPadding(10, 0, 0, 30);
                textViewName.SetTextColor(Android.Graphics.Color.Black);
                textViewName.SetTextSize(Android.Util.ComplexUnitType.Sp, 18);
                regularFont.SetFont(textViewName);

                linearLayout.AddView(textViewName);

                currLayout.AddView(linearLayout);

                linearLayout.Click += (sender, e) =>
                {
                    NextActivityHandler nextActivity = new NextActivityHandler(context, username, typeof(ClassesSpecific), "");
                    nextActivity.PassDataToNextActivity("classCode", classes["classCode"]);
                    nextActivity.PassDataToNextActivity("className", classes["className"]);
                    nextActivity.PassDataToNextActivity("username", username);
                    nextActivity.NavigateToNextActivity(context);
                };
            }
        }
    }
}