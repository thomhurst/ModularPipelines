# skopeo CLI reference

`ModularPipelines.Skopeo` provides strongly typed access to the `skopeo` CLI.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Skopeo
```

Import `ModularPipelines.Skopeo.Extensions`, then resolve the service with `context.Skopeo()`.

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines.Context;

using ModularPipelines.Models;

using ModularPipelines.Modules;

using ModularPipelines.Skopeo.Extensions;

using ModularPipelines.Skopeo.Options;



public class RunCommandModule : Module<CommandResult>

{

    protected override async Task<CommandResult?> ExecuteAsync(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        return await context.Skopeo().GenerateSigstoreKey(

            new SkopeoGenerateSigstoreKeyOptions(),

            cancellationToken: cancellationToken);

    }

}
```

## Commands[​](#commands "Direct link to Commands")

| CLI command                    | Options record                     |
| ------------------------------ | ---------------------------------- |
| `skopeo copy`                  | `SkopeoCopyOptions`                |
| `skopeo delete`                | `SkopeoDeleteOptions`              |
| `skopeo generate-sigstore-key` | `SkopeoGenerateSigstoreKeyOptions` |
| `skopeo inspect`               | `SkopeoInspectOptions`             |
| `skopeo list-tags`             | `SkopeoListTagsOptions`            |
| `skopeo login`                 | `SkopeoLoginOptions`               |
| `skopeo logout`                | `SkopeoLogoutOptions`              |
| `skopeo manifest-digest`       | `SkopeoManifestDigestOptions`      |
| `skopeo standalone-sign`       | `SkopeoStandaloneSignOptions`      |
| `skopeo standalone-verify`     | `SkopeoStandaloneVerifyOptions`    |
| `skopeo sync`                  | `SkopeoSyncOptions`                |
