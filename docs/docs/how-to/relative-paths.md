---
title: Relative paths
---

# Relative paths

Use `context.Files.GetFile("input.txt")` and `context.Files.GetFolder("artifacts")`
for paths relative to your pipeline's working directory. These methods resolve the
path immediately and retain the pipeline's file-system provider.

The pipeline working directory comes from `PipelineBuilderSettings.WorkingDirectory`.
When omitted, the configured content root takes precedence, followed by the calling
source file's project directory, then the process working directory. Configuring a
pipeline does not change `Environment.CurrentDirectory`.

## Which directory is used?

| API | Base for a relative path |
| --- | --- |
| `context.Files.GetFile`, `GetFolder`, `ReadAsync`, `WriteAsync` | Pipeline working directory |
| Context hashing, ZIP string path parameters, and downloader save paths | Pipeline working directory |
| Command execution without a working-directory override | Pipeline working directory |
| `new FilePath("input.txt")`, `new FolderPath("artifacts")` | Process current directory at construction |
| Implicit string conversion to `FilePath` or `FolderPath` | Process current directory at conversion |
| String destinations passed to path objects' `CopyTo`, `MoveTo`, and async variants | Process current directory at the operation |

Fully qualified paths identify the same location regardless of the base directory.
Path objects can be constructed outside a pipeline and do not carry its working
directory. Even a file obtained from `context.Files.GetFile` does not make later
relative string destinations pipeline-relative.

Typed path parameters retain the location resolved when the path object was created.
For example, pass `context.Files.GetFolder("input")` as the source folder to
`context.Files.Zip.CreateFromDirectory`. Passing a relative string to that
`FolderPath` parameter performs the process-relative implicit conversion first;
only the ZIP method's string output path is resolved against the pipeline directory.

For example, if the process runs in `/runner` and the pipeline working directory is
`/repo`, `new FilePath("input.txt")` points to `/runner/input.txt`, while
`context.Files.GetFile("input.txt")` points to `/repo/input.txt`.

## Resolve destinations explicitly

Inside a module, resolve both source and destination through the files context:

```csharp
var source = context.Files.GetFile("input.txt");
var output = context.Files.GetFolder("artifacts");
output.Create();

var destination = context.Files.GetFile("artifacts/copy.txt");
var copied = await source.CopyToAsync(destination.Path, cancellationToken);
```

For a copy that keeps the file name, pass the resolved folder directly:

```csharp
var copied = await source.CopyToAsync(output, cancellationToken);
```

Use the returned absolute `Path` when passing destinations to folder copy/move
methods too. Avoid changing the process current directory to make relative paths
match: it is shared by every concurrently running pipeline and module.

## V4 migration

Audit direct path construction, implicit string conversions, and copy/move
destinations when setting `WorkingDirectory` or `ContentRootPath`. Replace
pipeline-relative strings with `context.Files.GetFile`/`GetFolder` and resolved
absolute destinations. This guidance documents the existing behavior; V4 does not
silently rebase standalone path objects.
