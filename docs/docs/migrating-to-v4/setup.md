---
title: Packages and namespaces
sidebar_position: 1
---

# Packages and namespaces

## Packages and target framework

- V4 pipeline projects target **.NET 11**. V3.2.8 targeted .NET 10. Upgrade the pipeline's SDK and
  runtime. The applications your pipeline builds keep their own target frameworks.
- Keep the core package, integrations, and any project with shared module or command types on
  compatible V4 versions. Recompile them so their source-generated metadata is refreshed.
- The core package no longer brings in `Polly`, `CliWrap`, `AsyncSemaphore`, or
  `vertical-spectreconsolelogger`. Add your own `PackageReference` if your code uses one directly.
- `ModularPipelines.Azure` no longer references `Azure.ResourceManager.Kubernetes` or
  `Azure.ResourceManager.KubernetesConfiguration`.

## Namespaces

Most module-authoring types now live directly in `ModularPipelines`. For a typical module file:

| Do this | Why |
| --- | --- |
| Add `using ModularPipelines;` | Modules, contexts, results, attributes, and conditions live here |
| Replace `using ModularPipelines.Models;` with `using ModularPipelines;` | No public core types remain in `Models`, and there are no compatibility aliases |
| Remove `using ModularPipelines.Modules;` | Module base types moved to the root namespace |
| Remove `using ModularPipelines.Extensions;` | It now contains only internal types |
| Add `using ModularPipelines.Context;` | Needed for the generated `context.Tools.<Tool>` properties |
| Add `using ModularPipelines.FileSystem;` | Needed for file-system extensions such as `AssertExists()` |
| Keep `ModularPipelines.Options` and generated CLI option namespaces | These did not move |
| Keep `using ModularPipelines.Attributes;` in files that use CLI attributes | `CliOptionAttribute` and the other CLI attributes stay there |

Do not replace whole namespaces blindly. As the last two rows show, some types did not move.

Update fully qualified names too. For example:

- `ModularPipelines.Models.KeyValue` becomes `ModularPipelines.KeyValue`.
- `ModularPipelines.Attributes.ModuleCategoryAttribute` becomes
  `ModularPipelines.ModuleCategoryAttribute`. Shorthand such as `[ModuleCategory("Build")]` needs
  only the root namespace.

A migrated Git root search still looks like this, with the `FileSystem` import:

```csharp
root.FindFile(file => file.Name == "Example.csproj").AssertExists()
```

### Where each type moved

