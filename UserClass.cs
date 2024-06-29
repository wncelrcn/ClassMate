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

        HttpWebResponse response;
        HttpWebRequest request;
        string url = "http://192.168.1.94:8080/IT123P_FinalMP/REST";
        string result;
        List<Dictionary<string, string>> userClasses = new List<Dictionary<string, string>>();
        LinearLayout currLayout;


        private Context context;

        public UserClass(Context context)
        {
            this.context = context;
        }

        public UserClass(Context context, LinearLayout currLayout)
        {
            this.context = context;
            this.currLayout = currLayout;
        }


        public void UserAddClass(string username, string classCode, string className)
        {
            url = $"{url}/add_class.php?username={username}&className={className}&classCode={classCode}";

            request = (HttpWebRequest)WebRequest.Create(url);
            response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());

            result = reader.ReadToEnd();

            if (result.Contains("OK!"))
            {
                Toast.MakeText(context, "Classes Added", ToastLength.Short).Show();
            }
            else
            {
                Toast.MakeText(context, "Class is not Added", ToastLength.Short).Show();
            }
        }



        public async Task GetCurrStudClasses(string username)
        {
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

        public void CreateClassLayout(string username)
        {
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

                // Create a TextView for the class code
                TextView textViewCode = new TextView(context)
                {
                    Text = classes["classCode"].ToString(), // Ensure it's converted to string
                    
                };
                textViewCode.SetPadding(10, 30, 0, 50); // Adding padding below the text
                textViewCode.SetTextColor(Android.Graphics.Color.Black);
                textViewCode.SetTextSize(Android.Util.ComplexUnitType.Sp, 24); // Set text size to 24sp
                boldFont.SetFont(textViewCode); // Set the bold font

                // Add the class code TextView to the LinearLayout
                linearLayout.AddView(textViewCode);

                // Create a TextView for the class name
                TextView textViewName = new TextView(context)
                {
                    Text = classes["className"].ToString(), // Ensure it's converted to string
                    
                };
                textViewName.SetPadding(10, 0, 0, 30); // Set padding
                textViewName.SetTextColor(Android.Graphics.Color.Black);
                textViewName.SetTextSize(Android.Util.ComplexUnitType.Sp, 18); // Set text size to 18sp
                regularFont.SetFont(textViewName); // Set the regular font

                // Add the class name TextView to the LinearLayout
                linearLayout.AddView(textViewName);

                // Add the LinearLayout to the parent container
                currLayout.AddView(linearLayout);

                // Attach click event to the LinearLayout if you want to handle clicks
                linearLayout.Click += (sender, e) =>
                {


                    NextActivityHandler nextActivity = new NextActivityHandler(context, username, typeof(ClassesSpecific), "");
                    nextActivity.PassDataToNextActivity("classCode", classes["classCode"]);
                    nextActivity.PassDataToNextActivity("className", classes["className"]);
                    nextActivity.PassDataToNextActivity("username", username);
                    nextActivity.NavigateToNextActivity(context);

                    // Handle the click event, for example, navigate to another activity
                    Toast.MakeText(context, "Class Code: " + classes["classCode"], ToastLength.Short).Show();
                };
            }
        }





    }

}
