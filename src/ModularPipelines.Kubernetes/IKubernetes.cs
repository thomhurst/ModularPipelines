using ModularPipelines.Kubernetes.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Kubernetes;

public interface IKubernetes
{
    Task<CommandResult> Alpha(KubernetesAlphaOptions options, CancellationToken token = default);

    Task<CommandResult> Annotate(KubernetesAnnotateOptions options, CancellationToken token = default);

    Task<CommandResult> ApiResources(KubernetesApiResourcesOptions options, CancellationToken token = default);

    Task<CommandResult> ApiVersions(KubernetesApiVersionsOptions options, CancellationToken token = default);

    Task<CommandResult> ApplyEditLastApplied(KubernetesApplyEditLastAppliedOptions options, CancellationToken token = default);

    Task<CommandResult> Apply(KubernetesApplyOptions options, CancellationToken token = default);

    Task<CommandResult> ApplySetLastApplied(KubernetesApplySetLastAppliedOptions options, CancellationToken token = default);

    Task<CommandResult> ApplyViewLastApplied(KubernetesApplyViewLastAppliedOptions options, CancellationToken token = default);

    Task<CommandResult> Attach(KubernetesAttachOptions options, CancellationToken token = default);

    Task<CommandResult> AuthCanI(KubernetesAuthCanIOptions options, CancellationToken token = default);

    Task<CommandResult> Auth(KubernetesAuthOptions options, CancellationToken token = default);

    Task<CommandResult> AuthReconcile(KubernetesAuthReconcileOptions options, CancellationToken token = default);

    Task<CommandResult> Autoscale(KubernetesAutoscaleOptions options, CancellationToken token = default);

    Task<CommandResult> CertificateApprove(KubernetesCertificateApproveOptions options, CancellationToken token = default);

    Task<CommandResult> CertificateDeny(KubernetesCertificateDenyOptions options, CancellationToken token = default);

    Task<CommandResult> Certificate(KubernetesCertificateOptions options, CancellationToken token = default);

    Task<CommandResult> ClusterInfoDump(KubernetesClusterInfoDumpOptions options, CancellationToken token = default);

    Task<CommandResult> ClusterInfo(KubernetesClusterInfoOptions options, CancellationToken token = default);

    Task<CommandResult> Completion(KubernetesCompletionOptions options, CancellationToken token = default);

    Task<CommandResult> ConfigCurrentContext(KubernetesConfigCurrentContextOptions options, CancellationToken token = default);

    Task<CommandResult> ConfigDeleteCluster(KubernetesConfigDeleteClusterOptions options, CancellationToken token = default);

    Task<CommandResult> ConfigDeleteContext(KubernetesConfigDeleteContextOptions options, CancellationToken token = default);

    Task<CommandResult> ConfigDeleteUser(KubernetesConfigDeleteUserOptions options, CancellationToken token = default);

    Task<CommandResult> ConfigGetClusters(KubernetesConfigGetClustersOptions options, CancellationToken token = default);

    Task<CommandResult> ConfigGetContexts(KubernetesConfigGetContextsOptions options, CancellationToken token = default);

    Task<CommandResult> ConfigGetUsers(KubernetesConfigGetUsersOptions options, CancellationToken token = default);

    Task<CommandResult> Config(KubernetesConfigOptions options, CancellationToken token = default);

    Task<CommandResult> ConfigRenameContext(KubernetesConfigRenameContextOptions options, CancellationToken token = default);

    Task<CommandResult> ConfigSetCluster(KubernetesConfigSetClusterOptions options, CancellationToken token = default);

    Task<CommandResult> ConfigSetContext(KubernetesConfigSetContextOptions options, CancellationToken token = default);

    Task<CommandResult> ConfigSetCredentials(KubernetesConfigSetCredentialsOptions options, CancellationToken token = default);

    Task<CommandResult> ConfigSet(KubernetesConfigSetOptions options, CancellationToken token = default);

