using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Kubernetes.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Kubernetes.Services;

[ExcludeFromCodeCoverage]
public class KubernetesClusterInfo(
    ICommand internalCommand
    )
{
    private readonly ICommand _command = internalCommand;

    public async Task<CommandResult> Dump(KubernetesClusterInfoDumpOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesClusterInfoDumpOptions(), token);
    }
}