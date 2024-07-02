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

        HttpWebResponse response;
        HttpWebRequest request;

        string url = "http://172.18.11.241:8080/IT123P_FinalMP/REST";
        string result;

        private Context context;

        public UserConnection(Context context)
        {
            this.context = context;
        }

        public bool CheckStudUname(string studUname)
        {
            url = $"{url}/check_studUsername.php?uname={studUname}";

            request = (HttpWebRequest)WebRequest.Create(url);
            response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());

            result = reader.ReadToEnd();

            if (result.Contains("OK!"))
            {
                //Toast.MakeText(context, "Student ID is available", ToastLength.Short).Show();
                return true;
            }
            else
            {
                Toast.MakeText(context, "Username is already taken", ToastLength.Short).Show();
                return false;
            }

        }
        public bool CheckStudID(string studID)
        {
            url = $"{url}/check_studID.php?studID={studID}";

            request = (HttpWebRequest)WebRequest.Create(url);
            response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());

            result = reader.ReadToEnd();

            if (result.Contains("OK!"))
            {
                //Toast.MakeText(context, "Student ID is available", ToastLength.Short).Show();
                return true;
            }
            else
            {
                Toast.MakeText(context, "Student ID is already taken", ToastLength.Short).Show();
                return false;
            }

        }
        public void Register(string username, string password, string studID, string studName, string studSchool, string studCourse, string studIdentity)
        {
            url = $"{url}/register.php?uname={username}&pword={password}&studID={studID}&studName={studName}&studSchool={studSchool}&studCourse={studCourse}&studIdentity={studIdentity}";


            request = (HttpWebRequest)WebRequest.Create(url);
            response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());

            result = reader.ReadToEnd();

            if (result.Contains("OK!"))
            {
                NextActivityHandler nextActivityHandler = new NextActivityHandler(context, "Next...", typeof(Dashboard));

                nextActivityHandler.PassDataToNextActivity("username", username);

                nextActivityHandler.NavigateToNextActivity(context);

            }
            else
            {
                Toast.MakeText(context, "Registration Failed", ToastLength.Short).Show();
            }
        }

        public void Login(string username, string password)
        {
            url = $"{url}/login.php?uname={username}&pword={password}";

            request = (HttpWebRequest)WebRequest.Create(url);
            response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());

            string data = reader.ReadToEnd();

            if (data.Contains("OK!"))
            {
                //Toast.MakeText(context, "Login Success", ToastLength.Short).Show();

                NextActivityHandler nextActivityHandler = new NextActivityHandler(context, "Login Successful", typeof(Dashboard));
                nextActivityHandler.PassDataToNextActivity("username", username);
                nextActivityHandler.PassDataToNextActivity("password", password);
                nextActivityHandler.NavigateToNextActivity(context);
            }
            else
            {
                Toast.MakeText(context, "Login Failed", ToastLength.Short).Show();
            }


        }

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
                Toast.MakeText(context, "Error updating password", ToastLength.Short).Show();
                return false;
            }
        }




    }
}