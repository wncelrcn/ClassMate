using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Annotations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace IT123P_FinalMP
{
    internal class UserConnection
    {
        // HttpWebResponse and HttpWebRequest for REST API
        HttpWebResponse response;
        HttpWebRequest request;

        string url = "http://192.168.1.9:8080/IT123P_FinalMP/REST";
        string result;

        private Context context;

        // Constructor
        public UserConnection(Context context)
        {
            this.context = context;
        }

        // Function to Check the Student Username
        public bool CheckStudUname(string studUname)
        {
            // URL to check the student username
            url = $"{url}/check_studUsername.php?uname={studUname}";

            request = (HttpWebRequest)WebRequest.Create(url);
            response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());

            result = reader.ReadToEnd();

            // If the result contains "OK!", return true
            if (result.Contains("OK!"))
            {
                return true;
            }

            // If the result contains "NO!", return false
            else
            {
                Toast.MakeText(context, "Username is already taken.", ToastLength.Short).Show();
                return false;
            }

        }

        // Function to Check the Student ID
        public bool CheckStudID(string studID)
        {
            // URL to check the student ID
            url = $"{url}/check_studID.php?studID={studID}";

            request = (HttpWebRequest)WebRequest.Create(url);
            response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());

            result = reader.ReadToEnd();

            if (result.Contains("OK!"))
            {
                return true;
            }
            else
            {
                Toast.MakeText(context, "Student ID is already taken.", ToastLength.Short).Show();
                return false;
            }

        }

        // Function to Register the Student
        public void Register(string username, string password, string studID, string studName, string studSchool, string studCourse, string studIdentity)
        {
            url = $"{url}/register.php?uname={username}&pword={password}&studID={studID}&studName={studName}&studSchool={studSchool}&studCourse={studCourse}&studIdentity={studIdentity}";

            request = (HttpWebRequest)WebRequest.Create(url);
            response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());

            result = reader.ReadToEnd();

            if (result.Contains("OK!"))
            {
                // Successful Registration
                NextActivityHandler nextActivityHandler = new NextActivityHandler(context, "", typeof(Dashboard));
                nextActivityHandler.PassDataToNextActivity("username", username);
                nextActivityHandler.NavigateToNextActivity(context);

            }
            else
            {
                // Failed Registration
                Toast.MakeText(context, "Registration Failed", ToastLength.Short).Show();
            }
        }

        // Function to Login the Student
        public void Login(string username, string password)
        {
            url = $"{url}/login.php?uname={username}&pword={password}";

            request = (HttpWebRequest)WebRequest.Create(url);
            response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());

            string data = reader.ReadToEnd();

            if (data.Contains("OK!"))
            {
                // Successful Login
                NextActivityHandler nextActivityHandler = new NextActivityHandler(context, "Login Successful", typeof(Dashboard));
                nextActivityHandler.PassDataToNextActivity("username", username);
                nextActivityHandler.PassDataToNextActivity("password", password);
                nextActivityHandler.NavigateToNextActivity(context);
            }
            else
            {
                // Failed Login
                Toast.MakeText(context, "Incorrect username or password.", ToastLength.Short).Show();
            }
        }

        // Function to Update the Student's Password
        public bool UpdatePassword(string username, string oldPass, string newPass)
        {
            url = $"{url}/update_password.php?uname={username}&newPass={newPass}&oldPass={oldPass}";

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string result = reader.ReadToEnd();
                    return result.Contains("OK!");
                }
            }
            catch (Exception)
            {
                Toast.MakeText(context, "Error updating password.", ToastLength.Short).Show();
                return false;
            }
        }
    }
}