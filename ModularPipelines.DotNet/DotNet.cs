using ModularPipelines.Models;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Extensions;
using ModularPipelines.Options;

namespace ModularPipelines.DotNet;

public class DotNet : IDotNet
{
    private readonly ICommand _command;
    private readonly IFileSystemContext _fileSystemContext;
    private readonly ITrxParser _trxParser;

    public DotNet(ITrxParser trxParser, ICommand command, IFileSystemContext fileSystemContext)
    {
        _trxParser = trxParser;
        _command = command;
        _fileSystemContext = fileSystemContext;
    }

    public Task<CommandResult> Restore(DotNetRestoreOptions options, CancellationToken cancellationToken = default)
    {
        var commandLineToolOptions = options.ToCommandLineToolOptions("dotnet", "restore") with
        {
            AdditionalSwitches = options.AdditionalSwitches
        };

        return _command.ExecuteCommandLineTool(commandLineToolOptions, cancellationToken);    }

    public Task<CommandResult> Build(DotNetBuildOptions options, CancellationToken cancellationToken = default)
    {
        var commandLineToolOptions = options.ToCommandLineToolOptions("dotnet", "build") with
        {
            AdditionalSwitches = options.AdditionalSwitches
        };

        return _command.ExecuteCommandLineTool(commandLineToolOptions, cancellationToken);    }

    public Task<CommandResult> Publish(DotNetPublishOptions options, CancellationToken cancellationToken = default)
    {
        var commandLineToolOptions = options.ToCommandLineToolOptions("dotnet", "publish") with
        {
            AdditionalSwitches = options.AdditionalSwitches
        };

        return _command.ExecuteCommandLineTool(commandLineToolOptions, cancellationToken);    }

    public Task<CommandResult> Pack(DotNetPackOptions options, CancellationToken cancellationToken = default)
    {
        var commandLineToolOptions = options.ToCommandLineToolOptions("dotnet", "pack") with
        {
            AdditionalSwitches = options.AdditionalSwitches
        };

        return _command.ExecuteCommandLineTool(commandLineToolOptions, cancellationToken);    }

    public Task<CommandResult> Clean(DotNetCleanOptions options, CancellationToken cancellationToken = default)
    {
        var commandLineToolOptions = options.ToCommandLineToolOptions("dotnet", "clean") with
        {
            AdditionalSwitches = options.AdditionalSwitches
        };

        return _command.ExecuteCommandLineTool(commandLineToolOptions, cancellationToken);
    }

    public async Task<DotNetTestResult> Test(DotNetTestOptions options, CancellationToken cancellationToken = default)
    {
        var trxFilePath = Path.GetTempFileName();

        options.Logger ??= new List<string>();
        options.Logger.Add($"trx;logfilename={trxFilePath}");

        var commandLineToolOptions = options.ToCommandLineToolOptions("dotnet", "test") with
        {
            AdditionalSwitches = options.AdditionalSwitches
        };

        await _command.ExecuteCommandLineTool(commandLineToolOptions, cancellationToken);

        var trxContents = await _fileSystemContext.GetFile(trxFilePath).ReadAsync();

        return _trxParser.ParseTestResult(trxContents);
    }

    public Task<CommandResult> Format(DotNetFormatOptions options, CancellationToken cancellationToken = default)
    {
        var commandLineToolOptions = options.ToCommandLineToolOptions("dotnet", "format") with
        {
            AdditionalSwitches = options.AdditionalSwitches
        };

        return _command.ExecuteCommandLineTool(commandLineToolOptions, cancellationToken);    }

    public Task<CommandResult> Version(CommandLineOptions? options, CancellationToken cancellationToken = default)
    {
        options ??= new CommandLineOptions();

        var commandLineToolOptions = options.ToCommandLineToolOptions("dotnet", "--version");

        return _command.ExecuteCommandLineTool(commandLineToolOptions, cancellationToken);
    }

    public Task<CommandResult> CustomCommand(DotNetCommandOptions options, CancellationToken cancellationToken = default)
    {
        var commandLineToolOptions = options.ToCommandLineToolOptions("dotnet", options.Command ?? ArraySegment<string>.Empty) with
        {
            AdditionalSwitches = options.AdditionalSwitches
        };

        return _command.ExecuteCommandLineTool(commandLineToolOptions, cancellationToken);
    }
}
