using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spring-cloud")]
public class AzSpringCloudApp
{
    public AzSpringCloudApp(
        AzSpringCloudAppBinding binding,
        AzSpringCloudAppCustomDomain customDomain,
        AzSpringCloudAppDeployment deployment,
        AzSpringCloudAppIdentity identity,
        AzSpringCloudAppLog log,
        ICommand internalCommand
    )
    {
        Binding = binding;
        CustomDomain = customDomain;
        Deployment = deployment;
        Identity = identity;
        Log = log;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzSpringCloudAppBinding Binding { get; }

    public AzSpringCloudAppCustomDomain CustomDomain { get; }

    public AzSpringCloudAppDeployment Deployment { get; }

    public AzSpringCloudAppIdentity Identity { get; }

    public AzSpringCloudAppLog Log { get; }

    public async Task<CommandResult> AppendLoadedPublicCertificate(AzSpringCloudAppAppendLoadedPublicCertificateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AppendPersistentStorage(AzSpringCloudAppAppendPersistentStorageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(AzSpringCloudAppCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzSpringCloudAppDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Deploy(AzSpringCloudAppDeployOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzSpringCloudAppListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Logs(AzSpringCloudAppLogsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Restart(AzSpringCloudAppRestartOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Scale(AzSpringCloudAppScaleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetDeployment(AzSpringCloudAppSetDeploymentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzSpringCloudAppShowOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ShowDeployLog(AzSpringCloudAppShowDeployLogOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Start(AzSpringCloudAppStartOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Stop(AzSpringCloudAppStopOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> UnsetDeployment(AzSpringCloudAppUnsetDeploymentOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(AzSpringCloudAppUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}