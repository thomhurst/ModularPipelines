using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mobile-network")]
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
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzMobileNetworkPccpCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzMobileNetworkPccpDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMobileNetworkPccpDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzMobileNetworkPccpListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMobileNetworkPccpListOptions(), token);
    }

    public async Task<CommandResult> Reinstall(AzMobileNetworkPccpReinstallOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMobileNetworkPccpReinstallOptions(), token);
    }

    public async Task<CommandResult> Rollback(AzMobileNetworkPccpRollbackOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMobileNetworkPccpRollbackOptions(), token);
    }

    public async Task<CommandResult> Show(AzMobileNetworkPccpShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMobileNetworkPccpShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzMobileNetworkPccpUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMobileNetworkPccpUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzMobileNetworkPccpWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzMobileNetworkPccpWaitOptions(), token);
    }
}