| V3 location or type | V4 location or type |
| --- | --- |
| `ModularPipelines.Modules.Module<T>`, `Module`, `SyncModule<T>`, `SyncModule` | `ModularPipelines` |
| `ModularPipelines.Context.IModuleContext`, `IPipelineContext` | `ModularPipelines` |
| `ModularPipelines.Models.ModuleResult<T>`, `ModuleResult`, `CommandResult`, `None`, `SkipDecision` | `ModularPipelines` |
| `ModularPipelines.Models.PipelineSummary`, `IModuleResult` | `ModularPipelines` |
| `ModularPipelines.Configuration.ModuleConfigurationBuilder`, `ModuleConfiguration` | `ModularPipelines` |
| `ModularPipelines.Modules.IModule` | `ModularPipelines.IModule` |
| `ModularPipelines.Models.CommandLine`, `KeyValue` | `ModularPipelines` |
| `ModularPipelines.Plugins.IModularPipelinesPlugin` | `ModularPipelines.IModularPipelinesPlugin` |
| `ModularPipelines.Models.RequirementDecision` | `ModularPipelines.Requirements.RequirementDecision` |
| `ModularPipelines.Attributes.DependsOn*` | `ModularPipelines` |
| Run-condition types in `Attributes` / `Conditions` | `ModularPipelines` |
| `ModularPipelines.Enums.ModulePriority` | `ModularPipelines.ModulePriority` |
| `ModularPipelines.Interfaces.IParallelLimit` | `ModularPipelines.IParallelLimit` |
| `ModularPipelines.Attributes.NotInParallelAttribute`, `PriorityAttribute` | `ModularPipelines` (sealed; `NotInParallelAttribute.ConstraintKeys` is now `IReadOnlySet<string>`) |
| `ModularPipelines.Enums.BuildSystem` | `ModularPipelines.BuildSystem` |
| `ModularPipelines.Attributes.ModuleCategoryAttribute`, `ModuleTagAttribute` | `ModularPipelines` |
| `ModularPipelines.Attributes.ParallelLimiterAttribute`, `ParallelLimiterAttribute<T>` | `ModularPipelines` |
| Capability interfaces under `Context.Domains` and its subnamespaces | `ModularPipelines.Context` (some renamed; see [below](#renamed-and-removed-interfaces)) |
| `Context.Domains.IEnvironmentDomainContext` (`context.Environment`) | `ModularPipelines.Context.IEnvironmentContext` |
| `Context.Domains.Security.IHasherContext` (`context.Security.Hasher`) | `ModularPipelines.Context.IHashContext` (`context.Security.Hash`) |
| `Enums.ExecutionType` | `ModularPipelines.ExecutionHint` |
| `Attributes.ExecutionHintAttribute` | `ModularPipelines.ExecutionHintAttribute` |
| `Attributes.Events` handler interfaces and `Context.IModuleHookContext` | `ModularPipelines.Events` |
| `Attributes.SecretValueAttribute`, `Options.SecretMaskingOptions`, `Engine.ISecretRegistry` | `ModularPipelines.Secrets` (`ISecretObfuscator` is now internal) |
| `Engine.IModuleResultRepository`, `Engine.IModuleEstimatedTimeProvider` | `ModularPipelines.Reporting` |
| `Models.PipelineMetrics`, `Models.ModuleTimeline`, `Models.SubModuleEstimation` | `ModularPipelines.Reporting` |

Integration packages have their own namespace changes. See
[Integration packages](./integrations.md).

### The global using

The V4 package adds a `buildTransitive` global `using ModularPipelines;` to C# consumers.

If your own types collide with root types such as `Module` or `SkipDecision`, opt out with
`<Using Remove="ModularPipelines" />` in the project file, then add explicit or aliased usings.

### If you used a V4 pre-release build

These types also moved to the root `ModularPipelines` namespace: `PipelinePlan`,
`PipelinePlanModule`, `PipelinePlanWave`, `CliOptionValue`, `CliValuePair`, `CliValueGroup`, the CLI
value validation attributes, `ICancellableInitializer`, `ModuleIdAttribute`, `CacheInputsAttribute`,
and the artifact and capability attributes.

## Renamed and removed interfaces

This section matters if you inject these interfaces, implement them, or mock them in tests.

:::warning One name is reused with a different meaning
V4's `IEnvironmentContext` is the renamed V3 `IEnvironmentDomainContext` (`context.Environment`).
It is **not** V3's separate standalone `IEnvironmentContext`. Update mocks and signatures against
the V4 members.
:::

**Consolidated.** The older duplicate service interfaces, such as `ICommand`, `IBash`,
`IPowershell`, `IHttp`, `IJson`, and `IHasher`, are merged into the `*Context` interfaces. Update
implementations and mocks to the V4 contract, for example `ICommandContext`, `IPowerShellContext`,
`IHttpContext`, and `IHashContext`.

**Renamed.** The V4 interfaces below live in `ModularPipelines.Context`. The V3 `Context.Domains.*`
interfaces that already had these `*Context` names move to that namespace too.

| V3 interface | V4 interface | Context access |
| --- | --- | --- |
| `IZip` | `IZipContext` | `context.Files.Zip` |
| `IXml` | `IXmlContext` | `context.Data.Xml` |
| `IYaml` | `IYamlContext` | `context.Data.Yaml` |
| `IBase64` | `IBase64Context` | `context.Data.Base64` |
| `IHex` | `IHexContext` | `context.Data.Hex` |
| `ICertificates` | `ICertificatesContext` | `context.Security.Certificates` |
| `IDownloader` | `IDownloaderContext` | `context.Network.Downloader` |
| `IEnvironmentVariables` | `IEnvironmentVariablesContext` | `context.Environment.Variables` |

**Removed.**

- `IFilesContext.Checksum` and `IChecksumContext`. See
  [Utility replacements](./files-and-utilities.md#utility-replacements).
- The injectable `IFileSystemContext`. Use `FilePath` / `FolderPath` members or `context.Files`.
- `stream.ToMemoryStreamAsync()` is no longer public. Copy the stream to a `MemoryStream` yourself.

## `apt-get` and OS-specific options are removed

Removed, with no typed replacement in core:

- The `Context.Linux.AptGet` / `IAptGet` wrappers.
- The `ModularPipelines.Options.Linux.AptGet` family (`AptGetOptions`, `AptGetInstallOptions`, and so on).
- The `ModularPipelines.Options.Linux`, `.Mac`, and `.Windows` namespaces (`DpkgInstallOptions`,
  `MacBrewOptions`, `ExeInstallerOptions`, `MsiInstallerOptions`). See
  [Installers](./files-and-utilities.md#installers).

There is no `context.Tools.AptGet` property. To keep calling `apt-get`, run it as a command and
translate each old `AptGet*Options` value to its CLI argument:

```csharp
await context.Shell.RunAsync(
    "apt-get",
    ["install", "--yes", "curl"],
    cancellationToken: cancellationToken);
```

Run package-manager commands with the permissions your environment requires. For a typed API,
define your own integration with the
[custom integration guide](../how-to/generate-private-cli-integration.md).
