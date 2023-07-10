using ModularPipelines.Docker.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Docker;

public interface IDocker
{
    Task<CommandResult> Attach(DockerAttachOptions options, CancellationToken token = default);

    Task<CommandResult> Base(DockerBaseOptions options, CancellationToken token = default);

    Task<CommandResult> BuilderBuild(DockerBuilderBuildOptions options, CancellationToken token = default);

    Task<CommandResult> BuilderPrune(DockerBuilderPruneOptions options, CancellationToken token = default);

    Task<CommandResult> Build(DockerBuildOptions options, CancellationToken token = default);

    Task<CommandResult> BuildxBake(DockerBuildxBakeOptions options, CancellationToken token = default);

    Task<CommandResult> BuildxBuild(DockerBuildxBuildOptions options, CancellationToken token = default);

    Task<CommandResult> BuildxCreate(DockerBuildxCreateOptions options, CancellationToken token = default);

    Task<CommandResult> BuildxDebugShell(DockerBuildxDebugShellOptions options, CancellationToken token = default);

    Task<CommandResult> BuildxDu(DockerBuildxDuOptions options, CancellationToken token = default);

    Task<CommandResult> BuildxImagetoolsCreate(DockerBuildxImagetoolsCreateOptions options,
        CancellationToken token = default);

    Task<CommandResult> BuildxImagetoolsInspect(DockerBuildxImagetoolsInspectOptions options,
        CancellationToken token = default);

    Task<CommandResult> BuildxInspect(DockerBuildxInspectOptions options, CancellationToken token = default);

    Task<CommandResult> BuildxLs(DockerBuildxLsOptions options, CancellationToken token = default);

    Task<CommandResult> Buildx(DockerBuildxOptions options, CancellationToken token = default);

    Task<CommandResult> BuildxPrune(DockerBuildxPruneOptions options, CancellationToken token = default);

    Task<CommandResult> BuildxRm(DockerBuildxRmOptions options, CancellationToken token = default);

    Task<CommandResult> BuildxUse(DockerBuildxUseOptions options, CancellationToken token = default);

    Task<CommandResult> BuildxVersion(DockerBuildxVersionOptions options, CancellationToken token = default);

    Task<CommandResult> CheckpointCreate(DockerCheckpointCreateOptions options, CancellationToken token = default);

    Task<CommandResult> CheckpointLs(DockerCheckpointLsOptions options, CancellationToken token = default);

    Task<CommandResult> CheckpointRm(DockerCheckpointRmOptions options, CancellationToken token = default);

    Task<CommandResult> Commit(DockerCommitOptions options, CancellationToken token = default);

    Task<CommandResult> ComposeBuild(DockerComposeBuildOptions options, CancellationToken token = default);

    Task<CommandResult> ComposeConfig(DockerComposeConfigOptions options, CancellationToken token = default);

    Task<CommandResult> ComposeCp(DockerComposeCpOptions options, CancellationToken token = default);

    Task<CommandResult> ComposeCreate(DockerComposeCreateOptions options, CancellationToken token = default);

    Task<CommandResult> ComposeDown(DockerComposeDownOptions options, CancellationToken token = default);

    Task<CommandResult> ComposeEvents(DockerComposeEventsOptions options, CancellationToken token = default);

    Task<CommandResult> ComposeExec(DockerComposeExecOptions options, CancellationToken token = default);

    Task<CommandResult> ComposeImages(DockerComposeImagesOptions options, CancellationToken token = default);

    Task<CommandResult> ComposeKill(DockerComposeKillOptions options, CancellationToken token = default);

    Task<CommandResult> ComposeLogs(DockerComposeLogsOptions options, CancellationToken token = default);

    Task<CommandResult> ComposeLs(DockerComposeLsOptions options, CancellationToken token = default);

    Task<CommandResult> Compose(DockerComposeOptions options, CancellationToken token = default);

    Task<CommandResult> ComposePort(DockerComposePortOptions options, CancellationToken token = default);

