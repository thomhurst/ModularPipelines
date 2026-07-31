using System.Text;

namespace ModularPipelines.Testing.UnitTests;

public class InMemoryFileSystemProviderTests
{
    [Test]
    public async Task SupportsFileLifecycleOperations()
    {
        var provider = new InMemoryFileSystemProvider();
        var root = Path.Combine(provider.GetTempPath(), "lifecycle");
        var source = Path.Combine(root, "source.txt");
        var copy = Path.Combine(root, "copy.txt");
        var moved = Path.Combine(root, "moved.txt");

        provider.CreateDirectory(root);
        await provider.WriteAllTextAsync(source, "first");
        await provider.AppendAllTextAsync(source, " second");
        provider.CopyFile(source, copy, overwrite: false);
        provider.MoveFile(copy, moved);

        using (Assert.Multiple())
        {
            await Assert.That(await provider.ReadAllTextAsync(source)).IsEqualTo("first second");
            await Assert.That(await provider.ReadAllTextAsync(moved)).IsEqualTo("first second");
            await Assert.That(provider.FileExists(copy)).IsFalse();
            await Assert.That(provider.EnumerateFiles(root, "*.txt", SearchOption.TopDirectoryOnly))
                .Count()
                .IsEqualTo(2);
        }

        provider.DeleteFile(source);
        provider.DeleteDirectory(root, recursive: true);

        await Assert.That(provider.DirectoryExists(root)).IsFalse();
    }

    [Test]
    public async Task CommitsWritableStreamsOnDispose()
    {
        var provider = new InMemoryFileSystemProvider();
        var path = Path.Combine(provider.GetTempPath(), "stream.bin");

        await using (var stream = provider.Create(path))
        {
            await stream.WriteAsync(Encoding.UTF8.GetBytes("stream contents"));
        }

        await Assert.That(Encoding.UTF8.GetString(await provider.ReadAllBytesAsync(path)))
            .IsEqualTo("stream contents");
    }

    [Test]
    public async Task OpenReadStreamsRejectWrites()
    {
        var provider = new InMemoryFileSystemProvider();
        var path = Path.Combine(provider.GetTempPath(), "readonly.txt");
        await provider.WriteAllTextAsync(path, "contents");

        using var stream = provider.OpenRead(path);

        await Assert.That(() => stream.WriteByte(1)).Throws<NotSupportedException>();
#pragma warning disable CA1835 // The legacy overload is the behavior under test.
        await Assert.That(async () =>
                await stream.WriteAsync([1], 0, 1, CancellationToken.None))
            .Throws<NotSupportedException>();
#pragma warning restore CA1835
    }

    [Test]
    public async Task SeedsCurrentDirectoryHierarchy()
    {
        var provider = new InMemoryFileSystemProvider();

        await Assert.That(provider.DirectoryExists(Environment.CurrentDirectory)).IsTrue();
    }

    [Test]
    public async Task EnforcesExclusiveOpenHandles()
    {
        var provider = new InMemoryFileSystemProvider();
        var path = Path.Combine(provider.GetTempPath(), "exclusive.txt");
        await provider.WriteAllTextAsync(path, "contents");

        using (provider.OpenRead(path))
        {
            await Assert.That(() => provider.OpenRead(path)).Throws<IOException>();
        }

        using var reopened = provider.OpenRead(path);
        await Assert.That(reopened.CanRead).IsTrue();
    }

    [Test]
    public async Task MovesDirectoryTrees()
    {
        var provider = new InMemoryFileSystemProvider();
        var source = Path.Combine(provider.GetTempPath(), "source");
        var child = Path.Combine(source, "child");
        var destination = Path.Combine(provider.GetTempPath(), "destination");
        var sourceFile = Path.Combine(child, "artifact.txt");
        var destinationFile = Path.Combine(destination, "child", "artifact.txt");

        provider.CreateDirectory(child);
        await provider.WriteAllTextAsync(sourceFile, "artifact");
        provider.MoveDirectory(source, destination);

        using (Assert.Multiple())
        {
            await Assert.That(provider.DirectoryExists(source)).IsFalse();
            await Assert.That(provider.DirectoryExists(Path.Combine(destination, "child"))).IsTrue();
            await Assert.That(await provider.ReadAllTextAsync(destinationFile)).IsEqualTo("artifact");
        }
    }

    [Test]
    public async Task OpenOrCreateWithReadAccessCreatesAnEmptyFile()
    {
        var provider = new InMemoryFileSystemProvider();
        var path = Path.Combine(provider.GetTempPath(), "created-on-open.txt");

        using var stream = provider.Open(path, FileMode.OpenOrCreate, FileAccess.Read);

        using (Assert.Multiple())
        {
            await Assert.That(stream.Length).IsEqualTo(0);
            await Assert.That(provider.FileExists(path)).IsTrue();
        }
    }

