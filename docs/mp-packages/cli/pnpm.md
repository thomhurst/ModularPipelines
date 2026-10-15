# pnpm CLI reference

`ModularPipelines.Node` provides strongly typed access to the `pnpm` CLI.

## Executable prerequisite[​](#executable-prerequisite "Direct link to Executable prerequisite")

This package does not install the `pnpm` executable. Install it separately and ensure `pnpm` is available on `PATH`.

Follow the executable's official documentation for installation instructions.

## Package installation[​](#package-installation "Direct link to Package installation")

```
dotnet add package ModularPipelines.Node
```

Resolve the service with `context.Tools.Pnpm`. Projects using C# 13 or another .NET language can use `context.Tools.Get<ModularPipelines.Node.Services.IPnpm>()` instead.

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines;

using ModularPipelines.Node.Enums;

using ModularPipelines.Node.Options;



public class RunCommandModule : Module<CommandResult>

{

    protected override async Task<CommandResult> ExecuteAsync(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        return await context.Tools.Pnpm.AuditAsync(

            new PnpmAuditOptions()

            {

                AuditLevel = PnpmAuditAuditLevel.High,

            },

            cancellationToken: cancellationToken);

    }

}
```

## Global options[​](#global-options "Direct link to Global options")

Global options are rendered after the selected subcommand.

| CLI option             | Property            | Availability | Description                                                                                                                                                                                                                                             |
| ---------------------- | ------------------- | ------------ | ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `--color`              | `Color`             | All editions | Force colored output                                                                                                                                                                                                                                    |
| `--dir`                | `Dir`               | All editions | Set working directory. Accepted anywhere on the command line, before or after the subcommand, like every other rc-option \[default: .]                                                                                                                  |
| `--http-proxy`         | `HttpProxy`         | All editions | Proxy for HTTP registry and tarball requests                                                                                                                                                                                                            |
| `--https-proxy`        | `HttpsProxy`        | All editions | Proxy for HTTPS registry and tarball requests                                                                                                                                                                                                           |
| `--ignore-workspace`   | `IgnoreWorkspace`   | All editions | Run as if the project were standalone, ignoring any `pnpm-workspace.yaml` above it                                                                                                                                                                      |
| `--loglevel`           | `Loglevel`          | All editions | What level of logs to print. `silent` selects the silent reporter over any `--reporter` choice; the other levels cap the default reporter's output                                                                                                      |
| `--no-progress`        | `NoProgress`        | All editions | Disable dependency and download progress output                                                                                                                                                                                                         |
| `--no-proxy`           | `NoProxy`           | All editions | Hosts that bypass configured proxies                                                                                                                                                                                                                    |
| `--npmrc-auth-file`    | `NpmrcAuthFile`     | All editions | Path to an `.npmrc` to read auth settings from, overriding the default `~/.npmrc` \[alias: --userconfig]                                                                                                                                                |
| `--registry`           | `Registry`          | All editions | Base URL of the npm registry to resolve and fetch packages from. Universal rc-option: accepted on every command and layered onto the config like `--config.registry=&lt;url&gt;`. Commands that expose their own `--registry` still read the same value |
| `--reporter`           | `Reporter`          | All editions | Reporter output format                                                                                                                                                                                                                                  |
| `--state-dir`          | `StateDir`          | All editions | Directory in which pnpm persists machine-local state                                                                                                                                                                                                    |
| `--store-dir`          | `StoreDir`          | All editions | Directory in which the package store is created. Relative paths are resolved from the workspace root, or from `--dir` outside a workspace                                                                                                               |
| `--use-stderr`         | `UseStderr`         | All editions | Divert the reporter's output to stderr, leaving stdout for the command's own result                                                                                                                                                                     |
| `--workspace-packages` | `WorkspacePackages` | All editions | Glob patterns selecting the workspace's projects, overriding the `packages` field of `pnpm-workspace.yaml`. Repeat to add more                                                                                                                          |
| `--workspace-root`     | `WorkspaceRoot`     | All editions | Run the command on the root workspace project                                                                                                                                                                                                           |
| `--yes`                | `Yes`               | All editions | Automatically answer yes to prompts                                                                                                                                                                                                                     |

## Intentionally excluded commands[​](#intentionally-excluded-commands "Direct link to Intentionally excluded commands")

| CLI command             | Reason                                                                                                                                                                                 |
| ----------------------- | -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `pnpm audit signatures` | pnpm 12.4.1 no longer lists this path in a Commands section or exposes a distinct child synopsis. The parent command documents \[PARAMS]... instead; use its generated Params operand. |
| `pnpm stage approve`    | pnpm 12.4.1 no longer lists this path in a Commands section or exposes a distinct child synopsis. The parent command documents \[PARAMS]... instead; use its generated Params operand. |
| `pnpm stage download`   | pnpm 12.4.1 no longer lists this path in a Commands section or exposes a distinct child synopsis. The parent command documents \[PARAMS]... instead; use its generated Params operand. |
| `pnpm stage list`       | pnpm 12.4.1 no longer lists this path in a Commands section or exposes a distinct child synopsis. The parent command documents \[PARAMS]... instead; use its generated Params operand. |
| `pnpm stage publish`    | pnpm 12.4.1 no longer lists this path in a Commands section or exposes a distinct child synopsis. The parent command documents \[PARAMS]... instead; use its generated Params operand. |
| `pnpm stage reject`     | pnpm 12.4.1 no longer lists this path in a Commands section or exposes a distinct child synopsis. The parent command documents \[PARAMS]... instead; use its generated Params operand. |
| `pnpm stage view`       | pnpm 12.4.1 no longer lists this path in a Commands section or exposes a distinct child synopsis. The parent command documents \[PARAMS]... instead; use its generated Params operand. |

## Commands[​](#commands "Direct link to Commands")

| CLI command                  | Options record                   |
| ---------------------------- | -------------------------------- |
| `pnpm access`                | `PnpmAccessOptions`              |
| `pnpm add`                   | `PnpmAddOptions`                 |
| `pnpm approve`               | `PnpmApproveOptions`             |
| `pnpm approve-builds`        | `PnpmApproveBuildsOptions`       |
| `pnpm audit`                 | `PnpmAuditOptions`               |
| `pnpm bugs`                  | `PnpmBugsOptions`                |
| `pnpm cache`                 | `PnpmCacheOptions`               |
| `pnpm cache delete`          | `PnpmCacheDeleteOptions`         |
| `pnpm cache list`            | `PnpmCacheListOptions`           |
| `pnpm cache list-registries` | `PnpmCacheListRegistriesOptions` |
| `pnpm cache path`            | `PnpmCachePathOptions`           |
| `pnpm cache prune`           | `PnpmCachePruneOptions`          |
| `pnpm cache view`            | `PnpmCacheViewOptions`           |
| `pnpm cat-file`              | `PnpmCatFileOptions`             |
| `pnpm cat-index`             | `PnpmCatIndexOptions`            |
| `pnpm change`                | `PnpmChangeOptions`              |
| `pnpm ci`                    | `PnpmCiOptions`                  |
| `pnpm clean`                 | `PnpmCleanOptions`               |
| `pnpm config`                | `PnpmConfigOptions`              |
| `pnpm config delete`         | `PnpmConfigDeleteOptions`        |
| `pnpm config get`            | `PnpmConfigGetOptions`           |
| `pnpm config list`           | `PnpmConfigListOptions`          |
| `pnpm config set`            | `PnpmConfigSetOptions`           |
| `pnpm create`                | `PnpmCreateOptions`              |
| `pnpm dedupe`                | `PnpmDedupeOptions`              |
| `pnpm deploy`                | `PnpmDeployOptions`              |
| `pnpm deprecate`             | `PnpmDeprecateOptions`           |
| `pnpm dist-tag`              | `PnpmDistTagOptions`             |
| `pnpm dlx`                   | `PnpmDlxOptions`                 |
| `pnpm docs`                  | `PnpmDocsOptions`                |
| `pnpm doctor`                | `PnpmDoctorOptions`              |
| `pnpm edit`                  | `PnpmEditOptions`                |
| `pnpm exec`                  | `PnpmExecOptions`                |
| `pnpm fetch`                 | `PnpmFetchOptions`               |
| `pnpm find-hash`             | `PnpmFindHashOptions`            |
| `pnpm get`                   | `PnpmGetOptions`                 |
| `pnpm ignored-builds`        | `PnpmIgnoredBuildsOptions`       |
| `pnpm import`                | `PnpmImportOptions`              |
| `pnpm init`                  | `PnpmInitOptions`                |
| `pnpm install`               | `PnpmInstallOptions`             |
| `pnpm install-test`          | `PnpmInstallTestOptions`         |
| `pnpm lane`                  | `PnpmLaneOptions`                |
| `pnpm licenses`              | `PnpmLicensesOptions`            |
| `pnpm link`                  | `PnpmLinkOptions`                |
| `pnpm list`                  | `PnpmListOptions`                |
| `pnpm ll`                    | `PnpmLlOptions`                  |
| `pnpm login`                 | `PnpmLoginOptions`               |
| `pnpm logout`                | `PnpmLogoutOptions`              |
| `pnpm outdated`              | `PnpmOutdatedOptions`            |
| `pnpm owner`                 | `PnpmOwnerOptions`               |
| `pnpm pack`                  | `PnpmPackOptions`                |
| `pnpm pack-app`              | `PnpmPackAppOptions`             |
| `pnpm patch`                 | `PnpmPatchOptions`               |
| `pnpm patch-commit`          | `PnpmPatchCommitOptions`         |
| `pnpm patch-remove`          | `PnpmPatchRemoveOptions`         |
| `pnpm peers`                 | `PnpmPeersOptions`               |
| `pnpm permissions`           | `PnpmPermissionsOptions`         |
| `pnpm permissions approve`   | `PnpmPermissionsApproveOptions`  |
| `pnpm permissions list`      | `PnpmPermissionsListOptions`     |
| `pnpm ping`                  | `PnpmPingOptions`                |
| `pnpm pipeline`              | `PnpmPipelineOptions`            |
| `pnpm pkg`                   | `PnpmPkgOptions`                 |
| `pnpm pkg delete`            | `PnpmPkgDeleteOptions`           |
| `pnpm pkg fix`               | `PnpmPkgFixOptions`              |
| `pnpm pkg get`               | `PnpmPkgGetOptions`              |
| `pnpm pkg set`               | `PnpmPkgSetOptions`              |
| `pnpm prefix`                | `PnpmPrefixOptions`              |
| `pnpm profile`               | `PnpmProfileOptions`             |
| `pnpm prune`                 | `PnpmPruneOptions`               |
| `pnpm publish`               | `PnpmPublishOptions`             |
| `pnpm purge`                 | `PnpmPurgeOptions`               |
| `pnpm rb`                    | `PnpmRbOptions`                  |
| `pnpm rebuild`               | `PnpmRebuildOptions`             |
| `pnpm recursive`             | `PnpmRecursiveOptions`           |
| `pnpm remove`                | `PnpmRemoveOptions`              |
| `pnpm repo`                  | `PnpmRepoOptions`                |
| `pnpm restart`               | `PnpmRestartOptions`             |
| `pnpm run`                   | `PnpmRunOptions`                 |
| `pnpm runtime`               | `PnpmRuntimeOptions`             |
| `pnpm sbom`                  | `PnpmSbomOptions`                |
| `pnpm search`                | `PnpmSearchOptions`              |
| `pnpm self-update`           | `PnpmSelfUpdateOptions`          |
| `pnpm set`                   | `PnpmSetOptions`                 |
| `pnpm set-script`            | `PnpmSetScriptOptions`           |
| `pnpm setup`                 | `PnpmSetupOptions`               |
| `pnpm shim`                  | `PnpmShimOptions`                |
| `pnpm stage`                 | `PnpmStageOptions`               |
| `pnpm star`                  | `PnpmStarOptions`                |
| `pnpm stars`                 | `PnpmStarsOptions`               |
| `pnpm start`                 | `PnpmStartOptions`               |
| `pnpm stop`                  | `PnpmStopOptions`                |
| `pnpm store`                 | `PnpmStoreOptions`               |
| `pnpm store add`             | `PnpmStoreAddOptions`            |
| `pnpm store path`            | `PnpmStorePathOptions`           |
| `pnpm store prune`           | `PnpmStorePruneOptions`          |
| `pnpm store status`          | `PnpmStoreStatusOptions`         |
| `pnpm tasks`                 | `PnpmTasksOptions`               |
| `pnpm tasks status`          | `PnpmTasksStatusOptions`         |
| `pnpm team`                  | `PnpmTeamOptions`                |
| `pnpm test`                  | `PnpmTestOptions`                |
| `pnpm token`                 | `PnpmTokenOptions`               |
| `pnpm undeprecate`           | `PnpmUndeprecateOptions`         |
| `pnpm unlink`                | `PnpmUnlinkOptions`              |
| `pnpm unpublish`             | `PnpmUnpublishOptions`           |
| `pnpm unstar`                | `PnpmUnstarOptions`              |
| `pnpm update`                | `PnpmUpdateOptions`              |
| `pnpm version`               | `PnpmVersionOptions`             |
| `pnpm view`                  | `PnpmViewOptions`                |
| `pnpm whoami`                | `PnpmWhoamiOptions`              |
| `pnpm why`                   | `PnpmWhyOptions`                 |
| `pnpm with`                  | `PnpmWithOptions`                |
| `pnpm xmas`                  | `PnpmXmasOptions`                |
