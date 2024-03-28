using ModularPipelines.Kubernetes.Options;
using ModularPipelines.Kubernetes.Services;
using ModularPipelines.Models;

namespace ModularPipelines.Kubernetes;

public interface IKubernetes
{
    KubernetesApply ApplyCommands { get; }

    KubernetesAuth AuthCommands { get; }

    KubernetesCertificate CertificateCommands { get; }

    KubernetesClusterInfo ClusterInfoCommands { get; }

    KubernetesConfig ConfigCommands { get; }

    KubernetesCreate CreateCommands { get; }

    KubernetesPlugin PluginCommands { get; }

    KubernetesRollout RolloutCommands { get; }

    KubernetesSet SetCommands { get; }

    KubernetesTop TopCommands { get; }

    Task<CommandResult> Alpha(KubernetesAlphaOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Annotate(KubernetesAnnotateOptions options, CancellationToken token = default);

    Task<CommandResult> ApiResources(KubernetesApiResourcesOptions? options = default, CancellationToken token = default);

    Task<CommandResult> ApiVersions(KubernetesApiVersionsOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Apply(KubernetesApplyOptions options, CancellationToken token = default);

    Task<CommandResult> Attach(KubernetesAttachOptions options, CancellationToken token = default);

    Task<CommandResult> Auth(KubernetesAuthOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Autoscale(KubernetesAutoscaleOptions options, CancellationToken token = default);

    Task<CommandResult> Certificate(KubernetesCertificateOptions? options = default, CancellationToken token = default);

    Task<CommandResult> ClusterInfo(KubernetesClusterInfoOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Completion(KubernetesCompletionOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Config(KubernetesConfigOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Cordon(KubernetesCordonOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Cp(KubernetesCpOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Create(KubernetesCreateOptions options, CancellationToken token = default);

    Task<CommandResult> Debug(KubernetesDebugOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Delete(KubernetesDeleteOptions options, CancellationToken token = default);

    Task<CommandResult> Describe(KubernetesDescribeOptions options, CancellationToken token = default);

    Task<CommandResult> Diff(KubernetesDiffOptions options, CancellationToken token = default);

    Task<CommandResult> Drain(KubernetesDrainOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Edit(KubernetesEditOptions options, CancellationToken token = default);

    Task<CommandResult> Exec(KubernetesExecOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Explain(KubernetesExplainOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Expose(KubernetesExposeOptions options, CancellationToken token = default);

    Task<CommandResult> Get(KubernetesGetOptions options, CancellationToken token = default);

    Task<CommandResult> Kustomize(KubernetesKustomizeOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Label(KubernetesLabelOptions options, CancellationToken token = default);

    Task<CommandResult> Logs(KubernetesLogsOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Options(KubernetesOptionsOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Patch(KubernetesPatchOptions options, CancellationToken token = default);

    Task<CommandResult> Plugin(KubernetesPluginOptions? options = default, CancellationToken token = default);

    Task<CommandResult> PortForward(KubernetesPortForwardOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Proxy(KubernetesProxyOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Replace(KubernetesReplaceOptions options, CancellationToken token = default);

    Task<CommandResult> Rollout(KubernetesRolloutOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Run(KubernetesRunOptions options, CancellationToken token = default);

    Task<CommandResult> Scale(KubernetesScaleOptions options, CancellationToken token = default);

    Task<CommandResult> Set(KubernetesSetOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Taint(KubernetesTaintOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Top(KubernetesTopOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Uncordon(KubernetesUncordonOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Version(KubernetesVersionOptions? options = default, CancellationToken token = default);

    Task<CommandResult> Wait(KubernetesWaitOptions options, CancellationToken token = default);
}