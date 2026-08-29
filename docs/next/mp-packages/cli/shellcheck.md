# shellcheck CLI reference

`ModularPipelines.Shellcheck` provides strongly typed access to the `shellcheck` CLI.

## Executable prerequisite[​](#executable-prerequisite "Direct link to Executable prerequisite")

This package does not install the `shellcheck` executable. Install it separately and ensure `shellcheck` is available on `PATH`.

Follow the executable's official documentation for installation instructions.

## Package installation[​](#package-installation "Direct link to Package installation")

```
dotnet add package ModularPipelines.Shellcheck
```

Resolve the service with `context.Tools.Shellcheck`. Projects using C# 13 or another .NET language can use `context.Tools.Get<ModularPipelines.Shellcheck.Services.IShellcheck>()` instead.

## Module example[​](#module-example "Direct link to Module example")

Resolve the service in a module, then select a command from the table below. A runnable example is omitted when no command has complete safety metadata:

```
var shellcheck = context.Tools.Shellcheck;
```

## Commands[​](#commands "Direct link to Commands")

| CLI command  | Options record             |
| ------------ | -------------------------- |
| `shellcheck` | `ShellcheckExecuteOptions` |
