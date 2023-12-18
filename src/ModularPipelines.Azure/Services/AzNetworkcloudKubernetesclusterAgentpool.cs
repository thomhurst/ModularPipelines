using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Azure.Options;

namespace ModularPipelines.Azure.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkcloud", "kubernetescluster")]
public class AzNetworkcloudKubernetesclusterAgentpool
{
    public AzNetworkcloudKubernetesclusterAgentpool(
        ICommand internalCommand
    )
    {
        _command = internalCommand;
    }

    private readonly ICommand _command;

    public async Task<CommandResult> Create(AzNetworkcloudKubernetesclusterAgentpoolCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Delete(AzNetworkcloudKubernetesclusterAgentpoolDeleteOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudKubernetesclusterAgentpoolDeleteOptions(), token);
    }

    public async Task<CommandResult> List(AzNetworkcloudKubernetesclusterAgentpoolListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Show(AzNetworkcloudKubernetesclusterAgentpoolShowOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudKubernetesclusterAgentpoolShowOptions(), token);
    }

    public async Task<CommandResult> Update(AzNetworkcloudKubernetesclusterAgentpoolUpdateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudKubernetesclusterAgentpoolUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(AzNetworkcloudKubernetesclusterAgentpoolWaitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new AzNetworkcloudKubernetesclusterAgentpoolWaitOptions(), token);
    }
}