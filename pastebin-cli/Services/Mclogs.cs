using Newtonsoft.Json.Linq;

namespace pastebin_cli.Services;

public class Mclogs : Service.Service
{
    public Mclogs() : base("mclogs")
    {
    }

    protected override Response UploadFile0(string path)
    {
        return UploadText(File.ReadAllText(path));
    }

    protected override Response UploadText0(string text)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "https://api.mclo.gs/1/log");
        request.Headers.Add("User-Agent", UserAgent);
        request.Content = new FormUrlEncodedContent(new Dictionary<string, string> { { "contendt", text } });
        var httpResponse = Client.Send(request);
        var json = httpResponse.Content.ReadAsStringAsync().Result;
        var jObject = JObject.Parse(json);
        Response response;
        if ((bool)jObject["success"])
            response = new Response(true, (string)jObject["url"], json);
        else
            response = new Response(false, (string)jObject["error"], json);
        return response;
    }
}