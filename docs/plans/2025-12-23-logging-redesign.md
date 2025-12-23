# Logging System Redesign Implementation Plan

> **For Claude:** REQUIRED SUB-SKILL: Use superpowers:executing-plans to implement this plan task-by-task.

**Goal:** Redesign the collapsible logging system with a clean architecture, proper CI-adaptive formatting with duration/status, and remove redundant abstraction layers.

**Architecture:** Replace 6 redundant classes/interfaces (`SmartCollapsableLogging`, `SmartCollapsableLoggingStringBlockProvider`, `CollapsibleSectionManager`, `ICollapsableLogging`, `IInternalCollapsableLogging`, `ISmartCollapsableLoggingStringBlockProvider`) with a single consolidated `ModuleOutputWriter` that handles buffering, CI formatting, and section headers with duration/status. Keep the clean `IBuildSystemFormatter` hierarchy.

**Tech Stack:** C#, .NET, Microsoft.Extensions.DependencyInjection, Spectre.Console

---

## Task 1: Create New Public Interface

**Files:**
- Create: `src/ModularPipelines/Logging/IModuleOutputWriter.cs`

**Step 1: Create the new public interface**

```csharp
namespace ModularPipelines.Logging;

/// <summary>
/// Writes grouped/collapsible output for modules.
/// Automatically handles CI-specific formatting, buffering, and section headers with duration/status.
/// </summary>
/// <remarks>
/// This interface replaces <c>ICollapsableLogging</c> with a cleaner API.
/// Output is buffered during module execution and flushed as a grouped block when the module completes.
/// Section headers include module name, status (success/failure), and duration.
/// </remarks>
/// <example>
/// <code>
/// // Write to module output buffer
/// outputWriter.WriteLine("Building project...");
/// outputWriter.WriteLine("Build completed successfully");
///
/// // Create nested section within module output
/// using (outputWriter.BeginSection("Compile"))
/// {
///     outputWriter.WriteLine("Compiling source files...");
/// }
///
/// // Write directly to console (use sparingly)
/// outputWriter.WriteLineDirect("Critical: Starting long operation...");
/// </code>
/// </example>
public interface IModuleOutputWriter
{
    /// <summary>
    /// Writes a line to the module's output buffer.
    /// Content is flushed when the module completes, grouped under a collapsible section.
    /// </summary>
    /// <param name="message">The message to write.</param>
    void WriteLine(string message);

    /// <summary>
    /// Writes a line directly to console, bypassing the buffer.
    /// Use sparingly - for critical messages during long-running operations.
    /// </summary>
    /// <param name="message">The message to write.</param>
    void WriteLineDirect(string message);

    /// <summary>
    /// Creates a nested collapsible section within the module's output.
    /// </summary>
    /// <param name="name">The name of the section.</param>
    /// <returns>A disposable that ends the section when disposed.</returns>
    IDisposable BeginSection(string name);
}
```

**Step 2: Verify file compiles**

Run: `dotnet build src/ModularPipelines/ModularPipelines.csproj --no-restore`
Expected: Build succeeded

**Step 3: Commit**

```bash
git add src/ModularPipelines/Logging/IModuleOutputWriter.cs
git commit -m "feat(logging): add IModuleOutputWriter interface

New clean public interface replacing ICollapsableLogging with:
- WriteLine for buffered output
- WriteLineDirect for immediate output
- BeginSection for nested collapsible sections"
```

---

## Task 2: Create Module Output Context

**Files:**
- Create: `src/ModularPipelines/Logging/ModuleOutputContext.cs`

**Step 1: Create the context class**

