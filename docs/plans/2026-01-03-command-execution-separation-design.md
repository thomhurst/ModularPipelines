# Command Execution Options Separation Design

> **Issue**: #1811
> **Status**: Approved
> **Date**: 2026-01-03

## Problem Statement

Command options classes currently inherit execution-related properties (timeout, working directory, environment variables, etc.) from base classes. This creates:

1. **Naming conflicts** - CLI tools may have options that conflict with inherited property names (e.g., `--working-directory`, `--timeout`)
2. **SRP violation** - Tool options classes have two responsibilities: defining CLI arguments AND configuring execution behavior

## Design Decisions

| Decision | Choice |
|----------|--------|
| Backward compatibility | Breaking changes acceptable |
| Scope | Everything - core framework + regenerate all tool classes |
| Defaults | Global defaults + tool-specific defaults + per-call overrides |
| Method signature | Separate parameters: `(toolOptions, executionOptions?, cancellationToken)` |
| Base type | Abstract record `CommandLineToolOptions` (slimmed down) |
| Naming | Keep current names, rename `CommandLineOptions` → `CommandExecutionOptions` |
| `OptionsObject` | Remove - redundant with attributed tool options |
| Tool inheritance | Keep hierarchy (e.g., `DockerRunOptions : DockerOptions : CommandLineToolOptions`) |

## Core Type Changes

### CommandExecutionOptions (renamed from CommandLineOptions)

```csharp
public record CommandExecutionOptions
{
    public IDictionary<string, string?>? EnvironmentVariables { get; init; }
    public string? WorkingDirectory { get; init; }
    public TimeSpan? ExecutionTimeout { get; init; }
    public TimeSpan GracefulShutdownTimeout { get; init; } = TimeSpan.FromSeconds(30);
    public bool ThrowOnNonZeroExitCode { get; init; } = true;
    public CommandLoggingOptions? LogSettings { get; init; }
    public Func<string, string>? InputLoggingManipulator { get; init; }
    public Func<string, string>? OutputLoggingManipulator { get; init; }
    public bool Sudo { get; init; }
    public Credentials? CommandLineCredentials { get; init; }
}
```

### CommandLineToolOptions (slimmed down)

```csharp
public abstract record CommandLineToolOptions(string Tool)
{
    public string[]? CommandParts { get; init; }
    public IEnumerable<string>? Arguments { get; init; }
    public IEnumerable<string>? RunSettings { get; init; }
}
```

**Removed**: All execution-related properties and `OptionsObject`.

## Defaults Hierarchy

### Global defaults via PipelineOptions

```csharp
public class PipelineOptions
{
    // Existing properties...

    /// <summary>
    /// Default execution options applied to all commands unless overridden.
    /// </summary>
    public CommandExecutionOptions DefaultExecutionOptions { get; set; } = new();
}
```

### Tool-specific defaults

```csharp
public static class DockerExtensions
{
    public static IPipelineHostBuilder AddDocker(
        this IPipelineHostBuilder builder,
        Action<DockerConfiguration>? configure = null);
}

public class DockerConfiguration
{
    /// <summary>
    /// Default execution options for all Docker commands.
    /// Falls back to PipelineOptions.DefaultExecutionOptions if not set.
    /// </summary>
    public CommandExecutionOptions? DefaultExecutionOptions { get; set; }
}
```

### Resolution order (first non-null wins)

1. Per-call `executionOptions` parameter
2. Tool-specific `DockerConfiguration.DefaultExecutionOptions`
3. Global `PipelineOptions.DefaultExecutionOptions`
4. Hardcoded sensible defaults

## Method Signatures

### Tool service interface

```csharp
public interface IDocker
{
    Task<CommandResult> Run(
        DockerRunOptions options,
        CommandExecutionOptions? executionOptions = null,
        CancellationToken cancellationToken = default);
}
```

### Base command execution

```csharp
public interface ICommand
{
    Task<CommandResult> ExecuteCommandLineTool(
        CommandLineToolOptions toolOptions,
        CommandExecutionOptions? executionOptions = null,
        CancellationToken cancellationToken = default);
}
```

### Usage examples

```csharp
// Simple - uses defaults
await docker.Run(new DockerRunOptions { Image = "nginx" });

// With execution override
await docker.Run(
    new DockerRunOptions { Image = "nginx" },
    new CommandExecutionOptions { ExecutionTimeout = TimeSpan.FromMinutes(10) });

// With cancellation token only
await docker.Run(
    new DockerRunOptions { Image = "nginx" },
    cancellationToken: ct);
```

## Generator Changes

1. Update base class templates - `CommandLineToolOptions` no longer has execution properties
2. Remove any references to `OptionsObject` in generated code
3. Update generated service interfaces/implementations to use new method signature
4. Add tool configuration classes (e.g., `DockerConfiguration`)
5. Regenerate all tool packages

## Implementation Phases

### Phase 1 - Core Framework

1. Create `CommandExecutionOptions.cs`
2. Slim down `CommandLineToolOptions`
3. Delete/rename old `CommandLineOptions.cs`
4. Update `ICommand` and `Command` to use new signature
5. Add `DefaultExecutionOptions` to `PipelineOptions`
6. Update `CommandServiceBase`

### Phase 2 - Generators

1. Update base class templates in `OptionsGenerator`
2. Update service interface/implementation templates
3. Add tool configuration class templates

### Phase 3 - Regenerate Tool Packages

Run generators for: Docker, DotNet, Git, Helm, Kubernetes, Terraform, Azure CLI, AWS, etc.

### Phase 4 - Tests & Examples

1. Update unit tests
2. Update example pipelines

## Breaking Changes

```csharp
// OLD - no longer compiles
var options = new DockerRunOptions
{
    Image = "nginx",
    WorkingDirectory = "/app",  // Property gone
};
await docker.Run(options);

// NEW
var toolOptions = new DockerRunOptions { Image = "nginx" };
var execOptions = new CommandExecutionOptions { WorkingDirectory = "/app" };
await docker.Run(toolOptions, execOptions);
```

## Files Impacted

- `src/ModularPipelines/Options/CommandLineOptions.cs` → rename
- `src/ModularPipelines/Options/CommandLineToolOptions.cs` → slim down
- `src/ModularPipelines/Context/Command.cs`
- `src/ModularPipelines/Context/ICommand.cs`
- `src/ModularPipelines/PipelineOptions.cs`
- All tool service implementations
- All generated options classes (via regeneration)
- Generator templates in `tools/ModularPipelines.OptionsGenerator/`
