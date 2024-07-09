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
using Android.Service.Autofill;

namespace IT123P_FinalMP
{
    internal class UserClass
    {
        // Initialize the variables
        HttpWebResponse response;
        HttpWebRequest request;
        string url = "http://192.168.1.9:8080/IT123P_FinalMP/REST";
        string result;
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
            url = $"{url}/add_class.php?username={username}&className={className}&classCode={classCode}";

            // Create a new HttpWebRequest
            request = (HttpWebRequest)WebRequest.Create(url);
            response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());

            result = reader.ReadToEnd();

            // Check if the result contains "OK!"
            if (result.Contains("OK!"))
            {
                
                Toast.MakeText(context, "Class Added.", ToastLength.Short).Show();
                NextActivityHandler nextActivityHandler = new NextActivityHandler(context, "Next...", typeof(ClassesMainView));
                nextActivityHandler.PassDataToNextActivity("username", username);
                nextActivityHandler.NavigateToNextActivity(context);
            }
            // If the result does not contain "OK!"
            else
            {
                Toast.MakeText(context, "Class is not Added.", ToastLength.Short).Show();
            }
        }

        // Function to Delete Class
        public void UserDeleteClass(string username, string classCode)
        {
            // Concatenate the URL
            url = $"{url}/delete_class.php?username={username}&classCode={classCode}";

            request = (HttpWebRequest)WebRequest.Create(url);
            response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());

            result = reader.ReadToEnd();

            // Check if the result contains "OK!"
            if (result.Contains("OK!"))
            {
                Toast.MakeText(context, "Class is Deleted.", ToastLength.Short).Show();
                NextActivityHandler nextActivityHandler = new NextActivityHandler(context, "Next...", typeof(ClassesMainView));
                nextActivityHandler.PassDataToNextActivity("username", username);
                nextActivityHandler.NavigateToNextActivity(context);
            }

            // If the result does not contain "OK!"
            else
            {
                Toast.MakeText(context, "Class is not Deleted.", ToastLength.Short).Show();
            }
        }

        // Function to Get Current Student Classes
        public async Task GetCurrStudClasses(string username)
        {
            // Concatenate the URL
            url = $"{url}/get_StudClasses.php?username={username}";

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";

                using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    // Read the response to the end
                    var result = await reader.ReadToEndAsync();
                    using JsonDocument doc = JsonDocument.Parse(result);
                    JsonElement root = doc.RootElement;

                    // Assuming the JSON is an array
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
                        // Create the class layout
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
            url = $"{url}/get_StudClasses.php?username={username}";

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";

                using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    // Read the response to the end
                    var result = await reader.ReadToEndAsync();
                    using JsonDocument doc = JsonDocument.Parse(result);
                    JsonElement root = doc.RootElement;

                    // Assuming the JSON is an array
                    if (root.ValueKind == JsonValueKind.Array)
                    {
                        // Clear the userClasses list
                        userClasses.Clear();
                        foreach (JsonElement item in root.EnumerateArray())
                        {
                            // Check if the JSON contains the className and classCode properties
                            if (item.TryGetProperty("className", out JsonElement className) && item.TryGetProperty("classCode", out JsonElement classCode))
                            {
                                // Create a new dictionary

                                var user = new Dictionary<string, string>
                                {
                                    { "classCode", classCode.GetString() },
                                    { "className", className.GetString() }
                                };
                                // Add the dictionary to the userClasses list
                                userClasses.Add(user);
                            }
                        }
                        // Create a list of class codes
                        var classCodes = new List<string>();
                        // Loop through the userClasses list
                        foreach (var userClass in userClasses)
                        {
                            // Check if the classCode property exists
                            if (userClass.TryGetValue("classCode", out var classCode))
                            {
                                // Add the classCode to the classCodes list
                                classCodes.Add(classCode);
                            }
                        }
                        // Create an adapter for the spinner
                        ((Activity)context).RunOnUiThread(() =>
                        {
                            // Create an adapter for the spinner
                            var adapter = new ArrayAdapter<string>(context, Android.Resource.Layout.SimpleSpinnerItem, classCodes);
                            // Set the dropdown view resource
                            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
                            // Set the adapter to the spinner
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
            // Clear the current layout
            currLayout.RemoveAllViews();

            // Initialize your FontHandler instances
            FontHandler boldFont = new FontHandler(context, "Raleway-Bold.ttf");
            FontHandler mediumFont = new FontHandler(context, "Raleway-Medium.ttf");
            FontHandler regularFont = new FontHandler(context, "Raleway-Regular.ttf");
            FontHandler semiBoldFont = new FontHandler(context, "Raleway-Semibold.ttf");

            foreach (var classes in userClasses)
            {
                // Create a new LinearLayout
                LinearLayout linearLayout = new LinearLayout(context)
                {
                    Orientation = Orientation.Vertical
                };

                linearLayout.SetBackgroundColor(Android.Graphics.Color.ParseColor("#DDEDEA")); // Light green background

                // Create layout parameters 
                LinearLayout.LayoutParams layoutParams = new LinearLayout.LayoutParams(
                    LinearLayout.LayoutParams.MatchParent,
                    LinearLayout.LayoutParams.WrapContent
                );
                layoutParams.SetMargins(0, 0, 0, 50); 

                
                linearLayout.LayoutParameters = layoutParams;
                linearLayout.SetPadding(20, 20, 20, 20); 

                // Create a TextView for the class code
                TextView textViewCode = new TextView(context)
                {
                    Text = classes["classCode"].ToString(), 
                    
                };
                textViewCode.SetPadding(10, 30, 0, 50); 
                textViewCode.SetTextColor(Android.Graphics.Color.Black);
                textViewCode.SetTextSize(Android.Util.ComplexUnitType.Sp, 24); 
                boldFont.SetFont(textViewCode);

                // Add the class code TextView to the LinearLayout
                linearLayout.AddView(textViewCode);

                // Create a TextView for the class name
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
