---
title: Migrating to v2 Context API
---

# Migrating to v2 Context API

ModularPipelines v2 introduces a role-based context architecture that organizes functionality into domain-specific interfaces. This provides better discoverability, cleaner separation of concerns, and improved IntelliSense support.

## What's Changed

### Role-Based Domain Contexts

**Before (v1):**
```csharp
// Flat hierarchy with many properties directly on context
context.Command.ExecuteCommandLineTool(options);
context.Bash.Command("echo hello");
context.FileSystem.GetFile("/path/to/file");
context.Environment.EnvironmentVariables["PATH"];
context.Hasher.Hash(data, HashAlgorithmName.SHA256);
```

**After (v2):**
```csharp
// Domain-organized hierarchy
context.Shell.Command.ExecuteCommandLineTool(options);
context.Shell.Bash.Command("echo hello");
context.Files.GetFile("/path/to/file");
context.Environment.Variables["PATH"];
context.Security.Hasher.Hash(data, HashAlgorithmName.SHA256);
```

### New Interface Hierarchy

The v2 context API introduces domain-specific interfaces accessible through properties:

| v1 Access Pattern | v2 Access Pattern | Domain Interface |
|---|---|---|
| `context.Command` | `context.Shell.Command` | `IShellContext` |
| `context.Bash` | `context.Shell.Bash` | `IShellContext` |
| `context.PowerShell` | `context.Shell.PowerShell` | `IShellContext` |
| `context.FileSystem` | `context.Files` | `IFilesContext` |
| `context.Zip` | `context.Files.Zip` | `IFilesContext` |
| `context.Base64` | `context.Data.Base64` | `IDataContext` |
| `context.Hex` | `context.Data.Hex` | `IDataContext` |
| `context.Json` | `context.Data.Json` | `IDataContext` |
| `context.Xml` | `context.Data.Xml` | `IDataContext` |
| `context.Yaml` | `context.Data.Yaml` | `IDataContext` |
| `context.Environment.EnvironmentVariables` | `context.Environment.Variables` | `IEnvironmentDomainContext` |
| `context.Hasher` | `context.Security.Hasher` | `ISecurityContext` |
| `context.Encryptor` | `context.Security.Encryptor` | `ISecurityContext` |
| `context.NuGet` | `context.Installers.NuGet` | `IInstallersContext` |
| `context.Downloader` | `context.Network.Downloader` | `INetworkContext` |
| `context.Http` | `context.Network.Http` | `INetworkContext` |

### Removed Interfaces

The following facade interfaces have been removed in v2:

- `IPipelineServices` - Use `context.Services` directly
- `IPipelineLogging` - Use `context.Logger` directly
- `IPipelineTools` - Use domain contexts
- `IPipelineEncoding` - Use `context.Data`
- `IPipelineFileSystem` - Use `context.Files`
- `IPipelineEnvironment` - Use `context.Environment`

## Migration Steps

### 1. Update Property Access Paths

Use find-and-replace to update access patterns:

```csharp
// Shell operations
context.Command → context.Shell.Command
context.Bash → context.Shell.Bash
context.PowerShell → context.Shell.PowerShell

// File operations
context.FileSystem → context.Files
context.FileSystem.GetFolder(SpecialFolder) → context.Files.GetFolder(SpecialFolder)
context.Zip → context.Files.Zip

// Data operations
context.Base64 → context.Data.Base64
context.Hex → context.Data.Hex
context.Json → context.Data.Json
context.Xml → context.Data.Xml
context.Yaml → context.Data.Yaml

// Security operations
context.Hasher → context.Security.Hasher
context.Encryptor → context.Security.Encryptor

// Environment operations
context.Environment.EnvironmentVariables → context.Environment.Variables

// Installer operations
context.NuGet → context.Installers.NuGet

// Network operations
context.Downloader → context.Network.Downloader
context.Http → context.Network.Http
```

### 2. Special Folder Access

The `GetFolder(Environment.SpecialFolder)` overload is available on the `Files` domain:

```csharp
// Before (v1)
var appData = context.FileSystem.GetFolder(Environment.SpecialFolder.ApplicationData);

// After (v2)
var appData = context.Files.GetFolder(Environment.SpecialFolder.ApplicationData);
```

### 3. Update Type References

If you referenced the removed facade interfaces directly, update to the new types:

```csharp
// Before
IPipelineServices services = context.ServiceProvider;
IPipelineFileSystem fs = context.FileSystem;

// After
IServicesContext services = context.Services;
IFilesContext files = context.Files;
```

## For Extension Authors

If you maintain a ModularPipelines extension package, additional changes are required:

### Update Extension Method Signatures

**Before (v1):**
```csharp
using Microsoft.Extensions.DependencyInjection;

public static class MyToolExtensions
{
    public static IMyTool MyTool(this IPipelineHookContext context)
    {
        return context.ServiceProvider.GetRequiredService<IMyTool>();
    }
}
```

**After (v2):**
```csharp
public static class MyToolExtensions
{
    public static IMyTool MyTool(this IPipelineContext context)
    {
        return context.Services.Get<IMyTool>();
    }
}
```

Key changes:
- `IPipelineHookContext` → `IPipelineContext`
- `context.ServiceProvider.GetRequiredService<T>()` → `context.Services.Get<T>()`
- Remove `using Microsoft.Extensions.DependencyInjection;` if only used for `GetRequiredService`

## Benefits

- **Discoverability**: IntelliSense guides you through domain-specific functionality
- **Separation of Concerns**: Each domain context has a single responsibility
- **Reduced Surface Area**: Cleaner API with fewer top-level properties
- **Extension-Friendly**: `IPipelineContext` provides stable extension point
- **Type Safety**: Domain interfaces provide strong typing for each capability area

## Backward Compatibility

This is a breaking change. The v1 facade interfaces (`IPipelineServices`, `IPipelineTools`, etc.) have been removed. Code using these interfaces must be updated to use the new domain contexts.
