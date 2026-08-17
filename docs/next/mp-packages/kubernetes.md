# Kubernetes Package

Strongly typed kubectl and Kustomize commands.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Kubernetes
```

Required command-line tools: `kubectl`, `kustomize`. They must be installed and available on `PATH` when the pipeline runs.

## Context entry points[​](#context-entry-points "Direct link to Context entry points")

Use the discoverable `context.Tools` surface from a module:

* `context.Tools.Kubernetes`
* `context.Tools.Kustomize`

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines.Context;

using ModularPipelines.Models;

using ModularPipelines.Modules;

using ModularPipelines.Kubernetes.Options;



public class UseKubernetesModule : Module<CommandResult>

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

The package exposes generated options records for its supported CLI commands.
