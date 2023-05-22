using System.Collections;
using CliWrap;
using CliWrap.Buffered;
using TomLonghurst.Microsoft.Extensions.DependencyInjection.ServiceInitialization;

namespace Pipeline.NET;

public class EnvironmentContext : IEnvironmentContext, IInitializer
{
    private readonly IDictionary<string, string> _environmentVariables;
    
    public EnvironmentContext()
    {
        _environmentVariables = ParseEnvironmentVariables();
    }

    private IDictionary<string,string> ParseEnvironmentVariables()
    {
        var variables = Environment.GetEnvironmentVariables();

        return variables.Cast<DictionaryEntry>()
            .ToDictionary(variable => variable.Key.ToString()!, variable => variable.Value!.ToString()!);
    }

    public OperatingSystem OperatingSystem { get; } = Environment.OSVersion;
    public DirectoryInfo WorkingDirectory { get; set; } = new(Environment.CurrentDirectory);
    public DirectoryInfo? GitRootDirectory { get; set; }
    
    public string? GetEnvironmentVariable(string name)
    {
        return Environment.GetEnvironmentVariable(name);
    }

    public IDictionary<string, string> GetEnvironmentVariables()
    {
        return _environmentVariables;
    }

    public async Task InitializeAsync()
    {
        var gitCommandOutput = await Cli.Wrap("git")
            .WithArguments("rev-parse --show-toplevel")
            .WithValidation(CommandResultValidation.None)
            .ExecuteBufferedAsync();

        if (gitCommandOutput.ExitCode != 0)
        {
            return;
        }
        
        GitRootDirectory = new DirectoryInfo(gitCommandOutput.StandardOutput.Trim());
    }
}

public interface IEnvironmentContext
{
    OperatingSystem OperatingSystem { get; }
    DirectoryInfo WorkingDirectory { get; set; }
    DirectoryInfo? GitRootDirectory { get; set; }

    string? GetEnvironmentVariable(string name);
    
    IDictionary<string, string> GetEnvironmentVariables();
}