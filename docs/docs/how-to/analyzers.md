---
title: Analyzers
---

# Analyzers

Modular Pipelines includes Roslyn analyzers that catch pipeline authoring mistakes at compile time. Each rule uses the `MP####` ID family, so it can be configured with standard .NET analyzer settings.

See the [complete analyzer rule reference](../analyzers/) for every rule, its category, default severity, and suppression examples.

## Configure a rule

Set a rule's severity in `.editorconfig`:

```ini
[*.cs]
dotnet_diagnostic.MP0001.severity = warning
```

Valid values are `default`, `none`, `silent`, `suggestion`, `warning`, and `error`. Use `none` to disable a rule.

## Suppress a specific occurrence

Use `#pragma` when a violation is intentional and narrowly scoped:

```csharp
#pragma warning disable MP0001
// Code that intentionally violates MP0001.
#pragma warning restore MP0001
```

## ID migration

The legacy prose IDs were renamed so `.editorconfig` settings can use one predictable family:

| Legacy ID | Current ID |
|-----------|------------|
| `MissingDependsOnAttribute` | `MP0001` |
| `EnumerableModuleResult` | `MP0002` |
| `LoggerInConstructor` | `MP0003` |
| `ConsoleUse` | `MP0004` |
| `ConflictingDependsOnAttribute` | `MP0005` |
| `AsyncModule` | `MP0006` |
| `AwaitThis` | `MP0007` |
| `StatefulModule` | `MP0008` |
| `MPDEP001` | `MP0009` |
| `MPDEP002` | `MP0005` |
| `MPDEP003` | `MP0010` |
| `MPREG001` | `MP0013` |
| `MPASYNC001` | `MP0014` |
| `MPASYNC002` | `MP0015` |
| `MPASYNC003` | `MP0016` |
| `MPASYNC004` | `MP0017` |
| `MPTYPE001` | `MP0018` |
| `MPDEP004` | `MP0019` |

Update existing `dotnet_diagnostic.<id>.severity` entries and warning suppressions to the current IDs.
