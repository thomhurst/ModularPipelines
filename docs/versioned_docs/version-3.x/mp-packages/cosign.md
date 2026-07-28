---
title: Cosign Package
---

# Cosign Package

`ModularPipelines.Cosign` provides strongly typed access to the Sigstore Cosign CLI for
signing and verifying container images, blobs, attestations, and bundles.

## Installation

```shell
dotnet add package ModularPipelines.Cosign
```

The `cosign` executable must be installed and available on `PATH` when the pipeline runs.

## Sign container images

```csharp
using ModularPipelines.Cosign.Extensions;
using ModularPipelines.Cosign.Options;

var result = await context.Cosign().Sign(
    new CosignSignOptions(["registry.example/app@sha256:..."])
    {
        Key = "cosign.key",
        Annotations = ["team=platform"],
        Yes = true,
    },
    cancellationToken: cancellationToken);
```

This renders the equivalent of:

```shell
cosign sign registry.example/app@sha256:... --annotations=team=platform --key=cosign.key --yes
```

## Verify container images

```csharp
var result = await context.Cosign().Verify(
    new CosignVerifyOptions(["registry.example/app@sha256:..."])
    {
        Key = "cosign.pub",
    },
    cancellationToken: cancellationToken);
```

Generated password, token, OIDC secret, PIN, PUK, and hardware-management-key properties
are marked as secrets so Modular Pipelines masks their values in command logs.
