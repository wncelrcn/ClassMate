using Android.Content;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;

namespace IT123P_FinalMP
{
    internal class UserGetInfo
    {

        HttpWebResponse response;
        HttpWebRequest request;
        string url = "http://172.18.24.225/IT123P_FinalMP/REST";


        private Context context;

        public UserGetInfo(Context context)
        {
            this.context = context;
        }


        public string GetUserName(string username)
        {
            url = $"{url}/get_StudName.php?username={username}";

            request = (HttpWebRequest)WebRequest.Create(url);
            response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream());

            var result = reader.ReadToEnd();

            using JsonDocument doc = JsonDocument.Parse(result);
            JsonElement root = doc.RootElement;

            var name = root[0];

            return name.GetProperty("studName").GetString();
        }

    }
}