# Consolidate IPipeline Interfaces Design

> **For Claude:** REQUIRED SUB-SKILL: Use superpowers:executing-plans to implement this plan task-by-task.

**Goal:** Consolidate 23+ fragmented IPipeline* interfaces into a cleaner, more cohesive hierarchy that reduces cognitive load while maintaining backwards compatibility.

**Architecture:** Reorganize interfaces into three clear categories: (1) User-facing context interfaces for module developers, (2) Internal engine interfaces, and (3) Extension point interfaces. Maintain backwards compatibility through deprecation attributes and extension methods.

**Tech Stack:** C# 12, .NET 8/9/10, Interface Segregation Principle

---

## Problem Statement

The current codebase has 23+ interfaces prefixed with `IPipeline*`:

### User-Facing Context Interfaces (6)
- `IPipelineContext` - Main context, extends IPipelineHookContext
- `IPipelineHookContext` - Composite interface for hooks
- `IPipelineServices` - DI and configuration access
- `IPipelineLogging` - Logging capabilities
- `IPipelineTools` - Command execution helpers
- `IPipelineEncoding` - JSON/XML serialization
- `IPipelineFileSystem` - File operations
- `IPipelineEnvironment` - Environment and build system detection

### Internal Engine Interfaces (10)
- `IPipelineExecutor` - Pipeline execution orchestration
- `IPipelineInitializer` - Pipeline initialization
- `IPipelineOutputCoordinator` - Output coordination
- `IPipelineOutputScope` - Output scoping
- `IPipelineSummaryFactory` - Summary generation
- `IPipelineContextProvider` - Context provisioning
- `IPipelineFileWriter` - File writing
- `IPipelineSetupExecutor` - Setup execution
- `IPipelineValidationService` - Validation orchestration
- `IPipelineValidator` - Individual validators

### Extension Point Interfaces (4)
- `IPipelineHost` - Host abstraction
- `IPipelineGlobalHooks` - Global pipeline hooks
- `IPipelineModuleHooks` - Module-level hooks
- `IPipelineRequirement` - Pipeline requirements

### Additional (3)
- `IPipelineServiceContainerWrapper` - DI wrapper
- `IModuleContext` - Module-specific context

## Issues with Current Structure

1. **Cognitive Overload**: Too many interfaces to remember
2. **Unclear Boundaries**: Hard to know which interface to use when
3. **Deep Inheritance**: `IModuleContext : IPipelineContext : IPipelineHookContext : (6 interfaces)`
4. **Inconsistent Naming**: Mix of `IPipeline*` and `IModule*`
5. **Tight Coupling**: Engine interfaces exposed publicly when they should be internal

## Design Decisions

Based on design review, we're implementing **Option 4: Comprehensive Redesign**:

1. **Flatten the user-facing hierarchy**
2. **Mark internal interfaces as internal**
3. **Deprecate redundant interfaces**
4. **Add helper extension methods**

## Target Interface Structure

### User-Facing (Public)

```
IModuleContext (Primary interface for modules)
├── Services: IServiceProvider, Configuration, Options
├── FileSystem: IFileSystemContext, IZip, IChecksum
├── Environment: IEnvironmentContext, IBuildSystemDetector
├── Commands: ICommand, IBash, IPowershell
├── Http: IHttp, IDownloader
├── Encoding: IJson, IXml, IYaml
├── Logging: IModuleLogger Logger
└── Module Operations:
    ├── GetModule<TModule, TResult>()
    ├── GetModuleIfRegistered<TModule, TResult>()
    └── SubModule(name, action)
```

### Extension Points (Public)

```
IPipelineGlobalHooks - For global hook implementations
IPipelineModuleHooks - For module hook implementations
IPipelineRequirement - For requirement implementations
IPipelineHost - For host implementations
```

### Internal Engine (Internal)

All `IPipeline*Executor`, `IPipeline*Coordinator`, etc. become internal.

---

## Implementation Plan

### Task 1: Audit and Document Current Usage

**Files:**
- Analyze: All `IPipeline*.cs` files
- Create: `docs/architecture/interface-audit.md`

