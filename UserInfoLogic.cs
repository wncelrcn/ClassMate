using Android.Content;
using Android.Widget;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Text.Json;

namespace IT123P_FinalMP
{
    internal class UserInfoLogic
    {

        HttpWebResponse response;
        HttpWebRequest request;
        string url = "http://192.168.1.90:8080/IT123P_FinalMP/REST";


        private Context context;

        public UserInfoLogic(Context context)
        {
            this.context = context;
        }


        public void UpdateStudentInfo(string newName, string newStudID, string newCourse, string newSchool, string username)
        {
            url = $"{url}/update_StudInfo.php?uname={username}&studName={newName}&studID={newStudID}&studCourse={newCourse}&studSchool={newSchool}";

            request = (HttpWebRequest)WebRequest.Create(url);
            response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());

            var result = reader.ReadToEnd();

            if (result.Contains("OK!"))
            {
                Toast.MakeText(context, "Student Info Updated", ToastLength.Short).Show();

                NextActivityHandler nextActivityHandler = new NextActivityHandler(context, "Next...", typeof(ViewAccount));
                nextActivityHandler.PassDataToNextActivity("username", username);
                nextActivityHandler.NavigateToNextActivity(context);
            }
            else
            {
                Toast.MakeText(context, "Student Info not Updated", ToastLength.Short).Show();
            }
        }



        public Dictionary<string, string> GetUserDetails(string username)
        {
            url = $"{url}/get_StudInfo.php?username={username}";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());

            var result = reader.ReadToEnd();

            using JsonDocument doc = JsonDocument.Parse(result);
            JsonElement root = doc.RootElement;

            var userDetails = new Dictionary<string, string>();

            foreach (JsonProperty property in root[0].EnumerateObject())
            {
                userDetails[property.Name] = property.Value.GetString();
            }

            return userDetails;
        }



    }
}