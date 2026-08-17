# helm CLI reference

`ModularPipelines.Helm` provides strongly typed access to the `helm` CLI.

## Executable prerequisite[​](#executable-prerequisite "Direct link to Executable prerequisite")

This package does not install the `helm` executable. Install it separately and ensure `helm` is available on `PATH`.

Follow the executable's official documentation for installation instructions.

## Package installation[​](#package-installation "Direct link to Package installation")

```
dotnet add package ModularPipelines.Helm
```

Resolve the service with `context.Tools.Helm`. For projects older than C# 14, import `ModularPipelines.Helm.Extensions` and use the `context.Helm()` extension method as a compatibility fallback.

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines.Context;

using ModularPipelines.Models;

using ModularPipelines.Modules;

using ModularPipelines.Helm.Options;



public class RunCommandModule : Module<CommandResult>

{

    protected override async Task<CommandResult> ExecuteAsync(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        return await context.Tools.Helm.EnvAsync(

            new HelmEnvOptions(),

            cancellationToken: cancellationToken);

    }

}
```

## Commands[​](#commands "Direct link to Commands")

| CLI command              | Options record                |
| ------------------------ | ----------------------------- |
| `helm create`            | `HelmCreateOptions`           |
| `helm dependency`        | `HelmDependencyOptions`       |
| `helm dependency build`  | `HelmDependencyBuildOptions`  |
| `helm dependency list`   | `HelmDependencyListOptions`   |
| `helm dependency update` | `HelmDependencyUpdateOptions` |
| `helm env`               | `HelmEnvOptions`              |
| `helm get`               | `HelmGetOptions`              |
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
| `helm plugin`            | `HelmPluginOptions`           |
| `helm plugin install`    | `HelmPluginInstallOptions`    |
| `helm plugin list`       | `HelmPluginListOptions`       |
| `helm plugin uninstall`  | `HelmPluginUninstallOptions`  |
| `helm plugin update`     | `HelmPluginUpdateOptions`     |
| `helm pull`              | `HelmPullOptions`             |
| `helm push`              | `HelmPushOptions`             |
| `helm registry`          | `HelmRegistryOptions`         |
| `helm registry login`    | `HelmRegistryLoginOptions`    |
| `helm registry logout`   | `HelmRegistryLogoutOptions`   |
| `helm repo`              | `HelmRepoOptions`             |
| `helm repo add`          | `HelmRepoAddOptions`          |
| `helm repo index`        | `HelmRepoIndexOptions`        |
| `helm repo list`         | `HelmRepoListOptions`         |
| `helm repo remove`       | `HelmRepoRemoveOptions`       |
| `helm repo update`       | `HelmRepoUpdateOptions`       |
| `helm rollback`          | `HelmRollbackOptions`         |
| `helm search`            | `HelmSearchOptions`           |
| `helm search hub`        | `HelmSearchHubOptions`        |
| `helm search repo`       | `HelmSearchRepoOptions`       |
| `helm show`              | `HelmShowOptions`             |
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
