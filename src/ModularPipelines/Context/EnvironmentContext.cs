using System.Runtime.InteropServices;
using Microsoft.Extensions.Hosting;
using ModularPipelines.FileSystem;
using ModularPipelines.Logging;

namespace ModularPipelines.Context;

internal class EnvironmentContext : IEnvironmentContext
{
    private readonly IModuleLoggerProvider _moduleLoggerProvider;
    private readonly IHostEnvironment _hostEnvironment;
    private readonly ICommand _command;

    public EnvironmentContext(IModuleLoggerProvider moduleLoggerProvider,
        IHostEnvironment hostEnvironment,
        IEnvironmentVariables environmentVariables,
        ICommand command)
    {
        _moduleLoggerProvider = moduleLoggerProvider;
        _hostEnvironment = hostEnvironment;
        _command = command;
        EnvironmentVariables = environmentVariables;
        ContentDirectory = _hostEnvironment.ContentRootPath!;

        OperatingSystem = ModularPipelines.OperatingSystem.GetOperatingSystem();
    }

    public string EnvironmentName => _hostEnvironment.EnvironmentName;

    public OSPlatform OperatingSystem { get; }

    public Version OperatingSystemVersion { get; } = Environment.OSVersion.Version;

    public bool Is64BitOperatingSystem { get; } = Environment.Is64BitOperatingSystem;

    public Folder AppDomainDirectory { get; } = AppDomain.CurrentDomain.BaseDirectory!;

    public Folder ContentDirectory { get; set; }

    public Folder WorkingDirectory { get; set; } = Environment.CurrentDirectory!;
    public Folder? GitRootDirectory { get; set; }

    public Folder? GetFolder(Environment.SpecialFolder specialFolder)
    {
        return Environment.GetFolderPath(specialFolder);
    }

    public IEnvironmentVariables EnvironmentVariables { get; }
}
