using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Kubernetes.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Kubernetes.Services;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("service")]
public class KubernetesCreateServiceExternalname(
    ICommand internalCommand
    )
{
    private readonly ICommand _command = internalCommand;

    public async Task<CommandResult> External(KubernetesCreateServiceExternalnameExternalOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}