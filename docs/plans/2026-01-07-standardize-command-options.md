# Standardize Command Options with Fluent Builder Pattern

> **Issue**: #1872
> **Status**: Draft
> **Date**: 2026-01-07
> **Priority**: Medium

## Problem Statement

Command options across tool integrations follow inconsistent patterns:

1. **No Fluent API** - Users must construct options objects manually with verbose syntax
2. **Execution options separated** - `CommandExecutionOptions` passed as separate parameter, not chainable
3. **Discoverability** - IDE autocompletion doesn't guide users through available options
4. **Verbosity** - Simple operations require creating full options objects

### Current Usage Pattern

```csharp
// Verbose - requires knowing the options class name and properties
await context.DotNet().Build(
    new DotNetBuildOptions
    {
        Configuration = "Release",
        Framework = "net8.0",
        NoRestore = true
    },
    new CommandExecutionOptions
    {
        ExecutionTimeout = TimeSpan.FromMinutes(5),
        WorkingDirectory = "/src"
    });
```

### Desired Usage Pattern

```csharp
// Fluent - discoverable, chainable, concise
await context.DotNet().Build()
    .WithConfiguration("Release")
    .WithFramework("net8.0")
    .WithNoRestore()
    .WithTimeout(TimeSpan.FromMinutes(5))
    .WithWorkingDirectory("/src")
    .ExecuteAsync();
```

## Design Decisions

| Decision | Choice | Rationale |
|----------|--------|-----------|
| Backward compatibility | Full - both patterns coexist | Existing code continues to work |
| Builder returns | `ICommandBuilder<TOptions>` | Type-safe, enables chaining |
| Execution method | `ExecuteAsync()` terminal operation | Clear execution point |
| Options exposure | Builder can return built options | Enables hybrid usage |
| Generation strategy | Extend existing generator | Consistent with codebase patterns |
| Execution options | Unified in chain | Single fluent chain for all configuration |

## Architecture Overview

### Component Diagram

```
IDotNet (Service)
    |
    +-- Build(options?)           -> Task<CommandResult>     [Existing - unchanged]
    |
    +-- Build()                   -> IDotNetBuildBuilder     [New - fluent entry point]
            |
            +-- WithConfiguration()  -> IDotNetBuildBuilder
            +-- WithFramework()      -> IDotNetBuildBuilder
            +-- WithTimeout()        -> IDotNetBuildBuilder  [From execution options]
            +-- ...
            +-- ExecuteAsync()       -> Task<CommandResult>  [Terminal operation]
            +-- ToOptions()          -> (DotNetBuildOptions, CommandExecutionOptions)
```

## Core Interfaces

### Base Builder Interface

```csharp
namespace ModularPipelines.Builders;

/// <summary>
/// Base interface for all command builders providing execution options.
/// </summary>
public interface ICommandBuilder
{
    /// <summary>
    /// Sets the working directory for command execution.
    /// </summary>
    ICommandBuilder WithWorkingDirectory(string directory);

    /// <summary>
    /// Sets the execution timeout.
    /// </summary>
    ICommandBuilder WithTimeout(TimeSpan timeout);

    /// <summary>
    /// Sets environment variables for the command.
    /// </summary>
    ICommandBuilder WithEnvironmentVariable(string key, string value);

    /// <summary>
    /// Sets multiple environment variables.
    /// </summary>
    ICommandBuilder WithEnvironmentVariables(IDictionary<string, string?> variables);

    /// <summary>
    /// Configures the command to run with sudo.
    /// </summary>
    ICommandBuilder WithSudo(bool sudo = true);

    /// <summary>
    /// Configures whether to throw on non-zero exit code.
    /// </summary>
    ICommandBuilder WithThrowOnError(bool throwOnError = true);

    /// <summary>
    /// Sets the graceful shutdown timeout.
    /// </summary>
    ICommandBuilder WithGracefulShutdownTimeout(TimeSpan timeout);

    /// <summary>
    /// Configures logging options for the command.
    /// </summary>
    ICommandBuilder WithLogging(CommandLoggingOptions options);

    /// <summary>
    /// Configures logging options using a builder action.
    /// </summary>
    ICommandBuilder WithLogging(Action<CommandLoggingOptions> configure);
}

/// <summary>
/// Typed command builder with tool-specific options.
/// </summary>
/// <typeparam name="TBuilder">The concrete builder type for fluent chaining.</typeparam>
/// <typeparam name="TOptions">The tool options type.</typeparam>
public interface ICommandBuilder<TBuilder, TOptions> : ICommandBuilder
    where TBuilder : ICommandBuilder<TBuilder, TOptions>
    where TOptions : CommandLineToolOptions
{
    /// <summary>
    /// Executes the command with the configured options.
    /// </summary>
    Task<CommandResult> ExecuteAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Returns the built options without executing.
    /// Useful for inspection, testing, or hybrid usage patterns.
    /// </summary>
    (TOptions ToolOptions, CommandExecutionOptions ExecutionOptions) ToOptions();
}
```

