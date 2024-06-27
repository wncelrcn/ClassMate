using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace IT123P_FinalMP
{
    internal class UserClass
    {

        HttpWebResponse response;
        HttpWebRequest request;
        string url = "http://172.18.24.225/IT123P_FinalMP/REST";
        string result;

        private Context context;

        public UserClass(Context context)
        {
            this.context = context;
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

                //NextActivityHandler nextActivityHandler = new NextActivityHandler(context, "Registration Successful", typeof(DashboardActivity));
                //nextActivityHandler.NavigateToNextActivity();
            }
            else
            {
                Toast.MakeText(context, "Class is not Added", ToastLength.Short).Show();
            }
        }

    }
}