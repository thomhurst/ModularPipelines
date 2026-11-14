---
title: CLI options
sidebar_position: 9
---

# CLI options and custom commands

## Generated options were regenerated

V4 regenerates every integration from current CLI help. Any of these can differ from V3:

- Command names and subcommand grouping.
- Option property names and types.
- Enum values.
- Required inputs.

There is no universal find-and-replace. Migrate each call against the **installed V4
integration's** options and service signatures, then check the resulting command line. The
[CLI command catalogs](../mp-packages/cli/dotnet.md) list each tool's current option types.

Also:

- **Removed namespaces.** V3's duplicate `ModularPipelines.DotNet.Generated.*`,
  `ModularPipelines.Kubernetes.Generated.*` (`Kubectl*` types), and
  `ModularPipelines.Azure.Generated.*` namespaces are gone, as is
  `ModularPipelines.Git.Services.IGit`. Use the types in each package's `Options`, `Enums`, and
  `Services` namespaces.
- **Validation runs first.** Required arguments and option combinations are validated before the
  process starts. Invalid options throw `CommandOptionsValidationException`.
- **Key-value options** are typed `IReadOnlyList<KeyValue>?`. Collection expressions and tuple
  conversions still compile. Code that depends on `KeyValue[]` or `IEnumerable<KeyValue>` must
  change. Two-value options use `CliValuePair`, which only supports the space separator.
- **Do not hand-edit generated models** or add compatibility shims to keep old generated APIs. If
  a generated command is missing or a model is wrong, fix or report the scraper/generator.

The sections below cover the tools whose shape changed the most. Skip the ones you do not use.

## .NET

### `dotnet test`

`dotnet test` accepts different switches depending on the runner that `global.json` selects, so it
is generated twice:

| Runner | Options | Method |
| --- | --- | --- |
| Microsoft.Testing.Platform (`test.runner` in `global.json`) | `DotNetTestOptions` | `TestAsync` |
| VSTest (the SDK default) | `DotNetTestVstestOptions` | `TestVstestAsync` |

Choosing a method does not select the runner. Keep `global.json` consistent with the options you use.

**Microsoft.Testing.Platform.** `DotNetTestOptions` has `Project`, `Solution`, `TestModules`, and
the other platform switches. Test-application switches go in `PlatformOptions` and
`ExtensionOptions`:

```csharp
await context.Tools.DotNet.TestAsync(new DotNetTestOptions
{
    Project = "Tests.csproj",
    ExtensionOptions = ["--report-trx", "--coverage"],
});
```

**VSTest.** `DotNetTestVstestOptions` has the positional `ProjectSolution` and switches such as
`Filter`, `Logger`, `Collect`, `Settings`, and `Blame`. `Logger` and `Collect` are collections:

```csharp
await context.Tools.DotNet.TestVstestAsync(new DotNetTestVstestOptions
{
    ProjectSolution = "Tests.sln",
    Filter = "Category=Unit",
    Logger = ["trx"],
});
```

### Other .NET options

- `DotNetPackOptions.Version` accepts a package-version string. The type also has `Verbosity` and
  `UseCurrentRuntime`.
- `SelfContained` is on the build, publish, and run options.

## Helm and kubectl

Global settings moved to a base record in each package, so you set them directly on any command's
options instead of adding manual arguments.

| Package | Base record | Settings include |
| --- | --- | --- |
| Helm | `HelmOptions` | `Namespace`, `KubeContext`, `KubeConfig`, `KubeToken` |
| kubectl | `KubernetesOptions` | `Namespace`, `Context`, `KubeConfig` |

- V3's Helm `Kubeconfig` is now spelled `KubeConfig`.
- Rebuild consumers after upgrading. These properties are no longer declared on each Helm command
  record. Reflection code using `BindingFlags.DeclaredOnly` must also inspect the base record.
- Both packages render inherited settings after the subcommand.
- Local replacements, such as `KubernetesConfigSetContextOptions.Namespace`, keep their
  command-specific metadata and render only once, including when set through the base type.
- `HelmVersionOptions` and `KubernetesVersionOptions` are available through `VersionAsync`.

See the generated [Helm reference](../mp-packages/cli/helm.md) and
[kubectl reference](../mp-packages/cli/kubectl.md) for the full global option lists.

## Gcloud

Gcloud commands inherit wide flags from `GcloudOptions`, including `Project`, `BillingProject`,
`Account`, `Configuration`, and `Quiet`. Set these properties on the command options instead of
adding switches to `Arguments`. Inherited flags render after the complete command path.

| Property | Behavior |
| --- | --- |
| `FlagsFile` | A collection; repeats `--flags-file` for each file |
| `Flatten`, `ImpersonateServiceAccount` | Collections; values are joined with commas in one argument |
| `Quiet` | Keeps its `-q` alias; generated options render the long form by default |
| `UserOutputEnabled` | Nullable boolean: `true` emits `--user-output-enabled`, `false` emits `--no-user-output-enabled`, `null` omits both. Replace manual negative switches with this property |
| `TraceToken` | Masked in command logs |
| `AccessTokenFile` | A file path; stays visible in logs |

