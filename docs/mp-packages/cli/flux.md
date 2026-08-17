# flux CLI reference

`ModularPipelines.Flux` provides strongly typed access to the `flux` CLI.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Flux
```

Import `ModularPipelines.Flux.Extensions`, then resolve the service with `context.Flux()`.

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines.Context;

using ModularPipelines.Models;

using ModularPipelines.Modules;

using ModularPipelines.Flux.Extensions;

using ModularPipelines.Flux.Options;



public class RunCommandModule : Module<CommandResult>

{

    protected override async Task<CommandResult?> ExecuteAsync(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        return await context.Flux().Check(

            new FluxCheckOptions(),

            cancellationToken: cancellationToken);

    }

}
```

## Commands[​](#commands "Direct link to Commands")

| CLI command                       | Options record                        |
| --------------------------------- | ------------------------------------- |
| `flux bootstrap bitbucket-server` | `FluxBootstrapBitbucketServerOptions` |
| `flux bootstrap git`              | `FluxBootstrapGitOptions`             |
| `flux bootstrap gitea`            | `FluxBootstrapGiteaOptions`           |
| `flux bootstrap github`           | `FluxBootstrapGithubOptions`          |
| `flux bootstrap gitlab`           | `FluxBootstrapGitlabOptions`          |
| `flux build artifact`             | `FluxBuildArtifactOptions`            |
| `flux build kustomization`        | `FluxBuildKustomizationOptions`       |
| `flux check`                      | `FluxCheckOptions`                    |
| `flux create alert`               | `FluxCreateAlertOptions`              |
| `flux create alert-provider`      | `FluxCreateAlertProviderOptions`      |
| `flux create helmrelease`         | `FluxCreateHelmreleaseOptions`        |
| `flux create image`               | `FluxCreateImageOptions`              |
| `flux create image policy`        | `FluxCreateImagePolicyOptions`        |
| `flux create image repository`    | `FluxCreateImageRepositoryOptions`    |
| `flux create image update`        | `FluxCreateImageUpdateOptions`        |
| `flux create kustomization`       | `FluxCreateKustomizationOptions`      |
| `flux create receiver`            | `FluxCreateReceiverOptions`           |
| `flux create secret`              | `FluxCreateSecretOptions`             |
| `flux create secret git`          | `FluxCreateSecretGitOptions`          |
| `flux create secret githubapp`    | `FluxCreateSecretGithubappOptions`    |
| `flux create secret helm`         | `FluxCreateSecretHelmOptions`         |
| `flux create secret notation`     | `FluxCreateSecretNotationOptions`     |
| `flux create secret oci`          | `FluxCreateSecretOciOptions`          |
| `flux create secret proxy`        | `FluxCreateSecretProxyOptions`        |
| `flux create secret receiver`     | `FluxCreateSecretReceiverOptions`     |
| `flux create secret tls`          | `FluxCreateSecretTlsOptions`          |
| `flux create source`              | `FluxCreateSourceOptions`             |
| `flux create source bucket`       | `FluxCreateSourceBucketOptions`       |
| `flux create source chart`        | `FluxCreateSourceChartOptions`        |
| `flux create source git`          | `FluxCreateSourceGitOptions`          |
| `flux create source helm`         | `FluxCreateSourceHelmOptions`         |
| `flux create source oci`          | `FluxCreateSourceOciOptions`          |
| `flux create tenant`              | `FluxCreateTenantOptions`             |
| `flux debug helmrelease`          | `FluxDebugHelmreleaseOptions`         |
| `flux debug kustomization`        | `FluxDebugKustomizationOptions`       |
| `flux delete alert`               | `FluxDeleteAlertOptions`              |
| `flux delete alert-provider`      | `FluxDeleteAlertProviderOptions`      |
| `flux delete helmrelease`         | `FluxDeleteHelmreleaseOptions`        |
| `flux delete image`               | `FluxDeleteImageOptions`              |
| `flux delete image policy`        | `FluxDeleteImagePolicyOptions`        |
| `flux delete image repository`    | `FluxDeleteImageRepositoryOptions`    |
| `flux delete image update`        | `FluxDeleteImageUpdateOptions`        |
| `flux delete kustomization`       | `FluxDeleteKustomizationOptions`      |
| `flux delete receiver`            | `FluxDeleteReceiverOptions`           |
| `flux delete source`              | `FluxDeleteSourceOptions`             |
| `flux delete source bucket`       | `FluxDeleteSourceBucketOptions`       |
| `flux delete source chart`        | `FluxDeleteSourceChartOptions`        |
| `flux delete source git`          | `FluxDeleteSourceGitOptions`          |
| `flux delete source helm`         | `FluxDeleteSourceHelmOptions`         |
| `flux delete source oci`          | `FluxDeleteSourceOciOptions`          |
| `flux diff artifact`              | `FluxDiffArtifactOptions`             |
| `flux diff kustomization`         | `FluxDiffKustomizationOptions`        |
| `flux envsubst`                   | `FluxEnvsubstOptions`                 |
| `flux events`                     | `FluxEventsOptions`                   |
| `flux export alert`               | `FluxExportAlertOptions`              |
| `flux export alert-provider`      | `FluxExportAlertProviderOptions`      |
| `flux export artifact`            | `FluxExportArtifactOptions`           |
| `flux export artifact generator`  | `FluxExportArtifactGeneratorOptions`  |
| `flux export helmrelease`         | `FluxExportHelmreleaseOptions`        |
| `flux export image`               | `FluxExportImageOptions`              |
| `flux export image policy`        | `FluxExportImagePolicyOptions`        |
| `flux export image repository`    | `FluxExportImageRepositoryOptions`    |
| `flux export image update`        | `FluxExportImageUpdateOptions`        |
| `flux export kustomization`       | `FluxExportKustomizationOptions`      |
| `flux export receiver`            | `FluxExportReceiverOptions`           |
| `flux export source`              | `FluxExportSourceOptions`             |
| `flux export source bucket`       | `FluxExportSourceBucketOptions`       |
| `flux export source chart`        | `FluxExportSourceChartOptions`        |
| `flux export source external`     | `FluxExportSourceExternalOptions`     |
| `flux export source git`          | `FluxExportSourceGitOptions`          |
| `flux export source helm`         | `FluxExportSourceHelmOptions`         |
| `flux export source oci`          | `FluxExportSourceOciOptions`          |
| `flux get alert-providers`        | `FluxGetAlertProvidersOptions`        |
| `flux get alerts`                 | `FluxGetAlertsOptions`                |
| `flux get all`                    | `FluxGetAllOptions`                   |
| `flux get artifacts`              | `FluxGetArtifactsOptions`             |
| `flux get artifacts generators`   | `FluxGetArtifactsGeneratorsOptions`   |
| `flux get helmreleases`           | `FluxGetHelmreleasesOptions`          |
| `flux get images`                 | `FluxGetImagesOptions`                |
| `flux get images all`             | `FluxGetImagesAllOptions`             |
| `flux get images policy`          | `FluxGetImagesPolicyOptions`          |
| `flux get images repository`      | `FluxGetImagesRepositoryOptions`      |
| `flux get images update`          | `FluxGetImagesUpdateOptions`          |
| `flux get kustomizations`         | `FluxGetKustomizationsOptions`        |
| `flux get receivers`              | `FluxGetReceiversOptions`             |
| `flux get sources`                | `FluxGetSourcesOptions`               |
| `flux get sources all`            | `FluxGetSourcesAllOptions`            |
| `flux get sources bucket`         | `FluxGetSourcesBucketOptions`         |
| `flux get sources chart`          | `FluxGetSourcesChartOptions`          |
| `flux get sources external`       | `FluxGetSourcesExternalOptions`       |
| `flux get sources git`            | `FluxGetSourcesGitOptions`            |
| `flux get sources helm`           | `FluxGetSourcesHelmOptions`           |
| `flux get sources oci`            | `FluxGetSourcesOciOptions`            |
| `flux install`                    | `FluxInstallOptions`                  |
| `flux list artifacts`             | `FluxListArtifactsOptions`            |
| `flux logs`                       | `FluxLogsOptions`                     |
| `flux migrate`                    | `FluxMigrateOptions`                  |
| `flux plugin install`             | `FluxPluginInstallOptions`            |
| `flux plugin list`                | `FluxPluginListOptions`               |
| `flux plugin search`              | `FluxPluginSearchOptions`             |
| `flux plugin uninstall`           | `FluxPluginUninstallOptions`          |
| `flux plugin update`              | `FluxPluginUpdateOptions`             |
| `flux pull artifact`              | `FluxPullArtifactOptions`             |
| `flux push artifact`              | `FluxPushArtifactOptions`             |
| `flux reconcile helmrelease`      | `FluxReconcileHelmreleaseOptions`     |
| `flux reconcile image`            | `FluxReconcileImageOptions`           |
| `flux reconcile image policy`     | `FluxReconcileImagePolicyOptions`     |
| `flux reconcile image repository` | `FluxReconcileImageRepositoryOptions` |
| `flux reconcile image update`     | `FluxReconcileImageUpdateOptions`     |
| `flux reconcile kustomization`    | `FluxReconcileKustomizationOptions`   |
| `flux reconcile receiver`         | `FluxReconcileReceiverOptions`        |
| `flux reconcile source`           | `FluxReconcileSourceOptions`          |
| `flux reconcile source bucket`    | `FluxReconcileSourceBucketOptions`    |
| `flux reconcile source chart`     | `FluxReconcileSourceChartOptions`     |
| `flux reconcile source git`       | `FluxReconcileSourceGitOptions`       |
| `flux reconcile source helm`      | `FluxReconcileSourceHelmOptions`      |
| `flux reconcile source oci`       | `FluxReconcileSourceOciOptions`       |
| `flux resume alert`               | `FluxResumeAlertOptions`              |
| `flux resume alert-provider`      | `FluxResumeAlertProviderOptions`      |
| `flux resume helmrelease`         | `FluxResumeHelmreleaseOptions`        |
| `flux resume image`               | `FluxResumeImageOptions`              |
| `flux resume image policy`        | `FluxResumeImagePolicyOptions`        |
| `flux resume image repository`    | `FluxResumeImageRepositoryOptions`    |
| `flux resume image update`        | `FluxResumeImageUpdateOptions`        |
| `flux resume kustomization`       | `FluxResumeKustomizationOptions`      |
| `flux resume receiver`            | `FluxResumeReceiverOptions`           |
| `flux resume source`              | `FluxResumeSourceOptions`             |
| `flux resume source bucket`       | `FluxResumeSourceBucketOptions`       |
| `flux resume source chart`        | `FluxResumeSourceChartOptions`        |
| `flux resume source git`          | `FluxResumeSourceGitOptions`          |
| `flux resume source helm`         | `FluxResumeSourceHelmOptions`         |
| `flux resume source oci`          | `FluxResumeSourceOciOptions`          |
| `flux stats`                      | `FluxStatsOptions`                    |
| `flux suspend alert`              | `FluxSuspendAlertOptions`             |
| `flux suspend alert-provider`     | `FluxSuspendAlertProviderOptions`     |
| `flux suspend helmrelease`        | `FluxSuspendHelmreleaseOptions`       |
| `flux suspend image`              | `FluxSuspendImageOptions`             |
| `flux suspend image policy`       | `FluxSuspendImagePolicyOptions`       |
| `flux suspend image repository`   | `FluxSuspendImageRepositoryOptions`   |
| `flux suspend image update`       | `FluxSuspendImageUpdateOptions`       |
| `flux suspend kustomization`      | `FluxSuspendKustomizationOptions`     |
| `flux suspend receiver`           | `FluxSuspendReceiverOptions`          |
| `flux suspend source`             | `FluxSuspendSourceOptions`            |
| `flux suspend source bucket`      | `FluxSuspendSourceBucketOptions`      |
| `flux suspend source chart`       | `FluxSuspendSourceChartOptions`       |
| `flux suspend source git`         | `FluxSuspendSourceGitOptions`         |
| `flux suspend source helm`        | `FluxSuspendSourceHelmOptions`        |
| `flux suspend source oci`         | `FluxSuspendSourceOciOptions`         |
| `flux tag artifact`               | `FluxTagArtifactOptions`              |
| `flux trace`                      | `FluxTraceOptions`                    |
| `flux tree artifact`              | `FluxTreeArtifactOptions`             |
| `flux tree artifact generator`    | `FluxTreeArtifactGeneratorOptions`    |
| `flux tree kustomization`         | `FluxTreeKustomizationOptions`        |
| `flux trigger receiver`           | `FluxTriggerReceiverOptions`          |
| `flux uninstall`                  | `FluxUninstallOptions`                |