    Task<CommandResult> ComposePs(DockerComposePsOptions options, CancellationToken token = default);

    Task<CommandResult> ComposePull(DockerComposePullOptions options, CancellationToken token = default);

    Task<CommandResult> ComposePush(DockerComposePushOptions options, CancellationToken token = default);

    Task<CommandResult> ComposeRestart(DockerComposeRestartOptions options, CancellationToken token = default);

    Task<CommandResult> ComposeRm(DockerComposeRmOptions options, CancellationToken token = default);

    Task<CommandResult> ComposeRun(DockerComposeRunOptions options, CancellationToken token = default);

    Task<CommandResult> ComposeStop(DockerComposeStopOptions options, CancellationToken token = default);

    Task<CommandResult> ComposeUp(DockerComposeUpOptions options, CancellationToken token = default);

    Task<CommandResult> ComposeVersion(DockerComposeVersionOptions options, CancellationToken token = default);

    Task<CommandResult> ConfigCreate(DockerConfigCreateOptions options, CancellationToken token = default);

    Task<CommandResult> ConfigInspect(DockerConfigInspectOptions options, CancellationToken token = default);

    Task<CommandResult> ConfigLs(DockerConfigLsOptions options, CancellationToken token = default);

    Task<CommandResult> ContainerAttach(DockerContainerAttachOptions options, CancellationToken token = default);

    Task<CommandResult> ContainerCommit(DockerContainerCommitOptions options, CancellationToken token = default);

    Task<CommandResult> ContainerCp(DockerContainerCpOptions options, CancellationToken token = default);

    Task<CommandResult> ContainerCreate(DockerContainerCreateOptions options, CancellationToken token = default);

    Task<CommandResult> ContainerExec(DockerContainerExecOptions options, CancellationToken token = default);

    Task<CommandResult> ContainerExport(DockerContainerExportOptions options, CancellationToken token = default);

    Task<CommandResult> ContainerInspect(DockerContainerInspectOptions options, CancellationToken token = default);

    Task<CommandResult> ContainerKill(DockerContainerKillOptions options, CancellationToken token = default);

    Task<CommandResult> ContainerLogs(DockerContainerLogsOptions options, CancellationToken token = default);

    Task<CommandResult> ContainerLs(DockerContainerLsOptions options, CancellationToken token = default);

    Task<CommandResult> ContainerPrune(DockerContainerPruneOptions options, CancellationToken token = default);

    Task<CommandResult> ContainerRestart(DockerContainerRestartOptions options, CancellationToken token = default);

    Task<CommandResult> ContainerRm(DockerContainerRmOptions options, CancellationToken token = default);

    Task<CommandResult> ContainerRun(DockerContainerRunOptions options, CancellationToken token = default);

    Task<CommandResult> ContainerStart(DockerContainerStartOptions options, CancellationToken token = default);

    Task<CommandResult> ContainerStats(DockerContainerStatsOptions options, CancellationToken token = default);

    Task<CommandResult> ContainerStop(DockerContainerStopOptions options, CancellationToken token = default);

    Task<CommandResult> ContainerUpdate(DockerContainerUpdateOptions options, CancellationToken token = default);

    Task<CommandResult> ContextCreate(DockerContextCreateOptions options, CancellationToken token = default);

    Task<CommandResult> ContextExport(DockerContextExportOptions options, CancellationToken token = default);

    Task<CommandResult> ContextInspect(DockerContextInspectOptions options, CancellationToken token = default);

    Task<CommandResult> ContextLs(DockerContextLsOptions options, CancellationToken token = default);

    Task<CommandResult> ContextRm(DockerContextRmOptions options, CancellationToken token = default);

    Task<CommandResult> ContextUpdate(DockerContextUpdateOptions options, CancellationToken token = default);

    Task<CommandResult> Cp(DockerCpOptions options, CancellationToken token = default);

