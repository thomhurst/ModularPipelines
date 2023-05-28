using System.Collections.ObjectModel;
using CliWrap;
using CliWrap.Buffered;
using Pipeline.NET.Context;
using Pipeline.NET.DotNet.Options;
using Pipeline.NET.Models;
using Pipeline.NET.Modules;

namespace Pipeline.NET.DotNet.Modules;

public abstract class DotNetCommandModule : Module<BufferedCommandResult>
{
    public DotNetCommandModule(IModuleContext context) : base(context)
    {
    }

    protected abstract DotNetCommandModuleOptions Options { get; }

    protected override async Task<ModuleResult<BufferedCommandResult>?> ExecuteAsync(CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(Options.Command);
        
        var command = Cli.Wrap("dotnet");
        
        if (!string.IsNullOrEmpty(Options.WorkingDirectory))
        {
            command = command.WithWorkingDirectory(Options.WorkingDirectory);
        }

        var arguments = new List<string>
        {
            Options.Command,
            $"-c {Options.Configuration.ToString()}"
        };
        
        arguments.AddRange(Options.ExtraArguments ?? Array.Empty<string>());

        command = command.WithArguments(arguments);

        if (Options.EnvironmentVariables != null)
        {
            command = command.WithEnvironmentVariables(new ReadOnlyDictionary<string, string?>(Options.EnvironmentVariables));
        }

        var result = await command.ExecuteBufferedAsync(cancellationToken: cancellationToken);

        return result;
    }
}