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

        public void Register(string username, string password, string studID, string studName, string studSchool, string studCourse, string studIdentity)
        {
            url = $"{url}/register.php?uname={username}&pword={password}&studID={studID}&studName={studName}&studSchool={studSchool}&studCourse={studCourse}&studIdentity={studIdentity}";


            request = (HttpWebRequest)WebRequest.Create(url);
            response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());

            result = reader.ReadToEnd();

            if (result.Contains("OK!"))
            {
                Toast.MakeText(context, "Registration Success", ToastLength.Short).Show();

                //NextActivityHandler nextActivityHandler = new NextActivityHandler(context, "Registration Successful", typeof(DashboardActivity));
                //nextActivityHandler.NavigateToNextActivity();
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
                nextActivityHandler.NavigateToNextActivity(context);
            }
            else
            {
                Toast.MakeText(context, "Login Failed", ToastLength.Short).Show();
            }


        }


    }
}