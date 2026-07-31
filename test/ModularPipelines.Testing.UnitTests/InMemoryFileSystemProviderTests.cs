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
    public async Task AppendAllTextPreservesExistingBytes()
    {
        var provider = new InMemoryFileSystemProvider();
        var path = Path.Combine(provider.GetTempPath(), "binary-prefix.txt");
        await provider.WriteAllBytesAsync(path, [0xFF, 0xFE]);

        await provider.AppendAllTextAsync(path, "x");

        var bytes = await provider.ReadAllBytesAsync(path);
        byte[] expected = [0xFF, 0xFE, (byte) 'x'];
        await Assert.That(bytes.SequenceEqual(expected)).IsTrue();
    }

    [Test]
    public async Task ReadAllTextDetectsByteOrderMarks()
    {
        Encoding[] encodings =
        [
            new UTF8Encoding(encoderShouldEmitUTF8Identifier: true),
            Encoding.Unicode,
            Encoding.BigEndianUnicode,
            new UTF32Encoding(bigEndian: false, byteOrderMark: true),
            new UTF32Encoding(bigEndian: true, byteOrderMark: true),
        ];

        var provider = new InMemoryFileSystemProvider();
        foreach (var encoding in encodings)
        {
            var path = Path.Combine(provider.GetTempPath(), $"{encoding.CodePage}.txt");
            var contents = encoding.GetPreamble()
                .Concat(encoding.GetBytes("contents"))
                .ToArray();
            await provider.WriteAllBytesAsync(path, contents);

            await Assert.That(await provider.ReadAllTextAsync(path)).IsEqualTo("contents");
        }
    }

    [Test]
    public async Task DeleteFileRejectsDirectoryPaths()
    {
        var provider = new InMemoryFileSystemProvider();
        var path = Path.Combine(provider.GetTempPath(), "directory-as-file");
        provider.CreateDirectory(path);

        await Assert.That(() => provider.DeleteFile(path))
            .Throws<UnauthorizedAccessException>();
        await Assert.That(provider.DirectoryExists(path)).IsTrue();
    }

    [Test]
    public async Task RejectsCopyingFileOntoItself()
    {
        var provider = new InMemoryFileSystemProvider();
        var path = Path.Combine(provider.GetTempPath(), "same-file.txt");
        await provider.WriteAllTextAsync(path, "contents");

        await Assert.That(() => provider.CopyFile(path, path, overwrite: true))
            .Throws<IOException>();
        await Assert.That(await provider.ReadAllTextAsync(path)).IsEqualTo("contents");
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
    public async Task OpenReadAllowsSharedReadHandles()
    {
        var provider = new InMemoryFileSystemProvider();
        var path = Path.Combine(provider.GetTempPath(), "shared-read.txt");
        await provider.WriteAllTextAsync(path, "contents");

        using var first = provider.OpenRead(path);
        using var second = provider.OpenRead(path);

        using (Assert.Multiple())
        {
            await Assert.That(first.CanRead).IsTrue();
            await Assert.That(second.CanRead).IsTrue();
        }
    }

    [Test]
    public async Task GenericOpenEnforcesExclusiveHandles()
    {
        var provider = new InMemoryFileSystemProvider();
        var path = Path.Combine(provider.GetTempPath(), "exclusive.txt");
        await provider.WriteAllTextAsync(path, "contents");

        using var stream = provider.Open(path, FileMode.Open, FileAccess.Read);
        await Assert.That(() => provider.OpenRead(path)).Throws<IOException>();
    }

    [Test]
    public async Task GenericOpenRejectsDirectReadsAndCopies()
    {
        var provider = new InMemoryFileSystemProvider();
        var path = Path.Combine(provider.GetTempPath(), "exclusive-direct-read.txt");
        var copy = Path.Combine(provider.GetTempPath(), "exclusive-direct-read-copy.txt");
        await provider.WriteAllTextAsync(path, "contents");

        using var stream = provider.Open(path, FileMode.Open, FileAccess.Read);

        async Task ReadTextAsync() => _ = await provider.ReadAllTextAsync(path);
        async Task ReadBytesAsync() => _ = await provider.ReadAllBytesAsync(path);

        await Assert.That(ReadTextAsync).Throws<IOException>();
        await Assert.That(ReadBytesAsync).Throws<IOException>();
        await Assert.That(() => provider.CopyFile(path, copy, overwrite: false))
            .Throws<IOException>();
    }

    [Test]
    public async Task MoveFileRejectsOpenSourceHandles()
    {
        var provider = new InMemoryFileSystemProvider();
        var source = Path.Combine(provider.GetTempPath(), "open-move-source.txt");
        var destination = Path.Combine(provider.GetTempPath(), "open-move-destination.txt");
        await provider.WriteAllTextAsync(source, "original");

        await using (var stream = provider.Open(source, FileMode.Open, FileAccess.ReadWrite))
        {
            stream.SetLength(0);
            await stream.WriteAsync(Encoding.UTF8.GetBytes("updated"));

            await Assert.That(() => provider.MoveFile(source, destination))
                .Throws<IOException>();
            using (Assert.Multiple())
            {
                await Assert.That(provider.FileExists(source)).IsTrue();
                await Assert.That(provider.FileExists(destination)).IsFalse();
            }
        }

        provider.MoveFile(source, destination);
        using (Assert.Multiple())
        {
            await Assert.That(provider.FileExists(source)).IsFalse();
            await Assert.That(await provider.ReadAllTextAsync(destination)).IsEqualTo("updated");
        }
    }

    [Test]
    public async Task DeleteFileRejectsOpenWritableHandles()
    {
        var provider = new InMemoryFileSystemProvider();
        var path = Path.Combine(provider.GetTempPath(), "open-delete.txt");
        await provider.WriteAllTextAsync(path, "original");

        await using (var stream = provider.Open(path, FileMode.Open, FileAccess.ReadWrite))
        {
            stream.SetLength(0);
            await stream.WriteAsync(Encoding.UTF8.GetBytes("updated"));

            await Assert.That(() => provider.DeleteFile(path)).Throws<IOException>();
            await Assert.That(provider.FileExists(path)).IsTrue();
        }

        await Assert.That(await provider.ReadAllTextAsync(path)).IsEqualTo("updated");

        provider.DeleteFile(path);
        await Assert.That(provider.FileExists(path)).IsFalse();
    }

    [Test]
    public async Task DeleteDirectoryRejectsOpenDescendantHandles()
    {
        var provider = new InMemoryFileSystemProvider();
        var root = Path.Combine(provider.GetTempPath(), "open-delete-directory");
        var path = Path.Combine(root, "child", "artifact.txt");
        provider.CreateDirectory(Path.GetDirectoryName(path)!);
        await provider.WriteAllTextAsync(path, "original");

        await using (var stream = provider.Open(path, FileMode.Open, FileAccess.ReadWrite))
        {
            stream.SetLength(0);
            await stream.WriteAsync(Encoding.UTF8.GetBytes("updated"));

            await Assert.That(() => provider.DeleteDirectory(root, recursive: true))
                .Throws<IOException>();
            using (Assert.Multiple())
            {
                await Assert.That(provider.DirectoryExists(root)).IsTrue();
                await Assert.That(provider.FileExists(path)).IsTrue();
            }
        }

        await Assert.That(await provider.ReadAllTextAsync(path)).IsEqualTo("updated");
        provider.DeleteDirectory(root, recursive: true);
        await Assert.That(provider.DirectoryExists(root)).IsFalse();
    }

    [Test]
    public async Task OpenHandlesRejectDirectWrites()
    {
        var provider = new InMemoryFileSystemProvider();
        var path = Path.Combine(provider.GetTempPath(), "direct-write.txt");
        await provider.WriteAllTextAsync(path, "original");

        using (provider.OpenRead(path))
        {
            await Assert.That(() => provider.WriteAllTextAsync(path, "replacement"))
                .Throws<IOException>();
            await Assert.That(() => provider.AppendAllTextAsync(path, " appended"))
                .Throws<IOException>();
        }

        await Assert.That(await provider.ReadAllTextAsync(path)).IsEqualTo("original");
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
    public async Task MoveDirectoryRejectsOpenDescendantHandlesAtomically()
    {
        var provider = new InMemoryFileSystemProvider();
        var source = Path.Combine(provider.GetTempPath(), "open-move-directory");
        var destination = Path.Combine(provider.GetTempPath(), "moved-directory");
        var firstFile = Path.Combine(source, "first.txt");
        var openFile = Path.Combine(source, "second.txt");
        provider.CreateDirectory(source);
        await provider.WriteAllTextAsync(firstFile, "first");
        await provider.WriteAllTextAsync(openFile, "second");

        await using (provider.Open(openFile, FileMode.Open, FileAccess.ReadWrite))
        {
            await Assert.That(() => provider.MoveDirectory(source, destination))
                .Throws<IOException>();
            using (Assert.Multiple())
            {
                await Assert.That(provider.DirectoryExists(source)).IsTrue();
                await Assert.That(provider.DirectoryExists(destination)).IsFalse();
                await Assert.That(provider.FileExists(firstFile)).IsTrue();
                await Assert.That(provider.FileExists(openFile)).IsTrue();
            }
        }

        provider.MoveDirectory(source, destination);
        using (Assert.Multiple())
        {
            await Assert.That(provider.DirectoryExists(source)).IsFalse();
            await Assert.That(provider.FileExists(Path.Combine(destination, "first.txt"))).IsTrue();
            await Assert.That(provider.FileExists(Path.Combine(destination, "second.txt"))).IsTrue();
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
