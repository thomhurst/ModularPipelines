using ModularPipelines.FileSystem;
using Moq;

namespace ModularPipelines.UnitTests.FileSystem;

public class FolderPathCancellationTests
{
    [Test]
    [MatrixDataSource]
    public async Task CopyToAsync_Canceled_Preserves_Destination(
        [Matrix(false, true)] bool exists,
        [Matrix(false, true)] bool populated,
        [Matrix("default", "false", "true")] string timestamps)
    {
        await using var temporary = new TempFolder();
        var source = temporary.Folder.CreateFolder("source");
        var destination = temporary.Folder.GetFolder("destination");
        if (populated)
        {
            await source.GetFile("file.txt").WriteAsync("replacement");
            await source.CreateFolder("nested").GetFile("nested.txt").WriteAsync("nested");
        }

        var provider = SystemFileSystemProvider.Instance;
        var file = destination.GetFile("file.txt");
        if (exists)
        {
            destination.Create();
            await file.WriteAsync("preserved");
            foreach (var path in new[] { destination.Path, file.Path })
            {
                provider.SetCreationTimeUtc(path, new DateTime(2020, 1, 2, 3, 4, 5, DateTimeKind.Utc));
                provider.SetLastWriteTimeUtc(path, new DateTime(2021, 2, 3, 4, 5, 6, DateTimeKind.Utc));
                provider.SetLastAccessTimeUtc(path, new DateTime(2022, 3, 4, 5, 6, 7, DateTimeKind.Utc));
            }
        }

        var folderMetadata = exists ? ReadMetadata(provider, destination.Path) : default;
        var fileMetadata = exists ? ReadMetadata(provider, file.Path) : default;
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(() => CopyAsync(source, destination.Path, timestamps, cancellation.Token));

        await Assert.That(destination.Exists).IsEqualTo(exists);
        if (exists)
        {
            await Assert.That(ReadMetadata(provider, destination.Path)).IsEqualTo(folderMetadata);
            await Assert.That(ReadMetadata(provider, file.Path)).IsEqualTo(fileMetadata);
            await Assert.That(Directory.GetDirectories(destination.Path)).IsEmpty();
            await Assert.That(Directory.GetFiles(destination.Path)).Count().IsEqualTo(1);
            await Assert.That(await file.ReadAsync()).IsEqualTo("preserved");
        }
    }

    [Test]
    [Arguments("default")]
    [Arguments("false")]
    [Arguments("true")]
    public async Task CopyToAsync_Canceled_Does_Not_Access_Custom_Provider(string timestamps)
    {
        var provider = new Mock<IFileSystemProvider>(MockBehavior.Strict);
        var source = new FolderPath("/source", provider.Object);
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(() => CopyAsync(source, "/destination", timestamps, cancellation.Token));

        provider.VerifyNoOtherCalls();
    }

