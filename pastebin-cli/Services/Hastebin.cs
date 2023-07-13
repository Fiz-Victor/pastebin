using Newtonsoft.Json.Linq;

namespace pastebin_cli.Services;

public class Hastebin : Service.Service
{
    public Hastebin() : base("hastebin")
    {
    }

    protected override Response UploadFile0(string path)
    {
        return UploadText(File.ReadAllText(path));
    }

    protected override Response UploadText0(string text)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, "https://hst.sh/documents");
        request.Headers.Add("User-Agent", UserAgent);
        request.Content = new StringContent(text);
        var httpResponse = Client.Send(request);
        Response response;
        if (httpResponse.IsSuccessStatusCode)
        {
            var json = httpResponse.Content.ReadAsStringAsync().Result;
            var jObject = JObject.Parse(json);
            response = new Response(true, "https://hst.sh/" + (string)jObject["key"], "");
        }
        else
        {
            response = new Response(false, "", "");
        }

        return response;
    }
}