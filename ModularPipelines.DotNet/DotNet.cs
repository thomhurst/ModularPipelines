using ModularPipelines.Models;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Exceptions;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Exceptions;
using ModularPipelines.Extensions;

namespace ModularPipelines.DotNet;

internal class DotNet : IDotNet
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
        var args = new List<string>();
        args.AddNonNullOrEmpty(options.TargetPath);
        return ExecuteCommandLineTool(options, args, cancellationToken);
    }

    public Task<CommandResult> Build(DotNetBuildOptions options, CancellationToken cancellationToken = default)
    {
        var args = new List<string>();
        args.AddNonNullOrEmpty(options.TargetPath);
        return ExecuteCommandLineTool(options, args, cancellationToken);
    }

    public Task<CommandResult> Publish(DotNetPublishOptions options, CancellationToken cancellationToken = default)
    {
        var args = new List<string>();
        args.AddNonNullOrEmpty(options.TargetPath);
        return ExecuteCommandLineTool(options, args, cancellationToken);
    }

    public Task<CommandResult> Pack(DotNetPackOptions options, CancellationToken cancellationToken = default)
    {
        var args = new List<string>();
        args.AddNonNullOrEmpty(options.TargetPath);
        return ExecuteCommandLineTool(options, args, cancellationToken);
    }

    public Task<CommandResult> Clean(DotNetCleanOptions options, CancellationToken cancellationToken = default)
    {
        var args = new List<string>();
        args.AddNonNullOrEmpty(options.TargetPath);
        return ExecuteCommandLineTool(options, args, cancellationToken);
    }

    public async Task<DotNetTestResult> Test(DotNetTestOptions options, CancellationToken cancellationToken = default)
    {
        var trxFilePath = Path.GetTempFileName();

        options.Logger ??= new List<string>();
        options.Logger.Add($"trx;logfilename={trxFilePath}");

        options.DoNotErrorOnNonZeroExitCode = true;

        var result = await _command.ExecuteCommandLineTool(options, cancellationToken);

        var trxContents = await _fileSystemContext.GetFile(trxFilePath).ReadAsync();

        var parsedTestResults = _trxParser.ParseTestResult(trxContents);

        if (!parsedTestResults.Successful || result.ExitCode != 0)
        {
            throw new DotNetTestFailedException(result, parsedTestResults);
        }

        return parsedTestResults;
    }

    public Task<CommandResult> Format(DotNetFormatOptions options, CancellationToken cancellationToken = default)
    {
        var args = new List<string>();
        args.AddNonNullOrEmpty(options.TargetPath);
        return ExecuteCommandLineTool(options, args, cancellationToken);
    }

    public Task<CommandResult> Version(DotNetOptions? options, CancellationToken cancellationToken = default)
    {
        options ??= new DotNetOptions();

        return ExecuteCommandLineTool(options, new[] { "--version" }, cancellationToken);
    }

    public Task<CommandResult> CustomCommand(DotNetCommandOptions options, CancellationToken cancellationToken = default)
    {
        return _command.ExecuteCommandLineTool(options.WithArguments(options.Command ?? ArraySegment<string>.Empty), cancellationToken);
    }

    private Task<CommandResult> ExecuteCommandLineTool(DotNetOptions options, IEnumerable<string> arguments, CancellationToken cancellationToken)
    {
        return _command.ExecuteCommandLineTool(options.WithArguments(arguments), cancellationToken);
    }
}
