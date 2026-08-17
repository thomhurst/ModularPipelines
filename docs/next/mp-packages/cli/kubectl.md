# kubectl CLI reference

`ModularPipelines.Kubernetes` provides strongly typed access to the `kubectl` CLI.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Kubernetes
```

Resolve the service with `context.Tools.Kubernetes`. For projects older than C# 14, import `ModularPipelines.Kubernetes.Extensions` and use the `context.Kubernetes()` extension method as a compatibility fallback.

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines.Context;

using ModularPipelines.Models;

using ModularPipelines.Modules;

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
| `kubectl config get-contexts`     | `KubernetesConfigGetContextsOptions`    |
| `kubectl config set`              | `KubernetesConfigSetOptions`            |
| `kubectl config set-cluster`      | `KubernetesConfigSetClusterOptions`     |
| `kubectl config set-context`      | `KubernetesConfigSetContextOptions`     |
| `kubectl config set-credentials`  | `KubernetesConfigSetCredentialsOptions` |
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
