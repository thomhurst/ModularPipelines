using CliWrap.Buffered;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Command.Extensions;
using ModularPipelines.Command.Options;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Options;

namespace ModularPipelines.DotNet;

public class DotNet : IDotNet
{
    public IModuleContext Context { get; }

    [ActivatorUtilitiesConstructor]
    internal DotNet(IModuleContext context)
    {
        Context = context;
    }
    
    public Task<BufferedCommandResult> Restore(DotNetOptions options, CancellationToken cancellationToken = default)
    {
        return RunCommand(ToDotNetCommandOptions("restore", options), cancellationToken);
    }

    public Task<BufferedCommandResult> Build(DotNetOptions options, CancellationToken cancellationToken = default)
    {
        return RunCommand(ToDotNetCommandOptions("build", options), cancellationToken);
    }

    public Task<BufferedCommandResult> Publish(DotNetOptions options, CancellationToken cancellationToken = default)
    {
        return RunCommand(ToDotNetCommandOptions("publish", options), cancellationToken);
    }

    public Task<BufferedCommandResult> Pack(DotNetOptions options, CancellationToken cancellationToken = default)
    {
        return RunCommand(ToDotNetCommandOptions("pack", options), cancellationToken);
    }

    public Task<BufferedCommandResult> Clean(DotNetOptions options, CancellationToken cancellationToken = default)
    {
        return RunCommand(ToDotNetCommandOptions("clean", options), cancellationToken);
    }

    public Task<BufferedCommandResult> Test(DotNetOptions options, CancellationToken cancellationToken = default)
    {
        return RunCommand(ToDotNetCommandOptions("test", options), cancellationToken);
    }
    
    public Task<BufferedCommandResult> CustomCommand(DotNetCommandOptions options, CancellationToken cancellationToken = default)
    {
        return RunCommand(options, cancellationToken);
    }

    private static DotNetCommandOptions ToDotNetCommandOptions(string command, DotNetOptions options)
    {
        return new DotNetCommandOptions
        {
            Command = new []{ command },
            EnvironmentVariables = options.EnvironmentVariables,
            ExtraArguments = options.ExtraArguments,
            TargetPath = options.TargetPath,
            Configuration = options.Configuration,
            WorkingDirectory = options.WorkingDirectory
        };
    }

    private Task<BufferedCommandResult> RunCommand(DotNetCommandOptions options,
        CancellationToken cancellationToken = default)
    {
        var arguments = options.Command?.ToList() ?? new List<string>();

        if (options.TargetPath != null)
        {
            arguments.Add(options.TargetPath);
        }

        if (options.ExtraArguments != null)
        {
            arguments.AddRange(options.ExtraArguments);
        }

        if (options.Configuration != null)
        {
            arguments.Add("-c");
            arguments.Add(options.Configuration.ToString()!);
        }
        
        return Context.Command().UsingCommandLineTool(new CommandLineToolOptions("dotnet")
        {
            Arguments = arguments,
            EnvironmentVariables = options.EnvironmentVariables,
            WorkingDirectory = options.WorkingDirectory
        }, cancellationToken);
    }
}