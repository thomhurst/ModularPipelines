# ModuleResult Discriminated Union Implementation Plan

> **For Claude:** REQUIRED SUB-SKILL: Use superpowers:executing-plans to implement this plan task-by-task.

**Goal:** Replace exception-throwing `ModuleResult<T>.Value` with a type-safe discriminated union pattern using C# records.

**Architecture:** Three sealed record variants (`Success`, `Failure`, `Skipped`) inherit from abstract `ModuleResult<T>`. Common metadata lives in the base. Pattern matching replaces try-catch. Timeout is a `Failure` with `ModuleTimeoutException` (pattern match on exception type if needed).

**Tech Stack:** .NET 10, C# records, System.Text.Json polymorphic serialization

---

## Task 1: Update IModuleResult Interface

**Files:**
- Modify: `src/ModularPipelines/Models/IModuleResult.cs`

**Step 1: Update the interface to add safe accessors**

Replace the file content with:

```csharp
using ModularPipelines.Enums;

namespace ModularPipelines.Models;

/// <summary>
/// Non-generic interface for type-erased module result access.
/// </summary>
public interface IModuleResult
{
    /// <summary>
    /// Gets the name of the module.
    /// </summary>
    string ModuleName { get; }

    /// <summary>
    /// Gets how long the module ran for.
    /// </summary>
    TimeSpan ModuleDuration { get; }

    /// <summary>
    /// Gets when the module started.
    /// </summary>
    DateTimeOffset ModuleStart { get; }

    /// <summary>
    /// Gets when the module ended.
    /// </summary>
    DateTimeOffset ModuleEnd { get; }

    /// <summary>
    /// Gets the status of the module.
    /// </summary>
    Status ModuleStatus { get; }

    /// <summary>
    /// Gets the type of result that is held.
    /// </summary>
    ModuleResultType ModuleResultType { get; }

    /// <summary>
    /// Gets whether the result is a success.
    /// </summary>
    bool IsSuccess { get; }

    /// <summary>
    /// Gets whether the result is a failure.
    /// </summary>
    bool IsFailure { get; }

    /// <summary>
    /// Gets whether the result was skipped.
    /// </summary>
    bool IsSkipped { get; }

    /// <summary>
    /// Gets the value if successful, or null/default otherwise. Does not throw.
    /// </summary>
    object? ValueOrDefault { get; }

    /// <summary>
    /// Gets the exception if failed, or null otherwise.
    /// </summary>
    Exception? ExceptionOrDefault { get; }

    /// <summary>
    /// Gets the skip decision if skipped, or null otherwise.
    /// </summary>
    SkipDecision? SkipDecisionOrDefault { get; }
}
```

**Step 2: Build to verify compilation**