**Step 1: Generate usage report**
```bash
cd src/ModularPipelines
grep -r "IPipeline" --include="*.cs" | grep -v "//\|Binary" | sort | uniq -c | sort -rn > interface-usage.txt
```

**Step 2: Document each interface's purpose and consumers**

List each interface with:
- Current location
- Public/Internal status
- Number of implementations
- Number of usages
- Recommendation (keep/deprecate/make internal)

**Step 3: Commit audit**
```bash
git add docs/architecture/interface-audit.md
git commit -m "docs: audit IPipeline interface usage for consolidation"
```

---

### Task 2: Make Engine Interfaces Internal

**Files:**
- Modify: `src/ModularPipelines/Engine/Executors/IPipelineExecutor.cs`
- Modify: `src/ModularPipelines/Engine/Executors/IPipelineInitializer.cs`
- Modify: `src/ModularPipelines/Engine/Executors/IPipelineOutputCoordinator.cs`
- Modify: `src/ModularPipelines/Engine/Executors/IPipelineOutputScope.cs`
- Modify: `src/ModularPipelines/Engine/Executors/IPipelineSummaryFactory.cs`
- Modify: `src/ModularPipelines/Engine/IPipelineContextProvider.cs`
- Modify: `src/ModularPipelines/Engine/IPipelineFileWriter.cs`
- Modify: `src/ModularPipelines/Engine/IPipelineSetupExecutor.cs`

**Step 1: Write test verifying interfaces are not used externally**

Create `test/ModularPipelines.UnitTests/InterfaceVisibilityTests.cs`:
```csharp
[Test]
public void EngineInterfaces_ShouldBeInternal()
{
    var assembly = typeof(IPipelineContext).Assembly;
    var engineInterfaces = assembly.GetTypes()
        .Where(t => t.IsInterface)
        .Where(t => t.Namespace?.Contains("Engine") == true)
        .Where(t => !t.Name.Contains("IModule")); // Exclude module interfaces

    foreach (var iface in engineInterfaces)
    {
        Assert.That(iface.IsNotPublic || iface.IsNestedAssembly,
            $"{iface.Name} should be internal");
    }
}
```

**Step 2: Run test to verify it fails**
```bash
dotnet run --project test/ModularPipelines.UnitTests -- --filter "InterfaceVisibilityTests"
```
Expected: FAIL

**Step 3: Change visibility to internal**

For each file, change:
```csharp
// Before
public interface IPipelineExecutor { ... }

// After
internal interface IPipelineExecutor { ... }
```

**Step 4: Run test to verify it passes**
```bash
dotnet run --project test/ModularPipelines.UnitTests -- --filter "InterfaceVisibilityTests"
```
Expected: PASS

**Step 5: Build full solution to verify no external breaks**
```bash
dotnet build ModularPipelines.sln -c Release
```

**Step 6: Commit**
```bash
git add -A
git commit -m "refactor: make engine interfaces internal

Engine interfaces are implementation details and should not be
part of the public API surface. This reduces cognitive load for
library consumers.

Addresses #1867"
```

---

### Task 3: Simplify Context Interface Hierarchy

**Files:**
- Modify: `src/ModularPipelines/Context/IPipelineHookContext.cs`
- Modify: `src/ModularPipelines/Context/IPipelineContext.cs`
- Modify: `src/ModularPipelines/Context/IModuleContext.cs`

**Step 1: Write test for simplified hierarchy**

```csharp
[Test]
public void IModuleContext_ShouldProvideAllCapabilities()
{
    var moduleContextType = typeof(IModuleContext);

    // Should have direct access to all capabilities
    Assert.That(moduleContextType.GetProperty("ServiceProvider"), Is.Not.Null);
    Assert.That(moduleContextType.GetProperty("FileSystem"), Is.Not.Null);
    Assert.That(moduleContextType.GetProperty("Environment"), Is.Not.Null);
    Assert.That(moduleContextType.GetProperty("Command"), Is.Not.Null);
    Assert.That(moduleContextType.GetProperty("Logger"), Is.Not.Null);
}
```

