using System.IO.Compression;
using ModularPipelines.Context;
using ModularPipelines.FileSystem;
using Moq;

namespace ModularPipelines.UnitTests.Helpers;

public class ZipReliabilityTests
{
    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task PreservesUnixFilePermissions(bool asynchronous)
    {
        if (OperatingSystem.IsWindows())
        {
            Skip.Test("Unix file permissions require Unix.");
            return;
        }

        var root = Directory.CreateTempSubdirectory("zip-permissions-");
        try
        {
            var source = Directory.CreateDirectory(Path.Combine(root.FullName, "source"));
            var executable = Path.Combine(source.FullName, "tool.sh");
            var data = Path.Combine(source.FullName, "data.txt");
            await File.WriteAllTextAsync(executable, "#!/bin/sh\nexit 0\n");
            await File.WriteAllTextAsync(data, "data");
            const UnixFileMode executableMode = UnixFileMode.UserRead | UnixFileMode.UserWrite | UnixFileMode.UserExecute
                                                | UnixFileMode.GroupRead | UnixFileMode.GroupExecute;
            const UnixFileMode dataMode = UnixFileMode.UserRead | UnixFileMode.UserWrite;
            File.SetUnixFileMode(executable, executableMode);
            File.SetUnixFileMode(data, dataMode);
            IZipContext zip = new Zip(SystemFileSystemProvider.Instance, new PipelineWorkingDirectory(root.FullName));
            await Create(zip, source.FullName, asynchronous);
            using (var archive = ZipFile.OpenRead(Path.Combine(root.FullName, "archive.zip")))
            {
                await Assert.That((archive.GetEntry("tool.sh")!.ExternalAttributes >> 16) & 0xFFFF)
                    .IsEqualTo(0x8000 | (int) executableMode);
            }

            await Extract(zip, asynchronous);
            await Assert.That(File.GetUnixFileMode(Path.Combine(root.FullName, "destination", "tool.sh"))).IsEqualTo(executableMode);
            await Assert.That(File.GetUnixFileMode(Path.Combine(root.FullName, "destination", "data.txt"))).IsEqualTo(dataMode);
        }
        finally
        {
            root.Delete(recursive: true);
        }
    }

