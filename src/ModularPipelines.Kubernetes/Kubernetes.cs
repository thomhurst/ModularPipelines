using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;
using ModularPipelines.Kubernetes.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Kubernetes;

[ExcludeFromCodeCoverage]
internal class Kubernetes : IKubernetes
{
    private readonly ICommand _command;

    public Kubernetes(ICommand command)
    {
        _command = command;
    }

    public virtual async Task<CommandResult> Alpha(KubernetesAlphaOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Annotate(KubernetesAnnotateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> ApiResources(KubernetesApiResourcesOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> ApiVersions(KubernetesApiVersionsOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> ApplyEditLastApplied(KubernetesApplyEditLastAppliedOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Apply(KubernetesApplyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> ApplySetLastApplied(KubernetesApplySetLastAppliedOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> ApplyViewLastApplied(KubernetesApplyViewLastAppliedOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Attach(KubernetesAttachOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> AuthCanI(KubernetesAuthCanIOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Auth(KubernetesAuthOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> AuthReconcile(KubernetesAuthReconcileOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Autoscale(KubernetesAutoscaleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> CertificateApprove(KubernetesCertificateApproveOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> CertificateDeny(KubernetesCertificateDenyOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Certificate(KubernetesCertificateOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> ClusterInfoDump(KubernetesClusterInfoDumpOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> ClusterInfo(KubernetesClusterInfoOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Completion(KubernetesCompletionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> ConfigCurrentContext(KubernetesConfigCurrentContextOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> ConfigDeleteCluster(KubernetesConfigDeleteClusterOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> ConfigDeleteContext(KubernetesConfigDeleteContextOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> ConfigDeleteUser(KubernetesConfigDeleteUserOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> ConfigGetClusters(KubernetesConfigGetClustersOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> ConfigGetContexts(KubernetesConfigGetContextsOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> ConfigGetUsers(KubernetesConfigGetUsersOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Config(KubernetesConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> ConfigRenameContext(KubernetesConfigRenameContextOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> ConfigSetCluster(KubernetesConfigSetClusterOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> ConfigSetContext(KubernetesConfigSetContextOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> ConfigSetCredentials(KubernetesConfigSetCredentialsOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> ConfigSet(KubernetesConfigSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> ConfigUnset(KubernetesConfigUnsetOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> ConfigUseContext(KubernetesConfigUseContextOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> ConfigView(KubernetesConfigViewOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Cordon(KubernetesCordonOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Cp(KubernetesCpOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> CreateClusterRoleBinding(KubernetesCreateClusterRoleBindingOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> CreateClusterRole(KubernetesCreateClusterRoleOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> CreateConfigMap(KubernetesCreateConfigMapOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> CreateCronJob(KubernetesCreateCronJobOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> CreateDeployment(KubernetesCreateDeploymentOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> CreateIngress(KubernetesCreateIngressOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> CreateJob(KubernetesCreateJobOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> CreateNamespace(KubernetesCreateNamespaceOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Create(KubernetesCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> CreatePodDisruptionBudget(KubernetesCreatePodDisruptionBudgetOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> CreatePriorityClass(KubernetesCreatePriorityClassOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> CreateQuota(KubernetesCreateQuotaOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> CreateRoleBinding(KubernetesCreateRoleBindingOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> CreateRole(KubernetesCreateRoleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> CreateSecretDockerRegistry(KubernetesCreateSecretDockerRegistryOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> CreateSecretGeneric(KubernetesCreateSecretGenericOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> CreateSecret(KubernetesCreateSecretOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> CreateSecretTls(KubernetesCreateSecretTlsOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> CreateServiceAccount(KubernetesCreateServiceAccountOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> CreateServiceClusterip(KubernetesCreateServiceClusteripOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> CreateServiceExternalname(KubernetesCreateServiceExternalnameOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> CreateServiceLoadbalancer(KubernetesCreateServiceLoadbalancerOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> CreateServiceNodePort(KubernetesCreateServiceNodePortOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> CreateService(KubernetesCreateServiceOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Debug(KubernetesDebugOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Delete(KubernetesDeleteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Describe(KubernetesDescribeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Diff(KubernetesDiffOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Drain(KubernetesDrainOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Edit(KubernetesEditOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Exec(KubernetesExecOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Explain(KubernetesExplainOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Expose(KubernetesExposeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Get(KubernetesGetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Kustomize(KubernetesKustomizeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Label(KubernetesLabelOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Logs(KubernetesLogsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Options(KubernetesOptionsOptions kubernetesOptions, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(kubernetesOptions, token);
    }

    public virtual async Task<CommandResult> Patch(KubernetesPatchOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> PluginList(KubernetesPluginListOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Plugin(KubernetesPluginOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> PortForward(KubernetesPortForwardOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Proxy(KubernetesProxyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Replace(KubernetesReplaceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> RolloutHistory(KubernetesRolloutHistoryOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Rollout(KubernetesRolloutOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> RolloutPause(KubernetesRolloutPauseOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> RolloutRestart(KubernetesRolloutRestartOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> RolloutResume(KubernetesRolloutResumeOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> RolloutStatus(KubernetesRolloutStatusOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> RolloutUndo(KubernetesRolloutUndoOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Run(KubernetesRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Scale(KubernetesScaleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> SetEnv(KubernetesSetEnvOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> SetImage(KubernetesSetImageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Set(KubernetesSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> SetResources(KubernetesSetResourcesOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> SetSelector(KubernetesSetSelectorOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> SetServiceAccount(KubernetesSetServiceAccountOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> SetSubject(KubernetesSetSubjectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Taint(KubernetesTaintOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> TopNode(KubernetesTopNodeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Top(KubernetesTopOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> TopPod(KubernetesTopPodOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Uncordon(KubernetesUncordonOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Version(KubernetesVersionOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public virtual async Task<CommandResult> Wait(KubernetesWaitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}