```csharp
namespace ModularPipelines.Logging;

/// <summary>
/// Tracks module execution state for output formatting.
/// Used by <see cref="ModuleOutputWriter"/> to format section headers with duration and status.
/// </summary>
internal class ModuleOutputContext
{
    /// <summary>
    /// Gets the module name for section headers.
    /// </summary>
    public string ModuleName { get; }

    /// <summary>
    /// Gets the UTC time when the module started executing.
    /// </summary>
    public DateTime StartTimeUtc { get; }

    /// <summary>
    /// Gets or sets the exception if the module failed.
    /// </summary>
    public Exception? Exception { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ModuleOutputContext"/> class.
    /// </summary>
    /// <param name="moduleName">The module name.</param>
    public ModuleOutputContext(string moduleName)
    {
        ModuleName = moduleName;
        StartTimeUtc = DateTime.UtcNow;
    }

    /// <summary>
    /// Gets the duration since the module started.
    /// </summary>
    public TimeSpan GetDuration() => DateTime.UtcNow - StartTimeUtc;

    /// <summary>
    /// Formats the section header with module name, status, and duration.
    /// </summary>
    /// <returns>Formatted section header string.</returns>
    public string FormatSectionHeader()
    {
        var duration = GetDuration();
        var durationStr = duration.TotalSeconds >= 60
            ? $"{duration.TotalMinutes:F1}m"
            : $"{duration.TotalSeconds:F1}s";

        if (Exception != null)
        {
            return $"{ModuleName} \u2717 ({durationStr}) - {Exception.GetType().Name}";
        }

        return $"{ModuleName} \u2713 ({durationStr})";
    }
}
```

**Step 2: Verify file compiles**

Run: `dotnet build src/ModularPipelines/ModularPipelines.csproj --no-restore`
Expected: Build succeeded

**Step 3: Commit**

```bash
git add src/ModularPipelines/Logging/ModuleOutputContext.cs
git commit -m "feat(logging): add ModuleOutputContext for tracking module state

Tracks module name, start time, and exception status.
Formats section headers with status indicator and duration."
```

---

## Task 3: Create Module Output Writer Implementation

**Files:**
- Create: `src/ModularPipelines/Logging/ModuleOutputWriter.cs`

**Step 1: Create the implementation**

```csharp
using ModularPipelines.Engine;

namespace ModularPipelines.Logging;

/// <summary>
/// Consolidated implementation for module output handling.
/// Manages buffering, CI-specific formatting, and section headers with duration/status.
/// </summary>
/// <remarks>
/// Uses a global static lock to ensure module outputs don't interleave.
/// Each module's output is flushed as an atomic block when the module completes.
/// </remarks>
internal class ModuleOutputWriter : IModuleOutputWriter, IDisposable
{
    /// <summary>
    /// Global lock to ensure only one module can flush its output at a time.
    /// This prevents output from multiple modules being interwoven.
    /// </summary>
    private static readonly object FlushLock = new();

    private readonly ILogEventBuffer _buffer;
    private readonly IBuildSystemFormatter _formatter;
    private readonly IConsoleWriter _consoleWriter;
    private readonly ISecretObfuscator _secretObfuscator;
    private readonly ModuleOutputContext _context;
    private readonly object _writeLock = new();
    private bool _isDisposed;

    public ModuleOutputWriter(
        ILogEventBuffer buffer,
        IBuildSystemFormatterProvider formatterProvider,
        IConsoleWriter consoleWriter,
        ISecretObfuscator secretObfuscator,
        string moduleName)
    {
        _buffer = buffer;
        _formatter = formatterProvider.GetFormatter();
        _consoleWriter = consoleWriter;
        _secretObfuscator = secretObfuscator;
        _context = new ModuleOutputContext(moduleName);
    }

    /// <inheritdoc />
    public void WriteLine(string message)
    {
        lock (_writeLock)
        {
            if (_isDisposed)
            {
                return;
            }

            var obfuscated = _secretObfuscator.Obfuscate(message, null) ?? message;
            _buffer.Add(obfuscated);
        }
    }

    /// <inheritdoc />
    public void WriteLineDirect(string message)
    {
        var obfuscated = _secretObfuscator.Obfuscate(message, null) ?? message;
        _consoleWriter.LogToConsole(obfuscated);
    }

    /// <inheritdoc />
    public IDisposable BeginSection(string name)
    {
        return new OutputSection(this, name);
    }

    /// <summary>
    /// Sets the exception for failed module status in section header.
    /// </summary>
    public void SetException(Exception exception)
    {
        _context.Exception = exception;
    }

    /// <summary>
    /// Flushes all buffered output with section header/footer.
    /// </summary>
    public void Dispose()
    {
        lock (_writeLock)
        {
            if (_isDisposed)
            {
                return;
            }

            _isDisposed = true;
        }

        if (!_buffer.HasEvents)
        {
            return;
        }

        lock (FlushLock)
        {
            var sectionHeader = _context.FormatSectionHeader();

            // Start collapsible section
            var startCommand = _formatter.GetStartBlockCommand(sectionHeader);
            if (startCommand != null)
            {
                _consoleWriter.LogToConsole(startCommand);
            }

            // Flush all buffered content
            var events = _buffer.GetAndClear();
            foreach (var evt in events)
            {
                if (evt.IsString && evt.StringValue != null)
                {
                    _consoleWriter.LogToConsole(evt.StringValue);
                }
            }

            // End collapsible section
            var endCommand = _formatter.GetEndBlockCommand(sectionHeader);
            if (endCommand != null)
            {
                _consoleWriter.LogToConsole(endCommand);
            }
        }

        GC.SuppressFinalize(this);
    }

    ~ModuleOutputWriter()
    {
        Dispose();
    }

    /// <summary>
    /// Represents a nested output section within module output.
    /// </summary>
    private sealed class OutputSection : IDisposable
    {
        private readonly ModuleOutputWriter _writer;
        private readonly string _name;

        public OutputSection(ModuleOutputWriter writer, string name)
        {
            _writer = writer;
            _name = name;

            var startCommand = _writer._formatter.GetStartBlockCommand(name);
            if (startCommand != null)
            {
                _writer._buffer.Add(startCommand);
            }
        }

        public void Dispose()
        {
            var endCommand = _writer._formatter.GetEndBlockCommand(_name);
            if (endCommand != null)
            {
                _writer._buffer.Add(endCommand);
            }
        }
    }
}
```

