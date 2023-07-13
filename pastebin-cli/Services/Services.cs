namespace pastebin_cli.Services;

public static class Services
{
    private static readonly List<Service.Service> AllServices = new(new Service.Service[]
    {
        new Mclogs(),
        new NullPointer(),
        new Hastebin()
    });

    public static List<string> GetAllServiceNames()
    {
        return AllServices.Select(service => service.Name).ToList();
    }

    public static Service.Service ServiceFromName(string name)
    {
        return AllServices.ToDictionary(service => service.Name, service => service)[name];
    }
}