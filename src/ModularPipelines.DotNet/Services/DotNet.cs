using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Logging;
using ModularPipelines.Models;
using ModularPipelines.Options;

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

    public virtual async Task<CommandResult> New(DotNetNewOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token);
    }

    public virtual async Task<CommandResult> Restore(DotNetRestoreOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DotNetRestoreOptions(), executionOptions, token);
    }

    public virtual async Task<CommandResult> Build(DotNetBuildOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DotNetBuildOptions(), executionOptions, token);
    }

    public virtual async Task<CommandResult> Publish(DotNetPublishOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DotNetPublishOptions(), executionOptions, token);
    }

    public virtual async Task<CommandResult> Run(DotNetRunOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DotNetRunOptions(), executionOptions, token);
    }

    public virtual async Task<CommandResult> Test(DotNetTestOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DotNetTestOptions(), executionOptions, token);
    }

    public virtual async Task<CommandResult> Vstest(DotNetVstestOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token);
    }

    public virtual async Task<CommandResult> Pack(DotNetPackOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DotNetPackOptions(), executionOptions, token);
    }

    public virtual async Task<CommandResult> Migrate(DotNetMigrateOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DotNetMigrateOptions(), executionOptions, token);
    }

    public virtual async Task<CommandResult> Clean(DotNetCleanOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DotNetCleanOptions(), executionOptions, token);
    }

    public virtual async Task<CommandResult> Sln(DotNetSlnOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DotNetSlnOptions(), executionOptions, token);
    }

    public virtual async Task<CommandResult> Store(DotNetStoreOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token);
    }

    public virtual async Task<CommandResult> Watch(DotNetWatchOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token);
    }

    public virtual async Task<CommandResult> Format(DotNetFormatOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DotNetFormatOptions(), executionOptions, token);
    }

    public virtual async Task<CommandResult> Workload(DotNetWorkloadOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DotNetWorkloadOptions(), executionOptions, token);
    }

    public virtual async Task<CommandResult> Msbuild(DotNetMsbuildOptions options, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, executionOptions, token);
    }

    public virtual async Task<CommandResult> BuildServer(DotNetBuildServerOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DotNetBuildServerOptions(), executionOptions, token);
    }

    public virtual async Task<CommandResult> DevCerts(DotNetDevCertsOptions? options = default, CommandExecutionOptions? executionOptions = null, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DotNetDevCertsOptions(), executionOptions, token);
    }
}