**Step 2: Verify file compiles**

Run: `dotnet build src/ModularPipelines/ModularPipelines.csproj --no-restore`
Expected: Build succeeded

**Step 3: Commit**

```bash
git add src/ModularPipelines/Logging/ModuleOutputWriter.cs
git commit -m "feat(logging): add ModuleOutputWriter implementation

Consolidated output handling with:
- Buffered output with global flush lock
- CI-adaptive section headers with duration/status
- Nested section support via BeginSection
- Secret obfuscation"
```

---

## Task 4: Create Factory for ModuleOutputWriter

**Files:**
- Create: `src/ModularPipelines/Logging/IModuleOutputWriterFactory.cs`
- Create: `src/ModularPipelines/Logging/ModuleOutputWriterFactory.cs`

**Step 1: Create the factory interface**

```csharp
namespace ModularPipelines.Logging;

/// <summary>
/// Factory for creating scoped <see cref="IModuleOutputWriter"/> instances.
/// </summary>
internal interface IModuleOutputWriterFactory
{
    /// <summary>
    /// Creates a new output writer for the specified module.
    /// </summary>
    /// <param name="moduleName">The module name for section headers.</param>
    /// <returns>A new output writer instance.</returns>
    ModuleOutputWriter Create(string moduleName);
}
```

**Step 2: Create the factory implementation**

```csharp
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Engine;

namespace ModularPipelines.Logging;

/// <summary>
/// Factory for creating scoped <see cref="ModuleOutputWriter"/> instances.
/// </summary>
internal class ModuleOutputWriterFactory : IModuleOutputWriterFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IBuildSystemFormatterProvider _formatterProvider;
    private readonly IConsoleWriter _consoleWriter;
    private readonly ISecretObfuscator _secretObfuscator;

    public ModuleOutputWriterFactory(
        IServiceProvider serviceProvider,
        IBuildSystemFormatterProvider formatterProvider,
        IConsoleWriter consoleWriter,
        ISecretObfuscator secretObfuscator)
    {
        _serviceProvider = serviceProvider;
        _formatterProvider = formatterProvider;
        _consoleWriter = consoleWriter;
        _secretObfuscator = secretObfuscator;
    }

    /// <inheritdoc />
    public ModuleOutputWriter Create(string moduleName)
    {
        // Get a fresh scoped buffer for this module
        var scope = _serviceProvider.CreateScope();
        var buffer = scope.ServiceProvider.GetRequiredService<ILogEventBuffer>();

        return new ModuleOutputWriter(
            buffer,
            _formatterProvider,
            _consoleWriter,
            _secretObfuscator,
            moduleName);
    }
}
```

