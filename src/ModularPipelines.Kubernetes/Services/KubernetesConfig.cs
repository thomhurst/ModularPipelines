using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Kubernetes.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Kubernetes.Services;

[ExcludeFromCodeCoverage]
public class KubernetesConfig(
    ICommand internalCommand
    )
{
    private readonly ICommand _command = internalCommand;

    public async Task<CommandResult> CurrentContext(KubernetesConfigCurrentContextOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesConfigCurrentContextOptions(), token);
    }

    public async Task<CommandResult> DeleteCluster(KubernetesConfigDeleteClusterOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesConfigDeleteClusterOptions(), token);
    }

    public async Task<CommandResult> DeleteContext(KubernetesConfigDeleteContextOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesConfigDeleteContextOptions(), token);
    }

    public async Task<CommandResult> DeleteUser(KubernetesConfigDeleteUserOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesConfigDeleteUserOptions(), token);
    }

    public async Task<CommandResult> GetClusters(KubernetesConfigGetClustersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesConfigGetClustersOptions(), token);
    }

    public async Task<CommandResult> GetContexts(KubernetesConfigGetContextsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> GetUsers(KubernetesConfigGetUsersOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesConfigGetUsersOptions(), token);
    }

    public async Task<CommandResult> RenameContext(KubernetesConfigRenameContextOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesConfigRenameContextOptions(), token);
    }

    public async Task<CommandResult> Set(KubernetesConfigSetOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesConfigSetOptions(), token);
    }

    public async Task<CommandResult> SetCluster(KubernetesConfigSetClusterOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesConfigSetClusterOptions(), token);
    }

    public async Task<CommandResult> SetContext(KubernetesConfigSetContextOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesConfigSetContextOptions(), token);
    }

    public async Task<CommandResult> SetCredentials(KubernetesConfigSetCredentialsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesConfigSetCredentialsOptions(), token);
    }

    public async Task<CommandResult> Unset(KubernetesConfigUnsetOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesConfigUnsetOptions(), token);
    }

    public async Task<CommandResult> UseContext(KubernetesConfigUseContextOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesConfigUseContextOptions(), token);
    }

    public async Task<CommandResult> View(KubernetesConfigViewOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesConfigViewOptions(), token);
    }
}