### Tool-Specific Builder Interface (Generated)

```csharp
namespace ModularPipelines.DotNet.Builders;

/// <summary>
/// Fluent builder for dotnet build command.
/// </summary>
public interface IDotNetBuildBuilder : ICommandBuilder<IDotNetBuildBuilder, DotNetBuildOptions>
{
    /// <summary>
    /// Sets the target framework to build for.
    /// </summary>
    IDotNetBuildBuilder WithFramework(string framework);

    /// <summary>
    /// Sets the build configuration (Debug/Release).
    /// </summary>
    IDotNetBuildBuilder WithConfiguration(string configuration);

    /// <summary>
    /// Sets the target runtime.
    /// </summary>
    IDotNetBuildBuilder WithRuntime(string runtime);

    /// <summary>
    /// Sets the output directory.
    /// </summary>
    IDotNetBuildBuilder WithOutput(string outputPath);

    /// <summary>
    /// Disables restore before build.
    /// </summary>
    IDotNetBuildBuilder WithNoRestore(bool noRestore = true);

    /// <summary>
    /// Disables incremental building.
    /// </summary>
    IDotNetBuildBuilder WithNoIncremental(bool noIncremental = true);

    /// <summary>
    /// Sets the project or solution to build.
    /// </summary>
    IDotNetBuildBuilder ForProject(string projectPath);

    /// <summary>
    /// Adds an MSBuild property.
    /// </summary>
    IDotNetBuildBuilder WithProperty(string name, string value);

    // ... additional methods for each DotNetBuildOptions property
}
```

## Implementation Classes

### Abstract Base Builder

