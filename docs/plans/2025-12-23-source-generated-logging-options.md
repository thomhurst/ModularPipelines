# Source Generated Calling Methods with Customizable Logging Options

> **For Claude:** REQUIRED SUB-SKILL: Use superpowers:executing-plans to implement this plan task-by-task.

**Goal:** Add source-generated calling methods with comprehensive logging options, allowing per-command control over verbosity and output format.

**Architecture:** Extend existing `ServiceImplementationGenerator` and `ServiceInterfaceGenerator` to add a `CommandLoggingOptions` parameter to all generated methods. The `ICommand.ExecuteCommandLineTool` method will be extended to accept logging options, which flow through to `ICommandLogger`. DotNet will be migrated from manual services to generated ones first.

**Tech Stack:** C# 12, .NET 10, Source Generators (runtime via OptionsGenerator tool), CliWrap

---

## Phase 1: Core Logging Options Infrastructure

### Task 1: Create CommandLoggingOptions Class

**Files:**
- Create: `src/ModularPipelines/Options/CommandLoggingOptions.cs`
- Create: `src/ModularPipelines/Options/CommandLogVerbosity.cs`
- Create: `src/ModularPipelines/Options/CommandLogFormat.cs`

**Step 1: Create CommandLogVerbosity enum**

Create `src/ModularPipelines/Options/CommandLogVerbosity.cs`:

```csharp
namespace ModularPipelines.Options;

/// <summary>
/// Specifies the verbosity level for command logging.
/// </summary>
public enum CommandLogVerbosity
{
    /// <summary>
    /// No output at all.
    /// </summary>
    Silent = 0,

    /// <summary>
    /// Only errors and warnings.
    /// </summary>
    Minimal = 1,

    /// <summary>
    /// Standard output (default).
    /// </summary>
    Normal = 2,

    /// <summary>
    /// Include additional context like working directory and timing.
    /// </summary>
    Detailed = 3,

    /// <summary>
    /// Full verbose output for debugging.
    /// </summary>
    Diagnostic = 4
}
```

**Step 2: Create CommandLogFormat enum**

Create `src/ModularPipelines/Options/CommandLogFormat.cs`:

```csharp
namespace ModularPipelines.Options;

/// <summary>
/// Specifies the output format for command logging.
/// </summary>
public enum CommandLogFormat
{
    /// <summary>
    /// Plain text output (default).
    /// </summary>
    Text = 0,

    /// <summary>
    /// Structured JSON output.
    /// </summary>
    Structured = 1
}
```

**Step 3: Create CommandLoggingOptions class**

Create `src/ModularPipelines/Options/CommandLoggingOptions.cs`:

```csharp
namespace ModularPipelines.Options;

/// <summary>
/// Options for customizing command execution logging.
/// </summary>
public record CommandLoggingOptions
{
    /// <summary>
    /// Gets or sets the verbosity level. Default is Normal.
    /// </summary>
    public CommandLogVerbosity Verbosity { get; init; } = CommandLogVerbosity.Normal;

    /// <summary>
    /// Gets or sets the output format. Default is Text.
    /// </summary>
    public CommandLogFormat OutputFormat { get; init; } = CommandLogFormat.Text;

    /// <summary>
    /// Gets or sets whether to include timestamps in output. Default is false.
    /// </summary>
    public bool IncludeTimestamps { get; init; }

    /// <summary>
    /// Gets or sets whether to show command arguments. Default is true.
    /// </summary>
    public bool ShowCommandArguments { get; init; } = true;

    /// <summary>
    /// Gets or sets whether to show standard output. Default is true.
    /// </summary>
    public bool ShowStandardOutput { get; init; } = true;

    /// <summary>
    /// Gets or sets whether to show standard error. Default is true.
    /// </summary>
    public bool ShowStandardError { get; init; } = true;

    /// <summary>
    /// Gets or sets whether to show the exit code. Default is false.
    /// </summary>
    public bool ShowExitCode { get; init; }

    /// <summary>
    /// Gets or sets whether to show the working directory. Default is false.
    /// </summary>
    public bool ShowWorkingDirectory { get; init; }

    /// <summary>
    /// Gets or sets whether to show execution time. Default is false.
    /// </summary>
    public bool ShowExecutionTime { get; init; }

    /// <summary>
    /// Default logging options (Normal verbosity, all standard options enabled).
    /// </summary>
    public static CommandLoggingOptions Default { get; } = new();

    /// <summary>
    /// Silent logging options (no output).
    /// </summary>
    public static CommandLoggingOptions Silent { get; } = new() { Verbosity = CommandLogVerbosity.Silent };

    /// <summary>
    /// Diagnostic logging options (maximum verbosity, all options enabled).
    /// </summary>
    public static CommandLoggingOptions Diagnostic { get; } = new()
    {
        Verbosity = CommandLogVerbosity.Diagnostic,
        IncludeTimestamps = true,
        ShowCommandArguments = true,
        ShowStandardOutput = true,
        ShowStandardError = true,
        ShowExitCode = true,
        ShowWorkingDirectory = true,
        ShowExecutionTime = true
    };
}
```

