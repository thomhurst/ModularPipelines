# podman CLI reference

`ModularPipelines.Podman` provides strongly typed access to the `podman` CLI.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Podman
```

Import `ModularPipelines.Podman.Extensions`, then resolve the service with `context.Podman()`.

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines.Context;

using ModularPipelines.Models;

using ModularPipelines.Modules;

using ModularPipelines.Podman.Extensions;

using ModularPipelines.Podman.Options;



public class RunCommandModule : Module<CommandResult>

{

    protected override async Task<CommandResult?> ExecuteAsync(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        return await context.Podman().Attach(

            new PodmanAttachOptions(),

            cancellationToken: cancellationToken);

    }

}
```

## Commands[​](#commands "Direct link to Commands")

| CLI command                                    | Options record                                    |
| ---------------------------------------------- | ------------------------------------------------- |
| `podman attach`                                | `PodmanAttachOptions`                             |
| `podman auto-update`                           | `PodmanAutoUpdateOptions`                         |
| `podman build`                                 | `PodmanBuildOptions`                              |
| `podman commit`                                | `PodmanCommitOptions`                             |
| `podman compose attach`                        | `PodmanComposeAttachOptions`                      |
| `podman compose bridge`                        | `PodmanComposeBridgeOptions`                      |
| `podman compose bridge convert`                | `PodmanComposeBridgeConvertOptions`               |
| `podman compose bridge transformations`        | `PodmanComposeBridgeTransformationsOptions`       |
| `podman compose bridge transformations create` | `PodmanComposeBridgeTransformationsCreateOptions` |
| `podman compose bridge transformations list`   | `PodmanComposeBridgeTransformationsListOptions`   |
| `podman compose build`                         | `PodmanComposeBuildOptions`                       |
| `podman compose commit`                        | `PodmanComposeCommitOptions`                      |
| `podman compose config`                        | `PodmanComposeConfigOptions`                      |
| `podman compose cp`                            | `PodmanComposeCpOptions`                          |
| `podman compose create`                        | `PodmanComposeCreateOptions`                      |
| `podman compose down`                          | `PodmanComposeDownOptions`                        |
| `podman compose events`                        | `PodmanComposeEventsOptions`                      |
| `podman compose exec`                          | `PodmanComposeExecOptions`                        |
| `podman compose export`                        | `PodmanComposeExportOptions`                      |
| `podman compose images`                        | `PodmanComposeImagesOptions`                      |
| `podman compose kill`                          | `PodmanComposeKillOptions`                        |
| `podman compose logs`                          | `PodmanComposeLogsOptions`                        |
| `podman compose ls`                            | `PodmanComposeLsOptions`                          |
| `podman compose pause`                         | `PodmanComposePauseOptions`                       |
| `podman compose port`                          | `PodmanComposePortOptions`                        |
| `podman compose ps`                            | `PodmanComposePsOptions`                          |
| `podman compose publish`                       | `PodmanComposePublishOptions`                     |
| `podman compose pull`                          | `PodmanComposePullOptions`                        |
| `podman compose push`                          | `PodmanComposePushOptions`                        |
| `podman compose restart`                       | `PodmanComposeRestartOptions`                     |
| `podman compose rm`                            | `PodmanComposeRmOptions`                          |
| `podman compose run`                           | `PodmanComposeRunOptions`                         |
| `podman compose scale`                         | `PodmanComposeScaleOptions`                       |
| `podman compose start`                         | `PodmanComposeStartOptions`                       |
| `podman compose stats`                         | `PodmanComposeStatsOptions`                       |
| `podman compose stop`                          | `PodmanComposeStopOptions`                        |
| `podman compose top`                           | `PodmanComposeTopOptions`                         |
| `podman compose unpause`                       | `PodmanComposeUnpauseOptions`                     |
| `podman compose up`                            | `PodmanComposeUpOptions`                          |
| `podman compose volumes`                       | `PodmanComposeVolumesOptions`                     |
| `podman compose wait`                          | `PodmanComposeWaitOptions`                        |
| `podman compose watch`                         | `PodmanComposeWatchOptions`                       |
| `podman container attach`                      | `PodmanContainerAttachOptions`                    |
| `podman container checkpoint`                  | `PodmanContainerCheckpointOptions`                |
| `podman container cleanup`                     | `PodmanContainerCleanupOptions`                   |
| `podman container clone`                       | `PodmanContainerCloneOptions`                     |
| `podman container commit`                      | `PodmanContainerCommitOptions`                    |
| `podman container cp`                          | `PodmanContainerCpOptions`                        |
| `podman container create`                      | `PodmanContainerCreateOptions`                    |
| `podman container diff`                        | `PodmanContainerDiffOptions`                      |
| `podman container exec`                        | `PodmanContainerExecOptions`                      |
| `podman container exists`                      | `PodmanContainerExistsOptions`                    |
| `podman container export`                      | `PodmanContainerExportOptions`                    |
| `podman container init`                        | `PodmanContainerInitOptions`                      |
| `podman container inspect`                     | `PodmanContainerInspectOptions`                   |
| `podman container kill`                        | `PodmanContainerKillOptions`                      |
| `podman container list`                        | `PodmanContainerListOptions`                      |
| `podman container logs`                        | `PodmanContainerLogsOptions`                      |
| `podman container mount`                       | `PodmanContainerMountOptions`                     |
| `podman container pause`                       | `PodmanContainerPauseOptions`                     |
| `podman container port`                        | `PodmanContainerPortOptions`                      |
| `podman container prune`                       | `PodmanContainerPruneOptions`                     |
| `podman container ps`                          | `PodmanContainerPsOptions`                        |
| `podman container restart`                     | `PodmanContainerRestartOptions`                   |
| `podman container restore`                     | `PodmanContainerRestoreOptions`                   |
| `podman container rm`                          | `PodmanContainerRmOptions`                        |
| `podman container run`                         | `PodmanContainerRunOptions`                       |
| `podman container runlabel`                    | `PodmanContainerRunlabelOptions`                  |
| `podman container start`                       | `PodmanContainerStartOptions`                     |
| `podman container stats`                       | `PodmanContainerStatsOptions`                     |
| `podman container stop`                        | `PodmanContainerStopOptions`                      |
| `podman container top`                         | `PodmanContainerTopOptions`                       |
| `podman container unmount`                     | `PodmanContainerUnmountOptions`                   |
| `podman container unpause`                     | `PodmanContainerUnpauseOptions`                   |
| `podman container update`                      | `PodmanContainerUpdateOptions`                    |
| `podman container wait`                        | `PodmanContainerWaitOptions`                      |
| `podman cp`                                    | `PodmanCpOptions`                                 |
| `podman create`                                | `PodmanCreateOptions`                             |
| `podman diff`                                  | `PodmanDiffOptions`                               |
| `podman events`                                | `PodmanEventsOptions`                             |
| `podman exec`                                  | `PodmanExecOptions`                               |
| `podman export`                                | `PodmanExportOptions`                             |
| `podman farm build`                            | `PodmanFarmBuildOptions`                          |
| `podman farm create`                           | `PodmanFarmCreateOptions`                         |
| `podman farm list`                             | `PodmanFarmListOptions`                           |
| `podman farm remove`                           | `PodmanFarmRemoveOptions`                         |
| `podman farm update`                           | `PodmanFarmUpdateOptions`                         |
| `podman generate kube`                         | `PodmanGenerateKubeOptions`                       |
| `podman generate spec`                         | `PodmanGenerateSpecOptions`                       |
| `podman generate systemd`                      | `PodmanGenerateSystemdOptions`                    |
| `podman history`                               | `PodmanHistoryOptions`                            |
| `podman image build`                           | `PodmanImageBuildOptions`                         |
| `podman image diff`                            | `PodmanImageDiffOptions`                          |
| `podman image history`                         | `PodmanImageHistoryOptions`                       |
| `podman image import`                          | `PodmanImageImportOptions`                        |
| `podman image inspect`                         | `PodmanImageInspectOptions`                       |
| `podman image list`                            | `PodmanImageListOptions`                          |
| `podman image load`                            | `PodmanImageLoadOptions`                          |
| `podman image mount`                           | `PodmanImageMountOptions`                         |
| `podman image prune`                           | `PodmanImagePruneOptions`                         |
| `podman image pull`                            | `PodmanImagePullOptions`                          |
| `podman image push`                            | `PodmanImagePushOptions`                          |
| `podman image rm`                              | `PodmanImageRmOptions`                            |
| `podman image save`                            | `PodmanImageSaveOptions`                          |
| `podman image scp`                             | `PodmanImageScpOptions`                           |
| `podman image search`                          | `PodmanImageSearchOptions`                        |
| `podman image sign`                            | `PodmanImageSignOptions`                          |
| `podman image tree`                            | `PodmanImageTreeOptions`                          |
| `podman image trust set`                       | `PodmanImageTrustSetOptions`                      |
| `podman image trust show`                      | `PodmanImageTrustShowOptions`                     |
| `podman image unmount`                         | `PodmanImageUnmountOptions`                       |
| `podman images`                                | `PodmanImagesOptions`                             |
| `podman import`                                | `PodmanImportOptions`                             |
| `podman init`                                  | `PodmanInitOptions`                               |
| `podman inspect`                               | `PodmanInspectOptions`                            |
| `podman kill`                                  | `PodmanKillOptions`                               |
| `podman kube apply`                            | `PodmanKubeApplyOptions`                          |
| `podman kube down`                             | `PodmanKubeDownOptions`                           |
| `podman kube generate`                         | `PodmanKubeGenerateOptions`                       |
| `podman kube play`                             | `PodmanKubePlayOptions`                           |
| `podman load`                                  | `PodmanLoadOptions`                               |
| `podman login`                                 | `PodmanLoginOptions`                              |
| `podman logout`                                | `PodmanLogoutOptions`                             |
| `podman logs`                                  | `PodmanLogsOptions`                               |
| `podman machine init`                          | `PodmanMachineInitOptions`                        |
| `podman machine inspect`                       | `PodmanMachineInspectOptions`                     |
| `podman machine list`                          | `PodmanMachineListOptions`                        |
| `podman machine os apply`                      | `PodmanMachineOsApplyOptions`                     |
| `podman machine rm`                            | `PodmanMachineRmOptions`                          |
| `podman machine set`                           | `PodmanMachineSetOptions`                         |
| `podman machine ssh`                           | `PodmanMachineSshOptions`                         |
| `podman machine start`                         | `PodmanMachineStartOptions`                       |
| `podman manifest add`                          | `PodmanManifestAddOptions`                        |
| `podman manifest annotate`                     | `PodmanManifestAnnotateOptions`                   |
| `podman manifest create`                       | `PodmanManifestCreateOptions`                     |
| `podman manifest inspect`                      | `PodmanManifestInspectOptions`                    |
| `podman manifest push`                         | `PodmanManifestPushOptions`                       |
| `podman mount`                                 | `PodmanMountOptions`                              |
| `podman network connect`                       | `PodmanNetworkConnectOptions`                     |
| `podman network create`                        | `PodmanNetworkCreateOptions`                      |
| `podman network disconnect`                    | `PodmanNetworkDisconnectOptions`                  |
| `podman network inspect`                       | `PodmanNetworkInspectOptions`                     |
| `podman network ls`                            | `PodmanNetworkLsOptions`                          |
| `podman network prune`                         | `PodmanNetworkPruneOptions`                       |
| `podman network reload`                        | `PodmanNetworkReloadOptions`                      |
| `podman network rm`                            | `PodmanNetworkRmOptions`                          |
| `podman network update`                        | `PodmanNetworkUpdateOptions`                      |
| `podman pause`                                 | `PodmanPauseOptions`                              |
| `podman pod clone`                             | `PodmanPodCloneOptions`                           |
| `podman pod create`                            | `PodmanPodCreateOptions`                          |
| `podman pod exists`                            | `PodmanPodExistsOptions`                          |
| `podman pod inspect`                           | `PodmanPodInspectOptions`                         |
| `podman pod kill`                              | `PodmanPodKillOptions`                            |
| `podman pod logs`                              | `PodmanPodLogsOptions`                            |
| `podman pod pause`                             | `PodmanPodPauseOptions`                           |
| `podman pod prune`                             | `PodmanPodPruneOptions`                           |
| `podman pod ps`                                | `PodmanPodPsOptions`                              |
| `podman pod restart`                           | `PodmanPodRestartOptions`                         |
| `podman pod rm`                                | `PodmanPodRmOptions`                              |
| `podman pod start`                             | `PodmanPodStartOptions`                           |
| `podman pod stats`                             | `PodmanPodStatsOptions`                           |
| `podman pod stop`                              | `PodmanPodStopOptions`                            |
| `podman pod top`                               | `PodmanPodTopOptions`                             |
| `podman pod unpause`                           | `PodmanPodUnpauseOptions`                         |
| `podman port`                                  | `PodmanPortOptions`                               |
| `podman ps`                                    | `PodmanPsOptions`                                 |
| `podman pull`                                  | `PodmanPullOptions`                               |
| `podman push`                                  | `PodmanPushOptions`                               |
| `podman restart`                               | `PodmanRestartOptions`                            |
| `podman rm`                                    | `PodmanRmOptions`                                 |
| `podman rmi`                                   | `PodmanRmiOptions`                                |
| `podman run`                                   | `PodmanRunOptions`                                |
| `podman save`                                  | `PodmanSaveOptions`                               |
| `podman search`                                | `PodmanSearchOptions`                             |
| `podman secret create`                         | `PodmanSecretCreateOptions`                       |
| `podman secret inspect`                        | `PodmanSecretInspectOptions`                      |
| `podman secret ls`                             | `PodmanSecretLsOptions`                           |
| `podman secret rm`                             | `PodmanSecretRmOptions`                           |
| `podman start`                                 | `PodmanStartOptions`                              |
| `podman stats`                                 | `PodmanStatsOptions`                              |
| `podman stop`                                  | `PodmanStopOptions`                               |
| `podman system connection add`                 | `PodmanSystemConnectionAddOptions`                |
| `podman system connection list`                | `PodmanSystemConnectionListOptions`               |
| `podman system connection remove`              | `PodmanSystemConnectionRemoveOptions`             |
| `podman system df`                             | `PodmanSystemDfOptions`                           |
| `podman system events`                         | `PodmanSystemEventsOptions`                       |
| `podman system migrate`                        | `PodmanSystemMigrateOptions`                      |
| `podman system prune`                          | `PodmanSystemPruneOptions`                        |
| `podman system reset`                          | `PodmanSystemResetOptions`                        |
| `podman system service`                        | `PodmanSystemServiceOptions`                      |
| `podman top`                                   | `PodmanTopOptions`                                |
| `podman unmount`                               | `PodmanUnmountOptions`                            |
| `podman unpause`                               | `PodmanUnpauseOptions`                            |
| `podman unshare`                               | `PodmanUnshareOptions`                            |
| `podman update`                                | `PodmanUpdateOptions`                             |
| `podman volume create`                         | `PodmanVolumeCreateOptions`                       |
| `podman volume export`                         | `PodmanVolumeExportOptions`                       |
| `podman volume inspect`                        | `PodmanVolumeInspectOptions`                      |
| `podman volume ls`                             | `PodmanVolumeLsOptions`                           |
| `podman volume prune`                          | `PodmanVolumePruneOptions`                        |
| `podman volume rm`                             | `PodmanVolumeRmOptions`                           |
| `podman wait`                                  | `PodmanWaitOptions`                               |
