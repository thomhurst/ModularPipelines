# Command Execution Separation Implementation Plan

> **For Claude:** REQUIRED SUB-SKILL: Use superpowers:executing-plans to implement this plan task-by-task.

**Goal:** Separate command execution options from tool-specific options to eliminate naming conflicts and improve SRP compliance.

**Architecture:** Create `CommandExecutionOptions` for execution config, slim down `CommandLineToolOptions` to only tool concerns, update all method signatures to take both separately.

**Tech Stack:** C#, .NET, Source Generators

---

## Task 1: Create CommandExecutionOptions

**Files:**
- Create: `src/ModularPipelines/Options/CommandExecutionOptions.cs`

**Step 1: Write the new CommandExecutionOptions class**

```csharp
using CliWrap;

namespace ModularPipelines.Options;

/// <summary>
/// Configuration for how a command is executed, independent of tool-specific arguments.
/// </summary>
public record CommandExecutionOptions
{
    /// <summary>
    /// Gets any EnvironmentVariables to pass to the command.
    /// </summary>
    public IDictionary<string, string?>? EnvironmentVariables { get; init; }

    /// <summary>
    /// Gets the working directory to run the command from.
    /// </summary>
    public string? WorkingDirectory { get; init; }

    /// <inheritdoc cref="CommandLineCredentials"/>
    public Credentials? CommandLineCredentials { get; init; }

    /// <summary>
    /// Gets or sets logging options for command execution.
    /// </summary>
    public CommandLoggingOptions? LogSettings { get; init; }

    /// <summary>
    /// Gets if logging input, you can use this to edit how the input is logged.
    /// </summary>
    public Func<string, string>? InputLoggingManipulator { get; init; }

    /// <summary>
    /// Gets if logging output, you can use this to edit how the output is logged.
    /// </summary>
    public Func<string, string>? OutputLoggingManipulator { get; init; }

    /// <summary>
    /// Gets or sets a value indicating whether to prefix commands with Sudo.
    /// </summary>
    public bool Sudo { get; init; }

    /// <summary>
    /// Gets or sets a value indicating whether to throw an exception on non-zero exit codes.
    /// </summary>
    public bool ThrowOnNonZeroExitCode { get; init; } = true;

    /// <summary>
    /// Gets or sets the maximum time allowed for the command to complete.
    /// </summary>
    public TimeSpan? ExecutionTimeout { get; init; }

    /// <summary>
    /// Gets or sets the time to wait for graceful shutdown before forcefully terminating.
    /// </summary>
    public TimeSpan GracefulShutdownTimeout { get; init; } = TimeSpan.FromSeconds(30);

    internal bool InternalDryRun { get; set; }
}
```

**Step 2: Build to verify no errors**

Run: `dotnet build src/ModularPipelines -c Release`
Expected: Build succeeds

**Step 3: Commit**

```bash
git add src/ModularPipelines/Options/CommandExecutionOptions.cs
git commit -m "feat: Add CommandExecutionOptions class"
```

---

## Task 2: Slim down CommandLineToolOptions

**Files:**
- Modify: `src/ModularPipelines/Options/CommandLineToolOptions.cs`
- Delete: `src/ModularPipelines/Options/CommandLineOptions.cs`

**Step 1: Update CommandLineToolOptions to remove inheritance**

Replace the entire file with:

```csharp
namespace ModularPipelines.Options;

/// <summary>
/// Options for setting the command line tool and any arguments it needs.
/// </summary>
public abstract record CommandLineToolOptions(string Tool)
{
    /// <summary>
    /// Gets the command parts (subcommands) for the tool.
    /// </summary>
    public string[]? CommandParts { get; init; }

    /// <summary>
    /// Gets used for providing switches and arguments to the tool.
    /// </summary>
    public IEnumerable<string>? Arguments { get; init; }

    /// <summary>
    /// Gets used for command line tools that support -- syntax.
    /// </summary>
    public IEnumerable<string>? RunSettings { get; init; }
}
```

**Step 2: Delete CommandLineOptions.cs**

This file is replaced by CommandExecutionOptions.

**Step 3: Build to see what breaks**

Run: `dotnet build src/ModularPipelines -c Release`
Expected: Multiple errors - this is expected and guides next steps

**Step 4: Commit**

```bash
git add -A
git commit -m "refactor: Slim down CommandLineToolOptions, remove CommandLineOptions"
```

---

## Task 3: Update ICommand interface

**Files:**
- Modify: `src/ModularPipelines/Context/ICommand.cs`

**Step 1: Update the interface method signature**

```csharp
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Context;

/// <summary>
/// Provides functionality for executing command line tools and processes.
/// </summary>
public interface ICommand
{
    /// <summary>
    /// Execute a command line tool.
    /// </summary>
    /// <param name="options">The tool-specific options.</param>
    /// <param name="executionOptions">The execution configuration options.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task<CommandResult> ExecuteCommandLineTool(
        CommandLineToolOptions options,
        CommandExecutionOptions? executionOptions = null,
        CancellationToken cancellationToken = default);
}
```

**Step 2: Commit**

```bash
git add src/ModularPipelines/Context/ICommand.cs
git commit -m "refactor: Update ICommand signature with separate execution options"
```

---

## Task 4: Update Command implementation

**Files:**
- Modify: `src/ModularPipelines/Context/Command.cs`

**Step 1: Update ExecuteCommandLineTool method signature and implementation**

Update the method to accept both parameters and merge execution options with defaults.