**Step 4: Build to verify compilation**

Run: `dotnet build src/ModularPipelines/ModularPipelines.csproj -c Release`
Expected: Build succeeded

**Step 5: Commit**

```bash
git add src/ModularPipelines/Options/CommandLoggingOptions.cs src/ModularPipelines/Options/CommandLogVerbosity.cs src/ModularPipelines/Options/CommandLogFormat.cs
git commit -m "feat: add CommandLoggingOptions class for per-command logging control"
```

---

### Task 2: Extend ICommand Interface

**Files:**
- Modify: `src/ModularPipelines/Context/ICommand.cs`

**Step 1: Add new method overload to ICommand**

Edit `src/ModularPipelines/Context/ICommand.cs` to add a second method:

```csharp
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Context;

public interface ICommand
{
    /// <summary>
    /// Execute a command line tool.
    /// </summary>
    /// <param name="options">The command options.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task<CommandResult> ExecuteCommandLineTool(CommandLineToolOptions options, CancellationToken cancellationToken = default);

    /// <summary>
    /// Execute a command line tool with custom logging options.
    /// </summary>
    /// <param name="options">The command options.</param>
    /// <param name="loggingOptions">The logging options for this command execution.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task<CommandResult> ExecuteCommandLineTool(CommandLineToolOptions options, CommandLoggingOptions? loggingOptions, CancellationToken cancellationToken = default);
}
```

**Step 2: Build to verify interface compiles (will fail due to missing implementation)**

Run: `dotnet build src/ModularPipelines/ModularPipelines.csproj -c Release`
Expected: Build FAILED - Command class doesn't implement new method

**Step 3: Commit interface change**

```bash
git add src/ModularPipelines/Context/ICommand.cs
git commit -m "feat: add ICommand overload accepting CommandLoggingOptions"
```

---

### Task 3: Extend ICommandLogger Interface

**Files:**
- Modify: `src/ModularPipelines/Logging/ICommandLogger.cs`

**Step 1: Add logging options parameter to ICommandLogger**

Edit `src/ModularPipelines/Logging/ICommandLogger.cs`:

```csharp
using ModularPipelines.Options;

namespace ModularPipelines.Logging;

/// <summary>
/// Provides functionality for logging command execution details.
/// </summary>
public interface ICommandLogger
{
    /// <summary>
    /// Logs the details of a command execution.
    /// </summary>
    /// <param name="options">The command line tool options used for execution.</param>
    /// <param name="loggingOptions">The logging options to control output behavior.</param>
    /// <param name="inputToLog">The input command to log.</param>
    /// <param name="exitCode">The exit code returned by the command.</param>
    /// <param name="runTime">The time taken to execute the command.</param>
    /// <param name="standardOutput">The standard output from the command.</param>
    /// <param name="standardError">The standard error from the command.</param>
    /// <param name="commandWorkingDirPath">The working directory where the command was executed.</param>
    void Log(
        CommandLineToolOptions options,
        CommandLoggingOptions? loggingOptions,
        string? inputToLog,
        int? exitCode,
        TimeSpan? runTime,
        string standardOutput,
        string standardError,
        string commandWorkingDirPath);
}
```

**Step 2: Build to see errors (implementation missing)**

Run: `dotnet build src/ModularPipelines/ModularPipelines.csproj -c Release`
Expected: Build FAILED - implementations don't match

**Step 3: Commit interface change**

```bash
git add src/ModularPipelines/Logging/ICommandLogger.cs
git commit -m "feat: add loggingOptions parameter to ICommandLogger.Log"
```

---

### Task 4: Update CommandLogger Implementation

