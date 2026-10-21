using System.IO.Compression;
using System.Security.AccessControl;
using System.Security.Principal;
using ModularPipelines.Context;
using ModularPipelines.FileSystem;
using Moq;

namespace ModularPipelines.UnitTests.Helpers;

public class ZipReliabilityTests
{
    [Test]
    [MatrixDataSource]
    public async Task FailedWindowsPublicationPreservesOriginalContents(
        [Matrix(false, true)] bool asynchronous,
        [Matrix(0, 1, 2)] int failureState)
    {
        if (!OperatingSystem.IsWindows())
        {
            Skip.Test("Windows replacement recovery requires Windows.");
            return;
        }

        var root = Directory.CreateTempSubdirectory("zip-replacement-failure-");
        try
        {
            var source = Directory.CreateDirectory(Path.Combine(root.FullName, "source"));
            await File.WriteAllTextAsync(Path.Combine(source.FullName, "input.txt"), "new contents");
            string? retainedBackup = null;
            var failPublication = true;
            IZipContext zip = new Zip(SystemFileSystemProvider.Instance, new PipelineWorkingDirectory(root.FullName),
                (replacement, destination, backup) =>
                {
                    if (!failPublication)
                    {
                        File.Replace(replacement, destination, backup);
                        return;
                    }

                    // Model ReplaceFile failures before and after moving the original to its backup.
                    if (failureState != 0)
                    {
                        File.Move(destination, backup!);
                        retainedBackup = backup;
                    }

                    if (failureState == 2)
                    {
                        File.WriteAllText(destination, "competing contents");
                    }

                    throw new IOException("Simulated replacement publication failure.");
                });
            await Create(zip, source.FullName, asynchronous);
            var destinationDirectory = Directory.CreateDirectory(Path.Combine(root.FullName, "destination"));
            var destinationPath = Path.Combine(destinationDirectory.FullName, "input.txt");
            await File.WriteAllTextAsync(destinationPath, "original contents");

            var exception = await Assert.ThrowsAsync<IOException>(() => Extract(zip, asynchronous, overwriteFiles: true));
            if (failureState == 2)
            {
                await Assert.That(await File.ReadAllTextAsync(destinationPath)).IsEqualTo("competing contents");
                await Assert.That(await File.ReadAllTextAsync(retainedBackup!)).IsEqualTo("original contents");
                await Assert.That(exception!.InnerException!.Message).Contains(retainedBackup!);
                await Assert.That(Directory.GetFiles(destinationDirectory.FullName).Length).IsEqualTo(2);
                File.Delete(retainedBackup!);
            }
            else
            {
                await Assert.That(await File.ReadAllTextAsync(destinationPath)).IsEqualTo("original contents");
                await Assert.That(Directory.GetFiles(destinationDirectory.FullName).Length).IsEqualTo(1);
            }

            failPublication = false;
            await Extract(zip, asynchronous, overwriteFiles: true);
            await Assert.That(await File.ReadAllTextAsync(destinationPath)).IsEqualTo("new contents");
            await Assert.That(Directory.GetFiles(destinationDirectory.FullName).Length).IsEqualTo(1);
        }
        finally
        {
            root.Delete(recursive: true);
        }
    }

