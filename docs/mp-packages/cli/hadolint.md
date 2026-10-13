# hadolint CLI reference

`ModularPipelines.Hadolint` provides strongly typed access to the `hadolint` CLI.

## Executable prerequisite[​](#executable-prerequisite "Direct link to Executable prerequisite")

This package does not install the `hadolint` executable. Install it separately and ensure `hadolint` is available on `PATH`.

Follow the executable's official documentation for installation instructions.

## Package installation[​](#package-installation "Direct link to Package installation")

```
dotnet add package ModularPipelines.Hadolint
```

Resolve the service with `context.Tools.Hadolint`. Projects using C# 13 or another .NET language can use `context.Tools.Get<ModularPipelines.Hadolint.Services.IHadolint>()` instead.

## Module example[​](#module-example "Direct link to Module example")

Resolve the service in a module, then select a command from the table below. A runnable example is omitted when no command has complete safety metadata:

```
var hadolint = context.Tools.Hadolint;
```

## Commands[​](#commands "Direct link to Commands")

| CLI command | Options record           |
| ----------- | ------------------------ |
| `hadolint`  | `HadolintExecuteOptions` |