    Task<CommandResult> Create(DockerCreateOptions options, CancellationToken token = default);

    Task<CommandResult> Events(DockerEventsOptions options, CancellationToken token = default);

    Task<CommandResult> Exec(DockerExecOptions options, CancellationToken token = default);

    Task<CommandResult> Export(DockerExportOptions options, CancellationToken token = default);

    Task<CommandResult> History(DockerHistoryOptions options, CancellationToken token = default);

    Task<CommandResult> ImageBuild(DockerImageBuildOptions options, CancellationToken token = default);

    Task<CommandResult> ImageHistory(DockerImageHistoryOptions options, CancellationToken token = default);

    Task<CommandResult> ImageImport(DockerImageImportOptions options, CancellationToken token = default);

    Task<CommandResult> ImageInspect(DockerImageInspectOptions options, CancellationToken token = default);

    Task<CommandResult> ImageLoad(DockerImageLoadOptions options, CancellationToken token = default);

    Task<CommandResult> ImageLs(DockerImageLsOptions options, CancellationToken token = default);

    Task<CommandResult> ImagePrune(DockerImagePruneOptions options, CancellationToken token = default);

    Task<CommandResult> ImagePull(DockerImagePullOptions options, CancellationToken token = default);

    Task<CommandResult> ImagePush(DockerImagePushOptions options, CancellationToken token = default);

    Task<CommandResult> ImageRm(DockerImageRmOptions options, CancellationToken token = default);

    Task<CommandResult> ImageSave(DockerImageSaveOptions options, CancellationToken token = default);

    Task<CommandResult> Images(DockerImagesOptions options, CancellationToken token = default);

    Task<CommandResult> Import(DockerImportOptions options, CancellationToken token = default);

    Task<CommandResult> Info(DockerInfoOptions options, CancellationToken token = default);

    Task<CommandResult> Init(DockerInitOptions options, CancellationToken token = default);

    Task<CommandResult> Inspect(DockerInspectOptions options, CancellationToken token = default);

    Task<CommandResult> Kill(DockerKillOptions options, CancellationToken token = default);

    Task<CommandResult> Load(DockerLoadOptions options, CancellationToken token = default);

    Task<CommandResult> Login(DockerLoginOptions options, CancellationToken token = default);

    Task<CommandResult> Logs(DockerLogsOptions options, CancellationToken token = default);

    Task<CommandResult> ManifestAnnotate(DockerManifestAnnotateOptions options, CancellationToken token = default);

    Task<CommandResult> ManifestCreate(DockerManifestCreateOptions options, CancellationToken token = default);

    Task<CommandResult> ManifestInspect(DockerManifestInspectOptions options, CancellationToken token = default);

    Task<CommandResult> ManifestPush(DockerManifestPushOptions options, CancellationToken token = default);

    Task<CommandResult> NetworkConnect(DockerNetworkConnectOptions options, CancellationToken token = default);

    Task<CommandResult> NetworkCreate(DockerNetworkCreateOptions options, CancellationToken token = default);

    Task<CommandResult> NetworkDisconnect(DockerNetworkDisconnectOptions options, CancellationToken token = default);

    Task<CommandResult> NetworkInspect(DockerNetworkInspectOptions options, CancellationToken token = default);

    Task<CommandResult> NetworkLs(DockerNetworkLsOptions options, CancellationToken token = default);

    Task<CommandResult> NetworkPrune(DockerNetworkPruneOptions options, CancellationToken token = default);

    Task<CommandResult> NetworkRm(DockerNetworkRmOptions options, CancellationToken token = default);

    Task<CommandResult> NodeInspect(DockerNodeInspectOptions options, CancellationToken token = default);

    Task<CommandResult> NodeLs(DockerNodeLsOptions options, CancellationToken token = default);

    Task<CommandResult> NodePs(DockerNodePsOptions options, CancellationToken token = default);

    Task<CommandResult> NodeRm(DockerNodeRmOptions options, CancellationToken token = default);

