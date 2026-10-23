using System.IO.Compression;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.FileSystem;
using ModularPipelines.TestHelpers;
using ModularPipelines.TestHelpers.Assertions;
using Moq;

namespace ModularPipelines.UnitTests.Helpers;

public class ZipTests : TestBase
{
    private sealed class ZipFixture : IDisposable
    {
        private readonly DirectoryInfo _root = Directory.CreateTempSubdirectory("ModularPipelines-zip-tests-");

        public static string SourceDirectory => Path.Combine(AppContext.BaseDirectory, "Data", "Zip");

        public string ArchivePath => Path.Combine(_root.FullName, "LoremData.zip");

        public string ExtractionPath => Path.Combine(_root.FullName, "LoremDataUnzipped");

        public void CreateArchive() => ZipFile.CreateFromDirectory(SourceDirectory, ArchivePath);

        public void Dispose() => _root.Delete(recursive: true);
    }

    private class ZipModule(ZipFixture fixture) : SyncModule
    {
        protected override void Execute(IModuleContext context, CancellationToken cancellationToken)
        {
            var directory = context.Files.GetFolder(ZipFixture.SourceDirectory);
            context.Files.Zip.CreateFromDirectory(directory, fixture.ArchivePath);
        }
    }

    [Test]
    public async Task Has_Not_Errored()
    {
        using var fixture = new ZipFixture();
        var moduleResult = await await RunModule<ZipModule>(services => services.AddSingleton(fixture));

        await ModuleResultAssertions.AssertSuccess(moduleResult);
    }

    [Test]
    public async Task Zip_File_Exists()
    {
        using var fixture = new ZipFixture();
        var moduleResult = await await RunModule<ZipModule>(services => services.AddSingleton(fixture));
        await ModuleResultAssertions.AssertSuccess(moduleResult);

        var expectedFile = new FileInfo(fixture.ArchivePath);

        using (Assert.Multiple())
        {
            await Assert.That(expectedFile.Exists).IsTrue();
            await Assert.That(expectedFile.Length).IsGreaterThan(5000);
        }
    }

    private class UnZipModule(ZipFixture fixture) : SyncModule
    {
        protected override void Execute(IModuleContext context, CancellationToken cancellationToken)
        {
            // Extraction is synchronous; yielding here needlessly queues it behind other suite work.
            context.Files.Zip.ExtractToDirectory(fixture.ArchivePath, fixture.ExtractionPath);
        }
    }

    [Test]
    public async Task UnZip_Has_Not_Errored()
    {
        using var fixture = new ZipFixture();
        fixture.CreateArchive();
        var moduleResult = await await RunModule<UnZipModule>(services => services.AddSingleton(fixture));

        await ModuleResultAssertions.AssertSuccess(moduleResult);
    }

