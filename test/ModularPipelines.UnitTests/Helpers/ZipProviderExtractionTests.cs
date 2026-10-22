using System.IO.Compression;
using ModularPipelines.Context;
using ModularPipelines.FileSystem;
using Moq;

namespace ModularPipelines.UnitTests.Helpers;

public class ZipProviderExtractionTests
{
    [Test]
    [MatrixDataSource]
    public async Task FailedOpenRollsBackProviderOwnedCreationAndCanBeRetried(
        [Matrix(false, true)] bool asynchronous,
        [Matrix(false, true)] bool overwriteExisting)
    {
        using var fixture = new ExtractionFixture(overwriteExisting);
        var failure = new IOException("Stream initialization failed.");
        fixture.SetDestinationStream((path, mode, access) =>
        {
            var stream = File.Open(path, mode, access);
            try
            {
                // Simulate a provider failing after exclusive creation, before returning its stream.
                stream.WriteByte(42);
                throw failure;
            }
            catch
            {
                stream.Dispose();
                File.Delete(path);
                throw;
            }
        });

        var exception = await Assert.ThrowsAsync<IOException>(() => fixture.Extract(asynchronous, overwriteExisting));
        await Assert.That(ReferenceEquals(exception!.InnerException, failure)).IsTrue();
        await fixture.AssertPreserved(overwriteExisting);
        fixture.Provider.Verify(provider => provider.DeleteFile(It.IsAny<string>()), Times.Never);
        fixture.Provider.Verify(provider => provider.MoveFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Never);

        fixture.SetDestinationStream(File.Open);
        await fixture.Extract(asynchronous, overwriteExisting);
        await fixture.AssertPublished();
    }

    [Test]
    [MatrixDataSource]
    public async Task FailedOpenPreservesCompetingTemporaryFile(
        [Matrix(false, true)] bool asynchronous,
        [Matrix(false, true)] bool overwriteExisting)
    {
        using var fixture = new ExtractionFixture(overwriteExisting);
        string? competingPath = null;
        fixture.SetDestinationStream((path, mode, access) =>
        {
            competingPath = path;
            File.WriteAllText(path, "competing contents");
            // A real CreateNew collision must not grant the caller ownership of this path.
            return File.Open(path, mode, access);
        });

        await Assert.ThrowsAsync<IOException>(() => fixture.Extract(asynchronous, overwriteExisting));
        await Assert.That(File.ReadAllText(competingPath!)).IsEqualTo("competing contents");
        fixture.Provider.Verify(provider => provider.DeleteFile(It.IsAny<string>()), Times.Never);
        fixture.Provider.Verify(provider => provider.MoveFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Never);

        File.Delete(competingPath!);
        await fixture.AssertPreserved(overwriteExisting);
        fixture.SetDestinationStream(File.Open);
        await fixture.Extract(asynchronous, overwriteExisting);
        await fixture.AssertPublished();
    }

    [Test]
    [MatrixDataSource]
    public async Task FailedExtractionPreservesDestinationAndCanBeRetried(
        [Matrix(false, true)] bool asynchronous,
        [Matrix(false, true)] bool overwriteExisting,
        [Matrix("corrupt", "write", "dispose", "publish")] string failure)
    {
        using var fixture = new ExtractionFixture(overwriteExisting);
        var validArchive = File.ReadAllBytes(fixture.ArchivePath);
        if (failure == "corrupt")
        {
            using var stream = File.Open(fixture.ArchivePath, FileMode.Open, FileAccess.ReadWrite);
            using var reader = new BinaryReader(stream);
            stream.Position = 26;
            var nameLength = reader.ReadUInt16();
            var extraLength = reader.ReadUInt16();
            stream.Position = 30 + nameLength + extraLength;
            // Invalid DEFLATE block type, leaving ZIP headers intact.
            stream.WriteByte(0x07);
        }
        else if (failure == "publish")
        {
            fixture.Provider.Setup(provider => provider.MoveFile(It.IsAny<string>(), fixture.DestinationPath, overwriteExisting))
                .Throws(new IOException("Publication failed."));
        }
        else
        {
            fixture.SetDestinationStream((path, mode, access) => new FailingStream(path, mode, access)
            {
                AfterWrite = failure == "write" ? () => throw new IOException("Write failed.") : null,
                AfterDispose = failure == "dispose" ? () => throw new IOException("Dispose failed.") : null,
            });
        }

        if (failure == "corrupt")
        {
            await Assert.ThrowsAsync<InvalidDataException>(() => fixture.Extract(asynchronous, overwriteExisting));
        }
        else
        {
            await Assert.ThrowsAsync<IOException>(() => fixture.Extract(asynchronous, overwriteExisting));
        }

        await fixture.AssertPreserved(overwriteExisting);
        File.WriteAllBytes(fixture.ArchivePath, validArchive);
        fixture.SetDestinationStream(File.Open);
        fixture.SetPublication();
        await fixture.Extract(asynchronous, overwriteExisting);
        await fixture.AssertPublished();
    }

