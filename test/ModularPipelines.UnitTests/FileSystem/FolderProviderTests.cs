using ModularPipelines.FileSystem;
using Moq;

namespace ModularPipelines.UnitTests.FileSystem;

public class FolderProviderTests
{
    [Test]
    public async Task Create_UsesProvider()
    {
        var mockProvider = new Mock<IFileSystemProvider>();
        mockProvider.Setup(p => p.Combine(It.IsAny<string[]>())).Returns((string[] args) => Path.Combine(args));
        var folder = new Folder("/test/folder", mockProvider.Object);

        folder.Create();

        mockProvider.Verify(p => p.CreateDirectory(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public async Task Delete_UsesProvider()
    {
        var mockProvider = new Mock<IFileSystemProvider>();
        var folder = new Folder("/test/folder", mockProvider.Object);

        folder.Delete();

        mockProvider.Verify(p => p.DeleteDirectory(It.IsAny<string>(), true), Times.Once);
    }

    [Test]
    public async Task GetFile_ReturnsFileWithSameProvider()
    {
        var mockProvider = new Mock<IFileSystemProvider>();
        mockProvider.Setup(p => p.Combine(It.IsAny<string[]>())).Returns((string[] args) => Path.Combine(args));
        var folder = new Folder("/test/folder", mockProvider.Object);

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
        var folder = new Folder("/test/folder", mockProvider.Object);

        var subfolder = folder.GetFolder("subfolder");

        // Verify subfolder uses same provider
        subfolder.Create();
        mockProvider.Verify(p => p.CreateDirectory(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public async Task MoveTo_ReturnsNewFolderWithProvider()
    {
        var mockProvider = new Mock<IFileSystemProvider>();
        var folder = new Folder("/source/folder", mockProvider.Object);

        var moved = folder.MoveTo("/dest/folder");

        mockProvider.Verify(p => p.MoveDirectory(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

        // Verify new folder has provider
        moved.Delete();
        mockProvider.Verify(p => p.DeleteDirectory(It.IsAny<string>(), true), Times.Once);
    }

    [Test]
    public async Task DefaultConstructor_UsesSystemProvider()
    {
        var tempPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        var folder = new Folder(tempPath);

        // Should work without throwing
        folder.Create();
        folder.Delete();

        await Assert.That(true).IsTrue(); // If we got here, it worked
    }

    [Test]
    public async Task CreateAsync_UsesProvider()
    {
        var mockProvider = new Mock<IFileSystemProvider>();
        var folder = new Folder("/test/folder", mockProvider.Object);

        await folder.CreateAsync();

        mockProvider.Verify(p => p.CreateDirectory(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public async Task DeleteAsync_UsesProvider()
    {
        var mockProvider = new Mock<IFileSystemProvider>();
        var folder = new Folder("/test/folder", mockProvider.Object);

        await folder.DeleteAsync();

        mockProvider.Verify(p => p.DeleteDirectory(It.IsAny<string>(), true), Times.Once);
    }

    [Test]
    public async Task MoveToAsync_ReturnsNewFolderWithProvider()
    {
        var mockProvider = new Mock<IFileSystemProvider>();
        var folder = new Folder("/source/folder", mockProvider.Object);

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
        var folder = new Folder("/test/folder", mockProvider.Object);

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
        var folder = new Folder("/test/folder", mockProvider.Object);

        var file = folder.CreateFile("newfile.txt");

        // CreateFile calls GetFile().Create(), which uses provider.Create
        mockProvider.Verify(p => p.Create(It.IsAny<string>()), Times.Once);
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
            mockProvider.Setup(p => p.GetRelativePath(It.IsAny<string>(), It.IsAny<string>())).Returns((string a, string b) => Path.GetRelativePath(a, b));
            mockProvider.Setup(p => p.EnumerateDirectories(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<SearchOption>())).Returns(Array.Empty<string>());
            mockProvider.Setup(p => p.EnumerateFiles(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<SearchOption>())).Returns(Array.Empty<string>());
            // Actually create the directory so DirectoryInfo.Attributes can work
            mockProvider.Setup(p => p.CreateDirectory(It.IsAny<string>())).Callback((string path) => Directory.CreateDirectory(path));

            // Create source directory for DirectoryInfo to reference
            Directory.CreateDirectory(sourceDir);

            var folder = new Folder(sourceDir, mockProvider.Object);

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
            mockProvider.Setup(p => p.GetRelativePath(It.IsAny<string>(), It.IsAny<string>())).Returns((string a, string b) => Path.GetRelativePath(a, b));
            mockProvider.Setup(p => p.EnumerateDirectories(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<SearchOption>())).Returns(Array.Empty<string>());
            mockProvider.Setup(p => p.EnumerateFiles(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<SearchOption>())).Returns(Array.Empty<string>());
            // Actually create the directory so DirectoryInfo.Attributes can work
            mockProvider.Setup(p => p.CreateDirectory(It.IsAny<string>())).Callback((string path) => Directory.CreateDirectory(path));

            // Create source directory for DirectoryInfo to reference
            Directory.CreateDirectory(sourceDir);

            var folder = new Folder(sourceDir, mockProvider.Object);

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
