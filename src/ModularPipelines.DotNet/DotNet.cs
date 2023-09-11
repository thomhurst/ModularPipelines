using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using ModularPipelines.Models;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Exceptions;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Extensions;
using ModularPipelines.Helpers;
using ModularPipelines.Logging;

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
        var trxFilePath = Path.GetTempFileName();

        options.Logger ??= new List<string>();
        options.Logger.Add($"trx;logfilename={trxFilePath}");

        options.ThrowOnNonZeroExitCode = false;

        var result = await _command.ExecuteCommandLineTool(options, cancellationToken);

        await FileHelper.WaitForFileAsync(trxFilePath);
        
        var trxContents = await _fileSystemContext.GetFile(trxFilePath).ReadAsync();

        var parsedTestResults = _trxParser.ParseTestResult(trxContents);

        if (!parsedTestResults.Successful || result.ExitCode != 0)
        {
            _moduleLoggerProvider.GetLogger().LogInformation("Trx file contents: {Contents}", trxContents);
            throw new DotNetTestFailedException(result, parsedTestResults);
        }

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
