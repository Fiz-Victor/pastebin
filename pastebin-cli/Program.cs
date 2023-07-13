namespace pastebin_cli;

public static class Program
{
    public static void Main()
    {
        Console.WriteLine("---Pastebin by Fiz_Victor---");
        Console.WriteLine("Available psastebin services: (* means support file upload, without * you can still upload text files)");
        Services.Services.GetAllServiceNames().ForEach(Console.WriteLine);
        var service = Services.Services.ServiceFromName("hastebin");
        var response = service.UploadText("test");
        Console.WriteLine(response.Success);
        Console.WriteLine(response.Data);
        Console.WriteLine(response.Extra);
    }
}