**Step 3: Verify files compile**

Run: `dotnet build src/ModularPipelines/ModularPipelines.csproj --no-restore`
Expected: Build succeeded

**Step 4: Commit**

```bash
git add src/ModularPipelines/Logging/IModuleOutputWriterFactory.cs src/ModularPipelines/Logging/ModuleOutputWriterFactory.cs
git commit -m "feat(logging): add ModuleOutputWriterFactory

Factory creates scoped ModuleOutputWriter instances with fresh buffers per module."
```

---

## Task 5: Update DependencyInjectionSetup

**Files:**
- Modify: `src/ModularPipelines/DependencyInjection/DependencyInjectionSetup.cs`

**Step 1: Add new registrations and remove old ones**

In `DependencyInjectionSetup.cs`, make these changes:

1. Remove these lines from singletons section:
```csharp
// REMOVE these lines:
.AddSingleton<ICollapsableLogging, SmartCollapsableLogging>()
.AddSingleton<IInternalCollapsableLogging, SmartCollapsableLogging>()
.AddSingleton<ISmartCollapsableLoggingStringBlockProvider, SmartCollapsableLoggingStringBlockProvider>()
```

2. Remove this line from scoped section:
```csharp
// REMOVE this line:
.AddScoped<ICollapsibleSectionManager, CollapsibleSectionManager>()
```

3. Add new registration in singletons section:
```csharp
// ADD this line:
.AddSingleton<IModuleOutputWriterFactory, ModuleOutputWriterFactory>()
```

**Step 2: Verify build compiles (will have errors until old files are deleted)**

Run: `dotnet build src/ModularPipelines/ModularPipelines.csproj --no-restore`
Expected: Errors related to missing types (expected at this stage)

**Step 3: Commit**

```bash
git add src/ModularPipelines/DependencyInjection/DependencyInjectionSetup.cs
git commit -m "refactor(logging): update DI registrations for new output system

Remove old ICollapsableLogging registrations.
Add ModuleOutputWriterFactory registration."
```

---

## Task 6: Update ModuleLogger to Use New System

**Files:**
- Modify: `src/ModularPipelines/Logging/ModuleLogger.cs`

**Step 1: Simplify ModuleLogger**

Replace the `_lifecycleCoordinator` dependency with `IModuleOutputWriterFactory`. The key changes:

1. Remove `ILoggerLifecycleCoordinator` dependency
2. Add `IModuleOutputWriterFactory` dependency
3. Create `ModuleOutputWriter` on construction
4. Delegate `LogToConsole` to the output writer
5. Update `Dispose` to dispose the output writer

The `Log<TState>` method continues to buffer to `ILogEventBuffer` for ILogger events.
The `LogToConsole` method uses the new output writer.

**Step 2: Verify build**

Run: `dotnet build src/ModularPipelines/ModularPipelines.csproj --no-restore`
Expected: Build succeeded (or errors from files not yet deleted)

**Step 3: Commit**

```bash
git add src/ModularPipelines/Logging/ModuleLogger.cs
git commit -m "refactor(logging): update ModuleLogger to use ModuleOutputWriter

Replace lifecycle coordinator with output writer factory.
Simplify disposal to delegate to output writer."
```

---

## Task 7: Delete Old Files

**Files:**
- Delete: `src/ModularPipelines/SmartCollapsableLogging.cs`
- Delete: `src/ModularPipelines/SmartCollapsableLoggingStringBlockProvider.cs`
- Delete: `src/ModularPipelines/ISmartCollapsableLoggingStringBlockProvider.cs`
- Delete: `src/ModularPipelines/Interfaces/ICollapsableLogging.cs`
- Delete: `src/ModularPipelines/Interfaces/IInternalCollapsableLogging.cs`
- Delete: `src/ModularPipelines/Logging/CollapsibleSectionManager.cs`
- Delete: `src/ModularPipelines/Logging/LoggerLifecycleCoordinator.cs`

**Step 1: Delete the files**

