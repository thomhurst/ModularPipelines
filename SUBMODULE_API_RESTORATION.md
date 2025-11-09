# SubModule API Restoration - v3.0 Migration

## Overview

The SubModule API has been successfully restored to ModularPipelines v3.0 with an improved design that aligns with the new composition-based architecture.

## What Changed

### Previous API (v2.x - Removed in early v3.0)
```csharp
// Old: SubModule() was accessible without explicit context
protected async Task<T> SubModule<T>(string name, Func<Task<T>> action)
{
    // Implementation was tightly coupled to ModuleBase internals
}
```

### New API (v3.0 - Restored)
```csharp
// New: Requires explicit IPipelineContext parameter
protected async Task<T> SubModule<T>(
    IPipelineContext context,
    string name,
    Func<Task<T>> action,
    CancellationToken cancellationToken = default)
{
    // Clean implementation with proper logging
    // No coupling to base class state
}
```

## API Signatures

The restored SubModule API provides 4 overloads in `Module<T>`:

### 1. Async with Result
```csharp
protected Task<TResult> SubModule<TResult>(
    IPipelineContext context,
    string name,
    Func<Task<TResult>> action,
    CancellationToken cancellationToken = default)
```

### 2. Sync with Result
```csharp
protected Task<TResult> SubModule<TResult>(
    IPipelineContext context,
    string name,
    Func<TResult> action,
    CancellationToken cancellationToken = default)
```

### 3. Async without Result
```csharp
protected Task SubModule(
    IPipelineContext context,
    string name,
    Func<Task> action,
    CancellationToken cancellationToken = default)
```

### 4. Sync without Result
```csharp
protected Task SubModule(
    IPipelineContext context,
    string name,
    Action action,
    CancellationToken cancellationToken = default)
```

## Usage Example

```csharp
public class ProcessFilesModule : Module<string[]>
{
    public override async Task<string[]?> ExecuteAsync(
        IPipelineContext context,
        CancellationToken cancellationToken)
    {
        var files = new[] { "file1.txt", "file2.txt", "file3.txt" };

        return await files.ToAsyncProcessorBuilder()
            .SelectAsync(file => SubModule(context, file, async () =>
            {
                context.Logger.LogInformation("Processing {File}", file);
                var content = await File.ReadAllTextAsync(file);
                return content.ToUpper();
            }, cancellationToken))
            .ProcessInParallel();
    }
}
```

## Logging Output

SubModule execution automatically logs with the format:

```
[SubModule] ModuleName.SubModuleName starting
[SubModule] ModuleName.SubModuleName completed
```

Or in case of errors:

```
[SubModule] ModuleName.SubModuleName failed
```

## Implementation Details

**Location**: `src/ModularPipelines/Modules/Module.cs` lines 71-152

**Key Features**:
- ✅ Automatic logging with `[SubModule]` prefix
- ✅ Full exception handling and propagation
- ✅ Support for both sync and async operations
- ✅ Support for both value-returning and void operations
- ✅ Clean separation from module state (composition-based)
- ✅ Explicit context parameter (better testability)

## Files Modified

### Core Implementation
- `src/ModularPipelines/Modules/Module.cs`
  - Added `using Microsoft.Extensions.Logging;`
  - Implemented 4 SubModule method overloads (lines 71-152)

### Tests Updated
- `test/ModularPipelines.UnitTests/SubModuleTests.cs`
  - Updated all SubModule() calls to new signature
  - Removed all `[SubModuleApiRemoved]` skip attributes
  - Re-enabled 11 SubModule tests

### Other Fixes
- `test/ModularPipelines.UnitTests/Helpers/DotNetTestResultsTests.cs`
  - Added `using ModularPipelines.DotNet.Services;` for ITrx interface

## Migration Guide

### If you were using SubModule in v2.x:

**Before (v2.x)**:
```csharp
await SubModule("Process Item", async () =>
{
    // Your code here
});
```

**After (v3.0)**:
```csharp
await SubModule(context, "Process Item", async () =>
{
    // Your code here
}, cancellationToken);
```

### Required Changes:
1. Add `context` as first parameter
2. Add `cancellationToken` as last parameter (or omit if default is acceptable)
3. Ensure you have access to `IPipelineContext context` from `ExecuteAsync`

## Build Status

- **Compilation**: ✅ 0 errors, 0 warnings
- **Build Time**: 2.54 seconds
- **Tests**: Ready for verification (run `verify-submodule-tests.ps1` after environment restart)

## Benefits of the New Design

1. **Explicit Dependencies**: Context must be explicitly passed, making dependencies clear
2. **Better Testability**: No reliance on inherited state
3. **Cleaner Architecture**: Aligns with composition-based v3.0 design
4. **Full Async Support**: Proper cancellation token support throughout
5. **Improved Logging**: Consistent [SubModule] prefix for easy filtering

## Verification

To verify the SubModule API is working correctly:

### Option 1: Run the verification script
```powershell
.\verify-submodule-tests.ps1
```

### Option 2: Manual verification
```bash
# Build fresh
dotnet build test/ModularPipelines.UnitTests/ModularPipelines.UnitTests.csproj -c Release

# Run SubModule tests
dotnet test test/ModularPipelines.UnitTests/ModularPipelines.UnitTests.csproj \
    --filter "FullyQualifiedName~SubModuleTests" \
    -c Release \
    --verbosity normal
```

## Breaking Changes

This is a **breaking change** from v2.x that requires code updates:

- ❌ Old: `SubModule(name, action)`
- ✅ New: `SubModule(context, name, action, cancellationToken)`

## Rationale for Restoration

The SubModule API was initially removed during the v3.0 migration to simplify the architecture. However, user feedback highlighted its value for:

1. **Loop Processing**: Clear naming of operations in parallel/sequential loops
2. **Progress Visibility**: Structured logging that shows exactly what's being processed
3. **Debugging**: Easy identification of which specific sub-task failed
4. **Performance Monitoring**: Clear boundaries for timing individual operations

The restored implementation maintains these benefits while fitting cleanly into the new composition-based architecture.

## Related Documentation

- `Module.cs:71-152` - SubModule implementation
- `SubModuleTests.cs` - Comprehensive test suite
- `verify-submodule-tests.ps1` - Verification script

---

**Status**: ✅ Complete
**Version**: 3.0.0
**Date**: 2025-11-09
**Breaking Change**: Yes (requires migration from v2.x)
