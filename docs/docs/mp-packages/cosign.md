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
using ModularPipelines.Cosign.Options;

var result = await context.Tools.Cosign.SignAsync(
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
var result = await context.Tools.Cosign.VerifyAsync(
    new CosignVerifyOptions(["registry.example/app@sha256:..."])
    {
        Key = "cosign.pub",
    },
    cancellationToken: cancellationToken);
```

Generated password, token, OIDC secret, PIN, PUK, and hardware-management-key properties
are marked as secrets so Modular Pipelines masks their values in command logs.

## Shared logging and timeout settings

Every generated command inherits `OutputFile`, `Timeout`, and `Verbose` from
`CosignOptions`. These are Cosign's root persistent flags and render before the
subcommand. Signing, verification, registry, and key settings remain local to the
commands that advertise them.

```csharp
var options = new CosignVerifyOptions(["registry.example/app@sha256:..."])
{
    OutputFile = "verification.log",
    Timeout = "45s",
    Verbose = true,
    Key = "cosign.pub",
};
// cosign --output-file=verification.log --timeout=45s --verbose verify --key=cosign.pub registry.example/app@sha256:...
```

`Timeout` is a Cosign duration string such as `45s` or `2m30s`, not the framework's
process timeout in execution options. Leaving it `null` preserves Cosign's own
three-minute default. `Verbose = false` or `null` omits the flag. `OutputFile`
selects the CLI log destination; it is not a signature or bundle output path.
The CLI aliases `-t` and `-d` are retained in generated metadata.

For V4 migration, these three properties move from individual command records to
`CosignOptions` without changing their names or types. Existing object initializers
remain valid. Reflection code using `DeclaredOnly` must inspect inherited properties.
The audit uses Cosign 3.1.3; help/version operations and the command tree remain unchanged.
