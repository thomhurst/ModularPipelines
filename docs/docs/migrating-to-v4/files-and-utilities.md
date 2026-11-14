---
title: Files and utilities
sidebar_position: 12
---

# Files, working directories, and utilities

## `File` and `Folder` are renamed

`ModularPipelines.FileSystem.File` and `Folder` become `FilePath` and `FolderPath`. Their usual
read, write, and copy operations are still there.

- Update aliases and signatures, including custom result DTOs.
- Do not rename `System.IO.File` or `System.IO.Directory` calls.

| Previous member | V4 replacement |
| --- | --- |
| `LastWriteTimeUtc` on file and folder paths | `FilePath.LastWriteTime` / `FolderPath.LastWriteTime` (`DateTimeOffset`, normalized to UTC) |
| `FolderPath.Extension` (formerly `Folder.Extension`) | Removed; use `System.IO.Path.GetExtension(folder.Path)` if you need the directory name's suffix |

`FilePath.Extension` is still available for files. `CreationTime` on both path types is also
normalized to UTC. Call `.ToLocalTime()` when you need local time.

### `FilePath.GetStream`

`GetStream` no longer uses `FileMode.OpenOrCreate` for every access mode:

| Access | V3 | V4 |
| --- | --- | --- |
| `FileAccess.Read` | Created a missing empty file | `FileMode.Open`; throws `FileNotFoundException` for a missing file |
| `FileAccess.Write` | Opened without truncating | `FileMode.Create`; truncates an existing file |
| `FileAccess.ReadWrite` | `OpenOrCreate` | `OpenOrCreate` (unchanged) |

If you need a particular creation or truncation mode, open a stream with an explicit `FileMode`.

## Working directories

`context.Environment.WorkingDirectory` is now a read-only `FolderPath`.

| To set the directory for... | Use |
| --- | --- |
| The whole pipeline | `PipelineBuilderSettings.WorkingDirectory` |
| One command | `CommandExecutionOptions.WorkingDirectory` |

V4's default directory can come from the content root or the calling source project's directory.
Set it explicitly if your V3 pipeline depended on the directory the process was launched from.

### Which paths are pipeline-relative

Commands and context-relative file operations use the pipeline's directory. They do not change the
process-wide current directory.

| Resolves against the pipeline working directory | Resolves against the process current directory |
| --- | --- |
| Commands | `new FilePath(relativePath)`, `new FolderPath(relativePath)` |
| `context.Files.GetFile`, `context.Files.GetFolder` | Implicit string conversions to a path type |
| `context.Files.Exists` | `System.IO` operations |
| | A relative string destination passed to copy or move |

So use `context.Files.GetFile` and `context.Files.GetFolder` for pipeline-relative paths, and
resolve copy and move destinations the same way. A path object from the files context does not make
a later relative string destination pipeline-relative.

```csharp
var source = context.Files.GetFile("input.txt");
var output = context.Files.GetFolder("artifacts");
output.Create();
var destination = context.Files.GetFile("artifacts/copy.txt");
await source.CopyToAsync(destination.Path, cancellationToken);
```

- The same rule applies to synchronous methods and to folder copy and move operations.
- Absolute destinations keep their location.
- Standalone path types keep their existing resolution behavior. Do not change the shared process
  directory to make concurrent pipelines agree.

### `Files.ExistsAsync` becomes `Files.Exists`

```csharp
// V3
await context.Files.ExistsAsync(path, cancellationToken)

// V4
context.Files.Exists(path)
```

- The check is synchronous and takes only the path. Remove the `await` and the token.
- It returns `true` for either a file or a directory.
- Relative paths now resolve against the pipeline working directory.
- When the kind matters, use `context.Files.GetFile(path).Exists` or
  `context.Files.GetFolder(path).Exists`.

## Utility replacements

