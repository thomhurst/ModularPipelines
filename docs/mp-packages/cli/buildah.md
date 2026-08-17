# buildah CLI reference

`ModularPipelines.Buildah` provides strongly typed access to the `buildah` CLI.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Buildah
```

Import `ModularPipelines.Buildah.Extensions`, then resolve the service with `context.Buildah()`.

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines.Context;

using ModularPipelines.Models;

using ModularPipelines.Modules;

using ModularPipelines.Buildah.Extensions;

using ModularPipelines.Buildah.Options;



public class RunCommandModule : Module<CommandResult>

{

    protected override async Task<CommandResult?> ExecuteAsync(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        return await context.Buildah().Add(

            new BuildahAddOptions(),

            cancellationToken: cancellationToken);

    }

}
```

## Commands[​](#commands "Direct link to Commands")

| CLI command                 | Options record                   |
| --------------------------- | -------------------------------- |
| `buildah add`               | `BuildahAddOptions`              |
| `buildah build`             | `BuildahBuildOptions`            |
| `buildah commit`            | `BuildahCommitOptions`           |
| `buildah config`            | `BuildahConfigOptions`           |
| `buildah containers`        | `BuildahContainersOptions`       |
| `buildah copy`              | `BuildahCopyOptions`             |
| `buildah from`              | `BuildahFromOptions`             |
| `buildah images`            | `BuildahImagesOptions`           |
| `buildah inspect`           | `BuildahInspectOptions`          |
| `buildah login`             | `BuildahLoginOptions`            |
| `buildah logout`            | `BuildahLogoutOptions`           |
| `buildah manifest add`      | `BuildahManifestAddOptions`      |
| `buildah manifest annotate` | `BuildahManifestAnnotateOptions` |
| `buildah manifest create`   | `BuildahManifestCreateOptions`   |
| `buildah manifest exists`   | `BuildahManifestExistsOptions`   |
| `buildah manifest inspect`  | `BuildahManifestInspectOptions`  |
| `buildah manifest push`     | `BuildahManifestPushOptions`     |
| `buildah manifest remove`   | `BuildahManifestRemoveOptions`   |
| `buildah manifest rm`       | `BuildahManifestRmOptions`       |
| `buildah mkcw`              | `BuildahMkcwOptions`             |
| `buildah mount`             | `BuildahMountOptions`            |
| `buildah prune`             | `BuildahPruneOptions`            |
| `buildah pull`              | `BuildahPullOptions`             |
| `buildah push`              | `BuildahPushOptions`             |
| `buildah rename`            | `BuildahRenameOptions`           |
| `buildah rm`                | `BuildahRmOptions`               |
| `buildah rmi`               | `BuildahRmiOptions`              |
| `buildah run`               | `BuildahRunOptions`              |
| `buildah source add`        | `BuildahSourceAddOptions`        |
| `buildah source create`     | `BuildahSourceCreateOptions`     |
| `buildah source pull`       | `BuildahSourcePullOptions`       |
| `buildah source push`       | `BuildahSourcePushOptions`       |
| `buildah tag`               | `BuildahTagOptions`              |
| `buildah umount`            | `BuildahUmountOptions`           |
| `buildah unshare`           | `BuildahUnshareOptions`          |
