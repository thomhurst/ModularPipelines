using System.Collections.ObjectModel;
using CliWrap;
using CliWrap.Buffered;
using Pipeline.NET.DotNet.Options;
using Pipeline.NET.Exceptions;
using Pipeline.NET.Extensions;

namespace Pipeline.NET.DotNet.Modules;

public abstract class DotNetCommandModule : Module
{
    public DotNetCommandModule(IModuleContext context) : base(context)
    {
    }
    
    public abstract DotNetCommandModuleOptions Options { get; }

    public override async Task<ModuleResult?> ExecuteAsync(CancellationToken cancellationToken)
    {
        var command = Cli.Wrap("dotnet");

        if (!string.IsNullOrEmpty(Options.WorkingDirectory))
        {
            command = command.WithWorkingDirectory(Options.WorkingDirectory);
        }

        var arguments = new List<string>
        {
            Options.Command
        };
        
        arguments.AddRange(Options.ExtraArguments ?? Array.Empty<string>());

        command = command.WithArguments(arguments);

        if (Options.EnvironmentVariables != null)
        {
            command = command.WithEnvironmentVariables(new ReadOnlyDictionary<string, string?>(Options.EnvironmentVariables));
        }

        var result = await command.ExecuteBufferedAsync(cancellationToken: cancellationToken);

        if (result.ExitCode != 0)
        {
            throw new ModuleFailedException(result.StandardError);
        }
        
        return result.StandardOutput.ToModuleResult();
    }
}