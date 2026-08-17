# kustomize CLI reference

`ModularPipelines.Kubernetes` provides strongly typed access to the `kustomize` CLI.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Kubernetes
```

Import `ModularPipelines.Kubernetes.Extensions`, then resolve the service with `context.Kustomize()`.

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines.Context;

using ModularPipelines.Models;

using ModularPipelines.Modules;

using ModularPipelines.Kubernetes.Extensions;

using ModularPipelines.Kubernetes.Options;



public class RunCommandModule : Module<CommandResult>

{

    protected override async Task<CommandResult?> ExecuteAsync(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        return await context.Kustomize().Build(

            new KustomizeBuildOptions(),

            cancellationToken: cancellationToken);

    }

}
```

## Commands[​](#commands "Direct link to Commands")

| CLI command                                | Options record                               |
| ------------------------------------------ | -------------------------------------------- |
| `kustomize build`                          | `KustomizeBuildOptions`                      |
| `kustomize cfg cat`                        | `KustomizeCfgCatOptions`                     |
| `kustomize cfg count`                      | `KustomizeCfgCountOptions`                   |
| `kustomize cfg grep`                       | `KustomizeCfgGrepOptions`                    |
| `kustomize cfg tree`                       | `KustomizeCfgTreeOptions`                    |
| `kustomize create`                         | `KustomizeCreateOptions`                     |
| `kustomize edit add`                       | `KustomizeEditAddOptions`                    |
| `kustomize edit add annotation`            | `KustomizeEditAddAnnotationOptions`          |
| `kustomize edit add base`                  | `KustomizeEditAddBaseOptions`                |
| `kustomize edit add buildmetadata`         | `KustomizeEditAddBuildmetadataOptions`       |
| `kustomize edit add component`             | `KustomizeEditAddComponentOptions`           |
| `kustomize edit add configmap`             | `KustomizeEditAddConfigmapOptions`           |
| `kustomize edit add generator`             | `KustomizeEditAddGeneratorOptions`           |
| `kustomize edit add label`                 | `KustomizeEditAddLabelOptions`               |
| `kustomize edit add patch`                 | `KustomizeEditAddPatchOptions`               |
| `kustomize edit add resource`              | `KustomizeEditAddResourceOptions`            |
| `kustomize edit add secret`                | `KustomizeEditAddSecretOptions`              |
| `kustomize edit add transformer`           | `KustomizeEditAddTransformerOptions`         |
| `kustomize edit alpha-list-builtin-plugin` | `KustomizeEditAlphaListBuiltinPluginOptions` |
| `kustomize edit fix`                       | `KustomizeEditFixOptions`                    |
| `kustomize edit remove`                    | `KustomizeEditRemoveOptions`                 |
| `kustomize edit remove annotation`         | `KustomizeEditRemoveAnnotationOptions`       |
| `kustomize edit remove buildmetadata`      | `KustomizeEditRemoveBuildmetadataOptions`    |
| `kustomize edit remove configmap`          | `KustomizeEditRemoveConfigmapOptions`        |
| `kustomize edit remove label`              | `KustomizeEditRemoveLabelOptions`            |
| `kustomize edit remove patch`              | `KustomizeEditRemovePatchOptions`            |
| `kustomize edit remove resource`           | `KustomizeEditRemoveResourceOptions`         |
| `kustomize edit remove secret`             | `KustomizeEditRemoveSecretOptions`           |
| `kustomize edit remove transformer`        | `KustomizeEditRemoveTransformerOptions`      |
| `kustomize edit set`                       | `KustomizeEditSetOptions`                    |
| `kustomize edit set annotation`            | `KustomizeEditSetAnnotationOptions`          |
| `kustomize edit set buildmetadata`         | `KustomizeEditSetBuildmetadataOptions`       |
| `kustomize edit set configmap`             | `KustomizeEditSetConfigmapOptions`           |
| `kustomize edit set image`                 | `KustomizeEditSetImageOptions`               |
| `kustomize edit set label`                 | `KustomizeEditSetLabelOptions`               |
| `kustomize edit set nameprefix`            | `KustomizeEditSetNameprefixOptions`          |
| `kustomize edit set namespace`             | `KustomizeEditSetNamespaceOptions`           |
| `kustomize edit set namesuffix`            | `KustomizeEditSetNamesuffixOptions`          |
| `kustomize edit set replicas`              | `KustomizeEditSetReplicasOptions`            |
| `kustomize edit set secret`                | `KustomizeEditSetSecretOptions`              |
| `kustomize fn run`                         | `KustomizeFnRunOptions`                      |
| `kustomize localize`                       | `KustomizeLocalizeOptions`                   |
