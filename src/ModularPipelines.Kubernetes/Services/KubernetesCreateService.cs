using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Kubernetes.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Kubernetes.Services;

[ExcludeFromCodeCoverage]
public class KubernetesCreateService(
    KubernetesCreateServiceExternalname externalname,
    ICommand internalCommand
    )
{
    private readonly ICommand _command = internalCommand;

    public KubernetesCreateServiceExternalname Externalname { get; } = externalname;

    public async Task<CommandResult> Clusterip(KubernetesCreateServiceClusteripOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesCreateServiceClusteripOptions(), token);
    }

    public async Task<CommandResult> Loadbalancer(KubernetesCreateServiceLoadbalancerOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesCreateServiceLoadbalancerOptions(), token);
    }

    public async Task<CommandResult> Nodeport(KubernetesCreateServiceNodeportOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesCreateServiceNodeportOptions(), token);
    }
}