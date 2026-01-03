using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Azure.Options;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CliSubCommand("datafactory")]
public class AzDatafactoryIntegrationRuntime
{
    public AzDatafactoryIntegrationRuntime(
        AzDatafactoryIntegrationRuntimeLinkedIntegrationRuntime linkedIntegrationRuntime,
        AzDatafactoryIntegrationRuntimeManaged managed,
        AzDatafactoryIntegrationRuntimeSelfHosted selfHosted,
        ICommand internalCommand
    )
    {
        LinkedIntegrationRuntime = linkedIntegrationRuntime;
        Managed = managed;
        SelfHosted = selfHosted;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzDatafactoryIntegrationRuntimeLinkedIntegrationRuntime LinkedIntegrationRuntime { get; }

    public AzDatafactoryIntegrationRuntimeManaged Managed { get; }

    public AzDatafactoryIntegrationRuntimeSelfHosted SelfHosted { get; }

    public async Task<CommandResult> Delete(AzDatafactoryIntegrationRuntimeDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatafactoryIntegrationRuntimeDeleteOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> GetConnectionInfo(AzDatafactoryIntegrationRuntimeGetConnectionInfoOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatafactoryIntegrationRuntimeGetConnectionInfoOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> GetMonitoringData(AzDatafactoryIntegrationRuntimeGetMonitoringDataOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatafactoryIntegrationRuntimeGetMonitoringDataOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> GetStatus(AzDatafactoryIntegrationRuntimeGetStatusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatafactoryIntegrationRuntimeGetStatusOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> List(AzDatafactoryIntegrationRuntimeListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> ListAuthKey(AzDatafactoryIntegrationRuntimeListAuthKeyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> RegenerateAuthKey(AzDatafactoryIntegrationRuntimeRegenerateAuthKeyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatafactoryIntegrationRuntimeRegenerateAuthKeyOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> RemoveLink(AzDatafactoryIntegrationRuntimeRemoveLinkOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, cancellationToken: token);
    }

    public async Task<CommandResult> Show(AzDatafactoryIntegrationRuntimeShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatafactoryIntegrationRuntimeShowOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Start(AzDatafactoryIntegrationRuntimeStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatafactoryIntegrationRuntimeStartOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Stop(AzDatafactoryIntegrationRuntimeStopOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatafactoryIntegrationRuntimeStopOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> SyncCredentials(AzDatafactoryIntegrationRuntimeSyncCredentialsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatafactoryIntegrationRuntimeSyncCredentialsOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Update(AzDatafactoryIntegrationRuntimeUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatafactoryIntegrationRuntimeUpdateOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Upgrade(AzDatafactoryIntegrationRuntimeUpgradeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatafactoryIntegrationRuntimeUpgradeOptions(), cancellationToken: token);
    }

    public async Task<CommandResult> Wait(AzDatafactoryIntegrationRuntimeWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatafactoryIntegrationRuntimeWaitOptions(), cancellationToken: token);
    }
}