```csharp
namespace ModularPipelines.Builders;

/// <summary>
/// Base implementation for command builders.
/// </summary>
public abstract class CommandBuilderBase<TBuilder, TOptions> : ICommandBuilder<TBuilder, TOptions>
    where TBuilder : CommandBuilderBase<TBuilder, TOptions>
    where TOptions : CommandLineToolOptions, new()
{
    protected readonly ICommand Command;
    protected TOptions ToolOptions;
    protected CommandExecutionOptions ExecutionOptions = new();

    protected CommandBuilderBase(ICommand command)
    {
        Command = command;
        ToolOptions = new TOptions();
    }

    protected CommandBuilderBase(ICommand command, TOptions initialOptions)
    {
        Command = command;
        ToolOptions = initialOptions;
    }

    // Fluent method implementation pattern
    protected TBuilder Self => (TBuilder)this;

    public TBuilder WithWorkingDirectory(string directory)
    {
        ExecutionOptions = ExecutionOptions with { WorkingDirectory = directory };
        return Self;
    }

    public TBuilder WithTimeout(TimeSpan timeout)
    {
        ExecutionOptions = ExecutionOptions with { ExecutionTimeout = timeout };
        return Self;
    }

    public TBuilder WithEnvironmentVariable(string key, string value)
    {
        var vars = ExecutionOptions.EnvironmentVariables?.ToDictionary(k => k.Key, v => v.Value)
            ?? new Dictionary<string, string?>();
        vars[key] = value;
        ExecutionOptions = ExecutionOptions with { EnvironmentVariables = vars };
        return Self;
    }

    public TBuilder WithEnvironmentVariables(IDictionary<string, string?> variables)
    {
        var vars = ExecutionOptions.EnvironmentVariables?.ToDictionary(k => k.Key, v => v.Value)
            ?? new Dictionary<string, string?>();
        foreach (var kvp in variables)
        {
            vars[kvp.Key] = kvp.Value;
        }
        ExecutionOptions = ExecutionOptions with { EnvironmentVariables = vars };
        return Self;
    }

    public TBuilder WithSudo(bool sudo = true)
    {
        ExecutionOptions = ExecutionOptions with { Sudo = sudo };
        return Self;
    }

    public TBuilder WithThrowOnError(bool throwOnError = true)
    {
        ExecutionOptions = ExecutionOptions with { ThrowOnNonZeroExitCode = throwOnError };
        return Self;
    }

    public TBuilder WithGracefulShutdownTimeout(TimeSpan timeout)
    {
        ExecutionOptions = ExecutionOptions with { GracefulShutdownTimeout = timeout };
        return Self;
    }

    public TBuilder WithLogging(CommandLoggingOptions options)
    {
        ExecutionOptions = ExecutionOptions with { LogSettings = options };
        return Self;
    }

    public TBuilder WithLogging(Action<CommandLoggingOptions> configure)
    {
        var options = new CommandLoggingOptions();
        configure(options);
        return WithLogging(options);
    }

    // Non-generic interface implementations (explicit)
    ICommandBuilder ICommandBuilder.WithWorkingDirectory(string directory) => WithWorkingDirectory(directory);
    ICommandBuilder ICommandBuilder.WithTimeout(TimeSpan timeout) => WithTimeout(timeout);
    ICommandBuilder ICommandBuilder.WithEnvironmentVariable(string key, string value) => WithEnvironmentVariable(key, value);
    ICommandBuilder ICommandBuilder.WithEnvironmentVariables(IDictionary<string, string?> variables) => WithEnvironmentVariables(variables);
    ICommandBuilder ICommandBuilder.WithSudo(bool sudo) => WithSudo(sudo);
    ICommandBuilder ICommandBuilder.WithThrowOnError(bool throwOnError) => WithThrowOnError(throwOnError);
    ICommandBuilder ICommandBuilder.WithGracefulShutdownTimeout(TimeSpan timeout) => WithGracefulShutdownTimeout(timeout);
    ICommandBuilder ICommandBuilder.WithLogging(CommandLoggingOptions options) => WithLogging(options);
    ICommandBuilder ICommandBuilder.WithLogging(Action<CommandLoggingOptions> configure) => WithLogging(configure);

    public async Task<CommandResult> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        return await Command.ExecuteCommandLineTool(ToolOptions, ExecutionOptions, cancellationToken);
    }

    public (TOptions ToolOptions, CommandExecutionOptions ExecutionOptions) ToOptions()
    {
        return (ToolOptions, ExecutionOptions);
    }
}
```

### Concrete Builder (Generated)

```csharp
namespace ModularPipelines.DotNet.Builders;

/// <summary>
/// Fluent builder for dotnet build command.
/// </summary>
public class DotNetBuildBuilder : CommandBuilderBase<DotNetBuildBuilder, DotNetBuildOptions>, IDotNetBuildBuilder
{
    internal DotNetBuildBuilder(ICommand command) : base(command) { }

    internal DotNetBuildBuilder(ICommand command, DotNetBuildOptions options) : base(command, options) { }

    /// <inheritdoc />
    public IDotNetBuildBuilder WithFramework(string framework)
    {
        ToolOptions = ToolOptions with { Framework = framework };
        return this;
    }

    /// <inheritdoc />
    public IDotNetBuildBuilder WithConfiguration(string configuration)
    {
        ToolOptions = ToolOptions with { Configuration = configuration };
        return this;
    }

    /// <inheritdoc />
    public IDotNetBuildBuilder WithRuntime(string runtime)
    {
        ToolOptions = ToolOptions with { Runtime = runtime };
        return this;
    }

    /// <inheritdoc />
    public IDotNetBuildBuilder WithOutput(string outputPath)
    {
        ToolOptions = ToolOptions with { Output = outputPath };
        return this;
    }

    /// <inheritdoc />
    public IDotNetBuildBuilder WithNoRestore(bool noRestore = true)
    {
        ToolOptions = ToolOptions with { NoRestore = noRestore };
        return this;
    }

    /// <inheritdoc />
    public IDotNetBuildBuilder WithNoIncremental(bool noIncremental = true)
    {
        ToolOptions = ToolOptions with { NoIncremental = noIncremental };
        return this;
    }

    /// <inheritdoc />
    public IDotNetBuildBuilder ForProject(string projectPath)
    {
        ToolOptions = ToolOptions with { ProjectSolution = projectPath };
        return this;
    }

    /// <inheritdoc />
    public IDotNetBuildBuilder WithProperty(string name, string value)
    {
        var properties = ToolOptions.Properties?.ToList() ?? new List<KeyValue>();
        properties.Add(new KeyValue(name, value));
        ToolOptions = ToolOptions with { Properties = properties };
        return this;
    }

    // Explicit interface implementations to return interface type
    IDotNetBuildBuilder IDotNetBuildBuilder.WithFramework(string framework) => WithFramework(framework);
    IDotNetBuildBuilder IDotNetBuildBuilder.WithConfiguration(string configuration) => WithConfiguration(configuration);
    // ... etc for all methods
}
```

