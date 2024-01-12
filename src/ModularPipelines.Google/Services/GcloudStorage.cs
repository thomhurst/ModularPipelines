using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Google.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Services;

[ExcludeFromCodeCoverage]
public class GcloudStorage
{
    public GcloudStorage(
        GcloudStorageBuckets buckets,
        GcloudStorageHmac hmac,
        GcloudStorageInsights insights,
        GcloudStorageObjects objects,
        GcloudStorageOperations operations,
        ICommand internalCommand
    )
    {
        Buckets = buckets;
        Hmac = hmac;
        Insights = insights;
        Objects = objects;
        Operations = operations;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public GcloudStorageBuckets Buckets { get; }

    public GcloudStorageHmac Hmac { get; }

    public GcloudStorageInsights Insights { get; }

    public GcloudStorageObjects Objects { get; }

    public GcloudStorageOperations Operations { get; }

    public async Task<CommandResult> Cat(GcloudStorageCatOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Cp(GcloudStorageCpOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Du(GcloudStorageDuOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Hash(GcloudStorageHashOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Ls(GcloudStorageLsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Mv(GcloudStorageMvOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Restore(GcloudStorageRestoreOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Rm(GcloudStorageRmOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Rsync(GcloudStorageRsyncOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ServiceAgent(GcloudStorageServiceAgentOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new GcloudStorageServiceAgentOptions(), token);
    }

    public async Task<CommandResult> SignUrl(GcloudStorageSignUrlOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}