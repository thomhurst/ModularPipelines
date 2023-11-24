using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Enums;
using ModularPipelines.DotNet.Exceptions;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Extensions;
using ModularPipelines.Helpers;
using ModularPipelines.Logging;
using ModularPipelines.Models;

namespace ModularPipelines.DotNet;

[ExcludeFromCodeCoverage]
internal class DotNet : IDotNet
{
    private readonly ICommand _command;
    private readonly IFileSystemContext _fileSystemContext;
    private readonly IModuleLoggerProvider _moduleLoggerProvider;
    private readonly ITrxParser _trxParser;

    public DotNet(ITrxParser trxParser,
        ICommand command,
        IFileSystemContext fileSystemContext,
        IModuleLoggerProvider moduleLoggerProvider)
    {
        _trxParser = trxParser;
        _command = command;
        _fileSystemContext = fileSystemContext;
        _moduleLoggerProvider = moduleLoggerProvider;
    }

    public async Task<CommandResult> Restore(DotNetRestoreOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken);
    }

    public async Task<CommandResult> Build(DotNetBuildOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken);
    }

    public async Task<CommandResult> Publish(DotNetPublishOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken);
    }

    public async Task<CommandResult> Pack(DotNetPackOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken);
    }

    public async Task<CommandResult> Clean(DotNetCleanOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken);
    }

    public async Task<DotNetTestResult> Test(DotNetTestOptions options, CancellationToken cancellationToken = default)
    {
        var trxFilePath = FileSystem.File.GetNewTemporaryFilePath();

        options.Logger ??= new List<string>();
        options.Logger.Add($"trx;logfilename={trxFilePath}");

        options.ThrowOnNonZeroExitCode = false;

        var result = await _command.ExecuteCommandLineTool(options, cancellationToken);

        await FileHelper.WaitForFileAsync(trxFilePath);

        var logger = _moduleLoggerProvider.GetLogger();

        if (!File.Exists(trxFilePath))
        {
            logger.LogDebug("No trx file was found at {Path}", trxFilePath);
            throw new DotNetTestFailedException(result, null);
        }

        var trxContents = await _fileSystemContext.GetFile(trxFilePath).ReadAsync(cancellationToken);

        logger.LogDebug("Trx file contents: {Contents}", trxContents);

        var parsedTestResults = _trxParser.ParseTestResult(trxContents);

        if (!parsedTestResults.Successful || result.ExitCode != 0)
        {
            throw new DotNetTestFailedException(result, parsedTestResults);
        }

        var success = parsedTestResults.UnitTestResults.Count(x => x.Outcome == TestOutcome.Passed);
        var failed = parsedTestResults.UnitTestResults.Count(x => x.Outcome == TestOutcome.Failed);
        var skipped = parsedTestResults.UnitTestResults.Count(x => x.Outcome == TestOutcome.NotExecuted);

        logger.LogInformation("DotNet Test Results - Success: {Success} - Failed: {Failed} - Skipped: {Skipped}", success, failed, skipped);

        return parsedTestResults;
    }

    public async Task<CommandResult> Format(DotNetFormatOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken);
    }

    public async Task<CommandResult> Version(DotNetOptions? options, CancellationToken cancellationToken = default)
    {
        options ??= new DotNetOptions();

        return await _command.ExecuteCommandLineTool(options.WithArguments("--version"), cancellationToken);
    }

    public Task<CommandResult> CustomCommand(DotNetCommandOptions options, CancellationToken cancellationToken = default)
    {
        return _command.ExecuteCommandLineTool(options.WithArguments(options.Command ?? ArraySegment<string>.Empty), cancellationToken);
    }
}
