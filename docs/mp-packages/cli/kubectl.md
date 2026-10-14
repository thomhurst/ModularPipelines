# kubectl CLI reference

`ModularPipelines.Kubernetes` provides strongly typed access to the `kubectl` CLI.

## Executable prerequisite[​](#executable-prerequisite "Direct link to Executable prerequisite")

This package does not install the `kubectl` executable. Install it separately and ensure `kubectl` is available on `PATH`.

Follow the executable's official documentation for installation instructions.

## Package installation[​](#package-installation "Direct link to Package installation")

```
dotnet add package ModularPipelines.Kubernetes
```

Resolve the service with `context.Tools.Kubernetes`. Projects using C# 13 or another .NET language can use `context.Tools.Get<ModularPipelines.Kubernetes.Services.IKubernetes>()` instead.

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines;

using ModularPipelines.Kubernetes.Enums;

using ModularPipelines.Kubernetes.Options;



public class RunCommandModule : Module<CommandResult>

{

    protected override async Task<CommandResult> ExecuteAsync(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        return await context.Tools.Kubernetes.Config.ViewAsync(

            new KubernetesConfigViewOptions(),

            cancellationToken: cancellationToken);

    }

}
```

## Global options[​](#global-options "Direct link to Global options")

Global options are rendered after the selected subcommand.

| CLI option                   | Property                | Availability | Description                                                                                                                                                                                       |
| ---------------------------- | ----------------------- | ------------ | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `--as`                       | `As`                    | All editions | Username to impersonate for the operation. User could be a regular user or a service account in a namespace.                                                                                      |
| `--as-group`                 | `AsGroup`               | All editions | Group to impersonate for the operation, this flag can be repeated to specify multiple groups.                                                                                                     |
| `--as-uid`                   | `AsUid`                 | All editions | UID to impersonate for the operation.                                                                                                                                                             |
| `--as-user-extra`            | `AsUserExtra`           | All editions | User extras to impersonate for the operation, this flag can be repeated to specify multiple values for the same key.                                                                              |
| `--cache-dir`                | `CacheDir`              | All editions | Default cache directory.                                                                                                                                                                          |
| `--certificate-authority`    | `CertificateAuthority`  | All editions | Path to a cert file for the certificate authority                                                                                                                                                 |
| `--client-certificate`       | `ClientCertificate`     | All editions | Path to a client certificate file for TLS                                                                                                                                                         |
| `--client-key`               | `ClientKey`             | All editions | Path to a client key file for TLS                                                                                                                                                                 |
| `--cluster`                  | `Cluster`               | All editions | The name of the kubeconfig cluster to use                                                                                                                                                         |
| `--context`                  | `Context`               | All editions | The name of the kubeconfig context to use                                                                                                                                                         |
| `--disable-compression`      | `DisableCompression`    | All editions | If true, opt-out of response compression for all requests to the server                                                                                                                           |
| `--insecure-skip-tls-verify` | `InsecureSkipTlsVerify` | All editions | If true, the server's certificate will not be checked for validity. This will make your HTTPS connections insecure                                                                                |
| `--kubeconfig`               | `KubeConfig`            | All editions | Path to the kubeconfig file to use for CLI requests.                                                                                                                                              |
| `--kuberc`                   | `Kuberc`                | All editions | Path to the kuberc file to use for preferences. This can be disabled by exporting KUBECTL\_KUBERC=false feature gate or turning off the feature KUBERC=off.                                       |
| `--log-flush-frequency`      | `LogFlushFrequency`     | All editions | Maximum number of seconds between log flushes                                                                                                                                                     |
| `--match-server-version`     | `MatchServerVersion`    | All editions | Require server version to match client version                                                                                                                                                    |
| `--namespace`                | `Namespace`             | All editions | If present, the namespace scope for this CLI request                                                                                                                                              |
| `--password`                 | `Password`              | All editions | Password for basic authentication to the API server                                                                                                                                               |
| `--profile`                  | `Profile`               | All editions | Name of profile to capture. One of (none\|cpu\|heap\|goroutine\|threadcreate\|block\|mutex\|trace)                                                                                                |
| `--profile-output`           | `ProfileOutput`         | All editions | Name of the file to write the profile to                                                                                                                                                          |
| `--proxy-url`                | `ProxyUrl`              | All editions | Proxy URL to use for requests to the API server                                                                                                                                                   |
| `--request-timeout`          | `RequestTimeout`        | All editions | The length of time to wait before giving up on a single server request. Non-zero values should contain a corresponding time unit (e.g. 1s, 2m, 3h). A value of zero means don't timeout requests. |
| `--server`                   | `Server`                | All editions | The address and port of the Kubernetes API server                                                                                                                                                 |
| `--tls-server-name`          | `TlsServerName`         | All editions | Server name to use for server certificate validation. If it is not provided, the hostname used to contact the server is used                                                                      |
| `--token`                    | `Token`                 | All editions | Bearer token for authentication to the API server                                                                                                                                                 |
| `--user`                     | `User`                  | All editions | The name of the kubeconfig user to use                                                                                                                                                            |
| `--username`                 | `Username`              | All editions | Username for basic authentication to the API server                                                                                                                                               |
| `--v`                        | `V`                     | All editions | number for the log level verbosity                                                                                                                                                                |
| `--vmodule`                  | `Vmodule`               | All editions | comma-separated list of pattern=N settings for file-filtered logging (only works for the default text log format)                                                                                 |
| `--warnings-as-errors`       | `WarningsAsErrors`      | All editions | Treat warnings received from the server as errors and exit with a non-zero exit code                                                                                                              |

## Commands[​](#commands "Direct link to Commands")

| CLI command                       | Options record                          |
| --------------------------------- | --------------------------------------- |
| `kubectl annotate`                | `KubernetesAnnotateOptions`             |
| `kubectl apply`                   | `KubernetesApplyOptions`                |
| `kubectl apply edit-last-applied` | `KubernetesApplyEditLastAppliedOptions` |
| `kubectl apply set-last-applied`  | `KubernetesApplySetLastAppliedOptions`  |
| `kubectl apply view-last-applied` | `KubernetesApplyViewLastAppliedOptions` |
| `kubectl attach`                  | `KubernetesAttachOptions`               |
| `kubectl auth`                    | `KubernetesAuthOptions`                 |
| `kubectl auth can-i`              | `KubernetesAuthCanIOptions`             |
| `kubectl auth reconcile`          | `KubernetesAuthReconcileOptions`        |
| `kubectl auth whoami`             | `KubernetesAuthWhoamiOptions`           |
| `kubectl autoscale`               | `KubernetesAutoscaleOptions`            |
| `kubectl certificate`             | `KubernetesCertificateOptions`          |
| `kubectl certificate approve`     | `KubernetesCertificateApproveOptions`   |
| `kubectl certificate deny`        | `KubernetesCertificateDenyOptions`      |
| `kubectl cluster-info`            | `KubernetesClusterInfoOptions`          |
| `kubectl cluster-info dump`       | `KubernetesClusterInfoDumpOptions`      |
| `kubectl config`                  | `KubernetesConfigOptions`               |
| `kubectl config delete-cluster`   | `KubernetesConfigDeleteClusterOptions`  |
| `kubectl config delete-context`   | `KubernetesConfigDeleteContextOptions`  |
| `kubectl config delete-user`      | `KubernetesConfigDeleteUserOptions`     |
| `kubectl config get-contexts`     | `KubernetesConfigGetContextsOptions`    |
| `kubectl config rename-context`   | `KubernetesConfigRenameContextOptions`  |
| `kubectl config set`              | `KubernetesConfigSetOptions`            |
| `kubectl config set-cluster`      | `KubernetesConfigSetClusterOptions`     |
| `kubectl config set-context`      | `KubernetesConfigSetContextOptions`     |
| `kubectl config set-credentials`  | `KubernetesConfigSetCredentialsOptions` |
| `kubectl config unset`            | `KubernetesConfigUnsetOptions`          |
| `kubectl config use-context`      | `KubernetesConfigUseContextOptions`     |
| `kubectl config view`             | `KubernetesConfigViewOptions`           |
| `kubectl cordon`                  | `KubernetesCordonOptions`               |
| `kubectl cp`                      | `KubernetesCpOptions`                   |
| `kubectl debug`                   | `KubernetesDebugOptions`                |
| `kubectl describe`                | `KubernetesDescribeOptions`             |
| `kubectl diff`                    | `KubernetesDiffOptions`                 |
| `kubectl drain`                   | `KubernetesDrainOptions`                |
| `kubectl events`                  | `KubernetesEventsOptions`               |
| `kubectl exec`                    | `KubernetesExecOptions`                 |
| `kubectl kuberc`                  | `KubernetesKubercOptions`               |
| `kubectl kuberc set`              | `KubernetesKubercSetOptions`            |
| `kubectl kuberc view`             | `KubernetesKubercViewOptions`           |
| `kubectl label`                   | `KubernetesLabelOptions`                |
| `kubectl logs`                    | `KubernetesLogsOptions`                 |
| `kubectl patch`                   | `KubernetesPatchOptions`                |
| `kubectl port-forward`            | `KubernetesPortForwardOptions`          |
| `kubectl proxy`                   | `KubernetesProxyOptions`                |
| `kubectl replace`                 | `KubernetesReplaceOptions`              |
| `kubectl rollout`                 | `KubernetesRolloutOptions`              |
| `kubectl rollout history`         | `KubernetesRolloutHistoryOptions`       |
| `kubectl rollout pause`           | `KubernetesRolloutPauseOptions`         |
| `kubectl rollout restart`         | `KubernetesRolloutRestartOptions`       |
| `kubectl rollout resume`          | `KubernetesRolloutResumeOptions`        |
| `kubectl rollout status`          | `KubernetesRolloutStatusOptions`        |
| `kubectl rollout undo`            | `KubernetesRolloutUndoOptions`          |
| `kubectl scale`                   | `KubernetesScaleOptions`                |
| `kubectl taint`                   | `KubernetesTaintOptions`                |
| `kubectl top`                     | `KubernetesTopOptions`                  |
| `kubectl top node`                | `KubernetesTopNodeOptions`              |
| `kubectl top pod`                 | `KubernetesTopPodOptions`               |
| `kubectl uncordon`                | `KubernetesUncordonOptions`             |
| `kubectl version`                 | `KubernetesVersionOptions`              |
| `kubectl wait`                    | `KubernetesWaitOptions`                 |