**Files:**
- Modify: `src/ModularPipelines/Logging/CommandLogger.cs`

**Step 1: Find and read CommandLogger.cs**

First locate and read the current implementation to understand its structure.

**Step 2: Update CommandLogger to accept and use CommandLoggingOptions**

The implementation should:
1. Accept the new `loggingOptions` parameter
2. Check verbosity level before logging
3. Conditionally include timestamp, exit code, working directory, execution time
4. Handle structured output format if requested

Key changes:
- If `loggingOptions?.Verbosity == CommandLogVerbosity.Silent`, return early
- Use `loggingOptions?.ShowCommandArguments` to control argument display
- Use `loggingOptions?.ShowStandardOutput` to control stdout display
- Use `loggingOptions?.ShowStandardError` to control stderr display
- Use `loggingOptions?.IncludeTimestamps` to add timestamps
- Use `loggingOptions?.ShowExitCode` to show exit code
- Use `loggingOptions?.ShowWorkingDirectory` to show working dir
- Use `loggingOptions?.ShowExecutionTime` to show run time

**Step 3: Build to verify**

Run: `dotnet build src/ModularPipelines/ModularPipelines.csproj -c Release`
Expected: Build succeeded

**Step 4: Commit**

```bash
git add src/ModularPipelines/Logging/CommandLogger.cs
git commit -m "feat: implement CommandLoggingOptions support in CommandLogger"
```

---

### Task 5: Update Command Class Implementation

**Files:**
- Modify: `src/ModularPipelines/Context/Command.cs`

**Step 1: Add new overload implementation**

Add the new method that accepts `CommandLoggingOptions`:

```csharp
public async Task<CommandResult> ExecuteCommandLineTool(
    CommandLineToolOptions options,
    CommandLoggingOptions? loggingOptions,
    CancellationToken cancellationToken = default)
{
    // Store logging options for use in the execution
    // Pass to _commandLogger.Log calls
    // ... (same implementation as existing, but pass loggingOptions)
}
```

**Step 2: Update existing overload to call new one with default**

```csharp
public Task<CommandResult> ExecuteCommandLineTool(
    CommandLineToolOptions options,
    CancellationToken cancellationToken = default)
{
    return ExecuteCommandLineTool(options, null, cancellationToken);
}
```

**Step 3: Update all _commandLogger.Log calls to pass loggingOptions**

There are 4 places in Command.cs where `_commandLogger.Log` is called. Update each to pass the `loggingOptions` parameter.

**Step 4: Build to verify**

Run: `dotnet build src/ModularPipelines/ModularPipelines.csproj -c Release`
Expected: Build succeeded

**Step 5: Run tests**

Run: `dotnet test test/ModularPipelines.UnitTests/ModularPipelines.UnitTests.csproj`
Expected: All tests pass

**Step 6: Commit**

```bash
git add src/ModularPipelines/Context/Command.cs
git commit -m "feat: implement ICommand.ExecuteCommandLineTool with logging options"
```

---

## Phase 2: Update Code Generators

### Task 6: Update ServiceInterfaceGenerator

**Files:**
- Modify: `tools/ModularPipelines.OptionsGenerator/src/ModularPipelines.OptionsGenerator/Generators/ServiceInterfaceGenerator.cs`

**Step 1: Add using for ModularPipelines.Options in generated output**

Update `GenerateInterface` to include:
```csharp
sb.AppendLine("using ModularPipelines.Options;");
```

**Step 2: Update GenerateMethodSignature to include logging parameter**

Change the method signature generation from:
```csharp
sb.AppendLine($"    Task<CommandResult> {methodName}({command.ClassName} options, CancellationToken cancellationToken = default);");
```

To:
```csharp
sb.AppendLine($"    Task<CommandResult> {methodName}({command.ClassName} options, CommandLoggingOptions? loggingOptions = default, CancellationToken cancellationToken = default);");
```

**Step 3: Build generator**

Run: `dotnet build tools/ModularPipelines.OptionsGenerator/src/ModularPipelines.OptionsGenerator/ModularPipelines.OptionsGenerator.csproj -c Release`
Expected: Build succeeded

**Step 4: Commit**

```bash
git add tools/ModularPipelines.OptionsGenerator/src/ModularPipelines.OptionsGenerator/Generators/ServiceInterfaceGenerator.cs
git commit -m "feat: add loggingOptions parameter to generated interface methods"
```

