using pastebin_cli.Service;

namespace pastebin_cli.Services;

public class NullPointer : Service.Service
{
    public NullPointer() : base("0x0", typeof(Config), new Config(false))
    {
    }

    protected override Response UploadFile0(string path)
    {
        return GetResponse(new StreamContent(File.OpenRead(path)), Path.GetFileName(path));
    }

    protected override Response UploadText0(string text)
    {
        return GetResponse(new StringContent(text), "unnamed.txt");
    }

    private Response GetResponse(HttpContent httpContent, string fileName)
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, "https://0x0.st");
        var formData = new MultipartFormDataContent();
        formData.Add(httpContent, "file", fileName);
        if (((Config)Cfg).Secret) formData.Add(new StringContent(""), "secret");
        request.Headers.Add("User-Agent", UserAgent);
        request.Content = formData;
        var httpResponse = client.Send(request);
        var content = httpResponse.Content.ReadAsStringAsync().Result;
        Response response;
        if (httpResponse.IsSuccessStatusCode)
            response = new Response(true, content,
                httpResponse.Headers.TryGetValues("X-Token", out var token)
                    ? "X-Token is " + token.FirstOrDefault()
                    : "X-Token Unknown");
        else
            response = new Response(false, content, "");
        return response;
    }

    public record Config(bool Secret) : IConfig;
}