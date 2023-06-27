using CliWrap.Buffered;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Extensions;
using ModularPipelines.Options;
using File = ModularPipelines.FileSystem.File;

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

    public async Task<BufferedCommandResult> Test(DotNetOptions options, CancellationToken cancellationToken = default)
    {
        var trxFilePath = Path.GetTempFileName();
        var argumentsWithLogger = options.ExtraArguments?.ToList() ?? new List<string>();
        argumentsWithLogger.Add("--logger");
        argumentsWithLogger.Add($"trx;logfilename={trxFilePath}");
        
        var optionsWithLogger = options with
        {
            ExtraArguments = argumentsWithLogger
        };
        
        var command = await RunCommand(ToDotNetCommandOptions("test", optionsWithLogger), cancellationToken);

        var trxContents = await _context.FileSystem.GetFile(trxFilePath).ReadAsync();

        var parsedTrx = _trxParser.ParseTestResult(trxContents);
        
        return command;
    }

    public Task<BufferedCommandResult> Version(CommandEnvironmentOptions? options, CancellationToken cancellationToken = default)
    {
        options ??= new CommandEnvironmentOptions();
        
        return RunCommand(new DotNetCommandOptions
        {
            Command = new[] { "--version" },
            EnvironmentVariables = options.EnvironmentVariables,
            WorkingDirectory = options.WorkingDirectory,
            Credentials = options.Credentials,
            LogInput = options.LogInput,
            LogOutput = options.LogOutput
        }, cancellationToken);
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
            WorkingDirectory = options.WorkingDirectory,
            Credentials = options.Credentials,
            LogInput = options.LogInput,
            LogOutput = options.LogOutput
        };
    }

    private Task<BufferedCommandResult> RunCommand(DotNetCommandOptions options, CancellationToken cancellationToken)
    {
        var arguments = options.Command?.ToList() ?? new List<string>();

        arguments.AddNonNullOrEmpty(options.TargetPath);
        arguments.AddRangeNonNullOrEmpty(options.ExtraArguments);
        arguments.AddNonNullOrEmptyArgumentWithSwitch("-c", options.Configuration?.ToString());

        return _context.Command.UsingCommandLineTool(new CommandLineToolOptions("dotnet")
        {
            Arguments = arguments,
            EnvironmentVariables = options.EnvironmentVariables,
            WorkingDirectory = options.WorkingDirectory,
            Credentials = options.Credentials,
            LogInput = options.LogInput,
            LogOutput = options.LogOutput
        }, cancellationToken);
    }
}