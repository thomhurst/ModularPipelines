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

## Scan an image

```csharp
using ModularPipelines.Trivy.Extensions;
using ModularPipelines.Trivy.Enums;
using ModularPipelines.Trivy.Options;

var result = await context.Trivy().Image(
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
trivy image alpine:3.20 --format=json --output=trivy-results.json --severity=HIGH --severity=CRITICAL --ignore-unfixed
```

## Scan a filesystem

```csharp
var result = await context.Trivy().Filesystem(
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
