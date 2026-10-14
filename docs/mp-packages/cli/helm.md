# helm CLI reference

`ModularPipelines.Helm` provides strongly typed access to the `helm` CLI.

## Executable prerequisite[​](#executable-prerequisite "Direct link to Executable prerequisite")

This package does not install the `helm` executable. Install it separately and ensure `helm` is available on `PATH`.

Follow the executable's official documentation for installation instructions.

## Package installation[​](#package-installation "Direct link to Package installation")

```
dotnet add package ModularPipelines.Helm
```

Resolve the service with `context.Tools.Helm`. Projects using C# 13 or another .NET language can use `context.Tools.Get<ModularPipelines.Helm.Services.IHelm>()` instead.

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines;

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

## Global options[​](#global-options "Direct link to Global options")

Global options are rendered after the selected subcommand.

| CLI option                        | Property                    | Availability | Description                                                                                                                                 |
| --------------------------------- | --------------------------- | ------------ | ------------------------------------------------------------------------------------------------------------------------------------------- |
| `--burst-limit`                   | `BurstLimit`                | All editions | client-side default throttling limit (default 100)                                                                                          |
| `--debug`                         | `Debug`                     | All editions | enable verbose output                                                                                                                       |
| `--kube-apiserver`                | `KubeApiServer`             | All editions | the address and the port for the Kubernetes API server                                                                                      |
| `--kube-as-group`                 | `KubeAsGroup`               | All editions | group to impersonate for the operation, this flag can be repeated to specify multiple groups.                                               |
| `--kube-as-user`                  | `KubeAsUser`                | All editions | username to impersonate for the operation                                                                                                   |
| `--kube-ca-file`                  | `KubeCaFile`                | All editions | the certificate authority file for the Kubernetes API server connection                                                                     |
| `--kube-context`                  | `KubeContext`               | All editions | name of the kubeconfig context to use                                                                                                       |
| `--kube-insecure-skip-tls-verify` | `KubeInsecureSkipTlsVerify` | All editions | if true, the Kubernetes API server's certificate will not be checked for validity. This will make your HTTPS connections insecure           |
| `--kube-tls-server-name`          | `KubeTlsServerName`         | All editions | server name to use for Kubernetes API server certificate validation. If it is not provided, the hostname used to contact the server is used |
| `--kube-token`                    | `KubeToken`                 | All editions | bearer token used for authentication                                                                                                        |
| `--kubeconfig`                    | `KubeConfig`                | All editions | path to the kubeconfig file                                                                                                                 |
| `--namespace`                     | `Namespace`                 | All editions | namespace scope for this request                                                                                                            |
| `--qps`                           | `Qps`                       | All editions | queries per second used when communicating with the Kubernetes API, not including bursting                                                  |
| `--registry-config`               | `RegistryConfig`            | All editions | path to the registry config file                                                                                                            |
| `--repository-cache`              | `RepositoryCache`           | All editions | path to the directory containing cached repository indexes                                                                                  |
| `--repository-config`             | `RepositoryConfig`          | All editions | path to the file containing repository names and URLs                                                                                       |

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
| `helm version`           | `HelmVersionOptions`          |