---

### Task 7: Update ServiceImplementationGenerator

**Files:**
- Modify: `tools/ModularPipelines.OptionsGenerator/src/ModularPipelines.OptionsGenerator/Generators/ServiceImplementationGenerator.cs`

**Step 1: Add using for ModularPipelines.Options in generated output**

In `GenerateMainServiceClass`, add:
```csharp
sb.AppendLine("using ModularPipelines.Options;");
```

**Step 2: Update GenerateMethod to include logging parameter**

Change the method generation from:
```csharp
sb.AppendLine($"    public virtual async Task<CommandResult> {methodName}(");
sb.AppendLine($"        {optionsParam},");
sb.AppendLine("        CancellationToken cancellationToken = default)");
sb.AppendLine("    {");
if (hasRequiredParams)
{
    sb.AppendLine("        return await _command.ExecuteCommandLineTool(options, cancellationToken);");
}
else
{
    sb.AppendLine($"        return await _command.ExecuteCommandLineTool(options ?? new {command.ClassName}(), cancellationToken);");
}
sb.AppendLine("    }");
```

To:
```csharp
sb.AppendLine($"    public virtual async Task<CommandResult> {methodName}(");
sb.AppendLine($"        {optionsParam},");
sb.AppendLine("        CommandLoggingOptions? loggingOptions = default,");
sb.AppendLine("        CancellationToken cancellationToken = default)");
sb.AppendLine("    {");
if (hasRequiredParams)
{
    sb.AppendLine("        return await _command.ExecuteCommandLineTool(options, loggingOptions, cancellationToken);");
}
else
{
    sb.AppendLine($"        return await _command.ExecuteCommandLineTool(options ?? new {command.ClassName}(), loggingOptions, cancellationToken);");
}
sb.AppendLine("    }");
```

**Step 3: Build generator**

Run: `dotnet build tools/ModularPipelines.OptionsGenerator/src/ModularPipelines.OptionsGenerator/ModularPipelines.OptionsGenerator.csproj -c Release`
Expected: Build succeeded

**Step 4: Commit**

```bash
git add tools/ModularPipelines.OptionsGenerator/src/ModularPipelines.OptionsGenerator/Generators/ServiceImplementationGenerator.cs
git commit -m "feat: add loggingOptions parameter to generated implementation methods"
```

---

### Task 8: Update SubDomainClassGenerator

**Files:**
- Modify: `tools/ModularPipelines.OptionsGenerator/src/ModularPipelines.OptionsGenerator/Generators/SubDomainClassGenerator.cs`

**Step 1: Read the file to understand its structure**

**Step 2: Add using for ModularPipelines.Options**

**Step 3: Update method signatures to include logging parameter**

Same pattern as ServiceImplementationGenerator - add `CommandLoggingOptions? loggingOptions = default` parameter.

**Step 4: Build generator**

Run: `dotnet build tools/ModularPipelines.OptionsGenerator/src/ModularPipelines.OptionsGenerator/ModularPipelines.OptionsGenerator.csproj -c Release`
Expected: Build succeeded

**Step 5: Commit**

```bash
git add tools/ModularPipelines.OptionsGenerator/src/ModularPipelines.OptionsGenerator/Generators/SubDomainClassGenerator.cs
git commit -m "feat: add loggingOptions parameter to generated sub-domain methods"
```

---

## Phase 3: Regenerate Existing Tools

### Task 9: Regenerate Git Service

**Files:**
- Regenerate: `src/ModularPipelines.Git/Services/Git.Generated.cs`
- Regenerate: `src/ModularPipelines.Git/Services/IGit.Generated.cs`

**Step 1: Run generator for Git**

Run: `dotnet run --project tools/ModularPipelines.OptionsGenerator/src/ModularPipelines.OptionsGenerator/ModularPipelines.OptionsGenerator.csproj -- --tools git --output-dir .`

**Step 2: Build ModularPipelines.Git**

Run: `dotnet build src/ModularPipelines.Git/ModularPipelines.Git.csproj -c Release`
Expected: Build succeeded

**Step 3: Commit**

```bash
git add src/ModularPipelines.Git/Services/Git.Generated.cs src/ModularPipelines.Git/Services/IGit.Generated.cs
git commit -m "chore: regenerate Git service with logging options"
```

---