**Step 2: Run test to verify current state**

**Step 3: Flatten the hierarchy**

Current:
```
IModuleContext : IPipelineContext : IPipelineHookContext : (6 interfaces)
```

New:
```
IModuleContext : IPipelineHookContext
    // IPipelineContext methods moved here
    // IPipelineHookContext keeps inheriting from sub-interfaces
```

Mark `IPipelineContext` as obsolete:
```csharp
[Obsolete("Use IModuleContext directly. IPipelineContext will be removed in v2.0.")]
public interface IPipelineContext : IPipelineHookContext
{
    // Keep existing members for backwards compatibility
}
```

**Step 4: Run tests**
```bash
dotnet run --project test/ModularPipelines.UnitTests -- --filter "IModuleContext"
```

**Step 5: Build solution**
```bash
dotnet build ModularPipelines.sln -c Release
```

**Step 6: Commit**
```bash
git add -A
git commit -m "refactor: simplify context interface hierarchy

Flatten IModuleContext to inherit directly from IPipelineHookContext.
Mark IPipelineContext as obsolete - consumers should use IModuleContext.

Addresses #1867"
```

---

### Task 4: Add Context Extension Methods

**Files:**
- Create: `src/ModularPipelines/Context/ContextExtensions.cs`
- Test: `test/ModularPipelines.UnitTests/Context/ContextExtensionsTests.cs`

**Step 1: Write tests for extension methods**

```csharp
public class ContextExtensionsTests
{
    [Test]
    public void RunCommand_ShouldExecuteViaCommandHelper()
    {
        // Extension method should provide fluent access
        // await context.RunCommand("echo", "hello");
    }

    [Test]
    public void ReadFile_ShouldUseFileSystem()
    {
        // await context.ReadFile("path/to/file.txt");
    }

    [Test]
    public void GetService_ShouldResolveFromDI()
    {
        // var service = context.GetService<IMyService>();
    }
}
```

**Step 2: Run tests - expect fail**

**Step 3: Implement extension methods**

```csharp
namespace ModularPipelines.Context;

/// <summary>
/// Extension methods for <see cref="IModuleContext"/> providing simplified access to common operations.
/// </summary>
public static class ContextExtensions
{
    /// <summary>
    /// Executes a command and returns the result.
    /// </summary>
    public static Task<CommandResult> RunCommand(
        this IModuleContext context,
        string command,
        params string[] arguments)
    {
        return context.Command.ExecuteCommandLineTool(new CommandLineToolOptions(command)
        {
            Arguments = arguments
        });
    }

    /// <summary>
    /// Reads all text from a file.
    /// </summary>
    public static string ReadFile(this IModuleContext context, string path)
    {
        return context.FileSystem.ReadFile(path);
    }

    /// <summary>
    /// Gets a service from the DI container.
    /// </summary>
    public static T GetService<T>(this IModuleContext context) where T : notnull
    {
        return context.Get<T>() ?? throw new InvalidOperationException(
            $"Service {typeof(T).Name} is not registered");
    }

    /// <summary>
    /// Gets a service from the DI container, or null if not registered.
    /// </summary>
    public static T? TryGetService<T>(this IModuleContext context) where T : class
    {
        return context.Get<T>();
    }
}
```

**Step 4: Run tests - expect pass**

**Step 5: Commit**
```bash
git add -A
git commit -m "feat: add context extension methods for common operations

Provides simplified access to common operations:
- RunCommand() for quick command execution
- ReadFile() for file reading
- GetService<T>() for DI resolution

Addresses #1867"
```

---

### Task 5: Consolidate Validation Interfaces

**Files:**
- Modify: `src/ModularPipelines/Validation/IPipelineValidationService.cs`
- Modify: `src/ModularPipelines/Validation/IPipelineValidator.cs`

**Step 1: Analyze current validation interfaces**

```csharp
// IPipelineValidationService - orchestrates validation
// IPipelineValidator - individual validator

// Should be:
// - IPipelineValidationService stays (internal)
// - IPipelineValidator renamed to IValidator (simpler)
```

