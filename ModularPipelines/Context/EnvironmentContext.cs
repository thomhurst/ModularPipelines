using CliWrap;
using CliWrap.Buffered;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModularPipelines.FileSystem;
using TomLonghurst.Microsoft.Extensions.DependencyInjection.ServiceInitialization;

namespace ModularPipelines.Context;

public class EnvironmentContext : IEnvironmentContext, IInitializer
{
    private readonly ILogger<EnvironmentContext> _logger;
    private readonly IHostEnvironment _hostEnvironment;

    public EnvironmentContext(ILogger<EnvironmentContext> logger, 
        IHostEnvironment hostEnvironment, 
        IEnvironmentVariables environmentVariables)
    {
        _logger = logger;
        _hostEnvironment = hostEnvironment;
        EnvironmentVariables = environmentVariables;
        ContentDirectory = _hostEnvironment.ContentRootPath!;
    }

    public string EnvironmentName => _hostEnvironment.EnvironmentName;
    public OperatingSystem OperatingSystem { get; } = Environment.OSVersion;
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

    public async Task InitializeAsync()
    {
        var gitCommandOutput = await Cli.Wrap("git")
            .WithArguments("rev-parse --show-toplevel")
            .WithValidation(CommandResultValidation.None)
            .ExecuteBufferedAsync();

        if (gitCommandOutput.ExitCode != 0)
        {
            _logger.LogDebug("Error retrieving Git root directory: {Error}", gitCommandOutput.StandardError);
            return;
        }
        
        GitRootDirectory = new Folder(new DirectoryInfo(gitCommandOutput.StandardOutput.Trim()));
    }
}