### Task 10: Regenerate Docker Service

**Files:**
- Regenerate: `src/ModularPipelines.Docker/Services/Docker.Generated.cs`
- Regenerate: `src/ModularPipelines.Docker/Services/IDocker.Generated.cs`

**Step 1: Run generator for Docker**

Run: `dotnet run --project tools/ModularPipelines.OptionsGenerator/src/ModularPipelines.OptionsGenerator/ModularPipelines.OptionsGenerator.csproj -- --tools docker --output-dir .`

**Step 2: Build ModularPipelines.Docker**

Run: `dotnet build src/ModularPipelines.Docker/ModularPipelines.Docker.csproj -c Release`
Expected: Build succeeded

**Step 3: Commit**

```bash
git add src/ModularPipelines.Docker/Services/
git commit -m "chore: regenerate Docker service with logging options"
```

---

### Task 11: Regenerate Helm Service

**Files:**
- Regenerate: `src/ModularPipelines.Helm/Services/Helm.Generated.cs`
- Regenerate: `src/ModularPipelines.Helm/Services/IHelm.Generated.cs`

**Step 1: Run generator for Helm**

Run: `dotnet run --project tools/ModularPipelines.OptionsGenerator/src/ModularPipelines.OptionsGenerator/ModularPipelines.OptionsGenerator.csproj -- --tools helm --output-dir .`

**Step 2: Build ModularPipelines.Helm**

Run: `dotnet build src/ModularPipelines.Helm/ModularPipelines.Helm.csproj -c Release`
Expected: Build succeeded

**Step 3: Commit**

```bash
git add src/ModularPipelines.Helm/Services/
git commit -m "chore: regenerate Helm service with logging options"
```

---

### Task 12: Regenerate Google Cloud Service

**Files:**
- Regenerate: `src/ModularPipelines.Google/Services/Gcloud.Generated.cs`
- Regenerate: `src/ModularPipelines.Google/Services/IGcloud.Generated.cs`

**Step 1: Run generator for gcloud**

Run: `dotnet run --project tools/ModularPipelines.OptionsGenerator/src/ModularPipelines.OptionsGenerator/ModularPipelines.OptionsGenerator.csproj -- --tools gcloud --output-dir .`

**Step 2: Build ModularPipelines.Google**

Run: `dotnet build src/ModularPipelines.Google/ModularPipelines.Google.csproj -c Release`
Expected: Build succeeded

**Step 3: Commit**

```bash
git add src/ModularPipelines.Google/Services/
git commit -m "chore: regenerate Google Cloud service with logging options"
```

---

## Phase 4: Migrate DotNet to Generated Code

### Task 13: Generate DotNet Service Files

**Files:**
- Generate: `src/ModularPipelines.DotNet/Services/DotNet.Generated.cs`
- Generate: `src/ModularPipelines.DotNet/Services/IDotNet.Generated.cs`

**Step 1: Run generator for dotnet**

Run: `dotnet run --project tools/ModularPipelines.OptionsGenerator/src/ModularPipelines.OptionsGenerator/ModularPipelines.OptionsGenerator.csproj -- --tools dotnet --output-dir .`

**Step 2: Verify generated files exist**

Check that `DotNet.Generated.cs` and `IDotNet.Generated.cs` were created.