```bash
rm src/ModularPipelines/SmartCollapsableLogging.cs
rm src/ModularPipelines/SmartCollapsableLoggingStringBlockProvider.cs
rm src/ModularPipelines/ISmartCollapsableLoggingStringBlockProvider.cs
rm src/ModularPipelines/Interfaces/ICollapsableLogging.cs
rm src/ModularPipelines/Interfaces/IInternalCollapsableLogging.cs
rm src/ModularPipelines/Logging/CollapsibleSectionManager.cs
rm src/ModularPipelines/Logging/LoggerLifecycleCoordinator.cs
```

**Step 2: Verify build (will have errors from dependent packages)**

Run: `dotnet build src/ModularPipelines/ModularPipelines.csproj --no-restore`
Expected: Build succeeded for core package

**Step 3: Commit**

```bash
git add -A
git commit -m "refactor(logging): remove redundant logging abstraction layers

Delete 7 files replaced by ModuleOutputWriter:
- SmartCollapsableLogging
- SmartCollapsableLoggingStringBlockProvider
- ISmartCollapsableLoggingStringBlockProvider
- ICollapsableLogging
- IInternalCollapsableLogging
- CollapsibleSectionManager
- LoggerLifecycleCoordinator"
```

---

## Task 8: Update GitHub Package

**Files:**
- Modify: `src/ModularPipelines.GitHub/IGitHub.cs`
- Modify: `src/ModularPipelines.GitHub/GitHub.cs` (implementation)

**Step 1: Update interface to use new base**

Change `IGitHub` to inherit from `IModuleOutputWriter` instead of `ICollapsableLogging`:

```csharp
using ModularPipelines.Logging;
using Octokit;

namespace ModularPipelines.GitHub;

public interface IGitHub : IModuleOutputWriter
{
    IGitHubClient Client { get; }

    IGitHubRepositoryInfo RepositoryInfo { get; }

    IGitHubEnvironmentVariables EnvironmentVariables { get; }
}
```

**Step 2: Update implementation class**

Update the GitHub implementation to implement the new interface methods.

**Step 3: Verify build**

Run: `dotnet build src/ModularPipelines.GitHub/ModularPipelines.GitHub.csproj --no-restore`
Expected: Build succeeded

**Step 4: Commit**

```bash
git add src/ModularPipelines.GitHub/
git commit -m "refactor(github): update to use IModuleOutputWriter

Replace ICollapsableLogging with new IModuleOutputWriter interface."
```

---

## Task 9: Update Azure Pipelines Package

**Files:**
- Modify: `src/ModularPipelines.Azure.Pipelines/IAzurePipeline.cs`
- Modify: `src/ModularPipelines.Azure.Pipelines/AzurePipeline.cs` (implementation)

**Step 1: Update interface**

```csharp
using ModularPipelines.Logging;

namespace ModularPipelines.Azure.Pipelines;

public interface IAzurePipeline : IModuleOutputWriter
{
    public bool IsRunningOnAzurePipelines { get; }

    public AzurePipelineVariables Variables { get; }
}
```

**Step 2: Update implementation class**

Update the AzurePipeline implementation to implement the new interface methods.

**Step 3: Verify build**

Run: `dotnet build src/ModularPipelines.Azure.Pipelines/ModularPipelines.Azure.Pipelines.csproj --no-restore`
Expected: Build succeeded

**Step 4: Commit**

```bash
git add src/ModularPipelines.Azure.Pipelines/
git commit -m "refactor(azure): update to use IModuleOutputWriter

Replace ICollapsableLogging with new IModuleOutputWriter interface."
```

---

## Task 10: Update TeamCity Package

**Files:**
- Modify: `src/ModularPipelines.TeamCity/ITeamCity.cs`
- Modify: `src/ModularPipelines.TeamCity/TeamCity.cs` (implementation)

**Step 1: Update interface**

```csharp
using ModularPipelines.Logging;

namespace ModularPipelines.TeamCity;

public interface ITeamCity : IModuleOutputWriter
{
    ITeamCityEnvironmentVariables EnvironmentVariables { get; }
}
```

**Step 2: Update implementation class**

Update the TeamCity implementation to implement the new interface methods.

**Step 3: Verify build**

Run: `dotnet build src/ModularPipelines.TeamCity/ModularPipelines.TeamCity.csproj --no-restore`
Expected: Build succeeded

**Step 4: Commit**

