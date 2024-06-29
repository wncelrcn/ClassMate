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
using System.Globalization;

namespace IT123P_FinalMP
{
    internal class UserTask
    {


        HttpWebResponse response;
        HttpWebRequest request;
        string result;

        string url = "http://192.168.1.13:8080/IT123P_FinalMP/REST";


        private Context context;

        public UserTask(Context context)
        {
            this.context = context;
        }

        public void InsertTask(string username, string taskName, string taskDesc, bool isDone, string toDoDate, string dueDate, string taskClass)
        {
            DateTime originalDateTDD = DateTime.ParseExact(toDoDate, "dd/M/yyyy", null);
            string convertedDateStrTDD = originalDateTDD.ToString("yyyy-MM-dd");

            DateTime originalDateDD = DateTime.ParseExact(dueDate, "dd/M/yyyy", null);
            string convertedDateStrDD = originalDateDD.ToString("yyyy-MM-dd");

            url = $"{url}/add_task.php?username={username}&taskName={taskName}&taskDesc={taskDesc}&isDone={isDone}&toDoDate={convertedDateStrTDD}&dueDate={convertedDateStrDD}&taskClass={taskClass}";


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
    }
}