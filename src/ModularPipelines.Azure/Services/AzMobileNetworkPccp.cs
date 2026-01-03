using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("mobile-network")]
public class AzMobileNetworkPccp
{
    public AzMobileNetworkPccp(
        AzMobileNetworkPccpVersion version,
        ICommand internalCommand
    )
    {
        Version = version;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzMobileNetworkPccpVersion Version { get; }

    public async Task<CommandResult> CollectDiagnosticsPackage(AzMobileNetworkPccpCollectDiagnosticsPackageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Create(AzMobileNetworkPccpCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzMobileNetworkPccpDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMobileNetworkPccpDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzMobileNetworkPccpListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMobileNetworkPccpListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Reinstall(AzMobileNetworkPccpReinstallOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMobileNetworkPccpReinstallOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Rollback(AzMobileNetworkPccpRollbackOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMobileNetworkPccpRollbackOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzMobileNetworkPccpShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMobileNetworkPccpShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzMobileNetworkPccpUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMobileNetworkPccpUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzMobileNetworkPccpWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMobileNetworkPccpWaitOptions(), cancellationToken: token);
    }
}