    [Test]
    [MatrixDataSource]
    public async Task OverwritingPreservesWindowsAccessControl(
        [Matrix(false, true)] bool asynchronous,
        [Matrix(false, true)] bool protectAccessControl)
    {
        if (!OperatingSystem.IsWindows())
        {
            Skip.Test("Windows access control requires Windows.");
            return;
        }

        var root = Directory.CreateTempSubdirectory("zip-access-control-");
        try
        {
            var source = Directory.CreateDirectory(Path.Combine(root.FullName, "source"));
            await File.WriteAllTextAsync(Path.Combine(source.FullName, "input.txt"), "new contents");
            IZipContext zip = new Zip(SystemFileSystemProvider.Instance, new PipelineWorkingDirectory(root.FullName));
            await Create(zip, source.FullName, asynchronous);

            var destination = Directory.CreateDirectory(Path.Combine(root.FullName, "destination"));
            var file = new FileInfo(Path.Combine(destination.FullName, "input.txt"));
            await File.WriteAllTextAsync(file.FullName, "original contents");
            var security = file.GetAccessControl();
            security.SetAccessRuleProtection(isProtected: protectAccessControl, preserveInheritance: false);
            using var identity = WindowsIdentity.GetCurrent();
            security.AddAccessRule(new FileSystemAccessRule(
                identity.User!, FileSystemRights.FullControl, AccessControlType.Allow));
            file.SetAccessControl(security);
            var originalAccessControl = file.GetAccessControl().GetSecurityDescriptorSddlForm(AccessControlSections.Access);

            await Extract(zip, asynchronous, overwriteFiles: true);

            await Assert.That(await File.ReadAllTextAsync(file.FullName)).IsEqualTo("new contents");
            await Assert.That(file.GetAccessControl().GetSecurityDescriptorSddlForm(AccessControlSections.Access))
                .IsEqualTo(originalAccessControl);
            await Assert.That(Directory.GetFiles(destination.FullName).Length).IsEqualTo(1);
        }
        finally
        {
            root.Delete(recursive: true);
        }
    }

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
    [MatrixDataSource]
    public async Task ExtractsOnlyOrdinaryUnixPermissionsThroughUmask(
        [Matrix(false, true)] bool asynchronous,
        [Matrix(0, 0x8000, 0x81ED, 0x01ED, 0x8FED, 0x81FF, 0x8E00)] int unixAttributes,
        [Matrix(0, 3, 10)] int hostPlatform,
        [Matrix(false, true)] bool overwriteExisting)
    {
        var root = Directory.CreateTempSubdirectory("zip-external-permissions-");
        try
        {
            var archivePath = Path.Combine(root.FullName, "archive.zip");
            using (var archive = ZipFile.Open(archivePath, ZipArchiveMode.Create))
            {
                var entry = archive.CreateEntry("input.txt");
                entry.ExternalAttributes = unixAttributes << 16;
                using var writer = new StreamWriter(entry.Open());
                writer.Write("contents");
            }

            SetArchiveHostPlatform(archivePath, hostPlatform);
            using (var archive = ZipFile.OpenRead(archivePath))
            {
                await Assert.That(archive.GetEntry("input.txt")!.VersionMadeBy >> 8).IsEqualTo(hostPlatform);
            }

            var destination = Directory.CreateDirectory(Path.Combine(root.FullName, "destination"));
            var extracted = Path.Combine(destination.FullName, "input.txt");
            if (overwriteExisting)
            {
                await File.WriteAllTextAsync(extracted, "old contents");
                if (!OperatingSystem.IsWindows())
                {
                    File.SetUnixFileMode(extracted, UnixFileMode.UserRead | UnixFileMode.UserWrite);
                }
            }

            IZipContext zip = new Zip(SystemFileSystemProvider.Instance, new PipelineWorkingDirectory(root.FullName));
            await Extract(zip, asynchronous, overwriteExisting);
            await Assert.That(File.Exists(extracted)).IsTrue();
            if (!OperatingSystem.IsWindows())
            {
                // Let the OS apply the process umask without changing process-global state.
                var reference = Path.Combine(root.FullName, "reference.txt");
                var permissionBits = unixAttributes & 0x1FF;
                var permissions = (UnixFileMode) permissionBits;
                using (var stream = new FileStream(reference, new FileStreamOptions
                {
                    Access = FileAccess.Write,
                    Mode = FileMode.CreateNew,
                    UnixCreateMode = hostPlatform == 3 && unixAttributes != 0 ? permissions : null,
                }))
                {
                }

                await Assert.That(File.GetUnixFileMode(extracted)).IsEqualTo(File.GetUnixFileMode(reference));
                // Zero-mode entries are unreadable until the owner grants read permission.
                File.SetUnixFileMode(extracted, UnixFileMode.UserRead | UnixFileMode.UserWrite);
            }

            await Assert.That(await File.ReadAllTextAsync(extracted)).IsEqualTo("contents");
            await Assert.That(Directory.GetFiles(destination.FullName)).IsEquivalentTo([extracted]);
        }
        finally
        {
            root.Delete(recursive: true);
        }
    }

