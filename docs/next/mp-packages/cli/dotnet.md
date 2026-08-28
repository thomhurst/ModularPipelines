# dotnet CLI reference

`ModularPipelines.DotNet` provides strongly typed access to the `dotnet` CLI.

## Executable prerequisite[​](#executable-prerequisite "Direct link to Executable prerequisite")

This package does not install the `dotnet` executable. Install it separately and ensure `dotnet` is available on `PATH`.

Follow the executable's official documentation for installation instructions.

## Package installation[​](#package-installation "Direct link to Package installation")

```
dotnet add package ModularPipelines.DotNet
```

Resolve the service with `context.Tools.DotNet`. For projects older than C# 14, import `ModularPipelines.DotNet.Extensions` and use the `context.DotNet()` extension method as a compatibility fallback.

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines.Context;

using ModularPipelines.Models;

using ModularPipelines.Modules;

using ModularPipelines.DotNet.Options;



public class RunCommandModule : Module<CommandResult>

{

    protected override async Task<CommandResult> ExecuteAsync(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        return await context.Tools.DotNet.Workload.ListAsync(

            new DotNetWorkloadListOptions(),

            cancellationToken: cancellationToken);

    }

}
```

## Commands[​](#commands "Direct link to Commands")

| CLI command                       | Options record                       |
| --------------------------------- | ------------------------------------ |
| `dotnet build`                    | `DotNetBuildOptions`                 |
| `dotnet build-server`             | `DotNetBuildServerOptions`           |
| `dotnet build-server shutdown`    | `DotNetBuildServerShutdownOptions`   |
| `dotnet clean`                    | `DotNetCleanOptions`                 |
| `dotnet format`                   | `DotNetFormatOptions`                |
| `dotnet format analyzers`         | `DotNetFormatAnalyzersOptions`       |
| `dotnet format style`             | `DotNetFormatStyleOptions`           |
| `dotnet format whitespace`        | `DotNetFormatWhitespaceOptions`      |
| `dotnet msbuild`                  | `DotNetMsbuildOptions`               |
| `dotnet new`                      | `DotNetNewOptions`                   |
| `dotnet new create`               | `DotNetNewCreateOptions`             |
| `dotnet new details`              | `DotNetNewDetailsOptions`            |
| `dotnet new install`              | `DotNetNewInstallOptions`            |
| `dotnet new list`                 | `DotNetNewListOptions`               |
| `dotnet new search`               | `DotNetNewSearchOptions`             |
| `dotnet new uninstall`            | `DotNetNewUninstallOptions`          |
| `dotnet new update`               | `DotNetNewUpdateOptions`             |
| `dotnet nuget`                    | `DotNetNuGetOptions`                 |
| `dotnet nuget add`                | `DotNetNuGetAddOptions`              |
| `dotnet nuget add client-cert`    | `DotNetNuGetAddClientCertOptions`    |
| `dotnet nuget add source`         | `DotNetNuGetAddSourceOptions`        |
| `dotnet nuget config`             | `DotNetNuGetConfigOptions`           |
| `dotnet nuget config get`         | `DotNetNuGetConfigGetOptions`        |
| `dotnet nuget config paths`       | `DotNetNuGetConfigPathsOptions`      |
| `dotnet nuget config set`         | `DotNetNuGetConfigSetOptions`        |
| `dotnet nuget config unset`       | `DotNetNuGetConfigUnsetOptions`      |
| `dotnet nuget delete`             | `DotNetNuGetDeleteOptions`           |
| `dotnet nuget disable`            | `DotNetNuGetDisableOptions`          |
| `dotnet nuget disable source`     | `DotNetNuGetDisableSourceOptions`    |
| `dotnet nuget enable`             | `DotNetNuGetEnableOptions`           |
| `dotnet nuget enable source`      | `DotNetNuGetEnableSourceOptions`     |
| `dotnet nuget list`               | `DotNetNuGetListOptions`             |
| `dotnet nuget list client-cert`   | `DotNetNuGetListClientCertOptions`   |
| `dotnet nuget list source`        | `DotNetNuGetListSourceOptions`       |
| `dotnet nuget locals`             | `DotNetNuGetLocalsOptions`           |
| `dotnet nuget push`               | `DotNetNuGetPushOptions`             |
| `dotnet nuget remove`             | `DotNetNuGetRemoveOptions`           |
| `dotnet nuget remove client-cert` | `DotNetNuGetRemoveClientCertOptions` |
| `dotnet nuget remove source`      | `DotNetNuGetRemoveSourceOptions`     |
| `dotnet nuget sign`               | `DotNetNuGetSignOptions`             |
| `dotnet nuget trust`              | `DotNetNuGetTrustOptions`            |
| `dotnet nuget trust author`       | `DotNetNuGetTrustAuthorOptions`      |
| `dotnet nuget trust certificate`  | `DotNetNuGetTrustCertificateOptions` |
| `dotnet nuget trust list`         | `DotNetNuGetTrustListOptions`        |
| `dotnet nuget trust remove`       | `DotNetNuGetTrustRemoveOptions`      |
| `dotnet nuget trust repository`   | `DotNetNuGetTrustRepositoryOptions`  |
| `dotnet nuget trust source`       | `DotNetNuGetTrustSourceOptions`      |
| `dotnet nuget trust sync`         | `DotNetNuGetTrustSyncOptions`        |
| `dotnet nuget update`             | `DotNetNuGetUpdateOptions`           |
| `dotnet nuget update client-cert` | `DotNetNuGetUpdateClientCertOptions` |
| `dotnet nuget update source`      | `DotNetNuGetUpdateSourceOptions`     |
| `dotnet nuget verify`             | `DotNetNuGetVerifyOptions`           |
| `dotnet nuget why`                | `DotNetNuGetWhyOptions`              |
| `dotnet pack`                     | `DotNetPackOptions`                  |
| `dotnet package`                  | `DotNetPackageOptions`               |
| `dotnet package add`              | `DotNetPackageAddOptions`            |
| `dotnet package download`         | `DotNetPackageDownloadOptions`       |
| `dotnet package list`             | `DotNetPackageListOptions`           |
| `dotnet package remove`           | `DotNetPackageRemoveOptions`         |
| `dotnet package search`           | `DotNetPackageSearchOptions`         |
| `dotnet package update`           | `DotNetPackageUpdateOptions`         |
| `dotnet publish`                  | `DotNetPublishOptions`               |
| `dotnet reference`                | `DotNetReferenceOptions`             |
| `dotnet reference add`            | `DotNetReferenceAddOptions`          |
| `dotnet reference list`           | `DotNetReferenceListOptions`         |
| `dotnet reference remove`         | `DotNetReferenceRemoveOptions`       |
| `dotnet restore`                  | `DotNetRestoreOptions`               |
| `dotnet run`                      | `DotNetRunOptions`                   |
| `dotnet sdk`                      | `DotNetSdkOptions`                   |
| `dotnet sdk check`                | `DotNetSdkCheckOptions`              |
| `dotnet solution`                 | `DotNetSolutionOptions`              |
| `dotnet solution add`             | `DotNetSolutionAddOptions`           |
| `dotnet solution list`            | `DotNetSolutionListOptions`          |
| `dotnet solution migrate`         | `DotNetSolutionMigrateOptions`       |
| `dotnet solution remove`          | `DotNetSolutionRemoveOptions`        |
| `dotnet store`                    | `DotNetStoreOptions`                 |
| `dotnet test`                     | `DotNetTestOptions`                  |
| `dotnet tool`                     | `DotNetToolOptions`                  |
| `dotnet tool install`             | `DotNetToolInstallOptions`           |
| `dotnet tool list`                | `DotNetToolListOptions`              |
| `dotnet tool restore`             | `DotNetToolRestoreOptions`           |
| `dotnet tool run`                 | `DotNetToolRunOptions`               |
| `dotnet tool search`              | `DotNetToolSearchOptions`            |
| `dotnet tool uninstall`           | `DotNetToolUninstallOptions`         |
| `dotnet tool update`              | `DotNetToolUpdateOptions`            |
| `dotnet vstest`                   | `DotNetVstestOptions`                |
| `dotnet workload`                 | `DotNetWorkloadOptions`              |
| `dotnet workload clean`           | `DotNetWorkloadCleanOptions`         |
| `dotnet workload config`          | `DotNetWorkloadConfigOptions`        |
| `dotnet workload history`         | `DotNetWorkloadHistoryOptions`       |
| `dotnet workload install`         | `DotNetWorkloadInstallOptions`       |
| `dotnet workload list`            | `DotNetWorkloadListOptions`          |
| `dotnet workload repair`          | `DotNetWorkloadRepairOptions`        |
| `dotnet workload restore`         | `DotNetWorkloadRestoreOptions`       |
| `dotnet workload search`          | `DotNetWorkloadSearchOptions`        |
| `dotnet workload uninstall`       | `DotNetWorkloadUninstallOptions`     |
| `dotnet workload update`          | `DotNetWorkloadUpdateOptions`        |