    [Test]
    public async Task UnZipped_Folder_Exists()
    {
        using var fixture = new ZipFixture();
        fixture.CreateArchive();
        var moduleResult = await await RunModule<UnZipModule>(services => services.AddSingleton(fixture));
        await ModuleResultAssertions.AssertSuccess(moduleResult);

        var expectedFolder = new DirectoryInfo(fixture.ExtractionPath);

        using (Assert.Multiple())
        {
            await Assert.That(expectedFolder.Exists).IsTrue();
            await Assert.That(expectedFolder.GetFiles("*", SearchOption.AllDirectories)).Count().IsEqualTo(1);
            await Assert.That(await System.IO.File.ReadAllTextAsync(Path.Combine(fixture.ExtractionPath, "Lorem.txt")))
                .IsEqualTo(await System.IO.File.ReadAllTextAsync(Path.Combine(ZipFixture.SourceDirectory, "Lorem.txt")));
        }
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task PhysicalProviderPreservesFileLastWriteTimes(bool useAsync)
    {
        var root = Path.Combine(
            Path.GetTempPath(),
            $"ModularPipelines-zip-timestamp-{Guid.NewGuid():N}");
        var sourceDirectory = Path.Combine(root, "source");
        var sourceFile = Path.Combine(sourceDirectory, "nested", "artifact.txt");
        var zipPath = Path.Combine(root, "artifact.zip");
        var destinationDirectory = Path.Combine(root, "destination");
        var extractedFile = Path.Combine(destinationDirectory, "nested", "artifact.txt");
        var expectedTimestamp = new DateTime(2020, 1, 2, 3, 4, 6);
        Directory.CreateDirectory(Path.GetDirectoryName(sourceFile)!);

        try
        {
            await System.IO.File.WriteAllTextAsync(sourceFile, "contents");
            System.IO.File.SetLastWriteTime(sourceFile, expectedTimestamp);
            var zip = new Zip(SystemFileSystemProvider.Instance, new PipelineWorkingDirectory(root));

            if (useAsync)
            {
                await zip.CreateFromDirectoryAsync(new FolderPath(sourceDirectory), zipPath);
                await zip.ExtractToDirectoryAsync(zipPath, destinationDirectory, overwriteFiles: true);
            }
            else
            {
                zip.CreateFromDirectory(new FolderPath(sourceDirectory), zipPath, CompressionLevel.Optimal);
                zip.ExtractToDirectory(zipPath, destinationDirectory, overwriteFiles: true);
            }

            await Assert.That(System.IO.File.GetLastWriteTime(extractedFile))
                .IsEqualTo(expectedTimestamp);
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task CreateFromDirectoryRejectsExistingOutputWithoutChangingIt()
    {
        var root = Path.Combine(
            Path.GetTempPath(),
            $"ModularPipelines-existing-zip-{Guid.NewGuid():N}");
        var sourceDirectory = Path.Combine(root, "source");
        var sourceFile = Path.Combine(sourceDirectory, "artifact.txt");
        var zipPath = Path.Combine(root, "artifact.zip");
        Directory.CreateDirectory(sourceDirectory);

        try
        {
            await System.IO.File.WriteAllTextAsync(sourceFile, "contents");
            await System.IO.File.WriteAllTextAsync(zipPath, "existing");
            var zip = new Zip(SystemFileSystemProvider.Instance, new PipelineWorkingDirectory(root));

            await Assert.That(() =>
                    zip.CreateFromDirectory(
                        new FolderPath(sourceDirectory),
                        zipPath,
                        CompressionLevel.Optimal))
                .Throws<IOException>();
            await Assert.That(await System.IO.File.ReadAllTextAsync(zipPath))
                .IsEqualTo("existing");
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Test]
    public async Task UnZipDoesNotOverwriteFileCreatedAfterExistenceCheck()
    {
        var root = Path.Combine(
            Path.GetTempPath(),
            $"ModularPipelines-unzip-race-{Guid.NewGuid():N}");
        var zipPath = Path.Combine(root, "artifact.zip");
        var destinationDirectory = Path.Combine(root, "destination");
        var destinationPath = Path.Combine(destinationDirectory, "artifact.txt");
        Directory.CreateDirectory(root);

        try
        {
            using (var archive = ZipFile.Open(zipPath, ZipArchiveMode.Create))
            {
                await using var writer = new StreamWriter(archive.CreateEntry("artifact.txt").Open());
                await writer.WriteAsync("archive contents");
            }

            var competingFileCreated = false;
            var fileSystemProvider = new Mock<IFileSystemProvider>(MockBehavior.Strict);
            fileSystemProvider
                .Setup(provider => provider.FileExists(It.IsAny<string>()))
                .Returns((string path) =>
                {
                    if (path == destinationPath && !competingFileCreated)
                    {
                        System.IO.File.WriteAllText(destinationPath, "competing contents");
                        competingFileCreated = true;
                        return false;
                    }

                    return System.IO.File.Exists(path);
                });
            fileSystemProvider
                .Setup(provider => provider.CreateDirectory(It.IsAny<string>()))
                .Callback((string path) => Directory.CreateDirectory(path));
            fileSystemProvider
                .Setup(provider => provider.GetAttributes(It.IsAny<string>()))
                .Returns((string path) => System.IO.File.GetAttributes(path));
            fileSystemProvider
                .Setup(provider => provider.OpenRead(zipPath))
                .Returns(() => System.IO.File.OpenRead(zipPath));
            fileSystemProvider
                .Setup(provider => provider.Open(
                    It.IsAny<string>(),
                    It.IsAny<FileMode>(),
                    It.IsAny<FileAccess>()))
                .Returns((string path, FileMode mode, FileAccess access) =>
                    System.IO.File.Open(path, mode, access, FileShare.None));
            fileSystemProvider
                .Setup(provider => provider.MoveFile(It.IsAny<string>(), It.IsAny<string>(), false))
                .Callback((string source, string destination, bool overwrite) => System.IO.File.Move(source, destination, overwrite));
            fileSystemProvider
                .Setup(provider => provider.DeleteFile(It.IsAny<string>()))
                .Callback((string path) => System.IO.File.Delete(path));
            var zip = new Zip(fileSystemProvider.Object, new PipelineWorkingDirectory(root));

            await Assert.That(() =>
                    zip.ExtractToDirectory(zipPath, destinationDirectory, overwriteFiles: false))
                .Throws<IOException>();
            await Assert.That(await System.IO.File.ReadAllTextAsync(destinationPath))
                .IsEqualTo("competing contents");
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }
}
