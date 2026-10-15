using ModularPipelines.FileSystem;
using Moq;

namespace ModularPipelines.UnitTests.FileSystem;

public class FolderPathProviderTests
{
    [Test]
    public async Task Create_UsesProvider()
    {
        var mockProvider = new Mock<IFileSystemProvider>();
        mockProvider.Setup(p => p.Combine(It.IsAny<string[]>())).Returns((string[] args) => Path.Combine(args));
        var folder = new FolderPath("/test/folder", mockProvider.Object);

        folder.Create();

        mockProvider.Verify(p => p.CreateDirectory(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public async Task Delete_UsesProvider()
    {
        var mockProvider = new Mock<IFileSystemProvider>();
        var folder = new FolderPath("/test/folder", mockProvider.Object);

        folder.Delete();

        mockProvider.Verify(p => p.DeleteDirectory(It.IsAny<string>(), true), Times.Once);
    }

    [Test]
    public async Task GetFile_ReturnsFileWithSameProvider()
    {
        var mockProvider = new Mock<IFileSystemProvider>();
        mockProvider.Setup(p => p.Combine(It.IsAny<string[]>())).Returns((string[] args) => Path.Combine(args));
        var folder = new FolderPath("/test/folder", mockProvider.Object);

        var file = folder.GetFile("test.txt");

        // Verify file uses same provider by calling an operation
        file.Delete();
        mockProvider.Verify(p => p.DeleteFile(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public async Task GetFolder_ReturnsSubfolderWithSameProvider()
    {
        var mockProvider = new Mock<IFileSystemProvider>();
        mockProvider.Setup(p => p.Combine(It.IsAny<string[]>())).Returns((string[] args) => Path.Combine(args));
        var folder = new FolderPath("/test/folder", mockProvider.Object);

        var subfolder = folder.GetFolder("subfolder");

        // Verify subfolder uses same provider
        subfolder.Create();
        mockProvider.Verify(p => p.CreateDirectory(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public async Task MoveTo_ReturnsNewFolderWithProvider()
    {
        var mockProvider = new Mock<IFileSystemProvider>();
        var folder = new FolderPath("/source/folder", mockProvider.Object);

        var moved = folder.MoveTo("/dest/folder");

        mockProvider.Verify(p => p.MoveDirectory(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

        // Verify new folder has provider
        moved.Delete();
        mockProvider.Verify(p => p.DeleteDirectory(It.IsAny<string>(), true), Times.Once);
    }

    [Test]
    public void DefaultConstructor_UsesSystemProvider()
    {
        var tempPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        var folder = new FolderPath(tempPath);

        // Should work without throwing
        folder.Create();
        folder.Delete();
    }

    [Test]
    public async Task CreateAsync_UsesProvider()
    {
        var mockProvider = new Mock<IFileSystemProvider>();
        var folder = new FolderPath("/test/folder", mockProvider.Object);

        await folder.CreateAsync();

        mockProvider.Verify(p => p.CreateDirectory(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public async Task DeleteAsync_UsesProvider()
    {
        var mockProvider = new Mock<IFileSystemProvider>();
        var folder = new FolderPath("/test/folder", mockProvider.Object);

        await folder.DeleteAsync();

        mockProvider.Verify(p => p.DeleteDirectory(It.IsAny<string>(), true), Times.Once);
    }

    [Test]
    public async Task MoveToAsync_ReturnsNewFolderWithProvider()
    {
        var mockProvider = new Mock<IFileSystemProvider>();
        var folder = new FolderPath("/source/folder", mockProvider.Object);

        var moved = await folder.MoveToAsync("/dest/folder");

        mockProvider.Verify(p => p.MoveDirectory(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

        // Verify new folder has provider
        moved.Delete();
        mockProvider.Verify(p => p.DeleteDirectory(It.IsAny<string>(), true), Times.Once);
    }

    [Test]
    public async Task CreateFolder_UsesProvider()
    {
        var mockProvider = new Mock<IFileSystemProvider>();
        mockProvider.Setup(p => p.Combine(It.IsAny<string[]>())).Returns((string[] args) => Path.Combine(args));
        var folder = new FolderPath("/test/folder", mockProvider.Object);

        var subfolder = folder.CreateFolder("newsubfolder");

        // CreateFolder calls GetFolder().Create(), so CreateDirectory should be called
        mockProvider.Verify(p => p.CreateDirectory(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public async Task CreateFile_UsesProvider()
    {
        var mockProvider = new Mock<IFileSystemProvider>();
        mockProvider.Setup(p => p.Combine(It.IsAny<string[]>())).Returns((string[] args) => Path.Combine(args));
        // Return a stream that can be disposed
        mockProvider.Setup(p => p.Create(It.IsAny<string>())).Returns(new MemoryStream());
        var folder = new FolderPath("/test/folder", mockProvider.Object);

        var file = folder.CreateFile("newfile.txt");

        // CreateFile calls GetFile().Create(), which uses provider.Create
        mockProvider.Verify(p => p.Create(It.IsAny<string>()), Times.Once);
    }

    [Test]
    [MatrixDataSource]
    public async Task CopyTo_Uses_Provider_Separator_For_Parent_Relative_Destinations(
        [Matrix('/', '\\')] char separator,
        [Matrix(false, true)] bool useAsync)
    {
        var source = Path.GetFullPath("source");
        var target = Path.GetFullPath("target");
        var mockProvider = new Mock<IFileSystemProvider>();
        mockProvider.SetupGet(p => p.DirectorySeparatorChar).Returns(separator);
        mockProvider.Setup(p => p.GetRelativePath(source, target)).Returns($"..{separator}target");
        var folder = new FolderPath(source, mockProvider.Object);

        var copied = useAsync ? await folder.CopyToAsync(target) : folder.CopyTo(target);

        mockProvider.Verify(p => p.GetRelativePath(source, target), Times.Once);
        mockProvider.Verify(p => p.CreateDirectory(target), Times.Once);
        await Assert.That(copied.Provider).IsSameReferenceAs(mockProvider.Object);
    }

    [Test]
    [MatrixDataSource]
    public async Task Copy_Does_Not_Consume_Output_Through_An_Unrecognized_Alias(
        [Matrix(0, 1, 2, 3)] int overload,
        [Matrix(false, true)] bool liveDirectories)
    {
        var source = Path.GetFullPath("source");
        var target = Path.GetFullPath("alias");
        var child = Path.Combine(source, "child");
        var directories = new List<string> { child };
        var files = new List<string> { Path.Combine(source, "payload.txt") };
        var copiedFiles = new List<string>();
        var outputs = new List<MemoryStream>();
        var provider = new Mock<IFileSystemProvider>();
        provider.SetupGet(p => p.DirectorySeparatorChar).Returns(Path.DirectorySeparatorChar);
        provider.Setup(p => p.GetRelativePath(It.IsAny<string>(), It.IsAny<string>()))
            .Returns((string from, string to) => Path.GetRelativePath(from, to));
        provider.Setup(p => p.Combine(It.IsAny<string[]>())).Returns((string[] paths) => Path.Combine(paths));
        provider.Setup(p => p.EnumerateDirectories(source, "*", SearchOption.AllDirectories))
            .Returns(() => liveDirectories ? EnumerateLive(directories) : directories.ToArray());
        provider.Setup(p => p.EnumerateFiles(source, "*", SearchOption.AllDirectories))
            .Returns(() => EnumerateLive(files));

        // Model a bind mount: writes through the unrelated alias appear under source/child.
        provider.Setup(p => p.CreateDirectory(It.IsAny<string>())).Callback((string path) =>
        {
            var physicalPath = PhysicalPath(path);
            if (!directories.Contains(physicalPath))
            {
                directories.Add(physicalPath);
            }
        });
        provider.Setup(p => p.CopyFile(It.IsAny<string>(), It.IsAny<string>(), true))
            .Callback((string _, string destination, bool _) => RecordOutput(destination));
        provider.Setup(p => p.OpenRead(It.IsAny<string>())).Returns(() => new MemoryStream("contents"u8.ToArray()));
        provider.Setup(p => p.Create(It.IsAny<string>())).Returns((string destination) =>
        {
            RecordOutput(destination);
            var stream = new MemoryStream();
            outputs.Add(stream);
            return stream;
        });
        var folder = new FolderPath(source, provider.Object);

        var copied = overload switch
        {
            0 => folder.CopyTo(target),
            1 => folder.CopyTo(target, preserveTimestamps: true),
            2 => await folder.CopyToAsync(target),
            _ => await folder.CopyToAsync(target, preserveTimestamps: true),
        };

        await Assert.That(copied.OriginalPath).IsEqualTo(target);
        await Assert.That(directories).IsEquivalentTo(new[] { child, Path.Combine(child, "child") });
        await Assert.That(copiedFiles).IsEquivalentTo(new[] { Path.Combine(target, "payload.txt") });
        foreach (var stream in outputs)
        {
            await Assert.That(stream.ToArray()).IsEquivalentTo("contents"u8.ToArray());
        }

        string PhysicalPath(string path) => Path.GetFullPath(Path.Combine(child, Path.GetRelativePath(target, path)));

        void RecordOutput(string destination)
        {
            copiedFiles.Add(destination);
            files.Add(PhysicalPath(destination));
        }

        static IEnumerable<string> EnumerateLive(List<string> entries)
        {
            for (var index = 0; index < entries.Count; index++)
            {
                // Bound a regression without touching the real filesystem or exhausting resources.
                if (index == 8)
                {
                    throw new InvalidOperationException("Copy consumed its own output through an alias.");
                }

                yield return entries[index];
            }
        }
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Async_Copy_Cancellation_During_Snapshot_Does_Not_Mutate_Destination(bool cancelDuringEnumeration)
    {
        using var cancellation = new CancellationTokenSource();
        var source = Path.GetFullPath("source");
        var target = Path.GetFullPath("target");
        var provider = new Mock<IFileSystemProvider>();
        provider.SetupGet(p => p.DirectorySeparatorChar).Returns(Path.DirectorySeparatorChar);
        provider.Setup(p => p.GetRelativePath(source, target)).Returns(Path.GetRelativePath(source, target));
        provider.Setup(p => p.EnumerateDirectories(source, "*", SearchOption.AllDirectories))
            .Returns(EnumerateDirectories);
        if (!cancelDuringEnumeration)
        {
            cancellation.Cancel();
        }

        await Assert.That(() => (Task) new FolderPath(source, provider.Object).CopyToAsync(target, cancellation.Token))
            .Throws<OperationCanceledException>();

        provider.Verify(p => p.CreateDirectory(It.IsAny<string>()), Times.Never);
        provider.Verify(p => p.EnumerateFiles(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<SearchOption>()), Times.Never);

        IEnumerable<string> EnumerateDirectories()
        {
            yield return Path.Combine(source, "first");
            cancellation.Cancel();
            yield return Path.Combine(source, "second");
        }
    }

    [Test]
    public async Task CopyTo_UsesProviderAndReturnsFolderWithProvider()
    {
        // Use real temp directories because CopyTo sets DirectoryInfo.Attributes
        // which requires the target path to exist on the filesystem
        var sourceDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        var destDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

        try
        {
            var mockProvider = new Mock<IFileSystemProvider>();
            mockProvider.Setup(p => p.Combine(It.IsAny<string[]>())).Returns((string[] args) => Path.Combine(args));
            mockProvider.SetupGet(p => p.DirectorySeparatorChar).Returns(Path.DirectorySeparatorChar);
            mockProvider.Setup(p => p.GetRelativePath(It.IsAny<string>(), It.IsAny<string>())).Returns((string a, string b) => Path.GetRelativePath(a, b));
            mockProvider.Setup(p => p.EnumerateDirectories(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<SearchOption>())).Returns(Array.Empty<string>());
            mockProvider.Setup(p => p.EnumerateFiles(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<SearchOption>())).Returns(Array.Empty<string>());
            // Actually create the directory so DirectoryInfo.Attributes can work
            mockProvider.Setup(p => p.CreateDirectory(It.IsAny<string>())).Callback((string path) => Directory.CreateDirectory(path));

            // Create source directory for DirectoryInfo to reference
            Directory.CreateDirectory(sourceDir);

            var folder = new FolderPath(sourceDir, mockProvider.Object);

            var copied = folder.CopyTo(destDir);

            // Verify CreateDirectory was called for target path
            mockProvider.Verify(p => p.CreateDirectory(destDir), Times.Once);

            // Verify returned folder has provider
            copied.Delete();
            mockProvider.Verify(p => p.DeleteDirectory(It.IsAny<string>(), true), Times.Once);
        }
        finally
        {
            if (Directory.Exists(sourceDir))
            {
                Directory.Delete(sourceDir, true);
            }
            if (Directory.Exists(destDir))
            {
                Directory.Delete(destDir, true);
            }
        }
    }

    [Test]
    public async Task CopyToAsync_UsesProviderAndReturnsFolderWithProvider()
    {
        // Use real temp directories because CopyToAsync sets DirectoryInfo.Attributes
        // which requires the target path to exist on the filesystem
        var sourceDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        var destDir = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

        try
        {
            var mockProvider = new Mock<IFileSystemProvider>();
            mockProvider.Setup(p => p.Combine(It.IsAny<string[]>())).Returns((string[] args) => Path.Combine(args));
            mockProvider.SetupGet(p => p.DirectorySeparatorChar).Returns(Path.DirectorySeparatorChar);
            mockProvider.Setup(p => p.GetRelativePath(It.IsAny<string>(), It.IsAny<string>())).Returns((string a, string b) => Path.GetRelativePath(a, b));
            mockProvider.Setup(p => p.EnumerateDirectories(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<SearchOption>())).Returns(Array.Empty<string>());
            mockProvider.Setup(p => p.EnumerateFiles(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<SearchOption>())).Returns(Array.Empty<string>());
            // Actually create the directory so DirectoryInfo.Attributes can work
            mockProvider.Setup(p => p.CreateDirectory(It.IsAny<string>())).Callback((string path) => Directory.CreateDirectory(path));

            // Create source directory for DirectoryInfo to reference
            Directory.CreateDirectory(sourceDir);

            var folder = new FolderPath(sourceDir, mockProvider.Object);

            var copied = await folder.CopyToAsync(destDir);

            // Verify CreateDirectory was called for target path
            mockProvider.Verify(p => p.CreateDirectory(destDir), Times.Once);

            // Verify returned folder has provider
            copied.Delete();
            mockProvider.Verify(p => p.DeleteDirectory(It.IsAny<string>(), true), Times.Once);
        }
        finally
        {
            if (Directory.Exists(sourceDir))
            {
                Directory.Delete(sourceDir, true);
            }
            if (Directory.Exists(destDir))
            {
                Directory.Delete(destDir, true);
            }
        }
    }
}
