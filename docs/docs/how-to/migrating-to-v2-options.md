---
title: Migrating to v2 CommandLineToolOptions
---

# Migrating to v2 CommandLineToolOptions

ModularPipelines v2 introduces a redesigned `CommandLineToolOptions` architecture that provides better type safety, compile-time validation, and IntelliSense support.

## What's Changed

### Tool Definition via Attributes

**Before (v1):**
```csharp
public abstract record GitOptions() : CommandLineToolOptions("git");
```

**After (v2):**
```csharp
[CliTool("git")]
public abstract partial record GitOptions : CommandLineToolOptions;
```

The tool name is now declared via the `[CliTool]` attribute on the base class, not a constructor parameter.

### Subcommand Definition

**Before (v1):**
```csharp
// Inheritance implied the subcommand, but it wasn't explicit
public record GitAddOptions() : GitOptions;
```

**After (v2):**
```csharp
[CliSubCommand("add")]
public partial record GitAddOptions : GitOptions;
```

Nested subcommands use multiple strings:
```csharp
[CliSubCommand("remote", "set-url")]
public partial record GitRemoteSetUrlOptions : GitOptions;
```

### Typed Positional Arguments

**Before (v1):**
```csharp
// Dynamic values required the Arguments escape hatch
new GitRevParseOptions { Arguments = ["HEAD"] }
new GitDiffOptions { Arguments = ["origin/main"] }
```

**After (v2):**
```csharp
// Typed properties with [CliArgument] attribute
new GitRevParseOptions { Committish = "HEAD" }
new GitDiffOptions { BaseRef = "origin/main" }
```

### Per-Subcommand Options Classes

**Before (v1):**
```csharp
// Same generic class used for different subcommands
new GitRemoteOptions { Arguments = ["set-url", "origin", url] }
new GitRemoteOptions { Arguments = ["get-url", "origin"] }
new GitRemoteOptions { Arguments = ["show", "origin"] }
```

**After (v2):**
```csharp
// Separate typed classes for each subcommand
new GitRemoteSetUrlOptions { Remote = "origin", NewUrl = url }
new GitRemoteGetUrlOptions { Remote = "origin" }
new GitRemoteShowOptions { Remote = "origin" }
```

## Migration Steps

1. **Add `partial` keyword** to any custom options classes
2. **Remove constructor parameters** - tool name comes from `[CliTool]` attribute
3. **Replace `Arguments` usage** with typed positional argument properties
4. **Use granular options classes** for subcommands

## Escape Hatch

For tools without typed options, use `GenericCommandLineToolOptions`:

```csharp
await command.ExecuteCommandLineTool(new GenericCommandLineToolOptions("dotnet-gitversion")
{
    Arguments = ["/output", "json"]
});
```

## Source Generator Validation

The new source generator validates your options classes at compile time:

- **MP0001**: Options class must be marked `partial`
- **MP0002**: Options class missing `[CliTool]` in inheritance chain
- **MP0003**: Duplicate `[CliArgument]` positions
- **MP0004**: Property has no CLI attribute (warning)

## Migration Analyzer

The analyzer **MP0010** will warn when you use the deprecated `Arguments` property on typed options classes, helping you identify code that needs migration.

## Benefits

- **Type Safety**: Compile-time validation catches errors before runtime
- **IntelliSense**: Full IDE support for discovering available options
- **No Reflection**: Generated `Build()` methods eliminate runtime reflection
- **Clear Intent**: Per-subcommand classes make code self-documenting
