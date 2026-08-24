# newman CLI reference

`ModularPipelines.Newman` provides strongly typed access to the `newman` CLI.

## Executable prerequisite[​](#executable-prerequisite "Direct link to Executable prerequisite")

This package does not install the `newman` executable. Install it separately and ensure `newman` is available on `PATH`.

Follow the executable's official documentation for installation instructions.

## Package installation[​](#package-installation "Direct link to Package installation")

```
dotnet add package ModularPipelines.Newman
```

Resolve the service with `context.Tools.Newman`. For projects older than C# 14, import `ModularPipelines.Newman.Extensions` and use the `context.Newman()` extension method as a compatibility fallback.

## Module example[​](#module-example "Direct link to Module example")

Resolve the service in a module, then select a command from the table below. Unsafe or destructive commands do not receive runnable examples:

```
var newman = context.Tools.Newman;
```

## Commands[​](#commands "Direct link to Commands")

| CLI command  | Options record     |
| ------------ | ------------------ |
| `newman run` | `NewmanRunOptions` |
