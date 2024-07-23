using Android.Content;
using Android.Widget;
using System;
using System.IO;
using System.Net;

namespace IT123P_FinalMP
{
    internal class UserConnection
    {
        string url = "http://192.168.100.11/IT123P_FinalMP/REST";
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
            string requestUrl = $"{url}/check_studUsername.php?uname={studUname}";

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();

                    if (result.Contains("OK!"))
                    {
                        return true;
                    }
                    else
                    {
                        Toast.MakeText(context, "Username is already taken.", ToastLength.Short).Show();
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(context, $"Error: {ex.Message}", ToastLength.Short).Show();
                return false;
            }
        }

        // Function to Check the Student ID
        public bool CheckStudID(string studID)
        {
            // URL to check the student ID
            string requestUrl = $"{url}/check_studID.php?studID={studID}";

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
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
            }
            catch (Exception ex)
            {
                Toast.MakeText(context, $"Error: {ex.Message}", ToastLength.Short).Show();
                return false;
            }
        }

        // Function to Register the Student
        public void Register(string username, string password, string studID, string studName, string studSchool, string studCourse, string studIdentity)
        {
            string requestUrl = $"{url}/register.php?uname={username}&pword={password}&studID={studID}&studName={studName}&studSchool={studSchool}&studCourse={studCourse}&studIdentity={studIdentity}";

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
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
            }
            catch (Exception ex)
            {
                Toast.MakeText(context, $"Error: {ex.Message}", ToastLength.Short).Show();
            }
        }

        // Function to Login the Student
        public void Login(string username, string password)
        {
            string requestUrl = $"{url}/login.php?uname={username}&pword={password}";

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
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
            }
            catch (Exception ex)
            {
                Toast.MakeText(context, $"Error: {ex.Message}", ToastLength.Short).Show();
            }
        }

        // Function to Update the Student's Password
        public bool UpdatePassword(string username, string oldPass, string newPass)
        {
            string requestUrl = $"{url}/update_password.php?uname={username}&newPass={newPass}&oldPass={oldPass}";

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUrl);

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string result = reader.ReadToEnd();
                    return result.Contains("OK!");
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(context, $"Error: {ex.Message}", ToastLength.Short).Show();
                return false;
            }
        }
    }
}