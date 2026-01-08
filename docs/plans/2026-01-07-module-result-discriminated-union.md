# ModuleResult Discriminated Union Design

**Issue:** #1869
**Date:** 2026-01-07
**Status:** Approved

## Problem

The current `ModuleResult<T>.Value` property throws exceptions when accessed on failed or skipped results:

```csharp
var result = await myModule;
var value = result.Value; // Throws ModuleFailedException or ModuleSkippedException!
```

This leads to:
- Implicit exceptions and poor control flow
- No compile-time exhaustiveness checking
- Awkward try-catch patterns around property access

## Solution

Replace with a discriminated union pattern using C# records:

```csharp
public abstract record ModuleResult<T> : IModuleResult
{
    // Metadata (available on all outcomes)
    public required string ModuleName { get; init; }
    public required TimeSpan ModuleDuration { get; init; }
    public required DateTimeOffset ModuleStart { get; init; }
    public required DateTimeOffset ModuleEnd { get; init; }
    public required Status ModuleStatus { get; init; }

    // Quick checks
    public bool IsSuccess => this is Success;
    public bool IsFailure => this is Failure;
    public bool IsSkipped => this is Skipped;

    // Safe accessors (no exceptions)
    public T? ValueOrDefault => this is Success s ? s.Value : default;
    public Exception? ExceptionOrDefault => this is Failure f ? f.Exception : null;
    public SkipDecision? SkipDecisionOrDefault => this is Skipped sk ? sk.Decision : null;

    // Computed for compatibility
    public ModuleResultType ModuleResultType => this switch
    {
        Success => ModuleResultType.Success,
        Failure => ModuleResultType.Failure,
        Skipped => ModuleResultType.Skipped,
        _ => throw new InvalidOperationException()
    };

    // Discriminated variants
    public sealed record Success(T Value) : ModuleResult<T>;
    public sealed record Failure(Exception Exception) : ModuleResult<T>;
    public sealed record Skipped(SkipDecision Decision) : ModuleResult<T>;

    // Prevent external inheritance
    private ModuleResult() { }
}
```

## Usage Examples

### Pattern Matching (Recommended)

```csharp
var result = await buildModule;

switch (result)
{
    case ModuleResult<string>.Success { Value: var value }:
        Console.WriteLine($"Got: {value}");
        break;
    case ModuleResult<string>.Failure { Exception: var ex }:
        Console.WriteLine($"Failed: {ex.Message}");
        break;
    case ModuleResult<string>.Skipped { Decision.Reason: var reason }:
        Console.WriteLine($"Skipped: {reason}");
        break;
}
```

### Match Helper

```csharp
string message = result.Match(
    onSuccess: value => $"Got: {value}",
    onFailure: ex => $"Error: {ex.Message}",
    onSkipped: skip => $"Skipped: {skip.Reason}"
);
```

### Quick Checks

```csharp
if (result.IsSuccess)
{
    DoSomething(result.ValueOrDefault!);
}
```

## Helper Methods

```csharp
// Match - transform to single type
public TResult Match<TResult>(
    Func<T, TResult> onSuccess,
    Func<Exception, TResult> onFailure,
    Func<SkipDecision, TResult> onSkipped);

// Switch - side effects
public void Switch(
    Action<T> onSuccess,
    Action<Exception> onFailure,
    Action<SkipDecision> onSkipped);
```

## JSON Serialization

Uses .NET 7+ polymorphic serialization:

```csharp
[JsonDerivedType(typeof(Success), "Success")]
[JsonDerivedType(typeof(Failure), "Failure")]
[JsonDerivedType(typeof(Skipped), "Skipped")]
public abstract record ModuleResult<T> : IModuleResult
```

Output:
```json
{
  "$type": "Success",
  "Value": "build output",
  "ModuleName": "BuildModule",
  "ModuleDuration": "00:01:23"
}
```

## Non-Generic Interface

For type-erased scenarios (registries, summaries):

```csharp
public interface IModuleResult
{
    string ModuleName { get; }
    TimeSpan ModuleDuration { get; }
    DateTimeOffset ModuleStart { get; }
    DateTimeOffset ModuleEnd { get; }
    Status ModuleStatus { get; }
    ModuleResultType ModuleResultType { get; }

    object? ValueOrDefault { get; }
    Exception? ExceptionOrDefault { get; }
    SkipDecision? SkipDecisionOrDefault { get; }
}
```

## Factory Methods

Internal factory methods for engine use:

```csharp
internal static Success CreateSuccess(T value, ModuleExecutionContext ctx);
internal static Failure CreateFailure(Exception exception, ModuleExecutionContext ctx);
internal static Skipped CreateSkipped(SkipDecision decision, ModuleExecutionContext ctx);
```

## Breaking Changes

### Removed
- `ModuleResult<T>.Value` property (the throwing one)
- `ModuleFailedException` class
- `ModuleSkippedException` class
- `SkippedModuleResult<T>` subclass
- `TimedOutModuleResult<T>` subclass

### Preserved
- `ModuleResultType` property (now computed)
- `IModuleResult` interface
- All metadata property names
- JSON serialization support

## Target

.NET 10 only - no backward compatibility shims needed.

## Files to Modify

1. `src/ModularPipelines/Models/ModuleResult.cs` - Main implementation
2. `src/ModularPipelines/Models/IModuleResult.cs` - Update interface
3. `src/ModularPipelines/Engine/Execution/ModuleResultFactory.cs` - Update factory
4. `src/ModularPipelines/Exceptions/` - Remove ModuleFailedException, ModuleSkippedException
5. `src/ModularPipelines/Models/SkippedModuleResult.cs` - Delete
6. `src/ModularPipelines/Models/TimedOutModuleResult.cs` - Delete
7. All files accessing `.Value` - Update to pattern matching
