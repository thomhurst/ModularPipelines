using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzDiskPool
{
    public AzDiskPool(
        AzDiskPoolIscsiTarget iscsiTarget,
        ICommand internalCommand
    )
    {
        IscsiTarget = iscsiTarget;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzDiskPoolIscsiTarget IscsiTarget { get; }

    public async Task<CommandResult> Create(AzDiskPoolCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzDiskPoolDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDiskPoolDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzDiskPoolListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDiskPoolListOptions(), token);
    }

    public async Task<CommandResult> ListOutboundNetworkDependencyEndpoint(AzDiskPoolListOutboundNetworkDependencyEndpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListSkus(AzDiskPoolListSkusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListZones(AzDiskPoolListZonesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Redeploy(AzDiskPoolRedeployOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDiskPoolRedeployOptions(), token);
    }

    public async Task<CommandResult> Show(AzDiskPoolShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDiskPoolShowOptions(), token);
    }

    public async Task<CommandResult> Start(AzDiskPoolStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDiskPoolStartOptions(), token);
    }

    public async Task<CommandResult> Stop(AzDiskPoolStopOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDiskPoolStopOptions(), token);
    }

    public async Task<CommandResult> Update(AzDiskPoolUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDiskPoolUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzDiskPoolWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDiskPoolWaitOptions(), token);
    }
}