```bash
git add src/ModularPipelines.TeamCity/
git commit -m "refactor(teamcity): update to use IModuleOutputWriter

Replace ICollapsableLogging with new IModuleOutputWriter interface."
```

---

## Task 11: Update DependencyPrinter

**Files:**
- Modify: `src/ModularPipelines/Engine/DependencyPrinter.cs`

**Step 1: Update to use new API**

The `DependencyPrinter` uses `IInternalCollapsableLogging`. Update it to use `IModuleOutputWriter` or direct console output.

**Step 2: Verify build**

Run: `dotnet build src/ModularPipelines/ModularPipelines.csproj --no-restore`
Expected: Build succeeded

**Step 3: Commit**

```bash
git add src/ModularPipelines/Engine/DependencyPrinter.cs
git commit -m "refactor(logging): update DependencyPrinter to new output system"
```

---

## Task 12: Full Solution Build and Fix Remaining Issues

**Step 1: Build full solution**

Run: `dotnet build ModularPipelines.sln -c Release`
Expected: Identify any remaining compilation errors

**Step 2: Fix any remaining references**

Search for any remaining references to old types and update them.

**Step 3: Commit fixes**

```bash
git add -A
git commit -m "fix(logging): resolve remaining references to old logging types"
```

---

## Task 13: Run Tests

**Step 1: Run unit tests**

Run: `dotnet test test/ModularPipelines.UnitTests/ModularPipelines.UnitTests.csproj --no-build`
Expected: All tests pass (some may need updates)

**Step 2: Fix failing tests**

Update any tests that reference old logging types.

**Step 3: Commit test fixes**

```bash
git add -A
git commit -m "test(logging): update tests for new output system"
```

---

## Task 14: Final Verification

**Step 1: Clean build**

```bash
dotnet clean ModularPipelines.sln
dotnet build ModularPipelines.sln -c Release
```

**Step 2: Run all tests**

```bash
dotnet test ModularPipelines.sln -c Release
```

**Step 3: Final commit**

```bash
git add -A
git commit -m "feat(logging): complete logging system redesign

Breaking change: ICollapsableLogging replaced with IModuleOutputWriter

New features:
- Section headers with duration and status (ModuleName ✓ (2.3s))
- Cleaner API with WriteLine, WriteLineDirect, BeginSection
- Consolidated implementation (7 files → 4 files)
- Fixed naming inconsistency (Collapsable → consistent naming)

Closes #1402"
```

---

## Summary

**Files Created (4):**
- `src/ModularPipelines/Logging/IModuleOutputWriter.cs`
- `src/ModularPipelines/Logging/ModuleOutputContext.cs`
- `src/ModularPipelines/Logging/ModuleOutputWriter.cs`
- `src/ModularPipelines/Logging/ModuleOutputWriterFactory.cs`

**Files Deleted (7):**
- `src/ModularPipelines/SmartCollapsableLogging.cs`
- `src/ModularPipelines/SmartCollapsableLoggingStringBlockProvider.cs`
- `src/ModularPipelines/ISmartCollapsableLoggingStringBlockProvider.cs`
- `src/ModularPipelines/Interfaces/ICollapsableLogging.cs`
- `src/ModularPipelines/Interfaces/IInternalCollapsableLogging.cs`
- `src/ModularPipelines/Logging/CollapsibleSectionManager.cs`
- `src/ModularPipelines/Logging/LoggerLifecycleCoordinator.cs`

**Files Modified:**
- `src/ModularPipelines/DependencyInjection/DependencyInjectionSetup.cs`
- `src/ModularPipelines/Logging/ModuleLogger.cs`
- `src/ModularPipelines/Engine/DependencyPrinter.cs`
- `src/ModularPipelines.GitHub/IGitHub.cs` + implementation
- `src/ModularPipelines.Azure.Pipelines/IAzurePipeline.cs` + implementation
- `src/ModularPipelines.TeamCity/ITeamCity.cs` + implementation

**Breaking Changes:**
- `ICollapsableLogging` removed, replaced by `IModuleOutputWriter`
- `StartConsoleLogGroup`/`EndConsoleLogGroup` replaced by `BeginSection` (IDisposable pattern)
- `LogToConsole` replaced by `WriteLine`/`WriteLineDirect`
