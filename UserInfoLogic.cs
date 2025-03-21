﻿using Android.Content;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;

namespace IT123P_FinalMP
{
    internal class UserInfoLogic
    {
        string url = "http://192.168.100.11/IT123P_FinalMP/REST";
        private Context context;

        public UserInfoLogic(Context context)
        {
            this.context = context;
        }

        // Function to Update Student Information
        public void UpdateStudentInfo(string newName, string newStudID, string newCourse, string newSchool, string username)
        {
            string requestUrl = $"{url}/update_StudInfo.php?uname={username}&studName={newName}&studID={newStudID}&studCourse={newCourse}&studSchool={newSchool}";

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    var result = reader.ReadToEnd();

                    // If the result contains "OK!", show a toast message that the student info is updated
                    if (result.Contains("OK!"))
                    {
                        Toast.MakeText(context, "Student Info Updated", ToastLength.Short).Show();

                        NextActivityHandler nextActivityHandler = new NextActivityHandler(context, "", typeof(ViewAccount));
                        nextActivityHandler.PassDataToNextActivity("username", username);
                        nextActivityHandler.NavigateToNextActivity(context);
                    }
                    // If the result contains "NO!", show a toast message that the student info is not updated
                    else
                    {
                        Toast.MakeText(context, "Student Info not Updated", ToastLength.Short).Show();
                    }
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(context, $"Error: {ex.Message}", ToastLength.Short).Show();
            }
        }

        // Function to Get User Details
        public Dictionary<string, string> GetUserDetails(string username)
        {
            string requestUrl = $"{url}/get_StudInfo.php?username={username}";

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    var result = reader.ReadToEnd();

                    // Parse the JSON result
                    using JsonDocument doc = JsonDocument.Parse(result);
                    JsonElement root = doc.RootElement;

                    // Create a dictionary to store the user details
                    var userDetails = new Dictionary<string, string>();

                    // Store the user details in the dictionary
                    foreach (JsonProperty property in root[0].EnumerateObject())
                    {
                        userDetails[property.Name] = property.Value.GetString();
                    }

                    return userDetails;
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(context, $"Error: {ex.Message}", ToastLength.Short).Show();
                return null;
            }
        }
    }
}