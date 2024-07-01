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

        string url = "http://192.168.1.90:8080/IT123P_FinalMP/REST";


        private Context context;

        public UserTask(Context context)
        {
            this.context = context;
        }

        public void InsertTask(string username, string taskName, string taskDesc, bool isDone, string toDoDate, string dueDate, string taskClass)
        {
            HttpWebRequest request;
            HttpWebResponse response;
            string result;

            if (IsValidDate(toDoDate, out DateTime validToDoDate) && IsValidDate(dueDate, out DateTime validDueDate))
            {
                string convertedDateStrTDD = validToDoDate.ToString("yyyy-MM-dd");
                string convertedDateStrDD = validDueDate.ToString("yyyy-MM-dd");

                url = $"{url}/add_task.php?username={username}&taskName={taskName}&taskDesc={taskDesc}&isDone={isDone}&toDoDate={convertedDateStrTDD}&dueDate={convertedDateStrDD}&taskClass={taskClass}";

                request = (HttpWebRequest)WebRequest.Create(url);
                response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                }

                if (result.Contains("OK!"))
                {
                    Toast.MakeText(context, "Task Added", ToastLength.Short).Show();
                }
                else
                {
                    Toast.MakeText(context, "Task not Added", ToastLength.Short).Show();
                }
            }
            else
            {
                // Attempt to reformat the dates if invalid
                toDoDate = ReformatDate(toDoDate);
                dueDate = ReformatDate(dueDate);

                if (IsValidDate(toDoDate, out validToDoDate) && IsValidDate(dueDate, out validDueDate))
                {
                    string convertedDateStrTDD = validToDoDate.ToString("yyyy-MM-dd");
                    string convertedDateStrDD = validDueDate.ToString("yyyy-MM-dd");

                    url = $"{url}/add_task.php?username={username}&taskName={taskName}&taskDesc={taskDesc}&isDone={isDone}&toDoDate={convertedDateStrTDD}&dueDate={convertedDateStrDD}&taskClass={taskClass}";

                    request = (HttpWebRequest)WebRequest.Create(url);
                    response = (HttpWebResponse)request.GetResponse();
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        result = reader.ReadToEnd();
                    }

                    if (result.Contains("OK!"))
                    {
                        Toast.MakeText(context, "Task Added", ToastLength.Short).Show();
                    }
                    else
                    {
                        Toast.MakeText(context, "Task not Added", ToastLength.Short).Show();
                    }
                }
                else
                {
                    Toast.MakeText(context, "Invalid Date Format", ToastLength.Short).Show();
                }
            }
        }

        static bool IsValidDate(string date, out DateTime validDate)
        {
            string[] formats = { "d/M/yyyy", "dd/M/yyyy", "d/MM/yyyy", "dd/MM/yyyy" };
            bool isValid = DateTime.TryParseExact(date, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out validDate);
            return isValid;
        }

        static string ReformatDate(string date)
        {
            try
            {
                DateTime parsedDate = DateTime.ParseExact(date, "d/M/yyyy", CultureInfo.InvariantCulture);
                return parsedDate.ToString("dd/MM/yyyy");
            }
            catch
            {
                // If parsing fails, return the original date string
                return date;
            }
        }

    }
}