# Asynchronous file hashing and ZIP operations

Use the async file helpers for cancellable stream copying and hashing. Pass the
module's cancellation token to keep file work tied to the module lifetime.

```csharp
var source = context.Files.GetFolder("publish");
var archive = await context.Files.Zip.CreateFromDirectoryAsync(
    source,
    "artifacts/package.zip",
    CompressionLevel.Optimal,
    cancellationToken);

var hash = await context.Security.Hash.Sha256FileAsync(
    archive.Path,
    HashEncoding.Hex,
    cancellationToken);

var extracted = await context.Files.Zip.ExtractToDirectoryAsync(
    archive.Path,
    "extracted",
    overwriteFiles: false,
    cancellationToken: cancellationToken);
```

Import `ModularPipelines.Context` for `HashEncoding` and `System.IO.Compression`
for `CompressionLevel`. Hash and ZIP operations can also be used independently.

`Md5FileAsync`, `Sha1FileAsync`, `Sha256FileAsync`, `Sha384FileAsync`, and
`Sha512FileAsync` preserve the synchronous methods' hexadecimal and Base64 output.
The default encoding is `HashEncoding.Hex`. Existing synchronous methods remain available.

Relative string paths resolve against the pipeline working directory. Obtain the
source `FolderPath` through `context.Files.GetFolder` to use that same base;
an independently constructed `FolderPath` retains its own resolved path.
All operations use the configured file-system provider.

ZIP creation defaults to optimal compression and refuses to replace an existing
file. A directory destination receives an automatically named `.zip` file, matching
the synchronous API. Extraction overwrites existing files by default; pass
`overwriteFiles: false` to reject conflicts. Empty directories and the existing
archive traversal checks are preserved.

Cancellation propagates as `OperationCanceledException`. Creation or extraction
can leave a partial archive or partially extracted files; these operations are not
transactional. Archive finalization and cleanup still run after cancellation.

Content hashing and copying use asynchronous stream APIs without wrapping disk
work in `Task.Run`. File-system metadata operations and directory enumeration
remain synchronous. In .NET 10, ZIP central-directory enumeration and parts of
entry creation/finalization also perform synchronous I/O because the runtime does
not expose asynchronous alternatives for those steps. Custom providers determine
whether their streams implement asynchronous I/O directly.

## Migrating custom context implementations to V4

The new methods are required interface members. Applications that implement
`IHashContext` must add `Md5FileAsync`, `Sha1FileAsync`, `Sha256FileAsync`,
`Sha384FileAsync`, and `Sha512FileAsync`. Implementations of `IZipContext` must add
`CreateFromDirectoryAsync` and `ExtractToDirectoryAsync`. Rebuild custom
implementations against V4; preserving the synchronous methods does not preserve
source or binary compatibility for implementations of the older interfaces.

Use asynchronous stream reads and writes where supported, honor the supplied
`CancellationToken`, and preserve the synchronous implementation's encoding,
compression, overwrite, path-resolution, and provider behavior. Forwarding to a
synchronous method with `Task.FromResult` or `Task.Run` does not satisfy this
contract. The interfaces do not supply a synchronous fallback or a default method
that throws `NotSupportedException`.