    Task<CommandResult> NodeUpdate(DockerNodeUpdateOptions options, CancellationToken token = default);

    Task<CommandResult> DockerBase(DockerBaseOptions options, CancellationToken token = default);

    Task<CommandResult> PluginCreate(DockerPluginCreateOptions options, CancellationToken token = default);

    Task<CommandResult> PluginDisable(DockerPluginDisableOptions options, CancellationToken token = default);

    Task<CommandResult> PluginEnable(DockerPluginEnableOptions options, CancellationToken token = default);

    Task<CommandResult> PluginInspect(DockerPluginInspectOptions options, CancellationToken token = default);

    Task<CommandResult> PluginInstall(DockerPluginInstallOptions options, CancellationToken token = default);

    Task<CommandResult> PluginLs(DockerPluginLsOptions options, CancellationToken token = default);

    Task<CommandResult> PluginRm(DockerPluginRmOptions options, CancellationToken token = default);

    Task<CommandResult> PluginUpgrade(DockerPluginUpgradeOptions options, CancellationToken token = default);

    Task<CommandResult> Ps(DockerPsOptions options, CancellationToken token = default);

    Task<CommandResult> Pull(DockerPullOptions options, CancellationToken token = default);

    Task<CommandResult> Push(DockerPushOptions options, CancellationToken token = default);

    Task<CommandResult> Restart(DockerRestartOptions options, CancellationToken token = default);

    Task<CommandResult> Rmi(DockerRmiOptions options, CancellationToken token = default);

    Task<CommandResult> Rm(DockerRmOptions options, CancellationToken token = default);

    Task<CommandResult> Run(DockerRunOptions options, CancellationToken token = default);

    Task<CommandResult> Save(DockerSaveOptions options, CancellationToken token = default);

    Task<CommandResult> ScoutCompare(DockerScoutCompareOptions options, CancellationToken token = default);

    Task<CommandResult> ScoutCves(DockerScoutCvesOptions options, CancellationToken token = default);

    Task<CommandResult> ScoutEntitlement(DockerScoutEntitlementOptions options, CancellationToken token = default);

    Task<CommandResult> Scout(DockerScoutOptions options, CancellationToken token = default);

    Task<CommandResult> ScoutQuickview(DockerScoutQuickviewOptions options, CancellationToken token = default);

    Task<CommandResult> ScoutRecommendations(DockerScoutRecommendationsOptions options,
        CancellationToken token = default);

    Task<CommandResult> ScoutRepoList(DockerScoutRepoListOptions options, CancellationToken token = default);

    Task<CommandResult> ScoutSbom(DockerScoutSbomOptions options, CancellationToken token = default);

    Task<CommandResult> ScoutStream(DockerScoutStreamOptions options, CancellationToken token = default);

    Task<CommandResult> Search(DockerSearchOptions options, CancellationToken token = default);

    Task<CommandResult> SecretCreate(DockerSecretCreateOptions options, CancellationToken token = default);

    Task<CommandResult> SecretInspect(DockerSecretInspectOptions options, CancellationToken token = default);

    Task<CommandResult> SecretLs(DockerSecretLsOptions options, CancellationToken token = default);

    Task<CommandResult> ServiceCreate(DockerServiceCreateOptions options, CancellationToken token = default);

    Task<CommandResult> ServiceInspect(DockerServiceInspectOptions options, CancellationToken token = default);

    Task<CommandResult> ServiceLogs(DockerServiceLogsOptions options, CancellationToken token = default);

    Task<CommandResult> ServiceLs(DockerServiceLsOptions options, CancellationToken token = default);

    Task<CommandResult> ServicePs(DockerServicePsOptions options, CancellationToken token = default);

    Task<CommandResult> ServiceRollback(DockerServiceRollbackOptions options, CancellationToken token = default);

    Task<CommandResult> ServiceScale(DockerServiceScaleOptions options, CancellationToken token = default);