    [Test]
    public async Task RejectsMovingDirectoryInsideItself()
    {
        var provider = new InMemoryFileSystemProvider();
        var source = Path.Combine(provider.GetTempPath(), "source");
        var destination = Path.Combine(source, "nested");
        provider.CreateDirectory(source);

        await Assert.That(() => provider.MoveDirectory(source, destination))
            .Throws<IOException>();
    }

    [Test]
    public async Task EnumeratesFilesUnderFileSystemRoot()
    {
        var provider = new InMemoryFileSystemProvider();
        var root = Path.GetPathRoot(Environment.CurrentDirectory)!;
        var path = Path.Combine(root, $"root-{Guid.NewGuid():N}.txt");
        await provider.WriteAllTextAsync(path, "contents");

        await Assert.That(provider.EnumerateFiles(root, "*.txt", SearchOption.AllDirectories))
            .Contains(path);
    }

    [Test]
    public async Task ConcurrentAppendsDoNotLoseWrites()
    {
        var provider = new InMemoryFileSystemProvider();
        var path = Path.Combine(provider.GetTempPath(), "concurrent.txt");

        await Task.WhenAll(Enumerable.Range(0, 100)
            .Select(_ => provider.AppendAllTextAsync(path, "x")));

        await Assert.That((await provider.ReadAllTextAsync(path)).Length).IsEqualTo(100);
    }

    [Test]
    public async Task RejectsFilesWithoutExistingParent()
    {
        var provider = new InMemoryFileSystemProvider();
        var path = Path.Combine(provider.GetTempPath(), "missing", "artifact.txt");

        await Assert.That(() => provider.WriteAllTextAsync(path, "contents"))
            .Throws<DirectoryNotFoundException>();
    }

    [Test]
    public async Task RejectsFileAndDirectoryPathCollisions()
    {
        var provider = new InMemoryFileSystemProvider();
        var filePath = Path.Combine(provider.GetTempPath(), "artifact");
        await provider.WriteAllTextAsync(filePath, "contents");
        var nestedDirectoryPath = Path.Combine(filePath, "child");

        using (Assert.Multiple())
        {
            await Assert.That(() => provider.CreateDirectory(nestedDirectoryPath))
                .Throws<IOException>();
            await Assert.That(provider.DirectoryExists(nestedDirectoryPath)).IsFalse();
        }

        var directoryPath = Path.Combine(provider.GetTempPath(), "directory");
        provider.CreateDirectory(directoryPath);

        await Assert.That(() => provider.WriteAllTextAsync(directoryPath, "contents"))
            .Throws<IOException>();
    }

    [Test]
    public async Task MoveDirectoryRequiresExistingDestinationParent()
    {
        var provider = new InMemoryFileSystemProvider();
        var source = Path.Combine(provider.GetTempPath(), "source");
        var destination = Path.Combine(provider.GetTempPath(), "missing", "destination");
        provider.CreateDirectory(source);

        await Assert.That(() => provider.MoveDirectory(source, destination))
            .Throws<DirectoryNotFoundException>();
    }

    [Test]
    public async Task EmptyLineSequenceWritesEmptyFile()
    {
        var provider = new InMemoryFileSystemProvider();
        var path = Path.Combine(provider.GetTempPath(), "empty.txt");

        await provider.WriteAllLinesAsync(path, []);

        await Assert.That(await provider.ReadAllTextAsync(path)).IsEmpty();
    }

    [Test]
    public async Task WriteOnlyStreamsRejectReads()
    {
        var provider = new InMemoryFileSystemProvider();
        var path = Path.Combine(provider.GetTempPath(), "write-only.txt");

        using var stream = provider.Open(path, FileMode.Create, FileAccess.Write);

        using (Assert.Multiple())
        {
            await Assert.That(() => stream.ReadByte()).Throws<NotSupportedException>();
            await Assert.That(async () => await stream.ReadAsync(new byte[1]))
                .Throws<NotSupportedException>();
        }
    }

    [Test]
    public async Task AppendStreamsRejectSeekingBeforeOriginalEnd()
    {
        var provider = new InMemoryFileSystemProvider();
        var path = Path.Combine(provider.GetTempPath(), "append.txt");
        await provider.WriteAllTextAsync(path, "original");

        using var stream = provider.Open(path, FileMode.Append, FileAccess.Write);

        await Assert.That(() => stream.Position = 0).Throws<IOException>();
    }

}