| V3 API | V4 API |
| --- | --- |
| `Environment.Variables.GetEnvironmentVariable(...)` | `Environment.Variables.Get(...)` |
| `Environment.Variables.GetEnvironmentVariables(...)` (`IDictionary<string, string>`) | `Environment.Variables.GetAll(...)` (`IReadOnlyDictionary<string, string?>`; copy it before modifying) |
| `Environment.Variables.SetEnvironmentVariable(...)` | `Environment.Variables.Set(...)` |
| `await context.Files.ExistsAsync(path, cancellationToken)` | `context.Files.Exists(path)` |
| `context.Files.Checksum.Md5(path)` | `context.Security.Hash.Md5File(path)` |
| `context.Security.Hasher.Sha256(text)` | `context.Security.Hash.Sha256(text)` |
| `HashType` | `HashEncoding` |
| `context.Data.Base64.FromBase64String(value, encoding)` | `encoding.GetString(context.Data.Base64.FromBase64String(value))` |
| `context.Data.Hex.FromHex(value)` (`string`, UTF-8) | `context.Data.Hex.FromHex(value, Encoding.UTF8)` (`string`); the one-argument overload now returns `byte[]` |
| `context.Data.Hex.ToHex(bytes)` with `IEnumerable<byte>` | `context.Data.Hex.ToHex(bytes.ToArray())` |
| `context.Files.Zip.ZipFolder(...)` | `context.Files.Zip.CreateFromDirectory(...)` |
| `context.Files.Zip.UnZipToFolder(...)` | `context.Files.Zip.ExtractToDirectory(...)` |
| `value.AsTask()` | `Task.FromResult(value)` |
| `AsSkipDecisionIfTrue(reason)` | `SkipDecision.When(condition, reason)` |
| The framework's `FirstOrDefaultAsync` convenience extension | The .NET async-LINQ `FirstOrDefaultAsync` |
| `OperatingSystemHelper` / `OperatingSystemIdentifier` | `context.Environment.OperatingSystem` (`OSPlatform`), .NET's `OperatingSystem.IsWindows()` and equivalents, or the built-in module run conditions |

### Environment variables

- `AddToPath` no longer adds duplicate or empty entries. Its `User` and `Machine` targets throw
  `PlatformNotSupportedException` outside Windows.
- `GetPath` omits empty entries.

### Hashing

- Use the `*File` methods for file contents. A text hash of the path is a different result.
- V3's `Checksum.Md5` returned uppercase hex. `Md5File` returns lowercase hex, so compare hashes
  case-insensitively.
- Mocks of `IHasherContext` / `IChecksumContext` move to `IHashContext`.

### Base64

Decoding now returns bytes. Choose the text encoding yourself when you need text.

### Hex

:::warning `FromHex(value)` changed return type
V3's one-argument `FromHex` decoded UTF-8 text and returned `string`. V4's returns `byte[]`. This
affects `var` declarations as well as explicit string assignments.
:::

```csharp
// V3
string decoded = context.Data.Hex.FromHex(value);
IEnumerable<byte> bytes = new byte[] { 0x48, 0x69 };
string encoded = context.Data.Hex.ToHex(bytes);
```

```csharp
// V4 (using System.Text and System.Linq)
string decoded = context.Data.Hex.FromHex(value, Encoding.UTF8);
IEnumerable<byte> bytes = new byte[] { 0x48, 0x69 };
string encoded = context.Data.Hex.ToHex(bytes.ToArray());

// Use the one-argument overload when you want the decoded bytes.
byte[] raw = context.Data.Hex.FromHex(value);
```

- **Decoding to text.** Pass `Encoding.UTF8` to keep V3's result, or the encoding that matches the
  original text. `FromHex(value, encoding)` still returns a string.
- **Encoding bytes.** The byte overload of `ToHex` accepts only `byte[]`. Call `ToArray()` on other
  byte sequences. Existing `byte[]` arguments and the string overloads need no change. Output is
  still lowercase.
- **Stricter input.** V4 throws `FormatException` for hyphen-separated or odd-length input. V3
  removed hyphens and silently ignored a trailing unmatched hex digit. Normalize separators on
  purpose and validate complete byte pairs before you decode legacy input.

### The old standalone `IEnvironmentContext`

If you implemented V3's standalone `IEnvironmentContext`:

- Replace `OperatingSystemVersion` / `Is64BitOperatingSystem` with .NET's
  `Environment.OSVersion.Version` / `Environment.Is64BitOperatingSystem`.
- Use `context.Files.GetFolder(specialFolder)` instead of its removed `GetFolder` method.

## Installers

The nested `Installers.File`, `.Windows`, `.Linux`, `.Mac`, and `.Predefined` surfaces are removed.

| V3 | V4 |
| --- | --- |
| `Installers.File.InstallFromFileAsync(...)` | `context.Installers.InstallAsync(new InstallerOptions(path) { Arguments = [...] }, token)` |
| `Installers.File.InstallFromWebAsync(...)` | `context.Installers.InstallFromWebAsync(WebInstallerOptions, token)` |

Move package-manager-specific work to the matching integration or an explicit shell command. There
is no universal replacement for every old predefined installer. Keep its download, platform, and
unattended-install behavior deliberately.