**Step 2: Make validation service internal if not already**

**Step 3: Rename IPipelineValidator to IValidator**

Use `[Obsolete]` for backwards compatibility:
```csharp
[Obsolete("Use IValidator instead. IPipelineValidator will be removed in v2.0.")]
public interface IPipelineValidator : IValidator { }

public interface IValidator
{
    Task<ValidationResult> ValidateAsync(IPipelineHookContext context);
}
```

**Step 4: Update implementations**

**Step 5: Run tests**
```bash
dotnet build ModularPipelines.sln -c Release
dotnet run --project test/ModularPipelines.UnitTests
```

**Step 6: Commit**
```bash
git add -A
git commit -m "refactor: simplify validation interface naming

Rename IPipelineValidator to IValidator for clarity.
Keep IPipelineValidator as obsolete alias for backwards compatibility.

Addresses #1867"
```

---

### Task 6: Update Documentation

**Files:**
- Modify: `docs/docs/getting-started.md` (if exists)
- Create: `docs/architecture/interface-hierarchy.md`

**Step 1: Create interface hierarchy documentation**

```markdown
# Interface Hierarchy

## User-Facing Interfaces

### IModuleContext
The primary interface for module developers. Provides access to all pipeline capabilities.

### IPipelineHookContext
Base interface for hook implementations. Use IModuleContext for modules.

## Extension Point Interfaces

### IPipelineGlobalHooks
Implement to receive callbacks for all module lifecycle events.

### IPipelineModuleHooks
Implement to receive callbacks for specific module events.

### IPipelineRequirement
Implement to define pipeline requirements that are validated at startup.

## Internal Interfaces
Engine interfaces are internal and should not be used directly.
```

**Step 2: Commit**
```bash
git add -A
git commit -m "docs: document consolidated interface hierarchy

Addresses #1867"
```

---

### Task 7: Add Deprecation Analyzer

**Files:**
- Modify: `src/ModularPipelines.Analyzers/` (if exists)

**Step 1: Check if analyzer project exists**

**Step 2: Add analyzer rule for deprecated interfaces**

If analyzer project exists, add a rule that warns when:
- Using `IPipelineContext` directly (suggest `IModuleContext`)
- Using `IPipelineValidator` (suggest `IValidator`)

**Step 3: Commit**
```bash
git add -A
git commit -m "feat(analyzers): add deprecation warnings for old interfaces

Addresses #1867"
```

---

### Task 8: Final Verification

**Step 1: Build entire solution**
```bash
dotnet build ModularPipelines.sln -c Release
```

**Step 2: Run all tests**
```bash
cd src/ModularPipelines.Build
dotnet run -c Release --framework net10.0
```

**Step 3: Verify no public API breaks**

Check that existing code still compiles:
```bash
dotnet build ModularPipelines.Examples.sln -c Release
```

**Step 4: Final commit**
```bash
git add -A
git commit -m "chore: final verification for interface consolidation

All tests passing, no public API breaks.

Closes #1867"
```

---

## Backwards Compatibility

- `IPipelineContext` marked `[Obsolete]` but still works
- `IPipelineValidator` renamed but alias exists
- Engine interfaces made internal (if used externally, they were misused)
- Extension methods provide new simplified API without breaking old code

## Migration Guide

For library consumers:

```csharp
// Before
public class MyModule : Module<string>
{
    protected override Task<string> ExecuteAsync(IPipelineContext context, CancellationToken ct)
    {
        var service = context.ServiceProvider.GetService<IMyService>();
        // ...
    }
}

// After (recommended)
public class MyModule : Module<string>
{
    protected override Task<string> ExecuteAsync(IModuleContext context, CancellationToken ct)
    {
        var service = context.GetService<IMyService>();
        // ...
    }
}
```

## Success Criteria

1. Engine interfaces are internal (not part of public API)
2. `IModuleContext` is the primary interface for modules
3. Extension methods provide simplified access to common operations
4. All existing code compiles with deprecation warnings only
5. Documentation clearly explains the interface hierarchy
