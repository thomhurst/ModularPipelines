# newman CLI reference

`ModularPipelines.Newman` provides strongly typed access to the `newman` CLI.

## Executable prerequisite[​](#executable-prerequisite "Direct link to Executable prerequisite")

This package does not install the `newman` executable. Install it separately and ensure `newman` is available on `PATH`.

Follow the executable's official documentation for installation instructions.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Newman
```

Import `ModularPipelines.Newman.Extensions`, then resolve the service with `context.Newman()`.

## Module example[​](#module-example "Direct link to Module example")

Resolve the service in a module, then select a command from the table below. Unsafe or destructive commands do not receive runnable examples:

```
using ModularPipelines.Newman.Extensions;



var newman = context.Newman();
```

## Commands[​](#commands "Direct link to Commands")

| CLI command  | Options record     |
| ------------ | ------------------ |
| `newman run` | `NewmanRunOptions` |