    [Test]
    [Arguments(false, 0)]
    [Arguments(true, 0)]
    [Arguments(false, 0x8000)]
    [Arguments(true, 0x8000)]
    [Arguments(false, 0x81ED)]
    [Arguments(true, 0x81ED)]
    [Arguments(false, 0x01ED)]
    [Arguments(true, 0x01ED)]
    public async Task ExtractsExternalUnixPermissionsWhenPresent(bool asynchronous, int unixAttributes)
    {
        var root = Directory.CreateTempSubdirectory("zip-external-permissions-");
        try
        {
            var reference = Path.Combine(root.FullName, "reference.txt");
            await File.WriteAllTextAsync(reference, "reference");
            using (var archive = ZipFile.Open(Path.Combine(root.FullName, "archive.zip"), ZipArchiveMode.Create))
            {
                var entry = archive.CreateEntry("input.txt");
                entry.ExternalAttributes = unixAttributes << 16;
                using var writer = new StreamWriter(entry.Open());
                writer.Write("contents");
            }

            IZipContext zip = new Zip(SystemFileSystemProvider.Instance, new PipelineWorkingDirectory(root.FullName));
            await Extract(zip, asynchronous);
            var extracted = Path.Combine(root.FullName, "destination", "input.txt");
            await Assert.That(File.Exists(extracted)).IsTrue();
            if (!OperatingSystem.IsWindows())
            {
                var permissions = unixAttributes & 0xFFF;
                var expectedMode = unixAttributes == 0 ? File.GetUnixFileMode(reference) : (UnixFileMode) permissions;
                await Assert.That(File.GetUnixFileMode(extracted)).IsEqualTo(expectedMode);
            }
            else
            {
                await Assert.That(await File.ReadAllTextAsync(extracted)).IsEqualTo("contents");
            }
        }
        finally
        {
            root.Delete(recursive: true);
        }
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task FailedCreationLeavesNoArchiveAndCanBeRetried(bool asynchronous)
    {
        var root = Directory.CreateTempSubdirectory("zip-failure-");
        try
        {
            var source = Directory.CreateDirectory(Path.Combine(root.FullName, "source"));
            var input = Path.Combine(source.FullName, "input.txt");
            await File.WriteAllTextAsync(input, "contents");
            var provider = CreateProvider();
            provider.Setup(fileSystem => fileSystem.OpenRead(input)).Throws(new IOException("Read failed."));
            IZipContext zip = new Zip(provider.Object, new PipelineWorkingDirectory(root.FullName));

            await Assert.ThrowsAsync<IOException>(() => Create(zip, source.FullName, asynchronous));
            await Assert.That(Directory.GetFiles(root.FullName)).IsEmpty();

            provider.Setup(fileSystem => fileSystem.OpenRead(input)).Returns(() => File.OpenRead(input));
            await Create(zip, source.FullName, asynchronous);
            using var archive = ZipFile.OpenRead(Path.Combine(root.FullName, "archive.zip"));
            await Assert.That(archive.Entries.Select(entry => entry.FullName)).IsEquivalentTo(["input.txt"]);
            await Assert.That(Directory.GetFiles(root.FullName).Length).IsEqualTo(1);
        }
        finally
        {
            root.Delete(recursive: true);
        }
    }

    [Test]
    public async Task CancellationDuringCreationLeavesNoArchiveAndCanBeRetried()
    {
        var root = Directory.CreateTempSubdirectory("zip-cancellation-");
        try
        {
            var source = Directory.CreateDirectory(Path.Combine(root.FullName, "source"));
            var input = Path.Combine(source.FullName, "input.txt");
            await File.WriteAllTextAsync(input, "contents");
            using var cancellation = new CancellationTokenSource();
            var provider = CreateProvider();
            provider.Setup(fileSystem => fileSystem.OpenRead(input)).Returns(() => new AsyncIoTestStream(new MemoryStream([1, 2, 3]))
            {
                BeforeRead = token =>
                {
                    cancellation.Cancel();
                    token.ThrowIfCancellationRequested();
                },
            });
            IZipContext zip = new Zip(provider.Object, new PipelineWorkingDirectory(root.FullName));

            await Assert.ThrowsAsync<OperationCanceledException>(() => zip.CreateFromDirectoryAsync(
                new FolderPath(source.FullName), "archive.zip", cancellationToken: cancellation.Token));
            await Assert.That(Directory.GetFiles(root.FullName)).IsEmpty();

            provider.Setup(fileSystem => fileSystem.OpenRead(input)).Returns(() => File.OpenRead(input));
            await Create(zip, source.FullName, asynchronous: true);
            await Assert.That(Directory.GetFiles(root.FullName).Length).IsEqualTo(1);
        }
        finally
        {
            root.Delete(recursive: true);
        }
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task CompetingArchiveIsNotOverwritten(bool asynchronous)
    {
        var root = Directory.CreateTempSubdirectory("zip-publish-race-");
        try
        {
            var source = Directory.CreateDirectory(Path.Combine(root.FullName, "source"));
            var archivePath = Path.Combine(root.FullName, "archive.zip");
            var provider = CreateProvider();
            provider.Setup(fileSystem => fileSystem.MoveFile(It.IsAny<string>(), archivePath))
                .Callback((string temporary, string destination) =>
                {
                    File.WriteAllText(destination, "competing archive");
                    File.Move(temporary, destination);
                });
            IZipContext zip = new Zip(provider.Object, new PipelineWorkingDirectory(root.FullName));

            await Assert.ThrowsAsync<IOException>(() => Create(zip, source.FullName, asynchronous));
            await Assert.That(await File.ReadAllTextAsync(archivePath)).IsEqualTo("competing archive");
            await Assert.That(Directory.GetFiles(root.FullName).Length).IsEqualTo(1);
        }
        finally
        {
            root.Delete(recursive: true);
        }
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task LocalizedCollisionStillExplainsOverwriteOption(bool asynchronous)
    {
        var root = Directory.CreateTempSubdirectory("zip-localized-collision-");
        try
        {
            using (var archive = ZipFile.Open(Path.Combine(root.FullName, "archive.zip"), ZipArchiveMode.Create))
            {
                using var writer = new StreamWriter(archive.CreateEntry("input.txt").Open());
                writer.Write("contents");
            }

            var destination = Path.Combine(root.FullName, "destination", "input.txt");
            var provider = CreateProvider();
            provider.Setup(fileSystem => fileSystem.Open(destination, FileMode.CreateNew, FileAccess.Write))
                .Returns(() =>
                {
                    File.WriteAllText(destination, "competing contents");
                    throw new IOException("Die Datei ist bereits vorhanden.");
                });
            IZipContext zip = new Zip(provider.Object, new PipelineWorkingDirectory(root.FullName));

            var exception = await Assert.ThrowsAsync<IOException>(() => Extract(zip, asynchronous));
            await Assert.That(exception!.Message).Contains("Set overwriteFiles to true");
            await Assert.That(await File.ReadAllTextAsync(destination)).IsEqualTo("competing contents");
        }
        finally
        {
            root.Delete(recursive: true);
        }
    }

    private static Mock<IFileSystemProvider> CreateProvider()
    {
        var provider = new Mock<IFileSystemProvider>(MockBehavior.Strict);
        provider.Setup(fileSystem => fileSystem.FileExists(It.IsAny<string>())).Returns((string path) => File.Exists(path));
        provider.Setup(fileSystem => fileSystem.DirectoryExists(It.IsAny<string>())).Returns((string path) => Directory.Exists(path));
        provider.Setup(fileSystem => fileSystem.CreateDirectory(It.IsAny<string>())).Callback((string path) => Directory.CreateDirectory(path));
        provider.Setup(fileSystem => fileSystem.GetAttributes(It.IsAny<string>())).Returns((string path) => File.GetAttributes(path));
        provider.Setup(fileSystem => fileSystem.EnumerateDirectories(It.IsAny<string>(), "*", SearchOption.TopDirectoryOnly))
            .Returns((string path, string pattern, SearchOption option) => Directory.EnumerateDirectories(path, pattern, option));
        provider.Setup(fileSystem => fileSystem.EnumerateFiles(It.IsAny<string>(), "*", SearchOption.TopDirectoryOnly))
            .Returns((string path, string pattern, SearchOption option) => Directory.EnumerateFiles(path, pattern, option));
        provider.Setup(fileSystem => fileSystem.GetRelativePath(It.IsAny<string>(), It.IsAny<string>()))
            .Returns((string root, string path) => Path.GetRelativePath(root, path));
        provider.Setup(fileSystem => fileSystem.Open(It.IsAny<string>(), It.IsAny<FileMode>(), It.IsAny<FileAccess>()))
            .Returns((string path, FileMode mode, FileAccess access) => File.Open(path, mode, access));
        provider.Setup(fileSystem => fileSystem.OpenRead(It.IsAny<string>())).Returns((string path) => File.OpenRead(path));
        provider.Setup(fileSystem => fileSystem.MoveFile(It.IsAny<string>(), It.IsAny<string>()))
            .Callback((string source, string destination) => File.Move(source, destination));
        provider.Setup(fileSystem => fileSystem.DeleteFile(It.IsAny<string>())).Callback((string path) => File.Delete(path));
        return provider;
    }

    private static async Task Create(IZipContext zip, string source, bool asynchronous)
    {
        if (asynchronous)
        {
            await zip.CreateFromDirectoryAsync(new FolderPath(source), "archive.zip");
        }
        else
        {
            zip.CreateFromDirectory(new FolderPath(source), "archive.zip");
        }
    }

    private static async Task Extract(IZipContext zip, bool asynchronous)
    {
        if (asynchronous)
        {
            await zip.ExtractToDirectoryAsync("archive.zip", "destination", overwriteFiles: false);
        }
        else
        {
            zip.ExtractToDirectory("archive.zip", "destination", overwriteFiles: false);
        }
    }
}
