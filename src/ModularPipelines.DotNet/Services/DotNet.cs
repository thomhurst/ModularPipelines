using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Enums;
using ModularPipelines.DotNet.Exceptions;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Helpers;
using ModularPipelines.Logging;
using ModularPipelines.Models;

namespace ModularPipelines.DotNet.Services;

[ExcludeFromCodeCoverage]
public class DotNet : IDotNet
{
    public DotNet(
        DotNetTool tool,
        DotNetWorkload dotNetWorkload,
        DotNetRemove remove,
        DotNetSdk sdk,
        DotNetAdd add,
        DotNetList list,
        DotNetNuget nuget,
        ICommand internalCommand,
        IModuleLoggerProvider moduleLoggerProvider,
        IFileSystemContext fileSystemContext,
        ITrxParser trxParser
    )
    {
        Tool = tool;
        DotNetWorkload = dotNetWorkload;
        Remove = remove;
        Sdk = sdk;
        Add = add;
        List = list;
        Nuget = nuget;
        _command = internalCommand;
        _moduleLoggerProvider = moduleLoggerProvider;
        _fileSystemContext = fileSystemContext;
        _trxParser = trxParser;
    }

    private readonly ICommand _command;
    private readonly IModuleLoggerProvider _moduleLoggerProvider;
    private readonly IFileSystemContext _fileSystemContext;
    private readonly ITrxParser _trxParser;

    public DotNetTool Tool { get; }

    public DotNetWorkload DotNetWorkload { get; }

    public DotNetRemove Remove { get; }

    public DotNetSdk Sdk { get; }

    public DotNetAdd Add { get; }

    public DotNetList List { get; }

    public DotNetNuget Nuget { get; }

    public async Task<CommandResult> New(DotNetNewOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Restore(DotNetRestoreOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DotNetRestoreOptions(), token);
    }

    public async Task<CommandResult> Build(DotNetBuildOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DotNetBuildOptions(), token);
    }

    public async Task<CommandResult> Publish(DotNetPublishOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DotNetPublishOptions(), token);
    }

    public async Task<CommandResult> Run(DotNetRunOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DotNetRunOptions(), token);
    }

    public async Task<DotNetTestResult> Test(DotNetTestOptions? options = default, CancellationToken token = default)
    {
        var trxFilePath = FileSystem.File.GetNewTemporaryFilePath();

        options ??= new DotNetTestOptions();
        options.Logger = [$"trx;logfilename={trxFilePath}"];

        options.ThrowOnNonZeroExitCode = false;

        var result = await _command.ExecuteCommandLineTool(options, token);

        await FileHelper.WaitForFileAsync(trxFilePath);

        var logger = _moduleLoggerProvider.GetLogger();

        if (!File.Exists(trxFilePath))
        {
            logger.LogDebug("No trx file was found at {Path}", trxFilePath);
            throw new DotNetTestFailedException(result, null);
        }

        var trxContents = await _fileSystemContext.GetFile(trxFilePath).ReadAsync(token);

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

    public async Task<CommandResult> Vstest(DotNetVstestOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Pack(DotNetPackOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DotNetPackOptions(), token);
    }

    public async Task<CommandResult> Migrate(DotNetMigrateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DotNetMigrateOptions(), token);
    }

    public async Task<CommandResult> Clean(DotNetCleanOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DotNetCleanOptions(), token);
    }

    public async Task<CommandResult> Sln(DotNetSlnOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DotNetSlnOptions(), token);
    }

    public async Task<CommandResult> Store(DotNetStoreOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DotNetStoreOptions(), token);
    }

    public async Task<CommandResult> Watch(DotNetWatchOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Format(DotNetFormatOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DotNetFormatOptions(), token);
    }

    public async Task<CommandResult> Workload(DotNetWorkloadOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DotNetWorkloadOptions(), token);
    }

    public async Task<CommandResult> Msbuild(DotNetMsbuildOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BuildServer(DotNetBuildServerOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DotNetBuildServerOptions(), token);
    }

    public async Task<CommandResult> DevCerts(DotNetDevCertsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DotNetDevCertsOptions(), token);
    }
}
