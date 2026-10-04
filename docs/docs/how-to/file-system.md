---
title: Files and folders
---

# Files and folders

Use `context.Files.GetFile(path)` and `context.Files.GetFolder(path)` for paths
relative to the pipeline working directory. Direct `FilePath` and `FolderPath`
construction resolves relative paths against the process working directory.

## Existence and timestamps

`context.Files.Exists(path)` synchronously checks for either a file or a directory.
It returns false for missing or blank paths. Use `context.Files.GetFile(path).Exists`
or `context.Files.GetFolder(path).Exists` when only one kind should count.

Both path types expose `CreationTime` and `LastWriteTime` as `DateTimeOffset` values
normalized to UTC. Call `ToLocalTime()` explicitly when displaying local time.
`FilePath.Extension` identifies a file extension; folders have no extension property.

## Temporary resources

Prefer `TempFile` and `TempFolder` with `using` or `await using` so cleanup follows
the lifetime of the operation:

```csharp
using ModularPipelines.FileSystem;

await using var temporaryFile = new TempFile();
await temporaryFile.File.CreateAsync(cancellationToken);
await temporaryFile.File.WriteAsync("temporary contents", cancellationToken);

await using var temporaryFolder = new TempFolder();
// temporaryFolder.Folder already exists.
// Disposal deletes the file and recursively deletes the folder's contents.
```

| Helper | Creation | Cleanup |
| --- | --- | --- |
| `new TempFile()` | Generates a path; create or write `File` when needed | Deletes the file on disposal if it exists |
| `new TempFolder()` | Creates a folder immediately | Recursively deletes the folder on disposal |
| `FilePath.GetNewTemporaryFilePath()` | Generates a path without creating or reserving a file | Caller owns cleanup |
| `FolderPath.CreateTemporaryFolder()` | Creates a folder immediately | Caller owns cleanup |

Wrap an existing resource with `new TempFile(file)` or `new TempFolder(folder)`
when it should be deleted at the end of a scope. For a custom file-system provider,
wrap `FolderPath.CreateTemporaryFolder(provider)` in `TempFolder`.

`FilePath.CreateAsync(cancellationToken)` checks cancellation before creating or
truncating the file. It returns the same `FilePath` instance for chaining.