    private static void SetArchiveHostPlatform(string archivePath, int hostPlatform)
    {
        using var stream = File.Open(archivePath, FileMode.Open, FileAccess.ReadWrite);
        using var reader = new BinaryReader(stream);
        // These single-entry archives have no comment. The final six bytes contain
        // the central-directory offset followed by the two-byte comment length.
        stream.Seek(-6, SeekOrigin.End);
        var centralDirectoryOffset = reader.ReadUInt32();
        // The host byte follows the four-byte signature and one-byte ZIP version.
        stream.Position = centralDirectoryOffset + 5;
        stream.WriteByte((byte) hostPlatform);
    }

    [Test]
    [MatrixDataSource]
    public async Task FailedExtractionRemovesTemporaryFileAndPreservesExistingDestination(
        [Matrix(false, true)] bool asynchronous,
        [Matrix(false, true)] bool overwriteExisting)
    {
        var root = Directory.CreateTempSubdirectory("zip-extraction-failure-");
        try
        {
            var archivePath = Path.Combine(root.FullName, "archive.zip");
            using (var archive = ZipFile.Open(archivePath, ZipArchiveMode.Create))
            {
                using var writer = new StreamWriter(archive.CreateEntry("input.txt", CompressionLevel.Optimal).Open());
                writer.Write("contents");
            }

            var validArchive = await File.ReadAllBytesAsync(archivePath);
            // Keep ZIP headers intact and replace the first DEFLATE block with invalid block type 3.
            using (var stream = File.Open(archivePath, FileMode.Open, FileAccess.ReadWrite))
            using (var reader = new BinaryReader(stream))
            {
                stream.Position = 26;
                var nameLength = reader.ReadUInt16();
                var extraLength = reader.ReadUInt16();
                stream.Position = 30 + nameLength + extraLength;
                stream.WriteByte(0x07);
            }

            var destination = Directory.CreateDirectory(Path.Combine(root.FullName, "destination"));
            var extracted = Path.Combine(destination.FullName, "input.txt");
            if (overwriteExisting)
            {
                await File.WriteAllTextAsync(extracted, "old contents");
            }

            IZipContext zip = new Zip(SystemFileSystemProvider.Instance, new PipelineWorkingDirectory(root.FullName));
            await Assert.ThrowsAsync<InvalidDataException>(() => Extract(zip, asynchronous, overwriteExisting));
            if (overwriteExisting)
            {
                await Assert.That(await File.ReadAllTextAsync(extracted)).IsEqualTo("old contents");
                await Assert.That(Directory.GetFiles(destination.FullName)).IsEquivalentTo([extracted]);
            }
            else
            {
                await Assert.That(Directory.GetFiles(destination.FullName)).IsEmpty();
            }

            await File.WriteAllBytesAsync(archivePath, validArchive);
            await Extract(zip, asynchronous, overwriteExisting);
            await Assert.That(await File.ReadAllTextAsync(extracted)).IsEqualTo("contents");
            await Assert.That(Directory.GetFiles(destination.FullName)).IsEquivalentTo([extracted]);
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

    private static async Task Extract(IZipContext zip, bool asynchronous, bool overwriteFiles = false)
    {
        if (asynchronous)
        {
            await zip.ExtractToDirectoryAsync("archive.zip", "destination", overwriteFiles);
        }
        else
        {
            zip.ExtractToDirectory("archive.zip", "destination", overwriteFiles);
        }
    }
}