## Service Interface Changes

### Updated IDotNet Interface

```csharp
public partial interface IDotNet
{
    // EXISTING - unchanged, still works
    Task<CommandResult> Build(
        DotNetBuildOptions? options = default,
        CommandExecutionOptions? executionOptions = null,
        CancellationToken cancellationToken = default);

    // NEW - fluent entry point (parameterless overload)
    IDotNetBuildBuilder Build();

    // NEW - fluent with initial options (for modifying existing options)
    IDotNetBuildBuilder Build(DotNetBuildOptions options);
}
```

### Updated DotNet Implementation

```csharp
internal class DotNet : IDotNet
{
    private readonly ICommand _command;

    // EXISTING - unchanged
    public virtual async Task<CommandResult> Build(
        DotNetBuildOptions? options = default,
        CommandExecutionOptions? executionOptions = null,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(
            options ?? new DotNetBuildOptions(),
            executionOptions,
            cancellationToken);
    }

    // NEW - fluent entry points
    public IDotNetBuildBuilder Build() => new DotNetBuildBuilder(_command);

    public IDotNetBuildBuilder Build(DotNetBuildOptions options)
        => new DotNetBuildBuilder(_command, options);
}
```

## Usage Examples

### Basic Fluent Usage

```csharp
// Simple build
var result = await context.DotNet().Build()
    .WithConfiguration("Release")
    .ExecuteAsync();

// Build with multiple options
var result = await context.DotNet().Build()
    .ForProject("src/MyApp/MyApp.csproj")
    .WithConfiguration("Release")
    .WithFramework("net8.0")
    .WithNoRestore()
    .WithProperty("Version", "1.2.3")
    .WithTimeout(TimeSpan.FromMinutes(10))
    .WithWorkingDirectory("/repo")
    .ExecuteAsync();
```

### Hybrid Usage (Fluent + Options Object)

```csharp
// Start with existing options, modify fluently
var baseOptions = new DotNetBuildOptions
{
    Configuration = "Release",
    Framework = "net8.0"
};

var result = await context.DotNet().Build(baseOptions)
    .WithNoRestore()
    .WithTimeout(TimeSpan.FromMinutes(5))
    .ExecuteAsync();
```

### Extracting Options for Inspection

```csharp
// Build options without executing
var builder = context.DotNet().Build()
    .WithConfiguration("Release")
    .WithFramework("net8.0");

var (toolOptions, execOptions) = builder.ToOptions();

// Inspect or modify
Console.WriteLine($"Building with config: {toolOptions.Configuration}");

// Then execute
var result = await builder.ExecuteAsync();
```

### Using with Cancellation Token

```csharp
using var cts = new CancellationTokenSource(TimeSpan.FromMinutes(5));

var result = await context.DotNet().Build()
    .WithConfiguration("Release")
    .ExecuteAsync(cts.Token);
```

## Method Naming Conventions

### Tool Options Methods

| Option Type | Method Pattern | Example |
|-------------|----------------|---------|
| String value | `With{PropertyName}(string value)` | `WithConfiguration("Release")` |
| Boolean flag (true) | `With{PropertyName}(bool value = true)` | `WithNoRestore()` |
| Enum value | `With{PropertyName}({EnumType} value)` | `WithVerbosity(Verbosity.Detailed)` |
| Collection add | `With{PropertyName}(item)` | `WithProperty("Key", "Value")` |
| Path-like | `For{Noun}(string path)` | `ForProject("path/to.csproj")` |

### Execution Options Methods

All execution options use the base `ICommandBuilder` interface methods:

- `WithWorkingDirectory(string directory)`
- `WithTimeout(TimeSpan timeout)`
- `WithEnvironmentVariable(string key, string value)`
- `WithEnvironmentVariables(IDictionary<string, string?> variables)`
- `WithSudo(bool sudo = true)`
- `WithThrowOnError(bool throwOnError = true)`
- `WithGracefulShutdownTimeout(TimeSpan timeout)`
- `WithLogging(CommandLoggingOptions options)`
- `WithLogging(Action<CommandLoggingOptions> configure)`