    Task<CommandResult> ServiceUpdate(DockerServiceUpdateOptions options, CancellationToken token = default);

    Task<CommandResult> StackConfig(DockerStackConfigOptions options, CancellationToken token = default);

    Task<CommandResult> StackDeploy(DockerStackDeployOptions options, CancellationToken token = default);

    Task<CommandResult> StackLs(DockerStackLsOptions options, CancellationToken token = default);

    Task<CommandResult> Stack(DockerStackOptions options, CancellationToken token = default);

    Task<CommandResult> StackPs(DockerStackPsOptions options, CancellationToken token = default);

    Task<CommandResult> StackServices(DockerStackServicesOptions options, CancellationToken token = default);

    Task<CommandResult> Start(DockerStartOptions options, CancellationToken token = default);

    Task<CommandResult> Stats(DockerStatsOptions options, CancellationToken token = default);

    Task<CommandResult> Stop(DockerStopOptions options, CancellationToken token = default);

    Task<CommandResult> SwarmCa(DockerSwarmCaOptions options, CancellationToken token = default);

    Task<CommandResult> SwarmInit(DockerSwarmInitOptions options, CancellationToken token = default);

    Task<CommandResult> SwarmJoin(DockerSwarmJoinOptions options, CancellationToken token = default);

    Task<CommandResult> SwarmJoinToken(DockerSwarmJoinTokenOptions options, CancellationToken token = default);

    Task<CommandResult> SwarmLeave(DockerSwarmLeaveOptions options, CancellationToken token = default);

    Task<CommandResult> SwarmUnlockKey(DockerSwarmUnlockKeyOptions options, CancellationToken token = default);

    Task<CommandResult> SwarmUpdate(DockerSwarmUpdateOptions options, CancellationToken token = default);

    Task<CommandResult> SystemDf(DockerSystemDfOptions options, CancellationToken token = default);

    Task<CommandResult> SystemEvents(DockerSystemEventsOptions options, CancellationToken token = default);

    Task<CommandResult> SystemInfo(DockerSystemInfoOptions options, CancellationToken token = default);

    Task<CommandResult> SystemPrune(DockerSystemPruneOptions options, CancellationToken token = default);

    Task<CommandResult> TrustInspect(DockerTrustInspectOptions options, CancellationToken token = default);

    Task<CommandResult> TrustKeyGenerate(DockerTrustKeyGenerateOptions options, CancellationToken token = default);

    Task<CommandResult> TrustKeyLoad(DockerTrustKeyLoadOptions options, CancellationToken token = default);

    Task<CommandResult> TrustRevoke(DockerTrustRevokeOptions options, CancellationToken token = default);

    Task<CommandResult> TrustSignerAdd(DockerTrustSignerAddOptions options, CancellationToken token = default);

    Task<CommandResult> TrustSignerRemove(DockerTrustSignerRemoveOptions options, CancellationToken token = default);

    Task<CommandResult> TrustSign(DockerTrustSignOptions options, CancellationToken token = default);

    Task<CommandResult> Update(DockerUpdateOptions options, CancellationToken token = default);

    Task<CommandResult> Version(DockerVersionOptions options, CancellationToken token = default);

    Task<CommandResult> VolumeCreate(DockerVolumeCreateOptions options, CancellationToken token = default);

    Task<CommandResult> VolumeInspect(DockerVolumeInspectOptions options, CancellationToken token = default);

    Task<CommandResult> VolumeLs(DockerVolumeLsOptions options, CancellationToken token = default);

    Task<CommandResult> VolumePrune(DockerVolumePruneOptions options, CancellationToken token = default);

    Task<CommandResult> VolumeRm(DockerVolumeRmOptions options, CancellationToken token = default);

    Task<CommandResult> VolumeUpdate(DockerVolumeUpdateOptions options, CancellationToken token = default);

    Task<CommandResult> Tag(DockerTagOptions options, CancellationToken token = default);

}
