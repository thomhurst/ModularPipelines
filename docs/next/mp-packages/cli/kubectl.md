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
| `kubectl wait`                    | `KubernetesWaitOptions`                 |
