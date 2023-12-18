using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkcloud")]
public class AzNetworkcloudKubernetescluster
{
    public AzNetworkcloudKubernetescluster(
        AzNetworkcloudKubernetesclusterAgentpool agentpool,
        ICommand internalCommand
    )
    {
        Agentpool = agentpool;
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public AzNetworkcloudKubernetesclusterAgentpool Agentpool { get; }

    public async Task<CommandResult> Create(AzNetworkcloudKubernetesclusterCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkcloudKubernetesclusterDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> List(AzNetworkcloudKubernetesclusterListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RestartNode(AzNetworkcloudKubernetesclusterRestartNodeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetworkcloudKubernetesclusterShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudKubernetesclusterShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkcloudKubernetesclusterUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudKubernetesclusterUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkcloudKubernetesclusterWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudKubernetesclusterWaitOptions(), token);
    }
}