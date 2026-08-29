---
title: Generate a private CLI integration
---

# Generate a private CLI integration

`ModularPipelines.OptionsGenerator` is a .NET tool that generates the same strongly typed
options and services used by first-party ModularPipelines CLI packages. It accepts a
versioned JSON definition, so private and experimental tools do not need changes in the
ModularPipelines repository.

## Install and pin the generator

Use a local tool manifest so every developer and CI runner uses the same generator version:

```shell
dotnet new tool-manifest
dotnet tool install ModularPipelines.OptionsGenerator --version <version>
```

Commit `.config/dotnet-tools.json`. Update the pinned package version deliberately, review
the generated diff, and regenerate before merging the update.

The NuGet package follows the ModularPipelines semantic version. The input format is
versioned independently by `schemaVersion`. This release supports schema version `1` and
rejects unknown versions rather than interpreting them differently.

## Define the private tool

Create `tools/private-widget.json`:

```json
{
  "schemaVersion": 1,
  "tool": {
    "ownershipId": "private-widget-integration",
    "toolName": "private-widget",
    "namespacePrefix": "PrivateWidget",
    "targetNamespace": "Example.Build.PrivateWidget",
    "outputDirectory": "src/Example.Build.PrivateWidget",
    "documentationOutputDirectory": null,
    "executablePrerequisiteMetadataExemption": "Installation is controlled by the private repository.",
    "commands": [
      {
        "fullCommand": "private-widget deploy",
        "commandParts": ["deploy"],
        "className": "PrivateWidgetDeployOptions",
        "parentClassName": "PrivateWidgetOptions",
        "toolNamespacePrefix": "PrivateWidget",
        "description": "Deploys a private widget.",
        "options": [
          {
            "switchName": "--environment",
            "propertyName": "Environment",
            "cSharpType": "string?",
            "description": "Deployment environment."
          },
          {
            "switchName": "--dry-run",
            "propertyName": "DryRun",
            "cSharpType": "bool?",
            "isFlag": true,
            "valueArity": "none"
          }
        ]
      }
    ]
  }
}
```

`ownershipId` is an immutable identifier for the integration. Keep it unchanged when
renaming the tool, namespace prefix, target namespace, or output directories so the
generator can reconcile files it previously owned. Use a different value for every
independent definition.

`outputDirectory` and `documentationOutputDirectory` are relative to `--output-dir`.
Absolute paths, paths that escape that root, and paths traversing symbolic links or
filesystem reparse points are rejected. Generated namespaces, types, properties, methods,
and enum names must be valid C# syntax. Set
`documentationOutputDirectory` to `null` when the integration repository does not want a
generated Markdown reference.

The integration project needs a package reference to `ModularPipelines`:

```xml
<ItemGroup>
  <PackageReference Include="ModularPipelines" Version="..." />
</ItemGroup>
```

## Generate

Run the tool from any directory:

```shell
dotnet tool restore
dotnet tool run modular-pipelines-options -- \
  --input tools/private-widget.json \
  --output-dir .
```

The command is deterministic: identical metadata and generator versions produce identical
files. Generated C# files, command-coverage manifests, and optional documentation are owned
by the JSON definition and generator version. The generator records this set in
`.modular-pipelines-options/<namespacePrefix>.files`, allowing later runs to remove stale
generated documentation when its directory or tool name changes. Commit that manifest and
the generated files, but do not edit either manually. Change the JSON and rerun instead.

For CI, restore the manifest and run the same command, then fail when `git diff --exit-code`
finds uncommitted output:

```yaml
- uses: actions/setup-dotnet@v6.0.0
  with:
    dotnet-version: 10.0.x
- run: dotnet tool restore
- run: >-
    dotnet tool run modular-pipelines-options --
    --input tools/private-widget.json
    --output-dir .
- run: git diff --exit-code
```

Use `--change-manifest <path>` when automation needs the exact generated and deleted paths.
The existing `--tools` mode remains reserved for first-party scrapers; it cannot be combined
with `--input`.

## Migrate custom integration registration for v4

Version 4 replaces the process-wide `ModularPipelinesContextRegistry` and integration
module initializers with generated assembly metadata. Output from the current options
generator already uses the new mechanism.

For a hand-written integration, mark its service-registration method:

```csharp
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes;

public static class PrivateWidgetExtensions
{
    [ModularPipelinesIntegration]
    public static IServiceCollection RegisterPrivateWidget(
        this IServiceCollection services)
    {
        services.AddScoped<IPrivateWidget, PrivateWidget>();
        return services;
    }
}
```

The method must be accessible from its assembly, static, non-generic, declared on a
non-generic accessible type, and accept one `IServiceCollection` parameter. It can return
either `void` or `IServiceCollection`.

To expose the integration through the discoverable `context.Tools.PrivateWidget` API, keep
its public `IPipelineContext` extension accessor in the same static class as the attributed
registration method:

```csharp
public static IPrivateWidget PrivateWidget(this IPipelineContext context) =>
    context.Services.GetRequiredService<IPrivateWidget>();
```

The generator uses that shared declaring type to associate the accessor with its
registration. Accessor names must be unique across all referenced integration packages.
They also cannot use names already exposed by `IToolsContext` or `object`, such as `Get`
or `GetType`, because instance-member lookup would hide the generated property.
`context.Tools.*` is the preferred discoverable API; the original extension method remains
available for compatibility. Generated tool properties use C# 14 extension members. On
older language versions, registration metadata still generates and `MPG0008` explains why
the optional `Tools` property was skipped and names the `context.X()` compatibility accessor.

Referencing the `ModularPipelines` package includes the source generator as an analyzer.
The generator emits immutable assembly registration metadata, which each pipeline consumes
independently. Remove the old parameterless module initializer and any call to
`ModularPipelinesContextRegistry.RegisterContext`.

If the integration DLL is copied beside the application without a compile-time reference,
opt in to filename-based assembly discovery before building the pipeline:

```csharp
var builder = Pipeline.CreateBuilder(new PipelineBuilderSettings
{
    Args = args,
    LoadModularPipelinesAssemblies = true,
});
```

The option is disabled by default to avoid scanning and loading every matching assembly at
pipeline startup. Integrations already loaded through normal application use and explicitly
registered services do not need it.