Key changes:
1. Method signature adds `CommandExecutionOptions? executionOptions = null`
2. Resolve execution options from: per-call → pipeline defaults → hardcoded defaults
3. Use resolved execution options throughout the method instead of `options.WorkingDirectory`, `options.ExecutionTimeout`, etc.

**Step 2: Build and fix any remaining issues**

Run: `dotnet build src/ModularPipelines -c Release`

**Step 3: Commit**

```bash
git add src/ModularPipelines/Context/Command.cs
git commit -m "refactor: Update Command to use separate execution options"
```

---

## Task 5: Update CommandServiceBase

**Files:**
- Modify: `src/ModularPipelines/Context/CommandServiceBase.cs`

**Step 1: Update ExecuteAsync method**

```csharp
protected virtual async Task<CommandResult> ExecuteAsync<TOptions>(
    TOptions options,
    CommandExecutionOptions? executionOptions = null,
    CancellationToken cancellationToken = default)
    where TOptions : CommandLineToolOptions
{
    return await Command.ExecuteCommandLineTool(options, executionOptions, cancellationToken).ConfigureAwait(false);
}
```

**Step 2: Commit**

```bash
git add src/ModularPipelines/Context/CommandServiceBase.cs
git commit -m "refactor: Update CommandServiceBase with execution options parameter"
```

---

## Task 6: Add DefaultExecutionOptions to PipelineOptions

**Files:**
- Modify: `src/ModularPipelines/Options/PipelineOptions.cs`

**Step 1: Add the new property**

Add after existing properties:

```csharp
/// <summary>
/// Gets or sets the default execution options for all commands.
/// When set, these options apply to all command executions unless overridden per-call.
/// </summary>
public CommandExecutionOptions? DefaultExecutionOptions { get; set; }
```

**Step 2: Commit**

```bash
git add src/ModularPipelines/Options/PipelineOptions.cs
git commit -m "feat: Add DefaultExecutionOptions to PipelineOptions"
```

---

## Task 7: Update CommandLogger

**Files:**
- Modify: `src/ModularPipelines/Logging/CommandLogger.cs`
- Modify: `src/ModularPipelines/Logging/ICommandLogger.cs`

**Step 1: Update Log method to accept CommandExecutionOptions instead of CommandLineToolOptions**

The logger needs execution options (for LogSettings, manipulators) not tool options.

**Step 2: Commit**

```bash
git add src/ModularPipelines/Logging/
git commit -m "refactor: Update CommandLogger to use CommandExecutionOptions"
```

---

## Task 8: Fix remaining core framework compilation errors

**Files:**
- Various files in `src/ModularPipelines/`

**Step 1: Build and identify remaining errors**

Run: `dotnet build src/ModularPipelines -c Release`

**Step 2: Fix each error systematically**

Common fixes needed:
- Update any internal code that references old properties on CommandLineToolOptions
- Update extension methods
- Update any tests in the core project

**Step 3: Commit**

```bash
git add -A
git commit -m "fix: Resolve remaining core framework compilation errors"
```

---

## Task 9: Update OptionsGenerator templates

**Files:**
- Modify: `tools/ModularPipelines.OptionsGenerator/src/ModularPipelines.OptionsGenerator/Templates/`

**Step 1: Update service interface template**

Add `CommandExecutionOptions? executionOptions = null` parameter to generated method signatures.

**Step 2: Update service implementation template**

Pass execution options through to the base ExecuteAsync method.

**Step 3: Commit**

```bash
git add tools/ModularPipelines.OptionsGenerator/
git commit -m "refactor: Update generator templates for execution options separation"
```

---

## Task 10: Regenerate Docker package

**Files:**
- All generated files in `src/ModularPipelines.Docker/`

**Step 1: Run the generator for Docker**

```bash
cd tools/ModularPipelines.OptionsGenerator
dotnet run -- docker
```

**Step 2: Build Docker package**

Run: `dotnet build src/ModularPipelines.Docker -c Release`

**Step 3: Commit**

```bash
git add src/ModularPipelines.Docker/
git commit -m "refactor: Regenerate Docker package with new signatures"
```

---

## Task 11: Regenerate remaining tool packages

**Files:**
- All generated files in tool packages

**Step 1: Run generator for each tool**

Run generator for: DotNet, Git, Helm, Kubernetes, Terraform, Azure CLI, AWS, Node, etc.

**Step 2: Build all tool packages**

Run: `dotnet build ModularPipelines.sln -c Release`

**Step 3: Commit**

```bash
git add src/ModularPipelines.*/
git commit -m "refactor: Regenerate all tool packages with new signatures"
```

---

## Task 12: Update unit tests

**Files:**
- `test/ModularPipelines.UnitTests/`

**Step 1: Update tests to use new API**

Fix tests that use old API:
```csharp
// Old
new SomeToolOptions { WorkingDirectory = "/app" }

// New
new SomeToolOptions { }, new CommandExecutionOptions { WorkingDirectory = "/app" }
```

**Step 2: Run tests**

Run: `dotnet test test/ModularPipelines.UnitTests`

**Step 3: Commit**

```bash
git add test/
git commit -m "test: Update unit tests for new API"
```

---

## Task 13: Update examples

**Files:**
- `examples/` or example projects

**Step 1: Update example code to use new API**

**Step 2: Build examples**

**Step 3: Commit**

```bash
git add examples/
git commit -m "docs: Update examples for new API"
```

---

## Task 14: Final verification

**Step 1: Full solution build**

Run: `dotnet build ModularPipelines.sln -c Release`

**Step 2: Run all tests**

Run full test suite

**Step 3: Create PR**

```bash
git push -u origin fix-1811
gh pr create --title "refactor: Separate command execution options from tool options (#1811)" --body "..."
```
