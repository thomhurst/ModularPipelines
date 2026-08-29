# gradle CLI reference

`ModularPipelines.Java` provides strongly typed access to the `gradle` CLI.

## Executable prerequisite[​](#executable-prerequisite "Direct link to Executable prerequisite")

This package does not install the `gradle` executable. Install it separately and ensure `gradle` is available on `PATH`.

Follow the executable's official documentation for installation instructions.

## Package installation[​](#package-installation "Direct link to Package installation")

```
dotnet add package ModularPipelines.Java
```

Resolve the service with `context.Tools.Gradle`. Projects using C# 13 or another .NET language can use `context.Tools.Get<ModularPipelines.Java.Services.IGradle>()` instead.

## Module example[​](#module-example "Direct link to Module example")

Resolve the service in a module, then select a command from the table below. A runnable example is omitted when no command has complete safety metadata:

```
var gradle = context.Tools.Gradle;
```

## Commands[​](#commands "Direct link to Commands")

| CLI command | Options record         |
| ----------- | ---------------------- |
| `gradle`    | `GradleExecuteOptions` |
