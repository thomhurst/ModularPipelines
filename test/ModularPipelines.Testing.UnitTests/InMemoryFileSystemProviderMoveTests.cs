using ModularPipelines.FileSystem;

namespace ModularPipelines.Testing.UnitTests;

public class InMemoryFileSystemProviderMoveTests
{
    [Test]
    public async Task InMemoryProviderUsesHostSeparatorByDefault()
    {
        IFileSystemProvider provider = new InMemoryFileSystemProvider();
        await Assert.That(provider.DirectorySeparatorChar).IsEqualTo(Path.DirectorySeparatorChar);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task OverwriteMovePreservesSourceMetadata(bool destinationExists)
    {
        IFileSystemProvider provider = new InMemoryFileSystemProvider();
        var source = Path.Combine(provider.GetTempPath(), "source.txt");
        var destination = Path.Combine(provider.GetTempPath(), "destination.txt");
        await provider.WriteAllTextAsync(source, "new");
        if (destinationExists)
        {
            await provider.WriteAllTextAsync(destination, "old");
        }

        var timestamp = new DateTime(2020, 1, 2, 3, 4, 5, DateTimeKind.Utc);
        provider.SetAttributes(source, FileAttributes.Hidden | FileAttributes.ReadOnly);
        provider.SetCreationTimeUtc(source, timestamp);
        provider.SetLastWriteTimeUtc(source, timestamp.AddDays(1));
        provider.SetLastAccessTimeUtc(source, timestamp.AddDays(2));

        provider.MoveFile(source, destination, overwrite: true);

        await Assert.That(provider.FileExists(source)).IsFalse();
        await Assert.That(await provider.ReadAllTextAsync(destination)).IsEqualTo("new");
        await Assert.That(provider.GetAttributes(destination)).IsEqualTo(FileAttributes.Hidden | FileAttributes.ReadOnly);
        await Assert.That(provider.GetCreationTimeUtc(destination)).IsEqualTo(timestamp);
        await Assert.That(provider.GetLastWriteTimeUtc(destination)).IsEqualTo(timestamp.AddDays(1));
        await Assert.That(provider.GetLastAccessTimeUtc(destination)).IsEqualTo(timestamp.AddDays(2));
        await Assert.That(() => provider.GetAttributes(source)).Throws<FileNotFoundException>();
    }

    [Test]
    [Arguments(false, false)]
    [Arguments(false, true)]
    [Arguments(true, false)]
    [Arguments(true, true)]
    public async Task OverwriteMoveRejectsOpenFilesWithoutChangingEitherFile(bool openSource, bool readOnly)
    {
        IFileSystemProvider provider = new InMemoryFileSystemProvider();
        var source = Path.Combine(provider.GetTempPath(), "source.txt");
        var destination = Path.Combine(provider.GetTempPath(), "destination.txt");
        await provider.WriteAllTextAsync(source, "new");
        await provider.WriteAllTextAsync(destination, "old");
        var timestamp = new DateTime(2020, 1, 2, 3, 4, 5, DateTimeKind.Utc);
        provider.SetLastWriteTimeUtc(source, timestamp);
        provider.SetLastWriteTimeUtc(destination, timestamp.AddDays(1));
        var openPath = openSource ? source : destination;
        using (readOnly ? provider.OpenRead(openPath) : provider.Open(openPath, FileMode.Open, FileAccess.ReadWrite))
        {
            await Assert.That(() => provider.MoveFile(source, destination, overwrite: true)).Throws<IOException>();
            await Assert.That(provider.GetLastWriteTimeUtc(source)).IsEqualTo(timestamp);
            await Assert.That(provider.GetLastWriteTimeUtc(destination)).IsEqualTo(timestamp.AddDays(1));
        }

        await Assert.That(await provider.ReadAllTextAsync(source)).IsEqualTo("new");
        await Assert.That(await provider.ReadAllTextAsync(destination)).IsEqualTo("old");

        provider.MoveFile(source, destination, overwrite: true);
        await Assert.That(await provider.ReadAllTextAsync(destination)).IsEqualTo("new");
    }

    [Test]
    public async Task OverwriteMoveToSameNormalizedPathIsNoOp()
    {
        IFileSystemProvider provider = new InMemoryFileSystemProvider();
        var source = Path.Combine(provider.GetTempPath(), "source.txt");
        await provider.WriteAllTextAsync(source, "contents");
        var timestamp = new DateTime(2020, 1, 2, 3, 4, 5, DateTimeKind.Utc);
        provider.SetLastWriteTimeUtc(source, timestamp);

        provider.MoveFile(source, Path.Combine(provider.GetTempPath(), ".", "source.txt"), overwrite: true);

        await Assert.That(await provider.ReadAllTextAsync(source)).IsEqualTo("contents");
        await Assert.That(provider.GetLastWriteTimeUtc(source)).IsEqualTo(timestamp);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task NonOverwriteMoveRejectsExistingDestination(bool useLegacyOverload)
    {
        var provider = new InMemoryFileSystemProvider();
        var source = Path.Combine(provider.GetTempPath(), "source.txt");
        var destination = Path.Combine(provider.GetTempPath(), "destination.txt");
        await provider.WriteAllTextAsync(source, "new");
        await provider.WriteAllTextAsync(destination, "old");

        void Move()
        {
            if (useLegacyOverload)
            {
                provider.MoveFile(source, destination);
            }
            else
            {
                ((IFileSystemProvider) provider).MoveFile(source, destination, overwrite: false);
            }
        }

        await Assert.That(Move).Throws<IOException>();
        await Assert.That(await provider.ReadAllTextAsync(source)).IsEqualTo("new");
        await Assert.That(await provider.ReadAllTextAsync(destination)).IsEqualTo("old");
    }

    [Test]
    public async Task OverwriteMoveWithMissingSourcePreservesDestination()
    {
        IFileSystemProvider provider = new InMemoryFileSystemProvider();
        var source = Path.Combine(provider.GetTempPath(), "missing.txt");
        var destination = Path.Combine(provider.GetTempPath(), "destination.txt");
        await provider.WriteAllTextAsync(destination, "old");

        await Assert.That(() => provider.MoveFile(source, destination, overwrite: true)).Throws<FileNotFoundException>();

        await Assert.That(await provider.ReadAllTextAsync(destination)).IsEqualTo("old");
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task OverwriteMoveToInvalidDestinationPreservesSource(bool destinationIsDirectory)
    {
        IFileSystemProvider provider = new InMemoryFileSystemProvider();
        var source = Path.Combine(provider.GetTempPath(), "source.txt");
        var directory = Path.Combine(provider.GetTempPath(), "directory");
        await provider.WriteAllTextAsync(source, "new");
        provider.CreateDirectory(directory);
        var destination = destinationIsDirectory ? directory : Path.Combine(directory, "missing", "file.txt");

        await Assert.That(() => provider.MoveFile(source, destination, overwrite: true)).Throws<IOException>();

        await Assert.That(await provider.ReadAllTextAsync(source)).IsEqualTo("new");
        await Assert.That(provider.DirectoryExists(directory)).IsTrue();
        await Assert.That(provider.FileExists(destination)).IsFalse();
    }
}
