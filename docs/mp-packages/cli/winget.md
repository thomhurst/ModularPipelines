# winget CLI reference

`ModularPipelines.WinGet` provides strongly typed access to the `winget` CLI.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.WinGet
```

Import `ModularPipelines.WinGet.Extensions`, then resolve the service with `context.Winget()`.

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines.Context;

using ModularPipelines.Models;

using ModularPipelines.Modules;

using ModularPipelines.WinGet.Extensions;

using ModularPipelines.WinGet.Options;



public class RunCommandModule : Module<CommandResult>

{

    protected override async Task<CommandResult?> ExecuteAsync(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        return await context.Winget().Install(

            new WingetInstallOptions(),

            cancellationToken: cancellationToken);

    }

}
```

## Commands[​](#commands "Direct link to Commands")

| CLI command        | Options record           |
| ------------------ | ------------------------ |
| `winget configure` | `WingetConfigureOptions` |
| `winget download`  | `WingetDownloadOptions`  |
| `winget dscv3`     | `WingetDscv3Options`     |
| `winget export`    | `WingetExportOptions`    |
| `winget features`  | `WingetFeaturesOptions`  |
| `winget hash`      | `WingetHashOptions`      |
| `winget import`    | `WingetImportOptions`    |
| `winget install`   | `WingetInstallOptions`   |
| `winget list`      | `WingetListOptions`      |
| `winget pin`       | `WingetPinOptions`       |
| `winget repair`    | `WingetRepairOptions`    |
| `winget search`    | `WingetSearchOptions`    |
| `winget settings`  | `WingetSettingsOptions`  |
| `winget show`      | `WingetShowOptions`      |
| `winget source`    | `WingetSourceOptions`    |
| `winget uninstall` | `WingetUninstallOptions` |
| `winget upgrade`   | `WingetUpgradeOptions`   |
| `winget validate`  | `WingetValidateOptions`  |