**Step 3: Commit generated files (don't delete manual files yet)**

```bash
git add src/ModularPipelines.DotNet/Services/DotNet.Generated.cs src/ModularPipelines.DotNet/Services/IDotNet.Generated.cs
git commit -m "feat: generate DotNet service files with logging options"
```

---

### Task 14: Create Partial IDotNet Interface

**Files:**
- Modify: `src/ModularPipelines.DotNet/Services/IDotNet.cs`

**Step 1: Read existing IDotNet.cs**

Understand what manual interface members exist that aren't command methods.

**Step 2: Convert to partial interface referencing generated part**

If IDotNet.cs contains sub-domain properties (Tool, Nuget, etc.) that the generator doesn't handle, keep those in a partial class.

**Step 3: Build to verify**

Run: `dotnet build src/ModularPipelines.DotNet/ModularPipelines.DotNet.csproj -c Release`
Expected: May have errors if interfaces conflict

**Step 4: Commit**

```bash
git add src/ModularPipelines.DotNet/Services/IDotNet.cs
git commit -m "refactor: convert IDotNet to partial interface"
```

---

### Task 15: Remove Manual DotNet Service Methods

**Files:**
- Modify: `src/ModularPipelines.DotNet/Services/DotNet.cs`

**Step 1: Read existing DotNet.cs**

Identify which methods are command invocations (to be removed) vs infrastructure (to keep).

**Step 2: Convert to partial class, keep only infrastructure**

Keep:
- Constructor
- Sub-domain properties (Tool, Nuget, Remove, etc.)
- Any custom helper methods

Remove:
- All `public virtual async Task<CommandResult>` methods that just delegate to `_command.ExecuteCommandLineTool`

**Step 3: Build to verify**

Run: `dotnet build src/ModularPipelines.DotNet/ModularPipelines.DotNet.csproj -c Release`
Expected: Build succeeded

**Step 4: Commit**

```bash
git add src/ModularPipelines.DotNet/Services/DotNet.cs
git commit -m "refactor: remove manual command methods from DotNet service"
```

---

### Task 16: Remove Other Manual DotNet Service Files

**Files:**
- Modify/Delete: `src/ModularPipelines.DotNet/Services/DotNetTool.cs`
- Modify/Delete: `src/ModularPipelines.DotNet/Services/DotNetNuget.cs`
- Modify/Delete: `src/ModularPipelines.DotNet/Services/DotNetAdd.cs`
- Modify/Delete: `src/ModularPipelines.DotNet/Services/DotNetRemove.cs`
- Modify/Delete: `src/ModularPipelines.DotNet/Services/DotNetList.cs`
- Modify/Delete: `src/ModularPipelines.DotNet/Services/DotNetWorkload.cs`
- Modify/Delete: `src/ModularPipelines.DotNet/Services/DotNetSdk.cs`
- Modify/Delete: `src/ModularPipelines.DotNet/Services/DotNetNuget*.cs`

**Step 1: For each sub-domain service file**

If generator creates the corresponding sub-domain class, delete the manual file.
If generator doesn't handle it, keep it but convert to partial class.

**Step 2: Build after each removal**

Run: `dotnet build src/ModularPipelines.DotNet/ModularPipelines.DotNet.csproj -c Release`
Expected: Build succeeded

**Step 3: Commit**

```bash
git add -A src/ModularPipelines.DotNet/Services/
git commit -m "refactor: migrate DotNet sub-domain services to generated code"
```

---

## Phase 5: Testing

### Task 17: Add Unit Tests for CommandLoggingOptions

**Files:**
- Create: `test/ModularPipelines.UnitTests/Options/CommandLoggingOptionsTests.cs`

**Step 1: Write tests for default values**

```csharp
using ModularPipelines.Options;

namespace ModularPipelines.UnitTests.Options;

public class CommandLoggingOptionsTests
{
    [Test]
    public void Default_HasNormalVerbosity()
    {
        var options = CommandLoggingOptions.Default;
        Assert.That(options.Verbosity, Is.EqualTo(CommandLogVerbosity.Normal));
    }

    [Test]
    public void Default_ShowsCommandArguments()
    {
        var options = CommandLoggingOptions.Default;
        Assert.That(options.ShowCommandArguments, Is.True);
    }

    [Test]
    public void Silent_HasSilentVerbosity()
    {
        var options = CommandLoggingOptions.Silent;
        Assert.That(options.Verbosity, Is.EqualTo(CommandLogVerbosity.Silent));
    }

    [Test]
    public void Diagnostic_HasAllOptionsEnabled()
    {
        var options = CommandLoggingOptions.Diagnostic;
        Assert.Multiple(() =>
        {
            Assert.That(options.Verbosity, Is.EqualTo(CommandLogVerbosity.Diagnostic));
            Assert.That(options.IncludeTimestamps, Is.True);
            Assert.That(options.ShowCommandArguments, Is.True);
            Assert.That(options.ShowStandardOutput, Is.True);
            Assert.That(options.ShowStandardError, Is.True);
            Assert.That(options.ShowExitCode, Is.True);
            Assert.That(options.ShowWorkingDirectory, Is.True);
            Assert.That(options.ShowExecutionTime, Is.True);
        });
    }
}
```

**Step 2: Run tests**

Run: `dotnet test test/ModularPipelines.UnitTests/ModularPipelines.UnitTests.csproj --filter "FullyQualifiedName~CommandLoggingOptionsTests"`
Expected: All tests pass

**Step 3: Commit**

```bash
git add test/ModularPipelines.UnitTests/Options/CommandLoggingOptionsTests.cs
git commit -m "test: add unit tests for CommandLoggingOptions"
```

---

### Task 18: Add Integration Tests for Logging Options

**Files:**
- Create: `test/ModularPipelines.UnitTests/Logging/CommandLoggingIntegrationTests.cs`

**Step 1: Write integration test for silent logging**

```csharp
using ModularPipelines.Options;
// ... test that Silent logging produces no output

[Test]
public async Task ExecuteWithSilentLogging_ProducesNoOutput()
{
    // Arrange: mock ICommandLogger, verify Log is not called with Silent
    // Act: execute a command with Silent logging
    // Assert: logger was not invoked
}
```

**Step 2: Write integration test for diagnostic logging**

```csharp
[Test]
public async Task ExecuteWithDiagnosticLogging_IncludesAllDetails()
{
    // Arrange: capture logged output
    // Act: execute command with Diagnostic logging
    // Assert: output includes timestamps, exit code, working dir, execution time
}
```

**Step 3: Run tests**

Run: `dotnet test test/ModularPipelines.UnitTests/ModularPipelines.UnitTests.csproj --filter "FullyQualifiedName~CommandLoggingIntegrationTests"`
Expected: All tests pass

**Step 4: Commit**

```bash
git add test/ModularPipelines.UnitTests/Logging/CommandLoggingIntegrationTests.cs
git commit -m "test: add integration tests for command logging options"
```

---

### Task 19: Run Full Test Suite

**Step 1: Run all unit tests**

Run: `dotnet test test/ModularPipelines.UnitTests/ModularPipelines.UnitTests.csproj`
Expected: All tests pass

**Step 2: Build the entire solution**

Run: `dotnet build ModularPipelines.sln -c Release`
Expected: Build succeeded

**Step 3: Commit any fixes**

If tests fail, fix issues and commit:
```bash
git add -A
git commit -m "fix: resolve test failures after logging options migration"
```

---

## Phase 6: Documentation and Cleanup

### Task 20: Update CHANGELOG

**Files:**
- Modify: `CHANGELOG.md` (if exists)

**Step 1: Add entry for new feature**

```markdown
## [Unreleased]

### Added
- Source-generated calling methods with customizable logging options (#1400)
- `CommandLoggingOptions` class for per-command logging control
- Logging verbosity levels: Silent, Minimal, Normal, Detailed, Diagnostic
- Configurable output options: timestamps, exit codes, working directory, execution time
- DotNet services now use generated code for consistency
```

**Step 2: Commit**

```bash
git add CHANGELOG.md
git commit -m "docs: update CHANGELOG for logging options feature"
```

---

### Task 21: Final Build and Test

**Step 1: Clean and rebuild everything**

Run: `dotnet clean ModularPipelines.sln && dotnet build ModularPipelines.sln -c Release`
Expected: Build succeeded

**Step 2: Run all tests**

Run: `dotnet test ModularPipelines.sln -c Release`
Expected: All tests pass

**Step 3: Verify no uncommitted changes**

Run: `git status`
Expected: Clean working tree

---

## Summary

This plan implements Issue #1400 in 21 tasks across 6 phases:

1. **Phase 1** (Tasks 1-5): Core infrastructure - CommandLoggingOptions, ICommand, ICommandLogger, Command
2. **Phase 2** (Tasks 6-8): Generator updates - Interface, Implementation, SubDomain generators
3. **Phase 3** (Tasks 9-12): Regenerate existing tools - Git, Docker, Helm, Google
4. **Phase 4** (Tasks 13-16): Migrate DotNet to generated code
5. **Phase 5** (Tasks 17-19): Testing
6. **Phase 6** (Tasks 20-21): Documentation and cleanup

**Key API Change:**
```csharp
// Before
await context.DotNet().Build(options);

// After (backward compatible - loggingOptions defaults to null)
await context.DotNet().Build(options);

// New capability
await context.DotNet().Build(options, CommandLoggingOptions.Silent);
await context.DotNet().Build(options, CommandLoggingOptions.Diagnostic);
await context.DotNet().Build(options, new CommandLoggingOptions
{
    Verbosity = CommandLogVerbosity.Detailed,
    IncludeTimestamps = true,
    ShowExecutionTime = true
});
```
