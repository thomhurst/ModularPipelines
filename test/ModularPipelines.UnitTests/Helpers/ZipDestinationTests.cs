using System.IO.Compression;
using System.Text;
using ModularPipelines.Context;
using ModularPipelines.FileSystem;
using Moq;

namespace ModularPipelines.UnitTests.Helpers;

public class ZipDestinationTests
{
    [Test]
    [Arguments(false, false)]
    [Arguments(false, true)]
    [Arguments(true, false)]
    [Arguments(true, true)]
    public async Task ExtractsToDirectoryWithTrailingSeparator(bool useAlternateSeparator, bool asynchronous)
    {
        var separator = useAlternateSeparator ? Path.AltDirectorySeparatorChar : Path.DirectorySeparatorChar;
        var destination = Path.Combine(Path.GetTempPath(), "zip-destination") + separator;
        var (zip, provider, output, archivePath) = CreateZip("artifact.txt", destination);

        await Extract(zip, archivePath, destination, asynchronous);

        await Assert.That(Encoding.UTF8.GetString(output.ToArray())).IsEqualTo("archive contents");
        provider.Verify(fileSystem => fileSystem.Open(
            Path.GetFullPath(Path.Combine(destination, "artifact.txt")),
            FileMode.CreateNew,
            FileAccess.Write), Times.Once);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task ExtractsToFileSystemRootThroughProvider(bool asynchronous)
    {
        var destination = Path.GetPathRoot(Path.GetFullPath(Path.GetTempPath()))!;
        var (zip, provider, output, archivePath) = CreateZip("artifact.txt", destination);

        await Extract(zip, archivePath, destination, asynchronous);

        await Assert.That(Encoding.UTF8.GetString(output.ToArray())).IsEqualTo("archive contents");
        provider.Verify(fileSystem => fileSystem.Open(
            Path.Combine(destination, "artifact.txt"),
            FileMode.CreateNew,
            FileAccess.Write), Times.Once);
    }

    [Test]
    [Arguments(false, false)]
    [Arguments(false, true)]
    [Arguments(true, false)]
    [Arguments(true, true)]
    public async Task AllowsEntryForTrustedRoot(bool asynchronous, bool trailingSeparator)
    {
        var destination = Path.Combine(Path.GetTempPath(), "zip-destination");
        if (trailingSeparator)
        {
            destination += Path.DirectorySeparatorChar;
        }

        var (zip, provider, _, archivePath) = CreateZip("./", destination);

        await Extract(zip, archivePath, destination, asynchronous);

        provider.Verify(fileSystem => fileSystem.GetAttributes(It.IsAny<string>()), Times.Never);
        provider.Verify(fileSystem => fileSystem.Open(
            It.IsAny<string>(), It.IsAny<FileMode>(), It.IsAny<FileAccess>()), Times.Never);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task AllowsMissingAttributesSentinel(bool asynchronous)
    {
        var destination = Path.Combine(Path.GetTempPath(), "zip-destination");
        var (zip, provider, output, archivePath) = CreateZip("artifact.txt", destination);
        provider.Setup(fileSystem => fileSystem.GetAttributes(It.IsAny<string>())).Returns((FileAttributes) (-1));

        await Extract(zip, archivePath, destination, asynchronous);

        await Assert.That(Encoding.UTF8.GetString(output.ToArray())).IsEqualTo("archive contents");
    }

    [Test]
    [Arguments("../escape.txt", false)]
    [Arguments("../escape.txt", true)]
    [Arguments("../zip-destination-sibling/escape.txt", false)]
    [Arguments("../zip-destination-sibling/escape.txt", true)]
    [Arguments("nested/../../escape.txt", false)]
    [Arguments("nested/../../escape.txt", true)]
    public async Task RejectsTraversal(string entryName, bool trailingSeparator)
    {
        var destination = Path.Combine(Path.GetTempPath(), "zip-destination");
        if (trailingSeparator)
        {
            destination += Path.DirectorySeparatorChar;
        }

        var (zip, provider, _, archivePath) = CreateZip(entryName, destination);

        await Assert.That(() => zip.ExtractToDirectory(archivePath, destination, overwriteFiles: false))
            .Throws<InvalidOperationException>();

        provider.Verify(fileSystem => fileSystem.Open(
            It.IsAny<string>(), It.IsAny<FileMode>(), It.IsAny<FileAccess>()), Times.Never);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task RejectsRootedEntryOutsideDestination(bool trailingSeparator)
    {
        var destination = Path.Combine(Path.GetTempPath(), "zip-destination");
        if (trailingSeparator)
        {
            destination += Path.DirectorySeparatorChar;
        }

        var entryName = Path.Combine(Path.GetTempPath(), "escape.txt");
        var (zip, provider, _, archivePath) = CreateZip(entryName, destination);

        await Assert.That(() => zip.ExtractToDirectory(archivePath, destination, overwriteFiles: false))
            .Throws<InvalidOperationException>();

        provider.Verify(fileSystem => fileSystem.Open(
            It.IsAny<string>(), It.IsAny<FileMode>(), It.IsAny<FileAccess>()), Times.Never);
    }

    [Test]
    [Arguments(false, "ordinary/link/missing/payload.txt")]
    [Arguments(true, "ordinary/link/missing/payload.txt")]
    [Arguments(false, "ordinary/link/missing/")]
    [Arguments(true, "ordinary/link/missing/")]
    [Arguments(false, "ordinary/link/")]
    [Arguments(true, "ordinary/link/")]
    [Arguments(false, "ordinary/link")]
    [Arguments(true, "ordinary/link")]
    public async Task RejectsProviderLinksBeforeCreatingEntryDirectories(bool asynchronous, string entryName)
    {
        var destination = Path.Combine(Path.GetTempPath(), "virtual-zip-destination");
        var parent = Path.Combine(destination, "ordinary");
        var link = Path.Combine(parent, "link");
        var (zip, provider, _, archivePath) = CreateZip(entryName, destination);
        provider.Setup(fileSystem => fileSystem.GetAttributes(parent)).Returns(FileAttributes.Directory);
        // Existence probes return false, as they can for dangling links. Attributes are authoritative.
        provider.Setup(fileSystem => fileSystem.GetAttributes(link)).Returns(FileAttributes.ReparsePoint);

        if (asynchronous)
        {
            await Assert.That(() => (Task) zip.ExtractToDirectoryAsync(archivePath, destination, overwriteFiles: true))
                .Throws<InvalidOperationException>();
        }
        else
        {
            await Assert.That(() => zip.ExtractToDirectory(archivePath, destination, overwriteFiles: true))
                .Throws<InvalidOperationException>();
        }

        provider.Verify(fileSystem => fileSystem.GetAttributes(link), Times.Once);
        provider.Verify(fileSystem => fileSystem.GetAttributes(parent), Times.Once);
        provider.Verify(fileSystem => fileSystem.CreateDirectory(It.Is<string>(path => path != destination)), Times.Never);
        provider.Verify(fileSystem => fileSystem.Open(
            It.IsAny<string>(), It.IsAny<FileMode>(), It.IsAny<FileAccess>()), Times.Never);
        provider.Verify(fileSystem => fileSystem.GetAttributes(
            It.Is<string>(path => path.StartsWith(link + Path.DirectorySeparatorChar, StringComparison.Ordinal))), Times.Never);
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task MetadataAccessFailurePreventsEntryWrites(bool asynchronous)
    {
        var destination = Path.Combine(Path.GetTempPath(), "virtual-zip-destination");
        var (zip, provider, _, archivePath) = CreateZip("artifact.txt", destination);
        provider.Setup(fileSystem => fileSystem.GetAttributes(Path.Combine(destination, "artifact.txt")))
            .Throws<UnauthorizedAccessException>();

        await Assert.That(() => Extract(zip, archivePath, destination, asynchronous)).Throws<UnauthorizedAccessException>();

        provider.Verify(fileSystem => fileSystem.Open(
            It.IsAny<string>(), It.IsAny<FileMode>(), It.IsAny<FileAccess>()), Times.Never);
        // Only the trusted extraction root may be created before entry validation.
        provider.Verify(fileSystem => fileSystem.CreateDirectory(destination), Times.Once);
    }

    private static async Task Extract(Zip zip, string archivePath, string destination, bool asynchronous)
    {
        if (asynchronous)
        {
            await zip.ExtractToDirectoryAsync(archivePath, destination, overwriteFiles: false);
        }
        else
        {
            zip.ExtractToDirectory(archivePath, destination, overwriteFiles: false);
        }
    }

    private static (Zip Zip, Mock<IFileSystemProvider> Provider, MemoryStream Output, string ArchivePath) CreateZip(
        string entryName,
        string destination)
    {
        using var archiveBytes = new MemoryStream();
        using (var archive = new ZipArchive(archiveBytes, ZipArchiveMode.Create, leaveOpen: true))
        {
            using var entry = archive.CreateEntry(entryName).Open();
            entry.Write(Encoding.UTF8.GetBytes("archive contents"));
        }

        var archiveData = archiveBytes.ToArray();
        var archivePath = Path.Combine(Path.GetTempPath(), "zip-destination-test.zip");
        var output = new MemoryStream();
        var provider = new Mock<IFileSystemProvider>(MockBehavior.Strict);
        provider.Setup(fileSystem => fileSystem.FileExists(It.IsAny<string>())).Returns(false);
        provider.Setup(fileSystem => fileSystem.FileExists(archivePath)).Returns(true);
        provider.Setup(fileSystem => fileSystem.OpenRead(archivePath))
            .Returns(() => new MemoryStream(archiveData));
        provider.Setup(fileSystem => fileSystem.CreateDirectory(It.IsAny<string>()));
        provider.Setup(fileSystem => fileSystem.GetAttributes(It.IsAny<string>())).Throws<FileNotFoundException>();
        provider.Setup(fileSystem => fileSystem.Open(
                Path.GetFullPath(Path.Combine(destination, "artifact.txt")),
                FileMode.CreateNew,
                FileAccess.Write))
            .Returns(output);

        return (new Zip(provider.Object, new PipelineWorkingDirectory(Path.GetTempPath())), provider, output, archivePath);
    }
}
