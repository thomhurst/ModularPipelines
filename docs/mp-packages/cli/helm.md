# helm CLI reference

`ModularPipelines.Helm` provides strongly typed access to the `helm` CLI.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Helm
```

Import `ModularPipelines.Helm.Extensions`, then resolve the service with `context.Helm()`.

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines.Context;

using ModularPipelines.Models;

using ModularPipelines.Modules;

using ModularPipelines.Helm.Extensions;

using ModularPipelines.Helm.Options;



public class RunCommandModule : Module<CommandResult>

{

    protected override async Task<CommandResult?> ExecuteAsync(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        return await context.Helm().Env(

            new HelmEnvOptions(),

            cancellationToken: cancellationToken);

    }

}
```

## Commands[​](#commands "Direct link to Commands")

| CLI command              | Options record                |
| ------------------------ | ----------------------------- |
| `helm create`            | `HelmCreateOptions`           |
| `helm dependency build`  | `HelmDependencyBuildOptions`  |
| `helm dependency list`   | `HelmDependencyListOptions`   |
| `helm dependency update` | `HelmDependencyUpdateOptions` |
| `helm env`               | `HelmEnvOptions`              |
| `helm get all`           | `HelmGetAllOptions`           |
| `helm get hooks`         | `HelmGetHooksOptions`         |
| `helm get manifest`      | `HelmGetManifestOptions`      |
| `helm get metadata`      | `HelmGetMetadataOptions`      |
| `helm get notes`         | `HelmGetNotesOptions`         |
| `helm get values`        | `HelmGetValuesOptions`        |
| `helm history`           | `HelmHistoryOptions`          |
| `helm install`           | `HelmInstallOptions`          |
| `helm lint`              | `HelmLintOptions`             |
| `helm list`              | `HelmListOptions`             |
| `helm package`           | `HelmPackageOptions`          |
| `helm plugin install`    | `HelmPluginInstallOptions`    |
| `helm plugin list`       | `HelmPluginListOptions`       |
| `helm plugin uninstall`  | `HelmPluginUninstallOptions`  |
| `helm plugin update`     | `HelmPluginUpdateOptions`     |
| `helm pull`              | `HelmPullOptions`             |
| `helm push`              | `HelmPushOptions`             |
| `helm registry login`    | `HelmRegistryLoginOptions`    |
| `helm registry logout`   | `HelmRegistryLogoutOptions`   |
| `helm repo add`          | `HelmRepoAddOptions`          |
| `helm repo index`        | `HelmRepoIndexOptions`        |
| `helm repo list`         | `HelmRepoListOptions`         |
| `helm repo remove`       | `HelmRepoRemoveOptions`       |
| `helm repo update`       | `HelmRepoUpdateOptions`       |
| `helm rollback`          | `HelmRollbackOptions`         |
| `helm search hub`        | `HelmSearchHubOptions`        |
| `helm search repo`       | `HelmSearchRepoOptions`       |
| `helm show all`          | `HelmShowAllOptions`          |
| `helm show chart`        | `HelmShowChartOptions`        |
| `helm show crds`         | `HelmShowCrdsOptions`         |
| `helm show readme`       | `HelmShowReadmeOptions`       |
| `helm show values`       | `HelmShowValuesOptions`       |
| `helm status`            | `HelmStatusOptions`           |
| `helm template`          | `HelmTemplateOptions`         |
| `helm test`              | `HelmTestOptions`             |
| `helm uninstall`         | `HelmUninstallOptions`        |
| `helm upgrade`           | `HelmUpgradeOptions`          |
| `helm verify`            | `HelmVerifyOptions`           |
