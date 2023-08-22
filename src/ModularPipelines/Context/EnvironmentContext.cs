using System.Runtime.InteropServices;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModularPipelines.Exceptions;
using ModularPipelines.FileSystem;
using ModularPipelines.Helpers;
using ModularPipelines.Models;
using ModularPipelines.Options;
using TomLonghurst.Microsoft.Extensions.DependencyInjection.ServiceInitialization;

namespace ModularPipelines.Context;

internal class EnvironmentContext : IEnvironmentContext, IInitializer
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

        OperatingSystem = GetOperatingSystem();
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

    public async Task InitializeAsync()
    {
        CommandResult gitCommandOutput;
        try
        {
            gitCommandOutput = await _command.ExecuteCommandLineTool(new CommandLineToolOptions("git")
            {
                Arguments = new[] { "rev-parse", "--show-toplevel" },
                LogInput = false,
                LogOutput = false
            });
        }
        catch (CommandException e)
        {
            _moduleLoggerProvider.GetLogger().LogDebug("Error retrieving Git root directory: {Error}", e.CommandResult.StandardError);
            return;
        }

        GitRootDirectory = new Folder(new DirectoryInfo(gitCommandOutput.StandardOutput.Trim()));
    }

    private OSPlatform GetOperatingSystem()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return OSPlatform.Linux;
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return OSPlatform.Windows;
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return OSPlatform.OSX;
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD))
        {
            return OSPlatform.FreeBSD;
        }

        return OSPlatform.Create("Unknown");
    }
}
