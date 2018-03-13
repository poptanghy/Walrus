using CI.HttpClient;
using UnityEngine;
using UnityEngine.UI;

public class ExampleSceneManagerController : MonoBehaviour
{
    public Text LeftText;
    public Text RightText;
    public Slider ProgressSlider;

    public void Upload()
    {
        HttpClient client = new HttpClient();

        byte[] buffer = new byte[1000000];
        new System.Random().NextBytes(buffer);

        ByteArrayContent content = new ByteArrayContent(buffer, "application/bytes");

        ProgressSlider.value = 0;

        client.Post(new System.Uri("http://httpbin.org/post"), content, HttpCompletionOption.AllResponseContent, (r) =>
        {           
        }, (u) =>
        {
            LeftText.text = "Upload: " +  u.PercentageComplete.ToString() + "%";
            ProgressSlider.value = u.PercentageComplete;
        });
    }

    public void Download()
    {
        HttpClient client = new HttpClient();

        ProgressSlider.value = 100;

        client.GetByteArray(new System.Uri("http://download.thinkbroadband.com/5MB.zip"), HttpCompletionOption.StreamResponseContent, (r) =>
        {
            RightText.text = "Download: " + r.PercentageComplete.ToString() + "%";
            ProgressSlider.value = 100 - r.PercentageComplete;
        });
    }

    public void UploadDownload()
    {
        HttpClient client = new HttpClient();

        byte[] buffer = new byte[1000000];
        new System.Random().NextBytes(buffer);

        ByteArrayContent content = new ByteArrayContent(buffer, "application/bytes");

        ProgressSlider.value = 0;

        client.Post(new System.Uri("http://httpbin.org/post"), content, HttpCompletionOption.StreamResponseContent, (r) =>
        {
            RightText.text = "Download: " + r.PercentageComplete.ToString() + "%";
            ProgressSlider.value = 100 - r.PercentageComplete;
        }, (u) =>
        {
            LeftText.text = "Upload: " + u.PercentageComplete.ToString() + "%";
            ProgressSlider.value = u.PercentageComplete;
        });
    }

    public void Delete()
    {
        HttpClient client = new HttpClient();
        client.Delete(new System.Uri("http://httpbin.org/delete"), HttpCompletionOption.AllResponseContent, (r) =>
        {
        });
    }

    public void Get()
    {
        HttpClient client = new HttpClient();
        client.GetByteArray(new System.Uri("http://httpbin.org/get"), HttpCompletionOption.AllResponseContent, (r) =>
        {
        });
    }

    public void Test()
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


    
public void Patch()
    {
        HttpClient client = new HttpClient();

        StringContent content = new StringContent("Hello World");

        client.Patch(new System.Uri("http://httpbin.org/patch"), content, HttpCompletionOption.AllResponseContent, (r) =>
        {
        });
    }

    public void Post()
    {
        HttpClient client = new HttpClient();

        StringContent content = new StringContent("Hello World");

        client.Post(new System.Uri("http://httpbin.org/post"), content, HttpCompletionOption.AllResponseContent, (r) =>
        {
        });
    }

    public void Put()
    {
        HttpClient client = new HttpClient();

        StringContent content = new StringContent("Hello World");

        client.Put(new System.Uri("http://httpbin.org/put"), content, HttpCompletionOption.AllResponseContent, (r) =>
        {
        });
    }
}