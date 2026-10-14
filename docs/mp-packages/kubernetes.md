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
using ModularPipelines;

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

## Kustomize inherited error diagnostics[​](#kustomize-inherited-error-diagnostics "Direct link to Kustomize inherited error diagnostics")

Kustomize commands inherit `StackTrace` from `KustomizeOptions`. Setting it to `true` renders `--stack-trace` before the command path; false or null omits the flag. For example:

```
new KustomizeBuildOptions { StackTrace = true, Dir = "overlays/test" }

// kustomize --stack-trace build overlays/test
```

The v5.8.2 audit confirms this single inherited setting using root, build, edit, cfg, fn, and nested edit help. The [official root parser](https://github.com/kubernetes-sigs/kustomize/blob/kustomize/v5.8.2/kustomize/commands/commands.go) registers Go's flags as persistent Cobra flags. Root help is a control action. Build/plugin settings and edit options remain command-local; repeated names across commands do not make them universal settings. `StackTrace` has no short alias, value, collection, or secret metadata.

Existing `StackTrace` initializers remain valid after moving the property to the base record. Reflection consumers should include inherited properties instead of using `DeclaredOnly`. This applies only to Kustomize, not the separate kubectl option hierarchy in this package.

Full regeneration with Kustomize v5.8.3 also adds `edit add configuration` and `edit remove component`. It applies the existing scraper's required edit operands: pass scalar values or nonempty collections through the generated constructors, for example `new KustomizeEditAddAnnotationOptions(["team:platform"])` or `new KustomizeEditSetNamespaceOptions("integration")`. These replace incomplete parameterless edit invocations in the older snapshot.

Wrappers for these commands now require an options argument. Replace calls such as `kustomize.Edit.Add.AnnotationAsync()` with `kustomize.Edit.Add.AnnotationAsync(new KustomizeEditAddAnnotationOptions(["team:platform"]))`.

The complete command tree now contains 48 commands, including `version`, exposed through `VersionAsync` and `KustomizeVersionOptions`. See the [generated CLI reference](/ModularPipelines/docs/mp-packages/cli/kustomize.md) for the current command list.
