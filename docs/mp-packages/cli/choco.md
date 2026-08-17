# choco CLI reference

`ModularPipelines.Chocolatey` provides strongly typed access to the `choco` CLI.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Chocolatey
```

Import `ModularPipelines.Chocolatey.Extensions`, then resolve the service with `context.Choco()`.

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines.Context;

using ModularPipelines.Models;

using ModularPipelines.Modules;

using ModularPipelines.Chocolatey.Extensions;

using ModularPipelines.Chocolatey.Options;



public class RunCommandModule : Module<CommandResult>

{

    protected override async Task<CommandResult?> ExecuteAsync(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        return await context.Choco().Apikey(

            new ChocoApikeyOptions(),

            cancellationToken: cancellationToken);

    }

}
```

## Commands[​](#commands "Direct link to Commands")

| CLI command        | Options record           |
| ------------------ | ------------------------ |
| `choco apikey`     | `ChocoApikeyOptions`     |
| `choco cache`      | `ChocoCacheOptions`      |
| `choco config`     | `ChocoConfigOptions`     |
| `choco export`     | `ChocoExportOptions`     |
| `choco feature`    | `ChocoFeatureOptions`    |
| `choco features`   | `ChocoFeaturesOptions`   |
| `choco find`       | `ChocoFindOptions`       |
| `choco info`       | `ChocoInfoOptions`       |
| `choco install`    | `ChocoInstallOptions`    |
| `choco license`    | `ChocoLicenseOptions`    |
| `choco list`       | `ChocoListOptions`       |
| `choco new`        | `ChocoNewOptions`        |
| `choco outdated`   | `ChocoOutdatedOptions`   |
| `choco pack`       | `ChocoPackOptions`       |
| `choco pin`        | `ChocoPinOptions`        |
| `choco push`       | `ChocoPushOptions`       |
| `choco rule`       | `ChocoRuleOptions`       |
| `choco search`     | `ChocoSearchOptions`     |
| `choco setapikey`  | `ChocoSetapikeyOptions`  |
| `choco source`     | `ChocoSourceOptions`     |
| `choco sources`    | `ChocoSourcesOptions`    |
| `choco support`    | `ChocoSupportOptions`    |
| `choco template`   | `ChocoTemplateOptions`   |
| `choco templates`  | `ChocoTemplatesOptions`  |
| `choco uninstall`  | `ChocoUninstallOptions`  |
| `choco unpackself` | `ChocoUnpackselfOptions` |
| `choco upgrade`    | `ChocoUpgradeOptions`    |