Run: `dotnet build src/ModularPipelines/ModularPipelines.csproj -c Release`
Expected: Build errors (ModuleResult doesn't implement new members yet) - this is expected

**Step 3: Commit interface changes**

```bash
git add src/ModularPipelines/Models/IModuleResult.cs
git commit -m "refactor(IModuleResult): add safe accessor properties for discriminated union pattern"
```

---

## Task 2: Rewrite ModuleResult as Discriminated Union

**Files:**
- Modify: `src/ModularPipelines/Models/ModuleResult.cs`

**Step 1: Replace ModuleResult.cs with discriminated union implementation**

Replace the entire file with:

```csharp
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using ModularPipelines.Engine;
using ModularPipelines.Enums;

namespace ModularPipelines.Models;

/// <summary>
/// Represents the result of a module execution as a discriminated union.
/// Use pattern matching to handle Success, Failure, and Skipped cases.
/// </summary>
/// <typeparam name="T">The type of value returned on success.</typeparam>
/// <example>
/// <code>
/// var result = await myModule;
/// switch (result)
/// {
///     case ModuleResult&lt;string&gt;.Success { Value: var value }:
///         Console.WriteLine($"Got: {value}");
///         break;
///     case ModuleResult&lt;string&gt;.Failure { Exception: var ex }:
///         Console.WriteLine($"Failed: {ex.Message}");
///         break;
///     case ModuleResult&lt;string&gt;.Skipped { Decision: var skip }:
///         Console.WriteLine($"Skipped: {skip.Reason}");
///         break;
/// }
/// </code>
/// </example>
[JsonDerivedType(typeof(Success), "Success")]
[JsonDerivedType(typeof(Failure), "Failure")]
[JsonDerivedType(typeof(Skipped), "Skipped")]
public abstract record ModuleResult<T> : IModuleResult
{
    // === Metadata (available on all outcomes) ===

    /// <inheritdoc />
    [JsonInclude]
    public required string ModuleName { get; init; }

    /// <inheritdoc />
    [JsonInclude]
    public required TimeSpan ModuleDuration { get; init; }

    /// <inheritdoc />
    [JsonInclude]
    public required DateTimeOffset ModuleStart { get; init; }

    /// <inheritdoc />
    [JsonInclude]
    public required DateTimeOffset ModuleEnd { get; init; }

    /// <inheritdoc />
    [JsonInclude]
    public required Status ModuleStatus { get; init; }

    // === Quick checks ===

    /// <inheritdoc />
    [JsonIgnore]
    public bool IsSuccess => this is Success;

    /// <inheritdoc />
    [JsonIgnore]
    public bool IsFailure => this is Failure;

    /// <inheritdoc />
    [JsonIgnore]
    public bool IsSkipped => this is Skipped;

    // === Safe accessors (no exceptions) ===

    /// <summary>
    /// Gets the value if successful, or default(T) otherwise. Does not throw.
    /// </summary>
    [JsonIgnore]
    public T? ValueOrDefault => this is Success s ? s.Value : default;

    /// <inheritdoc />
    [JsonIgnore]
    object? IModuleResult.ValueOrDefault => ValueOrDefault;

    /// <inheritdoc />
    [JsonIgnore]
    public Exception? ExceptionOrDefault => this is Failure f ? f.Exception : null;

    /// <inheritdoc />
    [JsonIgnore]
    public SkipDecision? SkipDecisionOrDefault => this is Skipped sk ? sk.Decision : null;

    // === Computed for compatibility ===

    /// <inheritdoc />
    [JsonIgnore]
    public ModuleResultType ModuleResultType => this switch
    {
        Success => ModuleResultType.Success,
        Failure => ModuleResultType.Failure,
        Skipped => ModuleResultType.Skipped,
        _ => throw new InvalidOperationException("Unknown result type")
    };

    // === Internal: Module type tracking ===

    /// <summary>
    /// Gets or sets the type of the module that produced this result.
    /// </summary>
    [JsonIgnore]
    internal Type? ModuleType { get; init; }

    // === Pattern matching helpers ===

    /// <summary>
    /// Matches the result to one of three functions based on the outcome.
    /// </summary>
    /// <typeparam name="TResult">The return type of the match functions.</typeparam>
    /// <param name="onSuccess">Function to call if successful.</param>
    /// <param name="onFailure">Function to call if failed.</param>
    /// <param name="onSkipped">Function to call if skipped.</param>
    /// <returns>The result of the matched function.</returns>
    public TResult Match<TResult>(
        Func<T, TResult> onSuccess,
        Func<Exception, TResult> onFailure,
        Func<SkipDecision, TResult> onSkipped) => this switch
    {
        Success s => onSuccess(s.Value),
        Failure f => onFailure(f.Exception),
        Skipped sk => onSkipped(sk.Decision),
        _ => throw new InvalidOperationException("Unknown result type")
    };

    /// <summary>
    /// Executes one of three actions based on the outcome.
    /// </summary>
    /// <param name="onSuccess">Action to call if successful.</param>
    /// <param name="onFailure">Action to call if failed.</param>
    /// <param name="onSkipped">Action to call if skipped.</param>
    public void Switch(
        Action<T> onSuccess,
        Action<Exception> onFailure,
        Action<SkipDecision> onSkipped)
    {
        switch (this)
        {
            case Success s:
                onSuccess(s.Value);
                break;
            case Failure f:
                onFailure(f.Exception);
                break;
            case Skipped sk:
                onSkipped(sk.Decision);
                break;
        }
    }

    // === Discriminated variants ===

    /// <summary>
    /// Represents a successful module execution with a value.
    /// </summary>
    /// <param name="Value">The value produced by the module.</param>
    public sealed record Success(T Value) : ModuleResult<T>;

    /// <summary>
    /// Represents a failed module execution with an exception.
    /// </summary>
    /// <param name="Exception">The exception that caused the failure.</param>
    public sealed record Failure(Exception Exception) : ModuleResult<T>;

    /// <summary>
    /// Represents a skipped module execution.
    /// </summary>
    /// <param name="Decision">The skip decision containing the reason.</param>
    public sealed record Skipped(SkipDecision Decision) : ModuleResult<T>;

    // === Internal factory methods ===

    internal static Success CreateSuccess(T value, ModuleExecutionContext ctx) => new(value)
    {
        ModuleName = ctx.ModuleType.Name,
        ModuleDuration = (ctx.EndTime == DateTimeOffset.MinValue ? DateTimeOffset.Now : ctx.EndTime) -
                        (ctx.StartTime == DateTimeOffset.MinValue ? DateTimeOffset.Now : ctx.StartTime),
        ModuleStart = ctx.StartTime == DateTimeOffset.MinValue ? DateTimeOffset.Now : ctx.StartTime,
        ModuleEnd = ctx.EndTime == DateTimeOffset.MinValue ? DateTimeOffset.Now : ctx.EndTime,
        ModuleStatus = ctx.Status,
        ModuleType = ctx.ModuleType
    };

    internal static Failure CreateFailure(Exception exception, ModuleExecutionContext ctx) => new(exception)
    {
        ModuleName = ctx.ModuleType.Name,
        ModuleDuration = (ctx.EndTime == DateTimeOffset.MinValue ? DateTimeOffset.Now : ctx.EndTime) -
                        (ctx.StartTime == DateTimeOffset.MinValue ? DateTimeOffset.Now : ctx.StartTime),
        ModuleStart = ctx.StartTime == DateTimeOffset.MinValue ? DateTimeOffset.Now : ctx.StartTime,
        ModuleEnd = ctx.EndTime == DateTimeOffset.MinValue ? DateTimeOffset.Now : ctx.EndTime,
        ModuleStatus = ctx.Status,
        ModuleType = ctx.ModuleType
    };

    internal static Skipped CreateSkipped(SkipDecision decision, ModuleExecutionContext ctx) => new(decision)
    {
        ModuleName = ctx.ModuleType.Name,
        ModuleDuration = (ctx.EndTime == DateTimeOffset.MinValue ? DateTimeOffset.Now : ctx.EndTime) -
                        (ctx.StartTime == DateTimeOffset.MinValue ? DateTimeOffset.Now : ctx.StartTime),
        ModuleStart = ctx.StartTime == DateTimeOffset.MinValue ? DateTimeOffset.Now : ctx.StartTime,
        ModuleEnd = ctx.EndTime == DateTimeOffset.MinValue ? DateTimeOffset.Now : ctx.EndTime,
        ModuleStatus = ctx.Status,
        ModuleType = ctx.ModuleType
    };

    // Prevent external inheritance - only Success, Failure, Skipped are valid
    private protected ModuleResult()
    {
    }
}
```

**Step 2: Build to check for errors**

Run: `dotnet build src/ModularPipelines/ModularPipelines.csproj -c Release`
Expected: Build errors from files still referencing old ModuleResult pattern - this is expected

**Step 3: Commit the new discriminated union**

```bash
git add src/ModularPipelines/Models/ModuleResult.cs
git commit -m "refactor(ModuleResult): implement discriminated union with Success/Failure/Skipped variants"
```

---

## Task 3: Delete Obsolete Subclasses

**Files:**
- Delete: `src/ModularPipelines/Models/SkippedModuleResult.cs`
- Delete: `src/ModularPipelines/Models/TimedOutModuleResult.cs`

**Step 1: Delete the files**

```bash
rm src/ModularPipelines/Models/SkippedModuleResult.cs
rm src/ModularPipelines/Models/TimedOutModuleResult.cs
```

**Step 2: Commit deletions**

```bash
git add -A
git commit -m "refactor: remove SkippedModuleResult and TimedOutModuleResult (replaced by variants)"
```

---

## Task 4: Update ModuleResultFactory

**Files:**
- Modify: `src/ModularPipelines/Engine/Execution/ModuleResultFactory.cs`

**Step 1: Simplify the factory to use new static methods**

Replace the file with:

```csharp
using ModularPipelines.Models;

namespace ModularPipelines.Engine.Execution;

/// <summary>
/// Factory for creating ModuleResult instances.
/// </summary>
internal static class ModuleResultFactory
{
    /// <summary>
    /// Creates a successful ModuleResult for the specified value.
    /// </summary>
    public static ModuleResult<T> CreateSuccess<T>(T value, ModuleExecutionContext ctx)
    {
        return ModuleResult<T>.CreateSuccess(value, ctx);
    }

    /// <summary>
    /// Creates a failure ModuleResult for the specified exception.
    /// </summary>
    public static ModuleResult<T> CreateFailure<T>(Exception exception, ModuleExecutionContext ctx)
    {
        return ModuleResult<T>.CreateFailure(exception, ctx);
    }

    /// <summary>
    /// Creates a skipped ModuleResult for the specified skip decision.
    /// </summary>
    public static ModuleResult<T> CreateSkipped<T>(SkipDecision decision, ModuleExecutionContext ctx)
    {
        return ModuleResult<T>.CreateSkipped(decision, ctx);
    }

    /// <summary>
    /// Creates a skipped ModuleResult (type-erased version for engine use).
    /// </summary>
    public static IModuleResult CreateSkipped(Type resultType, ModuleExecutionContext executionContext)
    {
        var method = typeof(ModuleResultFactory)
            .GetMethod(nameof(CreateSkippedGeneric), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!
            .MakeGenericMethod(resultType);

        return (IModuleResult)method.Invoke(null, [executionContext.SkipResult ?? SkipDecision.DoNotSkip, executionContext])!;
    }

    /// <summary>
    /// Creates a failure ModuleResult (type-erased version for engine use).
    /// </summary>
    public static IModuleResult CreateException(Type resultType, Exception exception, ModuleExecutionContext executionContext)
    {
        var method = typeof(ModuleResultFactory)
            .GetMethod(nameof(CreateFailureGeneric), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!
            .MakeGenericMethod(resultType);

        return (IModuleResult)method.Invoke(null, [exception, executionContext])!;
    }

    private static IModuleResult CreateSkippedGeneric<T>(SkipDecision decision, ModuleExecutionContext ctx)
    {
        return ModuleResult<T>.CreateSkipped(decision, ctx);
    }

    private static IModuleResult CreateFailureGeneric<T>(Exception exception, ModuleExecutionContext ctx)
    {
        return ModuleResult<T>.CreateFailure(exception, ctx);
    }
}
```

**Step 2: Build to verify**

Run: `dotnet build src/ModularPipelines/ModularPipelines.csproj -c Release`
Expected: More build errors from other files - expected at this stage

**Step 3: Commit**

```bash
git add src/ModularPipelines/Engine/Execution/ModuleResultFactory.cs
git commit -m "refactor(ModuleResultFactory): simplify to use ModuleResult static factory methods"
```

---

## Task 5: Update ModuleExecutionPipeline

**Files:**
- Modify: `src/ModularPipelines/Engine/ModuleExecutionPipeline.cs`

**Step 1: Read current implementation**

Run: `grep -n "TimedOutModuleResult\|new ModuleResult\|SkippedModuleResult" src/ModularPipelines/Engine/ModuleExecutionPipeline.cs`

**Step 2: Update result creation to use factory methods**

Find and replace usages:
- Replace `new ModuleResult<T>(value, executionContext)` with `ModuleResult<T>.CreateSuccess(value, executionContext)`
- Replace `new ModuleResult<T>(exception, executionContext)` with `ModuleResult<T>.CreateFailure(exception, executionContext)`
- Replace `new SkippedModuleResult<T>(...)` with `ModuleResult<T>.CreateSkipped(...)`
- Replace `new TimedOutModuleResult<T>(...)` with `ModuleResult<T>.CreateFailure(timeoutException, executionContext)`

**Step 3: Build and fix any remaining issues**

Run: `dotnet build src/ModularPipelines/ModularPipelines.csproj -c Release`

**Step 4: Commit**

```bash
git add src/ModularPipelines/Engine/ModuleExecutionPipeline.cs
git commit -m "refactor(ModuleExecutionPipeline): use discriminated union factory methods"
```

---

## Task 6: Update Remaining Engine Files

**Files to update (fix compilation errors):**
- `src/ModularPipelines/Engine/ModuleExecutor.cs`
- `src/ModularPipelines/Engine/Execution/ModuleResultRegistrar.cs`
- `src/ModularPipelines/Engine/Executors/IgnoredModuleResultRegistrar.cs`
- `src/ModularPipelines/Engine/IModuleResultRegistry.cs`

**Step 1: Build and identify errors**

Run: `dotnet build src/ModularPipelines/ModularPipelines.csproj -c Release 2>&1 | head -50`

**Step 2: Fix each error by updating to new pattern**

Common fixes:
- Replace `.Value` access with `.ValueOrDefault` or pattern matching
- Replace `.Exception` access with `.ExceptionOrDefault` or pattern matching
- Update any code creating ModuleResult directly to use factory methods

**Step 3: Build until clean**

Run: `dotnet build src/ModularPipelines/ModularPipelines.csproj -c Release`
Expected: 0 errors

**Step 4: Commit**

```bash
git add -A
git commit -m "refactor(engine): update all engine files to use discriminated union pattern"
```

---

## Task 7: Delete Exception Classes (Optional - can deprecate instead)

**Files:**
- Modify: `src/ModularPipelines/Exceptions/ModuleFailedException.cs` (add Obsolete attribute)
- Modify: `src/ModularPipelines/Exceptions/ModuleSkippedException.cs` (add Obsolete attribute)

**Step 1: Mark as obsolete rather than delete (safer for consumers)**

Add to ModuleFailedException.cs:
```csharp
[Obsolete("Use pattern matching on ModuleResult<T>.Failure instead. This exception is no longer thrown.")]
```

Add to ModuleSkippedException.cs:
```csharp
[Obsolete("Use pattern matching on ModuleResult<T>.Skipped instead. This exception is no longer thrown.")]
```

**Step 2: Commit**

```bash
git add src/ModularPipelines/Exceptions/ModuleFailedException.cs
git add src/ModularPipelines/Exceptions/ModuleSkippedException.cs
git commit -m "deprecate: mark ModuleFailedException and ModuleSkippedException as obsolete"
```

---

## Task 8: Update Test Helpers

**Files:**
- Modify: `test/ModularPipelines.TestHelpers/Assertions/ModuleResultAssertions.cs` (if exists)

**Step 1: Find test helper files**

Run: `find test -name "*.cs" | xargs grep -l "ModuleResult" | head -10`

**Step 2: Update assertions to use new pattern**

Replace `.Value` accesses with `.ValueOrDefault` or pattern matching assertions.

**Step 3: Commit**

```bash
git add test/
git commit -m "test: update test helpers for discriminated union pattern"
```

---

## Task 9: Update Unit Tests

**Files:**
- `test/ModularPipelines.UnitTests/ReturnNothingTests.cs`
- Other test files using `.Value`

**Step 1: Find all test files using .Value on ModuleResult**

Run: `grep -rn "result\.Value" test/ --include="*.cs"`

**Step 2: Update each test to use new pattern**

Example change:
```csharp
// Before
await Assert.That(result.Value).IsNull();

// After
await Assert.That(result.IsSuccess).IsTrue();
await Assert.That(result.ValueOrDefault).IsNull();
```

**Step 3: Run tests**

Run: `dotnet run --project test/ModularPipelines.UnitTests/ModularPipelines.UnitTests.csproj -c Release`
Expected: All tests pass (486+)

**Step 4: Commit**

```bash
git add test/
git commit -m "test: update unit tests for discriminated union pattern"
```

---

## Task 10: Build Full Solution and Run All Tests

**Step 1: Build entire solution**

Run: `dotnet build ModularPipelines.sln -c Release`
Expected: 0 errors

**Step 2: Run all tests**

Run: `dotnet run --project test/ModularPipelines.UnitTests/ModularPipelines.UnitTests.csproj -c Release`
Expected: Same or better pass rate as baseline (486 passing)

**Step 3: Final commit**

```bash
git add -A
git commit -m "feat: complete discriminated union implementation for ModuleResult

BREAKING CHANGE: ModuleResult<T>.Value no longer throws exceptions.
Use pattern matching or IsSuccess/ValueOrDefault instead.

Closes #1869"
```

---

## Summary

| Task | Description | Estimated Steps |
|------|-------------|-----------------|
| 1 | Update IModuleResult interface | 3 |
| 2 | Rewrite ModuleResult as discriminated union | 3 |
| 3 | Delete obsolete subclasses | 2 |
| 4 | Update ModuleResultFactory | 3 |
| 5 | Update ModuleExecutionPipeline | 4 |
| 6 | Update remaining engine files | 4 |
| 7 | Deprecate exception classes | 2 |
| 8 | Update test helpers | 3 |
| 9 | Update unit tests | 4 |
| 10 | Final build and test | 3 |

**Total: ~31 steps**
