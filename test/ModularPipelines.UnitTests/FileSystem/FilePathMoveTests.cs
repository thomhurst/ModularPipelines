using ModularPipelines.FileSystem;
using Moq;

namespace ModularPipelines.UnitTests.FileSystem;

public class FilePathMoveTests
{
    [Test]
    [MatrixDataSource]
    public async Task MoveToAsync_Canceled_Preserves_Source_And_Destination(
        [Matrix(false, true)] bool exists,
        [Matrix(false, true)] bool useFolder,
        [Matrix(false, true)] bool useCustomProvider)
    {
        await using var temporary = new TempFolder();
        var sourceProvider = CreateProvider();
        var folderProvider = CreateProvider();
        var sourcePath = Path.Combine(temporary.Folder.Path, "source.txt");
        var folderPath = Path.Combine(temporary.Folder.Path, "destination");
        var destinationPath = Path.Combine(folderPath, "source.txt");
        await File.WriteAllTextAsync(sourcePath, "source");
        if (exists || !useFolder)
        {
            Directory.CreateDirectory(folderPath);
        }

        if (exists)
        {
            await File.WriteAllTextAsync(destinationPath, "preserved");
        }

        var source = new FilePath(sourcePath, useCustomProvider ? sourceProvider.Object : SystemFileSystemProvider.Instance);
        var folder = new FolderPath(folderPath, useCustomProvider ? folderProvider.Object : SystemFileSystemProvider.Instance);
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(() => useFolder
            ? source.MoveToAsync(folder, cancellation.Token)
            : source.MoveToAsync(destinationPath, cancellation.Token));

        sourceProvider.VerifyNoOtherCalls();
        folderProvider.VerifyNoOtherCalls();
        await Assert.That(File.Exists(sourcePath)).IsTrue();
        await Assert.That(await File.ReadAllTextAsync(sourcePath)).IsEqualTo("source");
        await Assert.That(Directory.Exists(folderPath)).IsEqualTo(exists || !useFolder);
        await Assert.That(File.Exists(destinationPath)).IsEqualTo(exists);
        if (exists)
        {
            await Assert.That(await File.ReadAllTextAsync(destinationPath)).IsEqualTo("preserved");
            await Assert.That(Directory.GetFileSystemEntries(folderPath)).Count().IsEqualTo(1);
        }
    }

    [Test]
    [MatrixDataSource]
    public async Task MoveToAsync_Folder_Moves_File_And_Preserves_Provider(
        [Matrix(false, true)] bool exists,
        [Matrix(false, true)] bool useCustomProvider)
    {
        await using var temporary = new TempFolder();
        var sourceProvider = CreateProvider();
        var sourcePath = Path.Combine(temporary.Folder.Path, "source.txt");
        var folderPath = Path.Combine(temporary.Folder.Path, "destination");
        await File.WriteAllTextAsync(sourcePath, "source");
        if (exists)
        {
            Directory.CreateDirectory(folderPath);
        }

        var source = new FilePath(sourcePath, useCustomProvider ? sourceProvider.Object : SystemFileSystemProvider.Instance);
        var folder = new FolderPath(folderPath, useCustomProvider ? sourceProvider.Object : SystemFileSystemProvider.Instance);
        using var cancellation = new CancellationTokenSource();

        var moved = await source.MoveToAsync(folder, cancellation.Token);

        await Assert.That(File.Exists(sourcePath)).IsFalse();
        await Assert.That(moved.Path).IsEqualTo(Path.Combine(folderPath, "source.txt"));
        await Assert.That(await File.ReadAllTextAsync(moved.Path)).IsEqualTo("source");
        await Assert.That(ReferenceEquals(moved.Provider, source.Provider)).IsTrue();
        if (useCustomProvider)
        {
            sourceProvider.Verify(p => p.CreateDirectory(folderPath), Times.Once);
            sourceProvider.Verify(p => p.MoveFile(sourcePath, moved.Path), Times.Once);
        }
    }

    private static Mock<IFileSystemProvider> CreateProvider()
    {
        var provider = new Mock<IFileSystemProvider>(MockBehavior.Strict);
        provider.Setup(p => p.CreateDirectory(It.IsAny<string>()))
            .Callback((string path) => Directory.CreateDirectory(path));
        provider.Setup(p => p.Combine(It.IsAny<string[]>()))
            .Returns((string[] paths) => Path.Combine(paths));
        provider.Setup(p => p.MoveFile(It.IsAny<string>(), It.IsAny<string>()))
            .Callback((string source, string destination) => File.Move(source, destination));
        return provider;
    }
}
