# yq CLI reference

`ModularPipelines.Yq` provides strongly typed access to the `yq` CLI.

## Executable prerequisite[​](#executable-prerequisite "Direct link to Executable prerequisite")

This package does not install the `yq` executable. Install it separately and ensure `yq` is available on `PATH`.

Follow the executable's official documentation for installation instructions.

## Package installation[​](#package-installation "Direct link to Package installation")

```
dotnet add package ModularPipelines.Yq
```

Resolve the service with `context.Tools.Yq`. For projects older than C# 14, import `ModularPipelines.Yq.Extensions` and use the `context.Yq()` extension method as a compatibility fallback.

## Module example[​](#module-example "Direct link to Module example")

Resolve the service in a module, then select a command from the table below. A runnable example is omitted when no command has complete safety metadata:

```
var yq = context.Tools.Yq;
```

## Commands[​](#commands "Direct link to Commands")

| CLI command   | Options record     |
| ------------- | ------------------ |
| `yq eval`     | `YqEvalOptions`    |
| `yq eval-all` | `YqEvalAllOptions` |
