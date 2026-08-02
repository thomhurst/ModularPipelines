# Interface Hierarchy

ModularPipelines exposes one shared pipeline context plus specialized contexts for
module execution and module lifecycle hooks.

## Context types

```text
IPipelineContext
├── IModuleContext
└── IModuleHookContext
```

- `IPipelineContext` exposes capabilities shared by the entire pipeline.
- `IModuleContext` adds module result lookup and submodule operations.
- `IModuleHookContext` adds information about the module whose lifecycle event is
  being observed.

The former hook-only marker was removed. Pipeline-level hooks, requirements, and
run conditions now receive `IPipelineContext` directly.

## Pipeline capabilities

`IPipelineContext` organizes capabilities by domain:

| Property | Interface | Purpose |
|---|---|---|
| `Shell` | `IShellContext` | Commands, Bash, and PowerShell |
| `Files` | `IFilesContext` | File operations, ZIP archives, and checksums |
| `Data` | `IDataContext` | JSON, XML, YAML, Base64, and hexadecimal data |
| `Environment` | `IEnvironmentDomainContext` | Environment and build-system information |
| `Installers` | `IInstallersContext` | Platform and predefined installers |
| `Network` | `INetworkContext` | HTTP and downloads |
| `Security` | `ISecurityContext` | Certificates and hashing |
| `Services` | `IServicesContext` | Dependency injection and configuration |

Each leaf capability has one public interface in its domain namespace. For example,
general command execution uses
`ModularPipelines.Context.Domains.Shell.ICommandContext`.

## Modules

Modules receive `IModuleContext`:

```csharp
public class BuildModule : Module<CommandResult>
{
    protected override Task<CommandResult> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken)
    {
        return context.Shell.Command.ExecuteCommandLineToolAsync(
            new DotNetBuildOptions(),
            cancellationToken: cancellationToken);
    }

    private sealed record DotNetBuildOptions : CommandLineToolOptions
    {
        public DotNetBuildOptions()
        {
            Tool = "dotnet";
            Arguments = ["build"];
        }
    }
}
```

## Pipeline hooks

Global hooks receive `IPipelineContext`:

```csharp
public class PipelineHooks : IPipelineGlobalHooks
{
    public Task OnPipelineStartAsync(IPipelineContext context)
    {
        context.Logger.LogInformation("Pipeline starting");
        return Task.CompletedTask;
    }

    public Task OnPipelineEndAsync(
        IPipelineContext context,
        PipelineSummary pipelineSummary)
    {
        context.Logger.LogInformation("Pipeline finished");
        return Task.CompletedTask;
    }
}
```

Global module event receivers receive `IModuleHookContext`:

```csharp
public class ModuleEvents : IModuleEventReceiver
{
    public Task OnModuleStartAsync(IModuleHookContext context)
    {
        context.Logger.LogInformation("Module starting");
        return Task.CompletedTask;
    }

    public Task OnModuleEndAsync(IModuleHookContext context)
    {
        context.Logger.LogInformation("Module finished");
        return Task.CompletedTask;
    }
}
```

## Requirements and run conditions

Pipeline requirements and run conditions receive `IPipelineContext`, giving them the
same shared capability surface as global hooks:

```csharp
public class LinuxRequirement : IPipelineRequirement
{
    public Task<RequirementDecision> MustAsync(IPipelineContext context)
    {
        return Task.FromResult(
            context.Environment.OperatingSystem == OSPlatform.Linux
                ? RequirementDecision.Passed
                : RequirementDecision.Failed("Linux is required"));
    }
}
```

## Guidance

1. Use `IModuleContext` in modules.
2. Use `IPipelineContext` in global hooks, requirements, and run conditions.
3. Use `IModuleHookContext` in module event receivers and attribute handlers.
4. Use domain properties to discover capabilities.
5. Do not depend on internal engine interfaces.