## Generator Changes

The existing `ModularPipelines.OptionsGenerator` needs to be extended to generate:

1. **Builder Interface** (`I{Tool}{Command}Builder`)
   - One method per options property
   - Inherits from `ICommandBuilder<TBuilder, TOptions>`

2. **Builder Class** (`{Tool}{Command}Builder`)
   - Inherits from `CommandBuilderBase<TBuilder, TOptions>`
   - Implements the builder interface
   - Internal constructor (created by service)

3. **Service Interface Methods**
   - Parameterless `{Command}()` returning builder interface
   - Overload `{Command}(options)` for hybrid usage

4. **Service Implementation Methods**
   - Return `new {Builder}(_command)` or `new {Builder}(_command, options)`

### Generator Template Additions

```csharp
// Template for builder interface
public interface I{{CommandName}}Builder : ICommandBuilder<I{{CommandName}}Builder, {{OptionsType}}>
{
{{#each Properties}}
    /// <summary>
    /// {{Description}}
    /// </summary>
    I{{CommandName}}Builder With{{PropertyName}}({{PropertyType}} {{parameterName}});
{{/each}}
}

// Template for builder class
public class {{CommandName}}Builder : CommandBuilderBase<{{CommandName}}Builder, {{OptionsType}}>, I{{CommandName}}Builder
{
    internal {{CommandName}}Builder(ICommand command) : base(command) { }
    internal {{CommandName}}Builder(ICommand command, {{OptionsType}} options) : base(command, options) { }

{{#each Properties}}
    public I{{CommandName}}Builder With{{PropertyName}}({{PropertyType}} {{parameterName}})
    {
        ToolOptions = ToolOptions with { {{PropertyName}} = {{parameterName}} };
        return this;
    }
{{/each}}
}
```

## Implementation Phases

### Phase 1: Core Framework (Week 1)

**Files to create/modify:**

1. `src/ModularPipelines/Builders/ICommandBuilder.cs` - Base interface
2. `src/ModularPipelines/Builders/CommandBuilderBase.cs` - Base implementation

**Tasks:**
- [ ] Create `Builders` namespace and folder
- [ ] Implement `ICommandBuilder` interface
- [ ] Implement `ICommandBuilder<TBuilder, TOptions>` interface
- [ ] Implement `CommandBuilderBase<TBuilder, TOptions>` abstract class
- [ ] Add unit tests for base builder

### Phase 2: Manual DotNet Reference Implementation (Week 1-2)

**Purpose:** Validate the pattern before automating generation

**Files to create/modify:**

1. `src/ModularPipelines.DotNet/Builders/IDotNetBuildBuilder.cs`
2. `src/ModularPipelines.DotNet/Builders/DotNetBuildBuilder.cs`
3. `src/ModularPipelines.DotNet/Services/IDotNet.cs` - Add fluent methods
4. `src/ModularPipelines.DotNet/Services/DotNet.cs` - Implement fluent methods

**Tasks:**
- [ ] Create `Builders` folder in DotNet project
- [ ] Implement `IDotNetBuildBuilder` interface
- [ ] Implement `DotNetBuildBuilder` class
- [ ] Add fluent overloads to `IDotNet` interface
- [ ] Implement fluent methods in `DotNet` class
- [ ] Add integration tests

### Phase 3: Generator Extension (Week 2-3)

**Files to modify:**

1. `tools/ModularPipelines.OptionsGenerator/` - Add builder generation

**Tasks:**
- [ ] Add builder interface template
- [ ] Add builder class template
- [ ] Add service interface method template
- [ ] Add service implementation method template
- [ ] Update generation pipeline to include builders
- [ ] Test generation with DotNet module

### Phase 4: Generate All Tool Builders (Week 3)

**Tasks:**
- [ ] Regenerate DotNet builders (replace manual)
- [ ] Generate Docker builders
- [ ] Generate Git builders
- [ ] Generate Azure CLI builders
- [ ] Generate AWS CLI builders
- [ ] Generate remaining tool builders

### Phase 5: Documentation and Examples (Week 4)

**Tasks:**
- [ ] Update Docusaurus documentation
- [ ] Add fluent API examples to docs
- [ ] Update example pipelines to use fluent API
- [ ] Add migration guide for existing users

## Testing Strategy

### Unit Tests

