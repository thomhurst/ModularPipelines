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
}