    Task<CommandResult> ConfigUnset(KubernetesConfigUnsetOptions options, CancellationToken token = default);

    Task<CommandResult> ConfigUseContext(KubernetesConfigUseContextOptions options, CancellationToken token = default);

    Task<CommandResult> ConfigView(KubernetesConfigViewOptions options, CancellationToken token = default);

    Task<CommandResult> Cordon(KubernetesCordonOptions options, CancellationToken token = default);

    Task<CommandResult> Cp(KubernetesCpOptions options, CancellationToken token = default);

    Task<CommandResult> CreateClusterRoleBinding(KubernetesCreateClusterRoleBindingOptions options, CancellationToken token = default);

    Task<CommandResult> CreateClusterRole(KubernetesCreateClusterRoleOptions options, CancellationToken token = default);

    Task<CommandResult> CreateConfigMap(KubernetesCreateConfigMapOptions options, CancellationToken token = default);

    Task<CommandResult> CreateCronJob(KubernetesCreateCronJobOptions options, CancellationToken token = default);

    Task<CommandResult> CreateDeployment(KubernetesCreateDeploymentOptions options, CancellationToken token = default);

    Task<CommandResult> CreateIngress(KubernetesCreateIngressOptions options, CancellationToken token = default);

    Task<CommandResult> CreateJob(KubernetesCreateJobOptions options, CancellationToken token = default);

    Task<CommandResult> CreateNamespace(KubernetesCreateNamespaceOptions options, CancellationToken token = default);

    Task<CommandResult> Create(KubernetesCreateOptions options, CancellationToken token = default);

    Task<CommandResult> CreatePodDisruptionBudget(KubernetesCreatePodDisruptionBudgetOptions options, CancellationToken token = default);

    Task<CommandResult> CreatePriorityClass(KubernetesCreatePriorityClassOptions options, CancellationToken token = default);

    Task<CommandResult> CreateQuota(KubernetesCreateQuotaOptions options, CancellationToken token = default);

    Task<CommandResult> CreateRoleBinding(KubernetesCreateRoleBindingOptions options, CancellationToken token = default);

    Task<CommandResult> CreateRole(KubernetesCreateRoleOptions options, CancellationToken token = default);

    Task<CommandResult> CreateSecretDockerRegistry(KubernetesCreateSecretDockerRegistryOptions options, CancellationToken token = default);

    Task<CommandResult> CreateSecretGeneric(KubernetesCreateSecretGenericOptions options, CancellationToken token = default);

    Task<CommandResult> CreateSecret(KubernetesCreateSecretOptions options, CancellationToken token = default);

    Task<CommandResult> CreateSecretTls(KubernetesCreateSecretTlsOptions options, CancellationToken token = default);

    Task<CommandResult> CreateServiceAccount(KubernetesCreateServiceAccountOptions options, CancellationToken token = default);

    Task<CommandResult> CreateServiceClusterip(KubernetesCreateServiceClusteripOptions options, CancellationToken token = default);

    Task<CommandResult> CreateServiceExternalname(KubernetesCreateServiceExternalnameOptions options, CancellationToken token = default);

    Task<CommandResult> CreateServiceLoadbalancer(KubernetesCreateServiceLoadbalancerOptions options, CancellationToken token = default);

    Task<CommandResult> CreateServiceNodePort(KubernetesCreateServiceNodePortOptions options, CancellationToken token = default);

    Task<CommandResult> CreateService(KubernetesCreateServiceOptions options, CancellationToken token = default);

    Task<CommandResult> Debug(KubernetesDebugOptions options, CancellationToken token = default);

    Task<CommandResult> Delete(KubernetesDeleteOptions options, CancellationToken token = default);

    Task<CommandResult> Describe(KubernetesDescribeOptions options, CancellationToken token = default);

    Task<CommandResult> Diff(KubernetesDiffOptions options, CancellationToken token = default);

    Task<CommandResult> Drain(KubernetesDrainOptions options, CancellationToken token = default);

