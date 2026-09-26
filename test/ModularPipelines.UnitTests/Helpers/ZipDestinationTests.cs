using System.IO.Compression;
using System.Text;
using ModularPipelines.Context;
using ModularPipelines.FileSystem;
using Moq;

namespace ModularPipelines.UnitTests.Helpers;

public class ZipDestinationTests
{
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task ExtractsToDirectoryWithTrailingSeparator(bool useAlternateSeparator)
    {
        var separator = useAlternateSeparator ? Path.AltDirectorySeparatorChar : Path.DirectorySeparatorChar;
        var destination = Path.Combine(Path.GetTempPath(), "zip-destination") + separator;
        var (zip, provider, output, archivePath) = CreateZip("artifact.txt", destination);

        zip.ExtractToDirectory(archivePath, destination, overwriteFiles: false);

        await Assert.That(Encoding.UTF8.GetString(output.ToArray())).IsEqualTo("archive contents");
        provider.Verify(fileSystem => fileSystem.Open(
            Path.GetFullPath(Path.Combine(destination, "artifact.txt")),
            FileMode.CreateNew,
            FileAccess.Write), Times.Once);
    }

    [Test]
    public async Task ExtractsToFileSystemRootThroughProvider()
    {
        var destination = Path.GetPathRoot(Path.GetFullPath(Path.GetTempPath()))!;
        var (zip, provider, output, archivePath) = CreateZip("artifact.txt", destination);

        zip.ExtractToDirectory(archivePath, destination, overwriteFiles: false);

        await Assert.That(Encoding.UTF8.GetString(output.ToArray())).IsEqualTo("archive contents");
        provider.Verify(fileSystem => fileSystem.Open(
            Path.Combine(destination, "artifact.txt"),
            FileMode.CreateNew,
            FileAccess.Write), Times.Once);
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
        provider.Setup(fileSystem => fileSystem.Open(
                Path.GetFullPath(Path.Combine(destination, "artifact.txt")),
                FileMode.CreateNew,
                FileAccess.Write))
            .Returns(output);

        return (new Zip(provider.Object, new PipelineWorkingDirectory(Path.GetTempPath())), provider, output, archivePath);
    }
}