    [Test]
    [Arguments("default")]
    [Arguments("false")]
    [Arguments("true")]
    public async Task CopyToAsync_Uncanceled_Copies_Contents(string timestamps)
    {
        await using var temporary = new TempFolder();
        var source = temporary.Folder.CreateFolder("source");
        await source.CreateFolder("nested").GetFile("file.txt").WriteAsync("contents");
        var destination = temporary.Folder.GetFolder("destination");

        var copied = await CopyAsync(source, destination.Path, timestamps, CancellationToken.None);

        await Assert.That(copied.Path).IsEqualTo(destination.Path);
        await Assert.That(await copied.GetFolder("nested").GetFile("file.txt").ReadAsync()).IsEqualTo("contents");
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task CopyToAsync_Observes_Cancellation_During_Enumeration(bool directories)
    {
        using var cancellation = new CancellationTokenSource();
        var provider = new Mock<IFileSystemProvider>();
        var source = new FolderPath("/source", provider.Object);
        provider.Setup(p => p.DirectorySeparatorChar).Returns(Path.DirectorySeparatorChar);
        provider.Setup(p => p.GetRelativePath(It.IsAny<string>(), It.IsAny<string>()))
            .Returns((string from, string to) => Path.GetRelativePath(from, to));
        provider.Setup(p => p.EnumerateDirectories(source.Path, "*", SearchOption.TopDirectoryOnly))
            .Returns(() => directories ? CancelThenEnumerate() : []);
        provider.Setup(p => p.EnumerateFiles(source.Path, "*", SearchOption.TopDirectoryOnly))
            .Returns(() => directories ? [] : CancelThenEnumerate());

        await Assert.ThrowsAsync<OperationCanceledException>(() => source.CopyToAsync("/destination", cancellation.Token));

        provider.Verify(p => p.CreateDirectory(It.IsAny<string>()), Times.Never);
        provider.Verify(p => p.OpenRead(It.IsAny<string>()), Times.Never);
        provider.Verify(p => p.Create(It.IsAny<string>()), Times.Never);
        provider.Verify(p => p.SetAttributes(It.IsAny<string>(), It.IsAny<FileAttributes>()), Times.Never);

        IEnumerable<string> CancelThenEnumerate()
        {
            cancellation.Cancel();
            yield return Path.Combine(source.Path, "entry");
        }
    }

    [Test]
    [Arguments("default")]
    [Arguments("false")]
    [Arguments("true")]
    public async Task CopyToAsync_Observes_Cancellation_During_IO(string timestamps)
    {
        using var cancellation = new CancellationTokenSource();
        var provider = new Mock<IFileSystemProvider>();
        var sourceStream = new Mock<Stream>();
        var destinationStream = new Mock<Stream>();
        var source = new FolderPath("/source", provider.Object);
        var file = Path.Combine(source.Path, "file.txt");
        provider.Setup(p => p.DirectorySeparatorChar).Returns(Path.DirectorySeparatorChar);
        provider.Setup(p => p.GetRelativePath(It.IsAny<string>(), It.IsAny<string>()))
            .Returns((string from, string to) => Path.GetRelativePath(from, to));
        provider.Setup(p => p.EnumerateDirectories(source.Path, "*", SearchOption.TopDirectoryOnly)).Returns([]);
        provider.Setup(p => p.EnumerateFiles(source.Path, "*", SearchOption.TopDirectoryOnly)).Returns([file]);
        provider.Setup(p => p.Combine(It.IsAny<string[]>())).Returns((string[] paths) => Path.Combine(paths));
        provider.Setup(p => p.OpenRead(file)).Returns(sourceStream.Object);
        provider.Setup(p => p.Create(It.IsAny<string>())).Returns(destinationStream.Object);
        sourceStream.Setup(s => s.CopyToAsync(destinationStream.Object, It.IsAny<int>(), cancellation.Token))
            .Returns(() =>
            {
                cancellation.Cancel();
                return Task.FromCanceled(cancellation.Token);
            });

        await Assert.ThrowsAsync<OperationCanceledException>(() => CopyAsync(source, "/destination", timestamps, cancellation.Token));

        sourceStream.Verify(s => s.CopyToAsync(destinationStream.Object, It.IsAny<int>(), cancellation.Token), Times.Once);
        sourceStream.Verify(s => s.DisposeAsync(), Times.Once);
        destinationStream.Verify(s => s.DisposeAsync(), Times.Once);
        provider.Verify(p => p.SetAttributes(It.IsAny<string>(), It.IsAny<FileAttributes>()), Times.Never);
    }

    private static Task<FolderPath> CopyAsync(FolderPath source, string destination, string timestamps, CancellationToken cancellationToken)
    {
        return timestamps switch
        {
            "default" => source.CopyToAsync(destination, cancellationToken),
            "false" => source.CopyToAsync(destination, preserveTimestamps: false, cancellationToken),
            "true" => source.CopyToAsync(destination, preserveTimestamps: true, cancellationToken),
            _ => throw new ArgumentOutOfRangeException(nameof(timestamps)),
        };
    }

    private static (FileAttributes Attributes, DateTime Created, DateTime Written, DateTime Accessed) ReadMetadata(IFileSystemProvider provider, string path)
    {
        return (provider.GetAttributes(path), provider.GetCreationTimeUtc(path), provider.GetLastWriteTimeUtc(path), provider.GetLastAccessTimeUtc(path));
    }
}
