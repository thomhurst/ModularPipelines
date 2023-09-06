using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;
using ModularPipelines.Docker.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Docker;

[ExcludeFromCodeCoverage]
internal class Docker : IDocker
{
    private readonly ICommand _command;

    public Docker(ICommand command)
    {
        _command = command;
    }

    public async Task<CommandResult> Attach(DockerAttachOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BuilderBuild(DockerBuilderBuildOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Builder(DockerBuilderOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BuilderPrune(DockerBuilderPruneOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerBuilderPruneOptions(), token);
    }

    public async Task<CommandResult> Build(DockerBuildOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BuildxBake(DockerBuildxBakeOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerBuildxBakeOptions(), token);
    }

    public async Task<CommandResult> BuildxBuild(DockerBuildxBuildOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BuildxCreate(DockerBuildxCreateOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerBuildxCreateOptions(), token);
    }

    public async Task<CommandResult> BuildxDebugShell(DockerBuildxDebugShellOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerBuildxDebugShellOptions(), token);
    }

    public async Task<CommandResult> BuildxDu(DockerBuildxDuOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerBuildxDuOptions(), token);
    }

    public async Task<CommandResult> BuildxImagetoolsCreate(DockerBuildxImagetoolsCreateOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerBuildxImagetoolsCreateOptions(), token);
    }

    public async Task<CommandResult> BuildxImagetoolsInspect(DockerBuildxImagetoolsInspectOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> BuildxInspect(DockerBuildxInspectOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerBuildxInspectOptions(), token);
    }

    public async Task<CommandResult> Buildx(DockerBuildxOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerBuildxOptions(), token);
    }

    public async Task<CommandResult> BuildxPrune(DockerBuildxPruneOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerBuildxPruneOptions(), token);
    }

    public async Task<CommandResult> BuildxRm(DockerBuildxRmOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerBuildxRmOptions(), token);
    }

    public async Task<CommandResult> BuildxStop(DockerBuildxStopOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerBuildxStopOptions(), token);
    }

    public async Task<CommandResult> BuildxUse(DockerBuildxUseOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CheckpointCreate(DockerCheckpointCreateOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CheckpointLs(DockerCheckpointLsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Checkpoint(DockerCheckpointOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> CheckpointRm(DockerCheckpointRmOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Commit(DockerCommitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ComposeBuild(DockerComposeBuildOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerComposeBuildOptions(), token);
    }

    public async Task<CommandResult> ComposeConfig(DockerComposeConfigOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerComposeConfigOptions(), token);
    }

    public async Task<CommandResult> ComposeCp(DockerComposeCpOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ComposeCreate(DockerComposeCreateOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerComposeCreateOptions(), token);
    }

    public async Task<CommandResult> ComposeDown(DockerComposeDownOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerComposeDownOptions(), token);
    }

    public async Task<CommandResult> ComposeEvents(DockerComposeEventsOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerComposeEventsOptions(), token);
    }

    public async Task<CommandResult> ComposeExec(DockerComposeExecOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ComposeImages(DockerComposeImagesOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerComposeImagesOptions(), token);
    }

    public async Task<CommandResult> ComposeKill(DockerComposeKillOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerComposeKillOptions(), token);
    }

    public async Task<CommandResult> ComposeLogs(DockerComposeLogsOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerComposeLogsOptions(), token);
    }

    public async Task<CommandResult> ComposeLs(DockerComposeLsOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerComposeLsOptions(), token);
    }

    public async Task<CommandResult> Compose(DockerComposeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ComposePause(DockerComposePauseOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerComposePauseOptions(), token);
    }

    public async Task<CommandResult> ComposePort(DockerComposePortOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ComposePs(DockerComposePsOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerComposePsOptions(), token);
    }

    public async Task<CommandResult> ComposePull(DockerComposePullOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerComposePullOptions(), token);
    }

    public async Task<CommandResult> ComposePush(DockerComposePushOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerComposePushOptions(), token);
    }

    public async Task<CommandResult> ComposeRestart(DockerComposeRestartOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerComposeRestartOptions(), token);
    }

    public async Task<CommandResult> ComposeRm(DockerComposeRmOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerComposeRmOptions(), token);
    }

    public async Task<CommandResult> ComposeRun(DockerComposeRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ComposeStart(DockerComposeStartOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerComposeStartOptions(), token);
    }

    public async Task<CommandResult> ComposeStop(DockerComposeStopOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerComposeStopOptions(), token);
    }

    public async Task<CommandResult> ComposeTop(DockerComposeTopOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerComposeTopOptions(), token);
    }

    public async Task<CommandResult> ComposeUnpause(DockerComposeUnpauseOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerComposeUnpauseOptions(), token);
    }

    public async Task<CommandResult> ComposeUp(DockerComposeUpOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerComposeUpOptions(), token);
    }

    public async Task<CommandResult> ComposeVersion(DockerComposeVersionOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerComposeVersionOptions(), token);
    }

    public async Task<CommandResult> ConfigCreate(DockerConfigCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ConfigInspect(DockerConfigInspectOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ConfigLs(DockerConfigLsOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerConfigLsOptions(), token);
    }

    public async Task<CommandResult> Config(DockerConfigOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ConfigRm(DockerConfigRmOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ContainerAttach(DockerContainerAttachOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ContainerCommit(DockerContainerCommitOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ContainerCp(DockerContainerCpOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ContainerCreate(DockerContainerCreateOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ContainerDiff(DockerContainerDiffOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ContainerExec(DockerContainerExecOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ContainerExport(DockerContainerExportOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ContainerInspect(DockerContainerInspectOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ContainerKill(DockerContainerKillOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ContainerLogs(DockerContainerLogsOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ContainerLs(DockerContainerLsOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerContainerLsOptions(), token);
    }

    public async Task<CommandResult> Container(DockerContainerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ContainerPause(DockerContainerPauseOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ContainerPort(DockerContainerPortOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ContainerPrune(DockerContainerPruneOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerContainerPruneOptions(), token);
    }

    public async Task<CommandResult> ContainerRename(DockerContainerRenameOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ContainerRestart(DockerContainerRestartOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ContainerRm(DockerContainerRmOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ContainerRun(DockerContainerRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ContainerStart(DockerContainerStartOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ContainerStats(DockerContainerStatsOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerContainerStatsOptions(), token);
    }

    public async Task<CommandResult> ContainerStop(DockerContainerStopOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ContainerTop(DockerContainerTopOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ContainerUnpause(DockerContainerUnpauseOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ContainerUpdate(DockerContainerUpdateOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ContainerWait(DockerContainerWaitOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ContextCreate(DockerContextCreateOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ContextExport(DockerContextExportOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ContextImport(DockerContextImportOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ContextInspect(DockerContextInspectOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerContextInspectOptions(), token);
    }

    public async Task<CommandResult> ContextLs(DockerContextLsOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerContextLsOptions(), token);
    }

    public async Task<CommandResult> Context(DockerContextOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ContextRm(DockerContextRmOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ContextUpdate(DockerContextUpdateOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ContextUse(DockerContextUseOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Cp(DockerCpOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Create(DockerCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Diff(DockerDiffOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Events(DockerEventsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerEventsOptions(), token);
    }

    public async Task<CommandResult> Exec(DockerExecOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Export(DockerExportOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> History(DockerHistoryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ImageBuild(DockerImageBuildOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ImageHistory(DockerImageHistoryOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ImageImport(DockerImageImportOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerImageImportOptions(), token);
    }

    public async Task<CommandResult> ImageInspect(DockerImageInspectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ImageLoad(DockerImageLoadOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerImageLoadOptions(), token);
    }

    public async Task<CommandResult> ImageLs(DockerImageLsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerImageLsOptions(), token);
    }

    public async Task<CommandResult> Image(DockerImageOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ImagePrune(DockerImagePruneOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerImagePruneOptions(), token);
    }

    public async Task<CommandResult> ImagePull(DockerImagePullOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerImagePullOptions(), token);
    }

    public async Task<CommandResult> ImagePush(DockerImagePushOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerImagePushOptions(), token);
    }

    public async Task<CommandResult> ImageRm(DockerImageRmOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ImageSave(DockerImageSaveOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Images(DockerImagesOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerImagesOptions(), token);
    }

    public async Task<CommandResult> ImageTag(DockerImageTagOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerImageTagOptions(), token);
    }

    public async Task<CommandResult> Import(DockerImportOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerImportOptions(), token);
    }

    public async Task<CommandResult> Info(DockerInfoOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerInfoOptions(), token);
    }

    public async Task<CommandResult> Init(DockerInitOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerInitOptions(), token);
    }

    public async Task<CommandResult> Inspect(DockerInspectOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Kill(DockerKillOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Load(DockerLoadOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerLoadOptions(), token);
    }

    public async Task<CommandResult> Login(DockerLoginOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerLoginOptions(), token);
    }

    public async Task<CommandResult> Logout(DockerLogoutOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerLogoutOptions(), token);
    }

    public async Task<CommandResult> Logs(DockerLogsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ManifestAnnotate(DockerManifestAnnotateOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ManifestCreate(DockerManifestCreateOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ManifestInspect(DockerManifestInspectOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Manifest(DockerManifestOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ManifestPush(DockerManifestPushOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ManifestRm(DockerManifestRmOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> NetworkConnect(DockerNetworkConnectOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> NetworkCreate(DockerNetworkCreateOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> NetworkDisconnect(DockerNetworkDisconnectOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> NetworkInspect(DockerNetworkInspectOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> NetworkLs(DockerNetworkLsOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerNetworkLsOptions(), token);
    }

    public async Task<CommandResult> Network(DockerNetworkOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> NetworkPrune(DockerNetworkPruneOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerNetworkPruneOptions(), token);
    }

    public async Task<CommandResult> NetworkRm(DockerNetworkRmOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> NodeDemote(DockerNodeDemoteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> NodeInspect(DockerNodeInspectOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerNodeInspectOptions(), token);
    }

    public async Task<CommandResult> NodeLs(DockerNodeLsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerNodeLsOptions(), token);
    }

    public async Task<CommandResult> Node(DockerNodeOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> NodePromote(DockerNodePromoteOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> NodePs(DockerNodePsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerNodePsOptions(), token);
    }

    public async Task<CommandResult> NodeRm(DockerNodeRmOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> NodeUpdate(DockerNodeUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Pause(DockerPauseOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PluginCreate(DockerPluginCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PluginDisable(DockerPluginDisableOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PluginEnable(DockerPluginEnableOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PluginInspect(DockerPluginInspectOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PluginInstall(DockerPluginInstallOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PluginLs(DockerPluginLsOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerPluginLsOptions(), token);
    }

    public async Task<CommandResult> Plugin(DockerPluginOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PluginRm(DockerPluginRmOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PluginSet(DockerPluginSetOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> PluginUpgrade(DockerPluginUpgradeOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Port(DockerPortOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Ps(DockerPsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerPsOptions(), token);
    }

    public async Task<CommandResult> Pull(DockerPullOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerPullOptions(), token);
    }

    public async Task<CommandResult> Push(DockerPushOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerPushOptions(), token);
    }

    public async Task<CommandResult> Rename(DockerRenameOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Restart(DockerRestartOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Rmi(DockerRmiOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Rm(DockerRmOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Run(DockerRunOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Save(DockerSaveOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ScoutCompare(DockerScoutCompareOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ScoutCves(DockerScoutCvesOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerScoutCvesOptions(), token);
    }

    public async Task<CommandResult> ScoutEntitlement(DockerScoutEntitlementOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Scout(DockerScoutOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ScoutQuickview(DockerScoutQuickviewOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerScoutQuickviewOptions(), token);
    }

    public async Task<CommandResult> ScoutRecommendations(DockerScoutRecommendationsOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerScoutRecommendationsOptions(), token);
    }

    public async Task<CommandResult> ScoutRepoDisable(DockerScoutRepoDisableOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ScoutRepoEnable(DockerScoutRepoEnableOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ScoutRepoList(DockerScoutRepoListOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ScoutSbom(DockerScoutSbomOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerScoutSbomOptions(), token);
    }

    public async Task<CommandResult> ScoutStream(DockerScoutStreamOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Search(DockerSearchOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SecretCreate(DockerSecretCreateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SecretInspect(DockerSecretInspectOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SecretLs(DockerSecretLsOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerSecretLsOptions(), token);
    }

    public async Task<CommandResult> Secret(DockerSecretOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SecretRm(DockerSecretRmOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ServiceCreate(DockerServiceCreateOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ServiceInspect(DockerServiceInspectOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ServiceLogs(DockerServiceLogsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ServiceLs(DockerServiceLsOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerServiceLsOptions(), token);
    }

    public async Task<CommandResult> Service(DockerServiceOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ServicePs(DockerServicePsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ServiceRm(DockerServiceRmOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ServiceRollback(DockerServiceRollbackOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ServiceScale(DockerServiceScaleOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> ServiceUpdate(DockerServiceUpdateOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StackConfig(DockerStackConfigOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerStackConfigOptions(), token);
    }

    public async Task<CommandResult> StackDeploy(DockerStackDeployOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StackLs(DockerStackLsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerStackLsOptions(), token);
    }

    public async Task<CommandResult> Stack(DockerStackOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StackPs(DockerStackPsOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StackRm(DockerStackRmOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> StackServices(DockerStackServicesOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Start(DockerStartOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Stats(DockerStatsOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerStatsOptions(), token);
    }

    public async Task<CommandResult> Stop(DockerStopOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SwarmCa(DockerSwarmCaOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerSwarmCaOptions(), token);
    }

    public async Task<CommandResult> SwarmInit(DockerSwarmInitOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerSwarmInitOptions(), token);
    }

    public async Task<CommandResult> SwarmJoin(DockerSwarmJoinOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SwarmJoinToken(DockerSwarmJoinTokenOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerSwarmJoinTokenOptions(), token);
    }

    public async Task<CommandResult> SwarmLeave(DockerSwarmLeaveOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerSwarmLeaveOptions(), token);
    }

    public async Task<CommandResult> Swarm(DockerSwarmOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SwarmUnlockKey(DockerSwarmUnlockKeyOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerSwarmUnlockKeyOptions(), token);
    }

    public async Task<CommandResult> SwarmUpdate(DockerSwarmUpdateOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerSwarmUpdateOptions(), token);
    }

    public async Task<CommandResult> SystemDf(DockerSystemDfOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerSystemDfOptions(), token);
    }

    public async Task<CommandResult> SystemEvents(DockerSystemEventsOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerSystemEventsOptions(), token);
    }

    public async Task<CommandResult> SystemInfo(DockerSystemInfoOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerSystemInfoOptions(), token);
    }

    public async Task<CommandResult> System(DockerSystemOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> SystemPrune(DockerSystemPruneOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerSystemPruneOptions(), token);
    }

    public async Task<CommandResult> Tag(DockerTagOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerTagOptions(), token);
    }

    public async Task<CommandResult> Top(DockerTopOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TrustInspect(DockerTrustInspectOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerTrustInspectOptions(), token);
    }

    public async Task<CommandResult> TrustKeyGenerate(DockerTrustKeyGenerateOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TrustKeyLoad(DockerTrustKeyLoadOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TrustKey(DockerTrustKeyOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Trust(DockerTrustOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TrustRevoke(DockerTrustRevokeOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerTrustRevokeOptions(), token);
    }

    public async Task<CommandResult> TrustSignerAdd(DockerTrustSignerAddOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TrustSigner(DockerTrustSignerOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TrustSignerRemove(DockerTrustSignerRemoveOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> TrustSign(DockerTrustSignOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Unpause(DockerUnpauseOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Update(DockerUpdateOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> Version(DockerVersionOptions? options = default, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerVersionOptions(), token);
    }

    public async Task<CommandResult> VolumeCreate(DockerVolumeCreateOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerVolumeCreateOptions(), token);
    }

    public async Task<CommandResult> VolumeInspect(DockerVolumeInspectOptions options,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> VolumeLs(DockerVolumeLsOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerVolumeLsOptions(), token);
    }

    public async Task<CommandResult> VolumePrune(DockerVolumePruneOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerVolumePruneOptions(), token);
    }

    public async Task<CommandResult> VolumeRm(DockerVolumeRmOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }

    public async Task<CommandResult> VolumeUpdate(DockerVolumeUpdateOptions? options = default,
        CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options ?? new DockerVolumeUpdateOptions(), token);
    }

    public async Task<CommandResult> Wait(DockerWaitOptions options, CancellationToken token = default)
    {
        return await _command.ExecuteCommandLineTool(options, token);
    }
}
