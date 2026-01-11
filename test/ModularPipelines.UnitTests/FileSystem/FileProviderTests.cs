using ModularPipelines.FileSystem;
using Moq;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.UnitTests.FileSystem;

public class FileProviderTests
{
    [Test]
    public async Task ReadAsync_UsesProvider()
    {
        var mockProvider = new Mock<IFileSystemProvider>();
        mockProvider.Setup(p => p.ReadAllTextAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("mocked content");

        var file = new File("/test/path.txt", mockProvider.Object);

        var result = await file.ReadAsync();

        await Assert.That(result).IsEqualTo("mocked content");
    }

    [Test]
    public async Task WriteAsync_UsesProvider()
    {
        var mockProvider = new Mock<IFileSystemProvider>();
        var file = new File("/test/path.txt", mockProvider.Object);

        await file.WriteAsync("test content");

        mockProvider.Verify(p => p.WriteAllTextAsync(It.IsAny<string>(), "test content", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task Delete_UsesProvider()
    {
        var mockProvider = new Mock<IFileSystemProvider>();
        var file = new File("/test/path.txt", mockProvider.Object);

        file.Delete();

        mockProvider.Verify(p => p.DeleteFile(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public async Task CopyTo_ReturnsFileWithSameProvider()
    {
        var mockProvider = new Mock<IFileSystemProvider>();
        var file = new File("/source/file.txt", mockProvider.Object);

        var copied = file.CopyTo("/dest/file.txt");

        // Verify copy was called
        mockProvider.Verify(p => p.CopyFile(It.IsAny<string>(), It.IsAny<string>(), true), Times.Once);

        // Verify new file uses same provider by calling Delete on it
        copied.Delete();
        mockProvider.Verify(p => p.DeleteFile(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public async Task MoveTo_ReturnsFileWithSameProvider()
    {
        var mockProvider = new Mock<IFileSystemProvider>();
        var file = new File("/source/file.txt", mockProvider.Object);

        var moved = file.MoveTo("/dest/file.txt");

        // Verify move was called
        mockProvider.Verify(p => p.MoveFile(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

        // Verify new file uses same provider by calling Delete on it
        moved.Delete();
        mockProvider.Verify(p => p.DeleteFile(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public async Task DefaultConstructor_UsesSystemProvider()
    {
        var tempFile = Path.GetTempFileName();
        try
        {
            var file = new File(tempFile);
            // Should work without throwing - uses default SystemFileSystemProvider
            await Assert.That(file.Exists).IsTrue();
        }
        finally
        {
            System.IO.File.Delete(tempFile);
        }
    }

    [Test]
    public async Task ReadBytesAsync_UsesProvider()
    {
        var mockProvider = new Mock<IFileSystemProvider>();
        var expectedBytes = new byte[] { 1, 2, 3, 4, 5 };
        mockProvider.Setup(p => p.ReadAllBytesAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedBytes);

        var file = new File("/test/path.txt", mockProvider.Object);

        var result = await file.ReadBytesAsync();

        await Assert.That(result).IsEqualTo(expectedBytes);
    }

    [Test]
    public async Task WriteAsync_Bytes_UsesProvider()
    {
        var mockProvider = new Mock<IFileSystemProvider>();
        var file = new File("/test/path.txt", mockProvider.Object);
        var contents = new byte[] { 1, 2, 3 };

        await file.WriteAsync(contents);

        mockProvider.Verify(p => p.WriteAllBytesAsync(It.IsAny<string>(), contents, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task WriteAsync_Lines_UsesProvider()
    {
        var mockProvider = new Mock<IFileSystemProvider>();
        var file = new File("/test/path.txt", mockProvider.Object);
        var lines = new[] { "line1", "line2" };

        await file.WriteAsync(lines);

        mockProvider.Verify(p => p.WriteAllLinesAsync(It.IsAny<string>(), lines, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task AppendAsync_UsesProvider()
    {
        var mockProvider = new Mock<IFileSystemProvider>();
        var file = new File("/test/path.txt", mockProvider.Object);

        await file.AppendAsync("appended content");

        mockProvider.Verify(p => p.AppendAllTextAsync(It.IsAny<string>(), "appended content", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task AppendAsync_Lines_UsesProvider()
    {
        var mockProvider = new Mock<IFileSystemProvider>();
        var file = new File("/test/path.txt", mockProvider.Object);
        var lines = new[] { "line1", "line2" };

        await file.AppendAsync(lines);

        mockProvider.Verify(p => p.AppendAllLinesAsync(It.IsAny<string>(), lines, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task Create_UsesProvider()
    {
        var mockProvider = new Mock<IFileSystemProvider>();
        // Use a MemoryStream which can be disposed
        var stream = new MemoryStream();
        mockProvider.Setup(p => p.Create(It.IsAny<string>())).Returns(stream);

        var file = new File("/test/path.txt", mockProvider.Object);

        file.Create();

        mockProvider.Verify(p => p.Create(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public async Task GetStream_UsesProvider()
    {
        // We need to return a FileStream, but we can't easily mock one
        // So we'll use a real temp file for this test
        var tempFile = Path.GetTempFileName();
        try
        {
            var file = new File(tempFile);

            await using var stream = file.GetStream(FileAccess.ReadWrite);
            await Assert.That(stream).IsNotNull();
        }
        finally
        {
            System.IO.File.Delete(tempFile);
        }
    }

    [Test]
    public async Task CopyToAsync_UsesProviderStreams()
    {
        var mockProvider = new Mock<IFileSystemProvider>();
        var sourceStream = new MemoryStream(new byte[] { 1, 2, 3 });
        var destStream = new MemoryStream();

        mockProvider.Setup(p => p.OpenRead(It.IsAny<string>())).Returns(sourceStream);
        mockProvider.Setup(p => p.Create(It.IsAny<string>())).Returns(destStream);

        var file = new File("/source/file.txt", mockProvider.Object);

        var copied = await file.CopyToAsync("/dest/file.txt");

        mockProvider.Verify(p => p.OpenRead(It.IsAny<string>()), Times.Once);
        mockProvider.Verify(p => p.Create("/dest/file.txt"), Times.Once);

        // Verify new file uses same provider
        copied.Delete();
        mockProvider.Verify(p => p.DeleteFile(It.IsAny<string>()), Times.Once);
    }
}
