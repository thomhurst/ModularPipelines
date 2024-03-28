using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Kubernetes.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Kubernetes.Services;

[ExcludeFromCodeCoverage]
public class KubernetesRollout(
    ICommand internalCommand
    )
{
    private readonly ICommand _command = internalCommand;

    public async Task<CommandResult> History(KubernetesRolloutHistoryOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesRolloutHistoryOptions(), token);
    }

    public async Task<CommandResult> Pause(KubernetesRolloutPauseOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesRolloutPauseOptions(), token);
    }

    public async Task<CommandResult> Restart(KubernetesRolloutRestartOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesRolloutRestartOptions(), token);
    }

    public async Task<CommandResult> Resume(KubernetesRolloutResumeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesRolloutResumeOptions(), token);
    }

    public async Task<CommandResult> Status(KubernetesRolloutStatusOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesRolloutStatusOptions(), token);
    }

    public async Task<CommandResult> Undo(KubernetesRolloutUndoOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesRolloutUndoOptions(), token);
    }
}