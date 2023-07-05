using ModularPipelines.Models;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Extensions;
using ModularPipelines.Options;

namespace ModularPipelines.DotNet;

public class DotNet : IDotNet
{
    private readonly IModuleContext _context;
    private readonly ITrxParser _trxParser;

    public DotNet(IModuleContext context, ITrxParser trxParser)
    {
        _context = context;
        _trxParser = trxParser;
    }
    
    public Task<CommandResult> Restore(DotNetRestoreOptions options, CancellationToken cancellationToken = default)
    {
        return RunCommand(ToDotNetCommandOptions("restore", options), options, cancellationToken);
    }

    public Task<CommandResult> Build(DotNetBuildOptions options, CancellationToken cancellationToken = default)
    {
        return RunCommand(ToDotNetCommandOptions("build", options), options, cancellationToken);
    }

    public Task<CommandResult> Publish(DotNetPublishOptions options, CancellationToken cancellationToken = default)
    {
        return RunCommand(ToDotNetCommandOptions("publish", options), options, cancellationToken);
    }

    public Task<CommandResult> Pack(DotNetPackOptions options, CancellationToken cancellationToken = default)
    {
        return RunCommand(ToDotNetCommandOptions("pack", options), options, cancellationToken);
    }

    public Task<CommandResult> Clean(DotNetCleanOptions options, CancellationToken cancellationToken = default)
    {
        return RunCommand(ToDotNetCommandOptions("clean", options), options, cancellationToken);
    }

    public async Task<DotNetTestResult> Test(DotNetTestOptions options, CancellationToken cancellationToken = default)
    {
        var trxFilePath = Path.GetTempFileName();

        options.Logger ??= new List<string>();
        options.Logger.Add($"trx;logfilename={trxFilePath}");
        
        var command = await RunCommand(ToDotNetCommandOptions("test", options), options, cancellationToken);

        var trxContents = await _context.FileSystem.GetFile(trxFilePath).ReadAsync();

        return _trxParser.ParseTestResult(trxContents);
    }

    public Task<CommandResult> Version(CommandLineOptions? options, CancellationToken cancellationToken = default)
    {
        options ??= new CommandLineOptions();
        
        return RunCommand(new DotNetCommandOptions
        {
            Command = new[] { "--version" },
            EnvironmentVariables = options.EnvironmentVariables,
            WorkingDirectory = options.WorkingDirectory,
            Credentials = options.Credentials,
            LogInput = options.LogInput,
            LogOutput = options.LogOutput
        }, null, cancellationToken);
    }

    public Task<CommandResult> CustomCommand(DotNetCommandOptions options, CancellationToken cancellationToken = default)
    {
        return RunCommand(options, null, cancellationToken);
    }

    private static DotNetCommandOptions ToDotNetCommandOptions(string command, DotNetOptions options)
    {
        return new DotNetCommandOptions
        {
            Command = new []{ command },
            EnvironmentVariables = options.EnvironmentVariables,
            AdditionalSwitches = options.AdditionalSwitches,
            TargetPath = options.TargetPath,
            WorkingDirectory = options.WorkingDirectory,
            Credentials = options.Credentials,
            LogInput = options.LogInput,
            LogOutput = options.LogOutput
        };
    }

    private Task<CommandResult> RunCommand(DotNetCommandOptions options, object? optionsObject, CancellationToken cancellationToken)
    {
        var arguments = options.Command?.ToList() ?? new List<string>();

        arguments.AddNonNullOrEmpty(options.TargetPath);

        return _context.Command.ExecuteCommandLineTool(new CommandLineToolOptions("dotnet")
        {
            Arguments = arguments,
            EnvironmentVariables = options.EnvironmentVariables,
            WorkingDirectory = options.WorkingDirectory,
            Credentials = options.Credentials,
            LogInput = options.LogInput,
            LogOutput = options.LogOutput,
            AdditionalSwitches = options.AdditionalSwitches,
            ArgumentsOptionObject = optionsObject
        }, cancellationToken);
    }
}