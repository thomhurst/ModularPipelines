using System.Collections;
using CliWrap;
using CliWrap.Buffered;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TomLonghurst.Microsoft.Extensions.DependencyInjection.ServiceInitialization;

namespace ModularPipelines.Context;

public class EnvironmentContext : IEnvironmentContext, IInitializer
{
    private readonly ILogger<EnvironmentContext> _logger;
    private readonly IHostEnvironment _hostEnvironment;

    public EnvironmentContext(ILogger<EnvironmentContext> logger, IHostEnvironment hostEnvironment)
    {
        _logger = logger;
        _hostEnvironment = hostEnvironment;
        ContentDirectory = new(_hostEnvironment.ContentRootPath);
    }

    public string EnvironmentName => _hostEnvironment.EnvironmentName;
    public OperatingSystem OperatingSystem { get; } = Environment.OSVersion;
    public bool Is64BitOperatingSystem { get; } = Environment.Is64BitOperatingSystem;
    public DirectoryInfo ContentDirectory { get; set; }
    public DirectoryInfo WorkingDirectory { get; set; } = new(Environment.CurrentDirectory);
    public DirectoryInfo? GitRootDirectory { get; set; }
    
    public string? GetEnvironmentVariable(string name)
    {
        return Environment.GetEnvironmentVariable(name);
    }

    public IDictionary<string, string> GetEnvironmentVariables()
    {
        return Environment.GetEnvironmentVariables()
            .Cast<DictionaryEntry>()
            .ToDictionary(variable => variable.Key.ToString()!, variable => variable.Value!.ToString()!);
    }

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
        
        GitRootDirectory = new DirectoryInfo(gitCommandOutput.StandardOutput.Trim());
    }
}