- When you migrate a scalar value to one of the collection properties, wrap it in a collection.
- Reflection code using `BindingFlags.DeclaredOnly` must inspect the base record too.
- **A command's own definition wins.** Command-specific options with the same name keep their
  required inputs, descriptions, and validation. For example, `GcloudAssetAnalyzeMoveOptions`
  still requires its resource project in the constructor. Setting `Project` through `GcloudOptions`
  does not replace that. Optional overrides share the base value and render once. Check the
  command's contract before you move a local setting into shared code.

See the generated [Gcloud reference](../mp-packages/cli/gcloud.md) for the full wide-flag list.

## Buildah

Buildah commands share storage, logging, registry configuration, and default UID/GID mapping
settings through `BuildahOptions`.

- `StorageOpt`, `UsernsUidMap`, and `UsernsGidMap` accept collections and repeat their switch for
  each value. Wrap an existing mapping string in a collection, such as `["0:1000:1"]`.
- The `build` and `from` commands keep their local mapping definitions and share the effective
  value with the base options.
- Settings render after the selected subcommand.

See the [Buildah package guide](../mp-packages/buildah.md#shared-storage-settings-and-local-overrides).

## Handwritten command options

This section is for option types you wrote yourself. See
[Custom Commands](../how-to/custom-commands.md) for the `CliTool` / `CliSubCommand` identity
attributes and the argument-ordering rules.

| V3 | V4 |
| --- | --- |
| `[CliCommand("tool", "sub", ...)]` | `[CliTool("tool")]` plus `[CliSubCommand("sub", ...)]` |
| `CliOptionAttribute.AllowMultiple` | Removed; collections repeat the option for each value, as before. Set `GroupValues = true` for one occurrence, or `CollectionSeparator` to join values |
| `CliOptionAttribute.CustomSeparator` | `Format` (`SpaceSeparated`, `EqualsSeparated`, `ColonSeparated`, `NoSeparator`); arbitrary separators are no longer supported |
| `CliOptionAttribute.GetSeparator()`, `CliOptionAttribute.GetEffectiveName()`, `CliFlagAttribute.GetEffectiveName()` | Removed |
| `CommandLineToolOptions.CommandParts` as `string[]?` | `IReadOnlyList<string>?` |
| `CliSubCommandAttribute.SubCommands`, `CliCommandAliasAttribute.CommandParts` as `string[]` | `IReadOnlyList<string>` |

`CommandLineToolOptions` is no longer abstract. `new CommandLineToolOptions("tool")` describes an
ad-hoc command.

### Argument placement

`CliArgumentAttribute.Placement` and the `ArgumentPlacement` enum are replaced by
`CliArgumentAttribute.Phase` (`CommandLinePhase`):

| V3 | V4 |
| --- | --- |
| `Placement = ArgumentPlacement.AfterOptions` (the V3 default) | `Phase = CommandLinePhase.Passthrough` (the V4 default) |
| `Placement = ArgumentPlacement.BeforeOptions` | `Phase = CommandLinePhase.EarlyOperand` |
| `Placement = ArgumentPlacement.ImmediatelyAfterCommand` | `Phase = CommandLinePhase.EarlyOperand`; it renders after the final subcommand |

V3 rendered `ImmediatelyAfterCommand` arguments before `BeforeOptions` arguments. Both now map to
`EarlyOperand`, which orders by position only. Check argument positions if a type used both.

### Placeholders in command parts

`CliArgumentAttribute.Name` and `<PLACEHOLDER>` substitution in command parts are removed. Every
`[CliArgument]` value renders as a positional argument. When the command chain depends on
constructor input, set `CommandParts` in the constructor:

```csharp
// V3: the argument replaced <ACTION>, or disappeared if it did not match.
[CliCommand("tool", "resource", "<ACTION>")]
public record ResourceOptions(
    [property: CliArgument(0, Name = "<ACTION>")] string Action)
    : CommandLineToolOptions;

// V4
[CliTool("tool")]
public record ResourceOptions : CommandLineToolOptions
{
    public ResourceOptions(string action)
    {
        CommandParts = ["resource", action];
    }
}
```

## Source generation and private integrations

Custom command-option types need generated runtime metadata.

- The core NuGet package includes the generator. Make sure it runs in the projects that declare
  your options, and do not exclude its analyzer assets.
- Rebuild shared libraries that declare command options against V4, so they carry V4 metadata.
- The process-wide `ModularPipelinesContextRegistry` and parameterless registration module
  initializers are removed. Mark an accessible static service-registration method that accepts
  `IServiceCollection` with `[ModularPipelinesIntegration]`, or regenerate the integration with the
  current generator.

Follow the
[private integration migration](../how-to/generate-private-cli-integration.md#migrate-custom-integration-registration-for-v4)
for the complete registration and `Tools` metadata pattern.
