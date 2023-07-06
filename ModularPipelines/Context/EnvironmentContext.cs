using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModularPipelines.Exceptions;
using ModularPipelines.FileSystem;
using ModularPipelines.Models;
using ModularPipelines.Options;
using TomLonghurst.Microsoft.Extensions.DependencyInjection.ServiceInitialization;

namespace ModularPipelines.Context;

public class EnvironmentContext : IEnvironmentContext, IInitializer
{
    private readonly ILogger<EnvironmentContext> _logger;
    private readonly IHostEnvironment _hostEnvironment;
    private readonly ICommand _command;

    public EnvironmentContext(ILogger<EnvironmentContext> logger,
        IHostEnvironment hostEnvironment,
        IEnvironmentVariables environmentVariables,
        ICommand command)
    {
        _logger = logger;
        _hostEnvironment = hostEnvironment;
        _command = command;
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
        CommandResult gitCommandOutput;
        try
        {
            gitCommandOutput = await _command.ExecuteCommandLineTool(new CommandLineToolOptions("git")
            {
                Arguments = new[]{ "rev-parse", "--show-toplevel" }
            });
        }
        catch (CommandException e)
        {
            _logger.LogDebug("Error retrieving Git root directory: {Error}", e.CommandResult.StandardError);
            return;
        }

        GitRootDirectory = new Folder(new DirectoryInfo(gitCommandOutput.StandardOutput.Trim()));
    }
}
