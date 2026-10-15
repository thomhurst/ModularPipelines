using ModularPipelines.FileSystem;
using Moq;

namespace ModularPipelines.UnitTests.FileSystem;

public class FilePathCancellationTests
{
    [Test]
    [Arguments(false, false)]
    [Arguments(false, true)]
    [Arguments(true, false)]
    [Arguments(true, true)]
    public async Task WriteAsync_Canceled_Preserves_Destination_And_Input(bool exists, bool useStream)
    {
        await using var temporary = new TempFile();
        if (exists)
        {
            await temporary.File.WriteAsync("preserved");
        }

        await using var input = new MemoryStream("replacement"u8.ToArray());
        input.Position = 2;
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(() => useStream
            ? temporary.File.WriteAsync(input, cancellation.Token)
            : temporary.File.WriteAsync(new ReadOnlyMemory<byte>(input.ToArray()), cancellation.Token));

        await Assert.That(input.Position).IsEqualTo(2);
        await Assert.That(input.CanRead).IsTrue();
        await Assert.That(temporary.File.Exists).IsEqualTo(exists);
        if (exists)
        {
            await Assert.That(await temporary.File.ReadAsync()).IsEqualTo("preserved");
        }
    }

    [Test]
    [Arguments(false, false)]
    [Arguments(false, true)]
    [Arguments(true, false)]
    [Arguments(true, true)]
    public async Task CopyToAsync_Canceled_Preserves_Destination(bool exists, bool useFolder)
    {
        await using var temporary = new TempFolder();
        var source = new FilePath(Path.Combine(temporary.Folder.Path, "source.txt"));
        await source.WriteAsync("source");
        var folder = new FolderPath(Path.Combine(temporary.Folder.Path, "destination"));
        var destination = new FilePath(Path.Combine(folder.Path, source.Name));
        if (exists || !useFolder)
        {
            folder.Create();
        }

        if (exists)
        {
            await destination.WriteAsync("preserved");
        }

        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(() => useFolder
            ? source.CopyToAsync(folder, cancellation.Token)
            : source.CopyToAsync(destination.Path, cancellation.Token));

        await Assert.That(destination.Exists).IsEqualTo(exists);
        await Assert.That(folder.Exists).IsEqualTo(exists || !useFolder);
        await Assert.That(await source.ReadAsync()).IsEqualTo("source");
        if (exists)
        {
            await Assert.That(await destination.ReadAsync()).IsEqualTo("preserved");
        }
    }

    [Test]
    [Arguments("memory")]
    [Arguments("stream")]
    [Arguments("path")]
    [Arguments("folder")]
    public async Task Canceled_Operation_Does_Not_Access_Custom_Providers(string operation)
    {
        var provider = new Mock<IFileSystemProvider>(MockBehavior.Strict);
        var folderProvider = new Mock<IFileSystemProvider>(MockBehavior.Strict);
        var file = new FilePath("/source.txt", provider.Object);
        var folder = new FolderPath("/destination", folderProvider.Object);
        await using var input = new MemoryStream("replacement"u8.ToArray());
        input.Position = 2;
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(() => operation switch
        {
            "memory" => file.WriteAsync(new ReadOnlyMemory<byte>(input.ToArray()), cancellation.Token),
            "stream" => file.WriteAsync(input, cancellation.Token),
            "path" => file.CopyToAsync("/destination.txt", cancellation.Token),
            "folder" => file.CopyToAsync(folder, cancellation.Token),
            _ => throw new ArgumentOutOfRangeException(nameof(operation)),
        });

        await Assert.That(input.Position).IsEqualTo(2);
        provider.VerifyNoOtherCalls();
        folderProvider.VerifyNoOtherCalls();
    }

    [Test]
    [Arguments("memory")]
    [Arguments("stream")]
    [Arguments("path")]
    [Arguments("folder")]
    public async Task Uncanceled_Operation_Replaces_Destination(string operation)
    {
        await using var temporary = new TempFolder();
        var source = new FilePath(Path.Combine(temporary.Folder.Path, "source.txt"));
        await source.WriteAsync("replacement");
        var folder = new FolderPath(Path.Combine(temporary.Folder.Path, "destination")).Create();
        var destination = new FilePath(Path.Combine(folder.Path, source.Name));
        await destination.WriteAsync("long existing contents");
        await using var input = new MemoryStream("replacement"u8.ToArray());
        input.Position = 2;

        var operationTask = operation switch
        {
            "memory" => destination.WriteAsync(new ReadOnlyMemory<byte>(input.ToArray())),
            "stream" => destination.WriteAsync(input),
            "path" => source.CopyToAsync(destination.Path),
            "folder" => source.CopyToAsync(folder),
            _ => throw new ArgumentOutOfRangeException(nameof(operation)),
        };
        await operationTask;

        await Assert.That(await destination.ReadAsync()).IsEqualTo("replacement");
        await Assert.That(input.CanRead).IsTrue();
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task WriteAsync_Uses_Custom_Provider_Stream(bool useStream)
    {
        var provider = new Mock<IFileSystemProvider>();
        using var destination = new MemoryStream();
        provider.Setup(p => p.Create(It.IsAny<string>())).Returns(destination);
        var file = new FilePath("/destination.txt", provider.Object);
        await using var input = new MemoryStream("replacement"u8.ToArray());
        input.Position = 2;

        if (useStream)
        {
            await file.WriteAsync(input);
        }
        else
        {
            await file.WriteAsync(new ReadOnlyMemory<byte>(input.ToArray()));
        }

        await Assert.That(System.Text.Encoding.UTF8.GetString(destination.ToArray())).IsEqualTo("replacement");
        await Assert.That(input.CanRead).IsTrue();
        provider.Verify(p => p.Create(file.Path), Times.Once);
    }

    [Test]
    [Arguments("memory")]
    [Arguments("stream")]
    [Arguments("path")]
    public async Task Operation_Observes_Cancellation_During_IO(string operation)
    {
        using var cancellation = new CancellationTokenSource();
        var provider = new Mock<IFileSystemProvider>();
        var source = new Mock<Stream>();
        var destination = new Mock<Stream>();
        source.Setup(s => s.CopyToAsync(It.IsAny<Stream>(), It.IsAny<int>(), cancellation.Token))
            .Returns(() =>
            {
                cancellation.Cancel();
                return Task.FromCanceled(cancellation.Token);
            });
        destination.Setup(s => s.WriteAsync(It.IsAny<ReadOnlyMemory<byte>>(), cancellation.Token))
            .Returns(() =>
            {
                cancellation.Cancel();
                return ValueTask.FromCanceled(cancellation.Token);
            });
        provider.Setup(p => p.OpenRead(It.IsAny<string>())).Returns(source.Object);
        provider.Setup(p => p.Create(It.IsAny<string>())).Returns(destination.Object);
        var file = new FilePath("/source.txt", provider.Object);

        await Assert.ThrowsAsync<OperationCanceledException>(() => operation switch
        {
            "memory" => file.WriteAsync(new ReadOnlyMemory<byte>("replacement"u8.ToArray()), cancellation.Token),
            "stream" => file.WriteAsync(source.Object, cancellation.Token),
            "path" => file.CopyToAsync("/destination.txt", cancellation.Token),
            _ => throw new ArgumentOutOfRangeException(nameof(operation)),
        });

        await Assert.That(cancellation.IsCancellationRequested).IsTrue();
        provider.Verify(p => p.Create(It.IsAny<string>()), Times.Once);
    }
}