    [Test]
    [MatrixDataSource]
    public async Task CancellationPreservesDestinationAndCanBeRetried(
        [Matrix(false, true)] bool overwriteExisting,
        [Matrix(false, true)] bool duringDisposal)
    {
        using var fixture = new ExtractionFixture(overwriteExisting);
        using var cancellation = new CancellationTokenSource();
        fixture.SetDestinationStream((path, mode, access) => new FailingStream(path, mode, access)
        {
            AfterWrite = duringDisposal ? null : cancellation.Cancel,
            AfterDispose = duringDisposal ? cancellation.Cancel : null,
        });

        await Assert.ThrowsAsync<OperationCanceledException>(() => fixture.Extract(true, overwriteExisting, cancellation.Token));
        await fixture.AssertPreserved(overwriteExisting);
        fixture.Provider.Verify(provider => provider.MoveFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()), Times.Never);

        fixture.SetDestinationStream(File.Open);
        await fixture.Extract(true, overwriteExisting);
        await fixture.AssertPublished();
    }

    [Test]
    [MatrixDataSource]
    public async Task CollisionPreservesDestinationAndExplainsOverwriteOption(
        [Matrix(false, true)] bool asynchronous,
        [Matrix(false, true)] bool competingWriter)
    {
        using var fixture = new ExtractionFixture(!competingWriter);
        if (competingWriter)
        {
            fixture.Provider.Setup(provider => provider.MoveFile(It.IsAny<string>(), fixture.DestinationPath, false))
                .Callback((string source, string destination, bool overwrite) =>
                {
                    File.WriteAllText(destination, "old contents");
                    throw new IOException("Die Datei ist bereits vorhanden.");
                });
        }

        var exception = await Assert.ThrowsAsync<IOException>(() => fixture.Extract(asynchronous, false));
        await Assert.That(exception!.Message).Contains("Set overwriteFiles to true");
        await fixture.AssertPreserved(true);
        if (!competingWriter)
        {
            fixture.Provider.Verify(provider => provider.Open(It.IsAny<string>(), FileMode.CreateNew, FileAccess.Write), Times.Never);
        }

        fixture.SetPublication();
        await fixture.Extract(asynchronous, true);
        await fixture.AssertPublished();
    }

    [Test]
    [MatrixDataSource]
    public async Task DefaultMoveContractRejectsUnsupportedOverwriteWithoutLosingContents(
        [Matrix(false, true)] bool asynchronous,
        [Matrix(false, true)] bool overwriteExisting)
    {
        using var fixture = new ExtractionFixture(overwriteExisting);
        fixture.Provider.Setup(provider => provider.MoveFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>())).CallBase();
        fixture.Provider.Setup(provider => provider.MoveFile(It.IsAny<string>(), It.IsAny<string>()))
            .Callback((string source, string destination) => File.Move(source, destination));

        if (overwriteExisting)
        {
            await Assert.ThrowsAsync<NotSupportedException>(() => fixture.Extract(asynchronous, true));
            await fixture.AssertPreserved(true);
            fixture.Provider.Verify(provider => provider.MoveFile(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            File.Delete(fixture.DestinationPath);
        }

        await fixture.Extract(asynchronous, true);
        await fixture.AssertPublished();
    }

    private sealed class ExtractionFixture : IDisposable
    {
        private readonly DirectoryInfo _root = Directory.CreateTempSubdirectory("zip-provider-extraction-");
        public Mock<IFileSystemProvider> Provider { get; } = new(MockBehavior.Strict);
        public string ArchivePath { get; }
        public string DestinationPath { get; }
        private string DestinationDirectory => Path.GetDirectoryName(DestinationPath)!;
        private IZipContext Zip { get; }

        public ExtractionFixture(bool existingDestination)
        {
            ArchivePath = Path.Combine(_root.FullName, "archive.zip");
            DestinationPath = Path.Combine(_root.FullName, "destination", "nested", "input.txt");
            using (var archive = ZipFile.Open(ArchivePath, ZipArchiveMode.Create))
            {
                using var writer = new StreamWriter(archive.CreateEntry("nested/input.txt", CompressionLevel.Optimal).Open());
                writer.Write("new contents");
            }

            Directory.CreateDirectory(DestinationDirectory);
            if (existingDestination)
            {
                File.WriteAllText(DestinationPath, "old contents");
            }

            Provider.Setup(provider => provider.FileExists(It.IsAny<string>())).Returns((string path) => File.Exists(path));
            Provider.Setup(provider => provider.OpenRead(ArchivePath)).Returns(() => File.OpenRead(ArchivePath));
            Provider.Setup(provider => provider.GetAttributes(It.IsAny<string>())).Returns((string path) => File.GetAttributes(path));
            Provider.Setup(provider => provider.CreateDirectory(It.IsAny<string>())).Callback((string path) => Directory.CreateDirectory(path));
            Provider.Setup(provider => provider.DeleteFile(It.IsAny<string>())).Callback((string path) => File.Delete(path));
            SetDestinationStream(File.Open);
            SetPublication();
            Zip = new Zip(Provider.Object, new PipelineWorkingDirectory(_root.FullName));
        }

        public void SetDestinationStream(Func<string, FileMode, FileAccess, Stream> open) =>
            Provider.Setup(provider => provider.Open(It.IsAny<string>(), It.IsAny<FileMode>(), FileAccess.Write))
                .Returns((string path, FileMode mode, FileAccess access) => open(path, mode, access));

        public void SetPublication() =>
            Provider.Setup(provider => provider.MoveFile(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Callback((string source, string destination, bool overwrite) =>
                {
                    // Opening the temporary file also verifies its stream has been disposed before publication.
                    if (source == destination || Path.GetDirectoryName(source) != Path.GetDirectoryName(destination)
                        || File.ReadAllText(source) != "new contents")
                    {
                        throw new InvalidOperationException("Expected a completed temporary file in the destination directory.");
                    }

                    File.Move(source, destination, overwrite);
                });

        public async Task Extract(bool asynchronous, bool overwrite, CancellationToken cancellationToken = default)
        {
            if (asynchronous)
            {
                await Zip.ExtractToDirectoryAsync("archive.zip", "destination", overwrite, cancellationToken);
            }
            else
            {
                Zip.ExtractToDirectory("archive.zip", "destination", overwrite);
            }
        }

        public async Task AssertPreserved(bool existingDestination)
        {
            await Assert.That(File.Exists(DestinationPath)).IsEqualTo(existingDestination);
            if (existingDestination)
            {
                await Assert.That(File.ReadAllText(DestinationPath)).IsEqualTo("old contents");
                await Assert.That(Directory.GetFiles(DestinationDirectory)).IsEquivalentTo([DestinationPath]);
            }
            else
            {
                await Assert.That(Directory.GetFiles(DestinationDirectory)).IsEmpty();
            }
        }

        public async Task AssertPublished()
        {
            await Assert.That(File.ReadAllText(DestinationPath)).IsEqualTo("new contents");
            await Assert.That(Directory.GetFiles(DestinationDirectory)).IsEquivalentTo([DestinationPath]);
            Provider.Verify(provider => provider.Open(
                It.Is<string>(path => path != DestinationPath && Path.GetDirectoryName(path) == DestinationDirectory),
                FileMode.CreateNew, FileAccess.Write), Times.AtLeastOnce);
            Provider.Verify(provider => provider.Open(DestinationPath, It.IsAny<FileMode>(), It.IsAny<FileAccess>()), Times.Never);
        }

        public void Dispose() => _root.Delete(recursive: true);
    }

    private sealed class FailingStream(string path, FileMode mode, FileAccess access) : FileStream(path, mode, access)
    {
        public Action? AfterWrite { get; init; }
        public Action? AfterDispose { get; init; }

        public override void Write(byte[] buffer, int offset, int count)
        {
            base.Write(buffer, offset, count);
            AfterWrite?.Invoke();
        }

        public override void Write(ReadOnlySpan<byte> buffer)
        {
            base.Write(buffer);
            AfterWrite?.Invoke();
        }

        public override async ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
        {
            await base.WriteAsync(buffer, cancellationToken);
            AfterWrite?.Invoke();
        }

        public override async ValueTask DisposeAsync()
        {
            await base.DisposeAsync();
            AfterDispose?.Invoke();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                AfterDispose?.Invoke();
            }
        }
    }
}
