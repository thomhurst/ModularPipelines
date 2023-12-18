using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datafactory")]
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

    public async Task<CommandResult> Delete(AzDatafactoryIntegrationRuntimeDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetConnectionInfo(AzDatafactoryIntegrationRuntimeGetConnectionInfoOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetMonitoringData(AzDatafactoryIntegrationRuntimeGetMonitoringDataOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetStatus(AzDatafactoryIntegrationRuntimeGetStatusOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzDatafactoryIntegrationRuntimeListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ListAuthKey(AzDatafactoryIntegrationRuntimeListAuthKeyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RegenerateAuthKey(AzDatafactoryIntegrationRuntimeRegenerateAuthKeyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RemoveLink(AzDatafactoryIntegrationRuntimeRemoveLinkOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzDatafactoryIntegrationRuntimeShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatafactoryIntegrationRuntimeShowOptions(), token);
    }

    public async Task<CommandResult> Start(AzDatafactoryIntegrationRuntimeStartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatafactoryIntegrationRuntimeStartOptions(), token);
    }

    public async Task<CommandResult> Stop(AzDatafactoryIntegrationRuntimeStopOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatafactoryIntegrationRuntimeStopOptions(), token);
    }

    public async Task<CommandResult> SyncCredentials(AzDatafactoryIntegrationRuntimeSyncCredentialsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatafactoryIntegrationRuntimeSyncCredentialsOptions(), token);
    }

    public async Task<CommandResult> Update(AzDatafactoryIntegrationRuntimeUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatafactoryIntegrationRuntimeUpdateOptions(), token);
    }

    public async Task<CommandResult> Upgrade(AzDatafactoryIntegrationRuntimeUpgradeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatafactoryIntegrationRuntimeUpgradeOptions(), token);
    }

    public async Task<CommandResult> Wait(AzDatafactoryIntegrationRuntimeWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzDatafactoryIntegrationRuntimeWaitOptions(), token);
    }
}

