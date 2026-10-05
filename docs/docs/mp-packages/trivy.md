---
title: Trivy Package
---

# Trivy Package

`ModularPipelines.Trivy` provides strongly typed access to the Trivy CLI for scanning
container images, filesystems, repositories, SBOMs, Kubernetes clusters, and virtual machines.

## Installation

```shell
dotnet add package ModularPipelines.Trivy
```

The `trivy` executable must be installed and available on `PATH` when the pipeline runs.

## Inherited settings

`TrivyOptions` declares `Cacert`, `CacheDir`, `Config`, `Debug`,
`GenerateDefaultConfig`, `Insecure`, `Quiet`, and `Timeout`. Every command inherits
these settings, including nested plugin and registry commands. They render before
the command path. For example:

```csharp
new TrivyPluginInstallOptions("aquasecurity/trivy-plugin")
{
    Config = "trivy.yaml",
    Quiet = true,
    Timeout = "2m",
};
```

This renders `trivy --config=trivy.yaml --quiet --timeout=2m plugin install aquasecurity/trivy-plugin`.
`Timeout` accepts Trivy duration text such as `30s` or `2m`; zero is preserved.
Nullable flags render only when true. `GenerateDefaultConfig` retains Trivy's
configuration-generation behavior, including skipping scans when selected.

The scope audit uses Trivy 0.75.0 root, image, plugin, plugin-install, and registry-login
help, plus its [persistent flag declarations](https://github.com/aquasecurity/trivy/blob/v0.75.0/pkg/flag/global_flags.go).
Root help/version controls and the root version-format flag are excluded. Scan
format, scanner selection, registry credentials, and other command settings remain
on their command records. Repeated password values retain secret metadata.

Migration: existing object initializers keep the same property names. Reflection
using `DeclaredOnly` must now inspect `TrivyOptions` for these eight properties.
Remove duplicate manual arguments for inherited settings. The separate version-command
work tracks new version APIs; this scope audit does not change version discovery.
Regeneration against 0.75.0 also adds the CLI's `crypto` image scanner value as
`TrivyImageScanners.Crypto`; all 31 existing commands remain available.

## Scan an image

```csharp
using ModularPipelines.Trivy.Enums;
using ModularPipelines.Trivy.Options;

var result = await context.Tools.Trivy.ImageAsync(
    new TrivyImageOptions
    {
        ImageName = "alpine:3.20",
        Format = TrivyImageFormat.Json,
        Output = "trivy-results.json",
        Severity = [TrivyImageSeverity.High, TrivyImageSeverity.Critical],
        IgnoreUnfixed = true,
    },
    cancellationToken: cancellationToken);
```

This renders the equivalent of:

```shell
trivy image --format=json --output=trivy-results.json --severity=HIGH --severity=CRITICAL --ignore-unfixed alpine:3.20
```

## Scan a filesystem

```csharp
var result = await context.Tools.Trivy.FilesystemAsync(
    new TrivyFilesystemOptions("./src")
    {
        Scanners =
        [
            TrivyFilesystemScanners.Vuln,
            TrivyFilesystemScanners.Secret,
            TrivyFilesystemScanners.Misconfig,
        ],
    },
    cancellationToken: cancellationToken);
```

Generated password, token, secret, and key properties are marked as secrets so Modular
Pipelines masks their values in command logs.
