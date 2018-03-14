using CI.HttpClient;

namespace CC
{
    public class Net
    {
        public static void SendWeb()
        {
            HttpClient client = new HttpClient();

            StringContent content = new StringContent("APP_PARAM=H4sIAAAAAAAA%2F02PzQrCMBCE732KEvDmIWnTtHhWQfDgyXuSbjTUJiGNVBHf3bQ24sIyMB%2BzP68sj4W4DNoatMlRZzt7tBdt0HpBzp3BDwsmq2KHY5OEhTbtydvHc6LJlDcNJhzaf6%2Fn8qoNTBbluKENxFnbmhV1FEpBRBGK0ShcKqYwKytVixQ3vJ%2BzUgYYAsFlAm6c15CipBVL5uA88HbvbZ9%2BGuE36q6%2Fh2Xv7AP15kWt%2FwAAAA%3D%3D");
            //client.Headers.Add(System.Net.HttpRequestHeader.ContentType, "application/x-www-form-urlencoded;charset=UTF-8");
            client.Headers.Add(System.Net.HttpRequestHeader.UserAgent, "android");
            client.CustomHeaders.Add("project", "kokogame");
            client.CustomHeaders.Add("version", "1.1.1.1");
            client.CustomHeaders.Add("user-device", "pc");
            client.CustomHeaders.Add("Charsert", "UTF-8");
            client.CustomHeaders.Add("timestamp", "2018-03-12 16:30:01");
            client.CustomHeaders.Add("ak", "272d34bf33d53f7cb35ded6172556429");
            client.CustomHeaders.Add("mathRandom", "36760793");
            client.Post(new System.Uri("http://192.168.17.202:8088/appbms/app/login/login1.htm"), content, HttpCompletionOption.AllResponseContent, (r) =>
            {
                string kString = System.Text.Encoding.UTF8.GetString(r.Data);
                byte[] acTemp1 = QL.Core.Base64.Decode(kString);
                byte[] acTemp2 = tablegen2.GzipHelper.processGZipDecode(acTemp1);
                string kJson = System.Text.Encoding.UTF8.GetString(acTemp2);
                //JsonUtility.FromJson(kJson);
            });
        }
    }
}

