using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;
using ModularPipelines.Kubernetes.Options;
using ModularPipelines.Kubernetes.Services;
using ModularPipelines.Models;

namespace ModularPipelines.Kubernetes;

[ExcludeFromCodeCoverage]
public class Kubernetes(
    KubernetesApply apply,
    KubernetesAuth auth,
    KubernetesCertificate certificate,
    KubernetesClusterInfo clusterInfo,
    KubernetesConfig config,
    KubernetesCreate create,
    KubernetesRollout rollout,
    KubernetesSet set,
    KubernetesTop top,
    ICommand internalCommand
    )
{
    private readonly ICommand _command = internalCommand;

    public KubernetesApply Apply { get; } = apply;

    public KubernetesAuth Auth { get; } = auth;

    public KubernetesCertificate Certificate { get; } = certificate;

    public KubernetesClusterInfo ClusterInfo { get; } = clusterInfo;

    public KubernetesConfig Config { get; } = config;

    public KubernetesCreate Create { get; } = create;

    public KubernetesRollout Rollout { get; } = rollout;

    public KubernetesSet Set { get; } = set;

    public KubernetesTop Top { get; } = top;

    public async Task<CommandResult> Alpha(KubernetesAlphaOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesAlphaOptions(), token);
    }

    public async Task<CommandResult> Annotate(KubernetesAnnotateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ApiResources(KubernetesApiResourcesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesApiResourcesOptions(), token);
    }

    public async Task<CommandResult> ApiVersions(KubernetesApiVersionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesApiVersionsOptions(), token);
    }

    public async Task<CommandResult> ApplyCommand(KubernetesApplyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Attach(KubernetesAttachOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> AuthCommand(KubernetesAuthOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesAuthOptions(), token);
    }

    public async Task<CommandResult> Autoscale(KubernetesAutoscaleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CertificateCommand(KubernetesCertificateOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesCertificateOptions(), token);
    }

    public async Task<CommandResult> ClusterInfoCommand(KubernetesClusterInfoOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesClusterInfoOptions(), token);
    }

    public async Task<CommandResult> Completion(KubernetesCompletionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesCompletionOptions(), token);
    }

    public async Task<CommandResult> ConfigCommand(KubernetesConfigOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesConfigOptions(), token);
    }

    public async Task<CommandResult> Cordon(KubernetesCordonOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesCordonOptions(), token);
    }

    public async Task<CommandResult> Cp(KubernetesCpOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesCpOptions(), token);
    }

    public async Task<CommandResult> CreateCommand(KubernetesCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Debug(KubernetesDebugOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesDebugOptions(), token);
    }

    public async Task<CommandResult> Delete(KubernetesDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Describe(KubernetesDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Diff(KubernetesDiffOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Drain(KubernetesDrainOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesDrainOptions(), token);
    }

    public async Task<CommandResult> Edit(KubernetesEditOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Exec(KubernetesExecOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesExecOptions(), token);
    }

    public async Task<CommandResult> Explain(KubernetesExplainOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesExplainOptions(), token);
    }

    public async Task<CommandResult> Expose(KubernetesExposeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Get(KubernetesGetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Kustomize(KubernetesKustomizeOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesKustomizeOptions(), token);
    }

    public async Task<CommandResult> Label(KubernetesLabelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Logs(KubernetesLogsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesLogsOptions(), token);
    }

    public async Task<CommandResult> Options(KubernetesOptionsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesOptionsOptions(), token);
    }

    public async Task<CommandResult> Patch(KubernetesPatchOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Plugin(KubernetesPluginOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesPluginOptions(), token);
    }

    public async Task<CommandResult> PortForward(KubernetesPortForwardOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesPortForwardOptions(), token);
    }

    public async Task<CommandResult> Proxy(KubernetesProxyOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesProxyOptions(), token);
    }

    public async Task<CommandResult> Replace(KubernetesReplaceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> RolloutCommand(KubernetesRolloutOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesRolloutOptions(), token);
    }

    public async Task<CommandResult> Run(KubernetesRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Scale(KubernetesScaleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SetCommand(KubernetesSetOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesSetOptions(), token);
    }

    public async Task<CommandResult> Taint(KubernetesTaintOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesTaintOptions(), token);
    }

    public async Task<CommandResult> TopCommand(KubernetesTopOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesTopOptions(), token);
    }

    public async Task<CommandResult> Uncordon(KubernetesUncordonOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesUncordonOptions(), token);
    }

    public async Task<CommandResult> Version(KubernetesVersionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new KubernetesVersionOptions(), token);
    }

    public async Task<CommandResult> Wait(KubernetesWaitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}