using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("synapse")]
public class AzSynapseIntegrationRuntime
{
    public AzSynapseIntegrationRuntime(
        AzSynapseIntegrationRuntimeManaged managed,
        AzSynapseIntegrationRuntimeSelfHosted selfHosted,
        ICommand internalCommand
    )
    {
        Managed = managed;
        SelfHosted = selfHosted;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSynapseIntegrationRuntimeManaged Managed { get; }

    public AzSynapseIntegrationRuntimeSelfHosted SelfHosted { get; }

    public async Task<CommandResult> Delete(AzSynapseIntegrationRuntimeDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSynapseIntegrationRuntimeDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> GetConnectionInfo(AzSynapseIntegrationRuntimeGetConnectionInfoOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSynapseIntegrationRuntimeGetConnectionInfoOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> GetMonitoringData(AzSynapseIntegrationRuntimeGetMonitoringDataOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSynapseIntegrationRuntimeGetMonitoringDataOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> GetStatus(AzSynapseIntegrationRuntimeGetStatusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSynapseIntegrationRuntimeGetStatusOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzSynapseIntegrationRuntimeListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> ListAuthKey(AzSynapseIntegrationRuntimeListAuthKeyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> RegenerateAuthKey(AzSynapseIntegrationRuntimeRegenerateAuthKeyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSynapseIntegrationRuntimeRegenerateAuthKeyOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzSynapseIntegrationRuntimeShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSynapseIntegrationRuntimeShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Start(AzSynapseIntegrationRuntimeStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSynapseIntegrationRuntimeStartOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Stop(AzSynapseIntegrationRuntimeStopOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSynapseIntegrationRuntimeStopOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> SyncCredentials(AzSynapseIntegrationRuntimeSyncCredentialsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSynapseIntegrationRuntimeSyncCredentialsOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzSynapseIntegrationRuntimeUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Upgrade(AzSynapseIntegrationRuntimeUpgradeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzSynapseIntegrationRuntimeUpgradeOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzSynapseIntegrationRuntimeWaitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }
}