using System.IO.Compression;
using System.Text;
using ModularPipelines.Context;
using ModularPipelines.FileSystem;
using Moq;

namespace ModularPipelines.UnitTests.Helpers;

public class AsyncZipTests
{
    private static readonly string Root = Path.Combine(Path.GetTempPath(), "virtual-async-zip");
    private static readonly byte[] Contents = Encoding.UTF8.GetBytes(new string('x', 4096));

    [Test]
    [Arguments(CompressionLevel.Optimal)]
    [Arguments(CompressionLevel.NoCompression)]
    public async Task Creates_And_Extracts_Through_Async_Provider_Streams(CompressionLevel compressionLevel)
    {
        var sourceDirectory = Path.Combine(Root, "source");
        var sourcePath = Path.Combine(sourceDirectory, "input.txt");
        var archivePath = Path.Combine(Root, "archive.zip");
        var archiveCreated = false;
        var input = new AsyncIoTestStream(new MemoryStream(Contents));
        // .NET 10 entry creation/finalization can write synchronously, even with OpenAsync.
        var output = new AsyncIoTestStream(new MemoryStream()) { AllowSynchronousArchiveWrites = true };
        var provider = new Mock<IFileSystemProvider>(MockBehavior.Strict);
        provider.Setup(fileSystem => fileSystem.DirectoryExists(archivePath)).Returns(false);
        provider.Setup(fileSystem => fileSystem.FileExists(archivePath)).Returns(() => archiveCreated);
        provider.Setup(fileSystem => fileSystem.CreateDirectory(Root));
        provider.Setup(fileSystem => fileSystem.EnumerateDirectories(sourceDirectory, "*", SearchOption.AllDirectories))
            .Returns([Path.Combine(sourceDirectory, "empty")]);
        provider.Setup(fileSystem => fileSystem.EnumerateFiles(sourceDirectory, "*", SearchOption.AllDirectories))
            .Returns([sourcePath]);
        provider.Setup(fileSystem => fileSystem.GetRelativePath(sourceDirectory, It.IsAny<string>()))
            .Returns((string root, string path) => Path.GetRelativePath(root, path));
        provider.Setup(fileSystem => fileSystem.Open(archivePath, FileMode.CreateNew, FileAccess.ReadWrite))
            .Callback(() => archiveCreated = true).Returns(output);
        provider.Setup(fileSystem => fileSystem.OpenRead(sourcePath)).Returns(input);
        IZipContext zip = new Zip(provider.Object, new PipelineWorkingDirectory(Root));

        var created = await zip.CreateFromDirectoryAsync(new FolderPath(sourceDirectory), "archive.zip", compressionLevel);

        await Assert.That(created.Path).IsEqualTo(archivePath);
        await Assert.That(input.AsyncReads).IsGreaterThan(0);
        await Assert.That(output.AsyncWrites).IsGreaterThan(0);
        await Assert.That(input.DisposedAsynchronously).IsTrue();
        await Assert.That(output.DisposedAsynchronously).IsTrue();
        using (var archive = new ZipArchive(new MemoryStream(output.Bytes), ZipArchiveMode.Read))
        {
            await Assert.That(archive.Entries.Select(entry => entry.FullName)).IsEquivalentTo(["empty/", "input.txt"]);
            var entry = archive.GetEntry("input.txt")!;
            if (compressionLevel == CompressionLevel.NoCompression)
            {
                await Assert.That(entry.CompressedLength).IsEqualTo((long) Contents.Length);
            }
            else
            {
                await Assert.That(entry.CompressedLength).IsLessThan((long) Contents.Length);
            }
        }

        var extraction = CreateExtraction(output.Bytes);
        var extracted = await extraction.Zip.ExtractToDirectoryAsync("archive.zip", "destination", overwriteFiles: false);
        await Assert.That(extracted.Path).IsEqualTo(Path.Combine(Root, "destination"));
        await Assert.That(extraction.Output.Bytes).IsEquivalentTo(Contents);
        await Assert.That(extraction.Input.AsyncReads).IsGreaterThan(0);
        await Assert.That(extraction.Output.AsyncWrites).IsGreaterThan(0);
        await Assert.That(extraction.Input.DisposedAsynchronously).IsTrue();
        await Assert.That(extraction.Output.DisposedAsynchronously).IsTrue();
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Async_And_Synchronous_Operations_Interoperate(bool asyncCreation)
    {
        var root = Directory.CreateTempSubdirectory("async-zip-parity-");
        try
        {
            var source = Directory.CreateDirectory(Path.Combine(root.FullName, "source"));
            Directory.CreateDirectory(Path.Combine(source.FullName, "empty"));
            await File.WriteAllBytesAsync(Path.Combine(source.FullName, "input.txt"), Contents);
            IZipContext zip = new Zip(SystemFileSystemProvider.Instance, new PipelineWorkingDirectory(root.FullName));
            if (asyncCreation)
            {
                await zip.CreateFromDirectoryAsync(new FolderPath(source.FullName), "archive.zip", CompressionLevel.Fastest);
                zip.ExtractToDirectory("archive.zip", "destination");
            }
            else
            {
                zip.CreateFromDirectory(new FolderPath(source.FullName), "archive.zip", CompressionLevel.Fastest);
                await zip.ExtractToDirectoryAsync("archive.zip", "destination");
            }

            var destination = Path.Combine(root.FullName, "destination");
            await Assert.That(await File.ReadAllBytesAsync(Path.Combine(destination, "input.txt"))).IsEquivalentTo(Contents);
            await Assert.That(Directory.Exists(Path.Combine(destination, "empty"))).IsTrue();
            await Assert.That(() => zip.CreateFromDirectory(new FolderPath(source.FullName), "archive.zip")).Throws<IOException>();
            await Assert.ThrowsAsync<IOException>(() => zip.CreateFromDirectoryAsync(new FolderPath(source.FullName), "archive.zip"));
            await Assert.ThrowsAsync<IOException>(() => zip.ExtractToDirectoryAsync("archive.zip", "destination", overwriteFiles: false));
            await File.WriteAllTextAsync(Path.Combine(destination, "input.txt"), "old content");
            await zip.ExtractToDirectoryAsync("archive.zip", "destination", overwriteFiles: true);
            await Assert.That(await File.ReadAllBytesAsync(Path.Combine(destination, "input.txt"))).IsEquivalentTo(Contents);
        }
        finally
        {
            root.Delete(recursive: true);
        }
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Creation_Read_Failure_Or_Cancellation_Disposes_Streams(bool cancel)
    {
        using var cancellation = new CancellationTokenSource();
        var input = new AsyncIoTestStream(new MemoryStream(Contents))
        {
            BeforeRead = token =>
            {
                if (cancel)
                {
                    cancellation.Cancel();
                    token.ThrowIfCancellationRequested();
                }

                throw new IOException("Provider read failed.");
            },
        };
        var output = new AsyncIoTestStream(new MemoryStream()) { AllowSynchronousArchiveWrites = true };
        var sourceDirectory = Path.Combine(Root, "source");
        var sourcePath = Path.Combine(sourceDirectory, "input.txt");
        var archivePath = Path.Combine(Root, "archive.zip");
        var provider = new Mock<IFileSystemProvider>(MockBehavior.Strict);
        provider.Setup(fileSystem => fileSystem.DirectoryExists(archivePath)).Returns(false);
        provider.Setup(fileSystem => fileSystem.FileExists(archivePath)).Returns(false);
        provider.Setup(fileSystem => fileSystem.CreateDirectory(Root));
        provider.Setup(fileSystem => fileSystem.EnumerateDirectories(sourceDirectory, "*", SearchOption.AllDirectories)).Returns([]);
        provider.Setup(fileSystem => fileSystem.EnumerateFiles(sourceDirectory, "*", SearchOption.AllDirectories)).Returns([sourcePath]);
        provider.Setup(fileSystem => fileSystem.GetRelativePath(sourceDirectory, sourcePath)).Returns("input.txt");
        provider.Setup(fileSystem => fileSystem.Open(archivePath, FileMode.CreateNew, FileAccess.ReadWrite)).Returns(output);
        provider.Setup(fileSystem => fileSystem.OpenRead(sourcePath)).Returns(input);
        IZipContext zip = new Zip(provider.Object, new PipelineWorkingDirectory(Root));

        if (cancel)
        {
            await Assert.ThrowsAsync<OperationCanceledException>(() => zip.CreateFromDirectoryAsync(
                new FolderPath(sourceDirectory), "archive.zip", cancellationToken: cancellation.Token));
        }
        else
        {
            var exception = await Assert.ThrowsAsync<IOException>(() => zip.CreateFromDirectoryAsync(new FolderPath(sourceDirectory), "archive.zip"));
            await Assert.That(exception!.Message).IsEqualTo("Provider read failed.");
        }

        await Assert.That(input.DisposedAsynchronously).IsTrue();
        await Assert.That(output.DisposedAsynchronously).IsTrue();
    }

    [Test]
    public async Task Directory_Destinations_And_Extensionless_File_Collisions_Match_Synchronous_Behavior()
    {
        var root = Directory.CreateTempSubdirectory("async-zip-destinations-");
        try
        {
            var source = Directory.CreateDirectory(Path.Combine(root.FullName, "source"));
            Directory.CreateDirectory(Path.Combine(root.FullName, "existing.folder"));
            IZipContext zip = new Zip(SystemFileSystemProvider.Instance, new PipelineWorkingDirectory(root.FullName));
            foreach (var destination in new[] { "existing.folder", "new-folder" })
            {
                var archive = await zip.CreateFromDirectoryAsync(new FolderPath(source.FullName), destination);
                await Assert.That(Path.GetDirectoryName(archive.Path)).IsEqualTo(Path.Combine(root.FullName, destination));
                await Assert.That(Path.GetExtension(archive.Path)).IsEqualTo(".zip");
                using var reader = ZipFile.OpenRead(archive.Path);
                await Assert.That(reader.Entries.Count).IsEqualTo(0);
            }

            var existingFile = Path.Combine(root.FullName, "existing-file");
            await File.WriteAllTextAsync(existingFile, "preserve");
            await Assert.ThrowsAsync<IOException>(() => zip.CreateFromDirectoryAsync(new FolderPath(source.FullName), "existing-file"));
            await Assert.That(await File.ReadAllTextAsync(existingFile)).IsEqualTo("preserve");
        }
        finally
        {
            root.Delete(recursive: true);
        }
    }

    [Test]
    public async Task Precancelled_Operations_Do_Not_Access_Provider()
    {
        var provider = new Mock<IFileSystemProvider>(MockBehavior.Strict);
        IZipContext zip = new Zip(provider.Object, new PipelineWorkingDirectory(Root));
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();

        await Assert.ThrowsAsync<OperationCanceledException>(() =>
            zip.CreateFromDirectoryAsync(new FolderPath(Root), "archive.zip", cancellationToken: cancellation.Token));
        await Assert.ThrowsAsync<OperationCanceledException>(() =>
            zip.ExtractToDirectoryAsync("archive.zip", "destination", cancellationToken: cancellation.Token));
        provider.VerifyNoOtherCalls();
    }

    [Test]
    [Arguments("../escape.txt")]
    [Arguments("nested/../../escape.txt")]
    [Arguments("../destination-sibling/escape.txt")]
    public async Task Rejects_Traversal_Before_Opening_Destination(string entryName)
    {
        var extraction = CreateExtraction(CreateArchive(entryName));
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            extraction.Zip.ExtractToDirectoryAsync("archive.zip", "destination"));
        await Assert.That(exception!.Message).Contains("would extract outside the target directory");
        extraction.Provider.Verify(fileSystem => fileSystem.Open(
            It.IsAny<string>(), It.IsAny<FileMode>(), It.IsAny<FileAccess>()), Times.Never);
    }

    [Test]
    public async Task Corrupt_Archive_Preserves_Diagnostic_And_Disposes_Stream()
    {
        var extraction = CreateExtraction(Encoding.UTF8.GetBytes("not a zip archive"));
        var exception = await Assert.ThrowsAsync<InvalidDataException>(() =>
            extraction.Zip.ExtractToDirectoryAsync("archive.zip", "destination"));
        await Assert.That(exception!.Message).Contains("corrupt or not a valid zip file");
        await Assert.That(extraction.Input.DisposedAsynchronously).IsTrue();
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Extraction_Write_Failure_Or_Cancellation_Disposes_Streams(bool cancel)
    {
        using var cancellation = new CancellationTokenSource();
        var extraction = CreateExtraction(CreateArchive("input.txt"), token =>
        {
            if (cancel)
            {
                cancellation.Cancel();
                token.ThrowIfCancellationRequested();
            }

            throw new IOException("Provider write failed.");
        });
        if (cancel)
        {
            await Assert.ThrowsAsync<OperationCanceledException>(() => extraction.Zip.ExtractToDirectoryAsync(
                "archive.zip", "destination", cancellationToken: cancellation.Token));
        }
        else
        {
            var exception = await Assert.ThrowsAsync<IOException>(() =>
                extraction.Zip.ExtractToDirectoryAsync("archive.zip", "destination"));
            await Assert.That(exception!.InnerException!.Message).IsEqualTo("Provider write failed.");
        }

        await Assert.That(extraction.Input.DisposedAsynchronously).IsTrue();
        await Assert.That(extraction.Output.DisposedAsynchronously).IsTrue();
    }

    private static byte[] CreateArchive(string entryName)
    {
        using var bytes = new MemoryStream();
        using (var archive = new ZipArchive(bytes, ZipArchiveMode.Create, leaveOpen: true))
        {
            using var entry = archive.CreateEntry(entryName).Open();
            entry.Write(Contents);
        }

        return bytes.ToArray();
    }

    private static Extraction CreateExtraction(byte[] archiveBytes, Action<CancellationToken>? beforeWrite = null)
    {
        var archivePath = Path.Combine(Root, "archive.zip");
        var input = new AsyncIoTestStream(new MemoryStream(archiveBytes)) { AllowSynchronousMetadataReads = true };
        var output = new AsyncIoTestStream(new MemoryStream()) { BeforeWrite = beforeWrite };
        var provider = new Mock<IFileSystemProvider>(MockBehavior.Strict);
        provider.Setup(fileSystem => fileSystem.FileExists(It.IsAny<string>())).Returns(false);
        provider.Setup(fileSystem => fileSystem.FileExists(archivePath)).Returns(true);
        provider.Setup(fileSystem => fileSystem.OpenRead(archivePath)).Returns(input);
        provider.Setup(fileSystem => fileSystem.CreateDirectory(It.IsAny<string>()));
        provider.Setup(fileSystem => fileSystem.Open(
                Path.Combine(Root, "destination", "input.txt"), It.IsAny<FileMode>(), FileAccess.Write))
            .Returns(output);
        return new Extraction(new Zip(provider.Object, new PipelineWorkingDirectory(Root)), provider, input, output);
    }

    private sealed record Extraction(IZipContext Zip, Mock<IFileSystemProvider> Provider, AsyncIoTestStream Input, AsyncIoTestStream Output);
}
