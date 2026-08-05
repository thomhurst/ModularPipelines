using Microsoft.Extensions.Hosting;
using ModularPipelines.Context.Domains.Environment;
using ModularPipelines.FileSystem;

namespace ModularPipelines.Context;

internal class EnvironmentContext : IEnvironmentContext
{
    private readonly IHostEnvironment _hostEnvironment;

    public EnvironmentContext(IHostEnvironment hostEnvironment,
        IEnvironmentVariablesContext environmentVariables,
        PipelineWorkingDirectory workingDirectory)
    {
        _hostEnvironment = hostEnvironment;
        EnvironmentVariables = environmentVariables;
        ContentDirectory = _hostEnvironment.ContentRootPath!;
        WorkingDirectory = new Folder(workingDirectory.Path);

        OperatingSystem = OperatingSystemHelper.GetOperatingSystem();
    }

    public string EnvironmentName => _hostEnvironment.EnvironmentName;

    public OperatingSystemIdentifier OperatingSystem { get; }

    public Version OperatingSystemVersion { get; } = Environment.OSVersion.Version;

    public bool Is64BitOperatingSystem { get; } = Environment.Is64BitOperatingSystem;

    public Folder AppDomainDirectory { get; } = AppDomain.CurrentDomain.BaseDirectory!;

    public Folder ContentDirectory { get; }

    public Folder WorkingDirectory { get; }

    public IEnvironmentVariablesContext EnvironmentVariables { get; }

    public Folder? GetFolder(Environment.SpecialFolder specialFolder)
    {
        return Environment.GetFolderPath(specialFolder);
    }
}
