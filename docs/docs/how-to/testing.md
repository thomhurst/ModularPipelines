---
title: Testing
sidebar_position: 9
---

## Testing with Mocked File System

ModularPipelines supports mocking file system operations for unit testing. All file I/O goes through `IFileSystemProvider`, which can be replaced with a mock implementation.

### Why Mock the File System?

- **Speed**: Tests run faster without actual disk I/O
- **Isolation**: Tests don't depend on file system state
- **Predictability**: No flaky tests due to file permissions or disk space
- **CI-friendly**: Works in any environment without file system setup

### Example: Mocking File Reads

```csharp
using Moq;
using ModularPipelines;
using ModularPipelines.FileSystem;
using ModularPipelines.Extensions;

[Test]
public async Task MyModule_ReadsConfigFile()
{
    // Create a mock provider
    var mockProvider = new Mock<IFileSystemProvider>();
    mockProvider.Setup(p => p.ReadAllTextAsync(
            It.IsAny<string>(),
            It.IsAny<CancellationToken>()))
        .ReturnsAsync("{\"setting\": \"value\"}");

    // Run pipeline with mock
    var builder = Pipeline.CreateBuilder(args);

    builder.Services.AddSingleton<IFileSystemProvider>(mockProvider.Object);
    builder.Services.AddModule<MyModule>();

    var result = await builder.Build().RunAsync();

    // Assert results
    Assert.That(result.Status, Is.EqualTo(PipelineStatus.Success));
}
```

### Example: Verifying File Writes

```csharp
[Test]
public async Task MyModule_WritesOutputFile()
{
    var mockProvider = new Mock<IFileSystemProvider>();

    var builder = Pipeline.CreateBuilder(args);

    builder.Services.AddSingleton<IFileSystemProvider>(mockProvider.Object);
    builder.Services.AddModule<OutputModule>();

    await builder.Build().RunAsync();

    // Verify the write occurred with expected content
    mockProvider.Verify(p => p.WriteAllTextAsync(
        It.Is<string>(path => path.Contains("output")),
        It.Is<string>(content => content.Contains("result")),
        It.IsAny<CancellationToken>()));
}
```

### Important Notes

- **Always use `context.Files`**: Files created via `context.Files.GetFile()` will use the injected provider. Files created directly via `new File("path")` use the real file system.

- **Provider Registration**: The mock provider must be registered before the pipeline runs. Using `services.AddSingleton<IFileSystemProvider>()` overrides the default `SystemFileSystemProvider`.

- **Mock ALL methods your code uses**: The mock provider only intercepts methods you explicitly set up. If your module calls `ReadAllTextAsync`, `FileExists`, and `Combine`, you must mock all three. Unmocked methods may throw or return default values depending on your mocking framework.

- **Implicit operators bypass mocking**: Implicit conversions like `File file = "/path/to/file"` create instances using the default `SystemFileSystemProvider`, not your mock. For full testability, always use `context.Files.GetFile()`.

- **Static methods are not mockable**: Methods like `File.GetNewTemporaryFilePath()` and `Folder.CreateTemporaryFolder()` use the real file system. Design your modules to receive paths via constructor or use `context.Files.CreateTemporaryFolder()` instead.

- **Mocking Path Operations**: If your code uses path operations, mock them too:
  ```csharp
  mockProvider.Setup(p => p.Combine(It.IsAny<string[]>()))
      .Returns((string[] paths) => Path.Combine(paths));
  ```

### What Gets Mocked

The `IFileSystemProvider` interface covers:
- File reads: `ReadAllTextAsync`, `ReadLinesAsync`, `ReadAllBytesAsync`
- File writes: `WriteAllTextAsync`, `WriteAllBytesAsync`, `WriteAllLinesAsync`, `AppendAllTextAsync`
- File management: `DeleteFile`, `CopyFile`, `MoveFile`, `FileExists`
- Directory operations: `CreateDirectory`, `DeleteDirectory`, `MoveDirectory`, `DirectoryExists`
- Enumeration: `EnumerateFiles`, `EnumerateDirectories`
- Path utilities: `GetTempPath`, `GetRandomFileName`, `Combine`, `GetRelativePath`
