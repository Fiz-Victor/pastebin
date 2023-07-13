namespace pastebin_cli.Service;

public abstract class Service
{
    protected const string UserAgent =
        "Mozilla/5.0 (Windows NT 10.0; WOW64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/113.0.5666.197 Safari/537.36";

    private readonly Type? _configType;

    protected readonly HttpClient Client = new();
    public readonly string Name;

    private IConfig? _config;

    /// <summary>Initialize the instance.</summary>
    /// <param name="name">The service name shown in help.</param>
    /// <param name="configType">The config type check for SetConfig, leave empty if service doesn't require config.</param>
    /// <param name="defaultConfig">The default config, leave empty for manual config setting.</param>
    protected Service(string name, Type? configType = null, IConfig? defaultConfig = null)
    {
        Name = name;
        _configType = configType;
        if (configType != null) Cfg = defaultConfig;
    }

    public IConfig? Cfg
    {
        protected get => _config;
        set
        {
            if (value?.GetType() == _configType) _config = value;
            else throw new NotSupportedException("Config type check failed.");
        }
    }

    /// <summary>Upload the file and get the response.</summary>
    /// <param name="path">File path.</param>
    /// <returns>Server response.</returns>
    public Response UploadFile(string path)
    {
        if (Cfg == null && _configType != null)
            throw new NotSupportedException("Attempt to upload when service is unconfigured.");
        return UploadFile0(path);
    }

    protected abstract Response UploadFile0(string path);

    /// <summary>Upload the text and get the response.</summary>
    /// <param name="text">Uploaded text.</param>
    /// <returns>Server response.</returns>
    public Response UploadText(string text)
    {
        if (Cfg == null && _configType != null)
            throw new NotSupportedException("Attempt to upload when service is unconfigured.");
        return UploadText0(text);
    }

    protected abstract Response UploadText0(string text);

    public record Response(bool Success, string Data, string Extra);
}