```csharp
[Test]
public void Build_WithConfiguration_SetsConfiguration()
{
    var builder = new DotNetBuildBuilder(_mockCommand.Object);

    builder.WithConfiguration("Release");

    var (options, _) = builder.ToOptions();
    Assert.That(options.Configuration, Is.EqualTo("Release"));
}

[Test]
public void Build_ChainedCalls_SetsAllOptions()
{
    var builder = new DotNetBuildBuilder(_mockCommand.Object);

    builder
        .WithConfiguration("Release")
        .WithFramework("net8.0")
        .WithTimeout(TimeSpan.FromMinutes(5));

    var (toolOptions, execOptions) = builder.ToOptions();
    Assert.That(toolOptions.Configuration, Is.EqualTo("Release"));
    Assert.That(toolOptions.Framework, Is.EqualTo("net8.0"));
    Assert.That(execOptions.ExecutionTimeout, Is.EqualTo(TimeSpan.FromMinutes(5)));
}

[Test]
public async Task ExecuteAsync_CallsCommand_WithBuiltOptions()
{
    var builder = new DotNetBuildBuilder(_mockCommand.Object);

    await builder
        .WithConfiguration("Release")
        .ExecuteAsync();

    _mockCommand.Verify(c => c.ExecuteCommandLineTool(
        It.Is<DotNetBuildOptions>(o => o.Configuration == "Release"),
        It.IsAny<CommandExecutionOptions>(),
        It.IsAny<CancellationToken>()), Times.Once);
}
```

### Integration Tests

```csharp
[Test]
public async Task Build_FluentApi_BuildsProject()
{
    var result = await context.DotNet().Build()
        .ForProject(TestProject)
        .WithConfiguration("Release")
        .WithNoRestore()
        .ExecuteAsync();

    Assert.That(result.ExitCode, Is.EqualTo(0));
}
```

## Migration Guide

### For Existing Code

No migration required - existing code continues to work:

```csharp
// This still works exactly as before
await context.DotNet().Build(
    new DotNetBuildOptions { Configuration = "Release" },
    new CommandExecutionOptions { ExecutionTimeout = TimeSpan.FromMinutes(5) });
```

### Recommended New Pattern

New code should prefer the fluent API for improved readability:

```csharp
// Preferred for new code
await context.DotNet().Build()
    .WithConfiguration("Release")
    .WithTimeout(TimeSpan.FromMinutes(5))
    .ExecuteAsync();
```

### Converting Existing Code

```csharp
// Before
await context.DotNet().Build(
    new DotNetBuildOptions
    {
        Configuration = "Release",
        Framework = "net8.0",
        NoRestore = true
    },
    new CommandExecutionOptions
    {
        ExecutionTimeout = TimeSpan.FromMinutes(5)
    });

// After
await context.DotNet().Build()
    .WithConfiguration("Release")
    .WithFramework("net8.0")
    .WithNoRestore()
    .WithTimeout(TimeSpan.FromMinutes(5))
    .ExecuteAsync();
```

## Files Impacted

### New Files

- `src/ModularPipelines/Builders/ICommandBuilder.cs`
- `src/ModularPipelines/Builders/CommandBuilderBase.cs`
- `src/ModularPipelines.DotNet/Builders/` (generated)
- `src/ModularPipelines.Docker/Builders/` (generated)
- Similar for all tool packages

### Modified Files

- `src/ModularPipelines.DotNet/Services/IDotNet.cs` - Add fluent overloads
- `src/ModularPipelines.DotNet/Services/DotNet.cs` - Implement fluent methods
- Similar for all tool service files
- `tools/ModularPipelines.OptionsGenerator/` - Add builder generation

## Open Questions

1. **Should `ExecuteAsync()` return `Task<CommandResult>` or a richer result type?**
   - Current design: Returns `CommandResult` for consistency
   - Alternative: Could return `ICommandExecutionResult<TOptions>` with options info

2. **Should builders be immutable (return new instance) or mutable (modify in place)?**
   - Current design: Mutable for simplicity and performance
   - Trade-off: Cannot safely share builder instances

3. **Should we provide a `Configure(Action<TOptions>)` escape hatch?**
   - Pro: Covers any options not exposed via fluent methods
   - Con: Breaks the fluent abstraction

## Success Criteria

1. Both patterns (options objects and fluent builders) work side by side
2. No breaking changes to existing code
3. IDE autocompletion guides users through available options
4. All tool packages support fluent API
5. Documentation updated with examples
6. At least 80% test coverage for new code
