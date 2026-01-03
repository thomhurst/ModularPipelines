using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
public class AzConnectedk8s
{
    public AzConnectedk8s(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Connect(AzConnectedk8sConnectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Delete(AzConnectedk8sDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectedk8sDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> DisableFeatures(AzConnectedk8sDisableFeaturesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> EnableFeatures(AzConnectedk8sEnableFeaturesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> List(AzConnectedk8sListOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectedk8sListOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Proxy(AzConnectedk8sProxyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectedk8sProxyOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzConnectedk8sShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectedk8sShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Troubleshoot(AzConnectedk8sTroubleshootOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzConnectedk8sUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectedk8sUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Upgrade(AzConnectedk8sUpgradeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzConnectedk8sUpgradeOptions(), cancellationToken: token);
    }
}