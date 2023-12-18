using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("synapse")]
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

    public async Task<CommandResult> Create(AzSynapseIntegrationRuntimeCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzSynapseIntegrationRuntimeDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetConnectionInfo(AzSynapseIntegrationRuntimeGetConnectionInfoOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetMonitoringData(AzSynapseIntegrationRuntimeGetMonitoringDataOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetStatus(AzSynapseIntegrationRuntimeGetStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzSynapseIntegrationRuntimeListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAuthKey(AzSynapseIntegrationRuntimeListAuthKeyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RegenerateAuthKey(AzSynapseIntegrationRuntimeRegenerateAuthKeyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzSynapseIntegrationRuntimeShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Start(AzSynapseIntegrationRuntimeStartOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Stop(AzSynapseIntegrationRuntimeStopOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SyncCredentials(AzSynapseIntegrationRuntimeSyncCredentialsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzSynapseIntegrationRuntimeUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Upgrade(AzSynapseIntegrationRuntimeUpgradeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Wait(AzSynapseIntegrationRuntimeWaitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}