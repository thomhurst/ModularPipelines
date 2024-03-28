using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Kubernetes.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Kubernetes.Services;

[ExcludeFromCodeCoverage]
public class KubernetesSet(
    ICommand internalCommand
    )
{
    private readonly ICommand _command = internalCommand;

    public async Task<CommandResult> Env(KubernetesSetEnvOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesSetEnvOptions(), token);
    }

    public async Task<CommandResult> Image(KubernetesSetImageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Resources(KubernetesSetResourcesOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Selector(KubernetesSetSelectorOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Serviceaccount(KubernetesSetServiceaccountOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Subject(KubernetesSetSubjectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}