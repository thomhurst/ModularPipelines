# flyway CLI reference

`ModularPipelines.Flyway` provides strongly typed access to the `flyway` CLI.

## Executable prerequisite[​](#executable-prerequisite "Direct link to Executable prerequisite")

This package does not install the `flyway` executable. Install it separately and ensure `flyway` is available on `PATH`.

Follow the executable's official documentation for installation instructions.

## Package installation[​](#package-installation "Direct link to Package installation")

```
dotnet add package ModularPipelines.Flyway
```

Resolve the service with `context.Tools.Flyway`. For projects older than C# 14, import `ModularPipelines.Flyway.Extensions` and use the `context.Flyway()` extension method as a compatibility fallback.

## Module example[​](#module-example "Direct link to Module example")

Resolve the service in a module, then select a command from the table below. A runnable example is omitted when no command has complete safety metadata:

```
var flyway = context.Tools.Flyway;
```

## Commands[​](#commands "Direct link to Commands")

| CLI command           | Options record             |
| --------------------- | -------------------------- |
| `flyway add`          | `FlywayAddOptions`         |
| `flyway auth`         | `FlywayAuthOptions`        |
| `flyway baseline`     | `FlywayBaselineOptions`    |
| `flyway check`        | `FlywayCheckOptions`       |
| `flyway clean`        | `FlywayCleanOptions`       |
| `flyway deploy`       | `FlywayDeployOptions`      |
| `flyway diff`         | `FlywayDiffOptions`        |
| `flyway diffApply`    | `FlywayDiffApplyOptions`   |
| `flyway diffText`     | `FlywayDiffTextOptions`    |
| `flyway generate`     | `FlywayGenerateOptions`    |
| `flyway info`         | `FlywayInfoOptions`        |
| `flyway init`         | `FlywayInitOptions`        |
| `flyway list-engines` | `FlywayListEnginesOptions` |
| `flyway migrate`      | `FlywayMigrateOptions`     |
| `flyway prepare`      | `FlywayPrepareOptions`     |
| `flyway repair`       | `FlywayRepairOptions`      |
| `flyway snapshot`     | `FlywaySnapshotOptions`    |
| `flyway undo`         | `FlywayUndoOptions`        |
| `flyway validate`     | `FlywayValidateOptions`    |