    Task<CommandResult> Edit(KubernetesEditOptions options, CancellationToken token = default);

    Task<CommandResult> Exec(KubernetesExecOptions options, CancellationToken token = default);

    Task<CommandResult> Explain(KubernetesExplainOptions options, CancellationToken token = default);

    Task<CommandResult> Expose(KubernetesExposeOptions options, CancellationToken token = default);

    Task<CommandResult> Get(KubernetesGetOptions options, CancellationToken token = default);

    Task<CommandResult> Kustomize(KubernetesKustomizeOptions options, CancellationToken token = default);

    Task<CommandResult> Label(KubernetesLabelOptions options, CancellationToken token = default);

    Task<CommandResult> Logs(KubernetesLogsOptions options, CancellationToken token = default);

    Task<CommandResult> Options(KubernetesOptionsOptions kubernetesOptions, CancellationToken token = default);

    Task<CommandResult> Patch(KubernetesPatchOptions options, CancellationToken token = default);

    Task<CommandResult> PluginList(KubernetesPluginListOptions options, CancellationToken token = default);

    Task<CommandResult> Plugin(KubernetesPluginOptions options, CancellationToken token = default);

    Task<CommandResult> PortForward(KubernetesPortForwardOptions options, CancellationToken token = default);

    Task<CommandResult> Proxy(KubernetesProxyOptions options, CancellationToken token = default);

    Task<CommandResult> Replace(KubernetesReplaceOptions options, CancellationToken token = default);

    Task<CommandResult> RolloutHistory(KubernetesRolloutHistoryOptions options, CancellationToken token = default);

    Task<CommandResult> Rollout(KubernetesRolloutOptions options, CancellationToken token = default);

    Task<CommandResult> RolloutPause(KubernetesRolloutPauseOptions options, CancellationToken token = default);

    Task<CommandResult> RolloutRestart(KubernetesRolloutRestartOptions options, CancellationToken token = default);

    Task<CommandResult> RolloutResume(KubernetesRolloutResumeOptions options, CancellationToken token = default);

    Task<CommandResult> RolloutStatus(KubernetesRolloutStatusOptions options, CancellationToken token = default);

    Task<CommandResult> RolloutUndo(KubernetesRolloutUndoOptions options, CancellationToken token = default);

    Task<CommandResult> Run(KubernetesRunOptions options, CancellationToken token = default);

    Task<CommandResult> Scale(KubernetesScaleOptions options, CancellationToken token = default);

    Task<CommandResult> SetEnv(KubernetesSetEnvOptions options, CancellationToken token = default);

    Task<CommandResult> SetImage(KubernetesSetImageOptions options, CancellationToken token = default);

    Task<CommandResult> Set(KubernetesSetOptions options, CancellationToken token = default);

    Task<CommandResult> SetResources(KubernetesSetResourcesOptions options, CancellationToken token = default);

    Task<CommandResult> SetSelector(KubernetesSetSelectorOptions options, CancellationToken token = default);

    Task<CommandResult> SetServiceAccount(KubernetesSetServiceAccountOptions options, CancellationToken token = default);

    Task<CommandResult> SetSubject(KubernetesSetSubjectOptions options, CancellationToken token = default);

    Task<CommandResult> Taint(KubernetesTaintOptions options, CancellationToken token = default);

    Task<CommandResult> TopNode(KubernetesTopNodeOptions options, CancellationToken token = default);

    Task<CommandResult> Top(KubernetesTopOptions options, CancellationToken token = default);

    Task<CommandResult> TopPod(KubernetesTopPodOptions options, CancellationToken token = default);

    Task<CommandResult> Uncordon(KubernetesUncordonOptions options, CancellationToken token = default);

    Task<CommandResult> Version(KubernetesVersionOptions options, CancellationToken token = default);

    Task<CommandResult> Wait(KubernetesWaitOptions options, CancellationToken token = default);
}
