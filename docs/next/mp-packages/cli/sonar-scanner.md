# sonar-scanner CLI reference

`ModularPipelines.SonarScanner` provides strongly typed access to the `sonar-scanner` CLI.

## Executable prerequisite[​](#executable-prerequisite "Direct link to Executable prerequisite")

This package does not install the `sonar-scanner` executable. Install it separately and ensure `sonar-scanner` is available on `PATH`.

The generation workflow is pinned to `sonar-scanner` version `8.0.1.6346`.

See the [sonar-scanner installation guide](https://docs.sonarsource.com/sonarqube-server/analyzing-source-code/scanners/sonarscanner).

The generator workflow downloads the Linux x64 SonarScanner CLI distribution.

## Package installation[​](#package-installation "Direct link to Package installation")

```
dotnet add package ModularPipelines.SonarScanner
```

Resolve the service with `context.Tools.SonarScanner`. For projects older than C# 14, import `ModularPipelines.SonarScanner.Extensions` and use the `context.SonarScanner()` extension method as a compatibility fallback.

## Module example[​](#module-example "Direct link to Module example")

Resolve the service in a module, then select a command from the table below. A runnable example is omitted when no command has complete safety metadata:

```
var sonarScanner = context.Tools.SonarScanner;
```

## Commands[​](#commands "Direct link to Commands")

| CLI command     | Options record               |
| --------------- | ---------------------------- |
| `sonar-scanner` | `SonarScannerExecuteOptions` |
