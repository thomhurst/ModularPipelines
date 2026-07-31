using System.IO.Compression;
using ModularPipelines.Context;
using ModularPipelines.FileSystem;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using ModularPipelines.TestHelpers.Assertions;

namespace ModularPipelines.UnitTests.Helpers;

public class ZipTests : TestBase
{
    private static readonly List<string> CreatedArtifacts = [];

    [Before(Class)]
    public static void ResetArtifacts()
    {
        CreatedArtifacts.Clear();
    }

    [After(Class)]
    public static async Task Cleanup()
    {
        await Task.CompletedTask;

        // Clean up tracked artifacts
        foreach (var artifact in CreatedArtifacts)
        {
            try
            {
                if (System.IO.File.Exists(artifact))
                {
                    System.IO.File.Delete(artifact);
                }
                else if (Directory.Exists(artifact))
                {
                    Directory.Delete(artifact, recursive: true);
                }
            }
            catch
            {
                // Best effort cleanup - ignore errors
            }
        }

        CreatedArtifacts.Clear();

        // Also clean up known test artifact locations as fallback
        var zipFile = Path.Combine(TestContext.WorkingDirectory, "LoremData.zip");
        var unzippedDir = Path.Combine(TestContext.WorkingDirectory, "LoremDataUnzipped");

        if (System.IO.File.Exists(zipFile))
        {
            System.IO.File.Delete(zipFile);
        }

        if (Directory.Exists(unzippedDir))
        {
            Directory.Delete(unzippedDir, recursive: true);
        }
    }

    private static void TrackArtifact(string path)
    {
        if (!CreatedArtifacts.Contains(path))
        {
            CreatedArtifacts.Add(path);
        }
    }

    private class ZipModule : Module<string>
    {
        protected internal override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            var repositoryInfo = await context.Git().Information.GetInfoAsync()
                ?? throw new InvalidOperationException("Git repository information is unavailable.");
            var directory = repositoryInfo.Root.GetFolder("test")
                .GetFolder("ModularPipelines.UnitTests")
                .GetFolder("Data")
                .GetFolder("Zip");

            var fileToWrite = context.Files.GetFile(Path.Combine(context.Environment.WorkingDirectory, "LoremData.zip"));

            // Track for cleanup
            TrackArtifact(fileToWrite.Path);

            fileToWrite.Delete();

            context.Files.Zip.ZipFolder(directory, fileToWrite.Path);

            return null;
        }
    }

    [Test]
    [NotInParallel(nameof(ZipTests), Order = 1)]
    public async Task Has_Not_Errored()
    {
        var moduleResult = await await RunModule<ZipModule>();

        await ModuleResultAssertions.AssertSuccess(moduleResult);
    }

    [Test]
    [NotInParallel(nameof(ZipTests), Order = 2)]
    public async Task Zip_File_Exists()
    {
        await RunModule<ZipModule>();

        var expectedFile = new FileInfo(Path.Combine(TestContext.WorkingDirectory, "LoremData.zip"));

        using (Assert.Multiple())
        {
            await Assert.That(expectedFile.Exists).IsTrue();
            await Assert.That(expectedFile.Length).IsGreaterThan(5000);
        }
    }

    private class UnZipModule : Module<string>
    {
        protected internal override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();

            var zipLocation = context.Files.GetFile(Path.Combine(context.Environment.WorkingDirectory, "LoremData.zip"));

            var unzippedLocation = context.Files.GetFolder(Path.Combine(context.Environment.WorkingDirectory, "LoremDataUnzipped"));

            // Track for cleanup
            TrackArtifact(unzippedLocation.Path);

            context.Files.Zip.UnZipToFolder(zipLocation.Path, unzippedLocation.Path);

            return null;
        }
    }

    [Test]
    [NotInParallel(nameof(ZipTests), Order = 3)]
    public async Task UnZip_Has_Not_Errored()
    {
        var moduleResult = await await RunModule<UnZipModule>();

        await ModuleResultAssertions.AssertSuccess(moduleResult);
    }

    [Test]
    [NotInParallel(nameof(ZipTests), Order = 4)]
    public async Task UnZipped_Folder_Exists()
    {
        await RunModule<UnZipModule>();

        var expectedFolder = new DirectoryInfo(Path.Combine(TestContext.WorkingDirectory, "LoremDataUnzipped"));

        using (Assert.Multiple())
        {
            await Assert.That(expectedFolder.Exists).IsTrue();
            await Assert.That(expectedFolder.GetFiles("*", SearchOption.AllDirectories)).Count().IsEqualTo(1);
        }
    }

    [Test]
    public async Task PhysicalProviderPreservesFileLastWriteTimes()
    {
        var root = Path.Combine(
            Path.GetTempPath(),
            $"ModularPipelines-zip-timestamp-{Guid.NewGuid():N}");
        var sourceDirectory = Path.Combine(root, "source");
        var sourceFile = Path.Combine(sourceDirectory, "artifact.txt");
        var zipPath = Path.Combine(root, "artifact.zip");
        var destinationDirectory = Path.Combine(root, "destination");
        var extractedFile = Path.Combine(destinationDirectory, "artifact.txt");
        var expectedTimestamp = new DateTime(2020, 1, 2, 3, 4, 6);
        Directory.CreateDirectory(sourceDirectory);

        try
        {
            await System.IO.File.WriteAllTextAsync(sourceFile, "contents");
            System.IO.File.SetLastWriteTime(sourceFile, expectedTimestamp);
            var zip = new Zip(SystemFileSystemProvider.Instance);

            zip.ZipFolder(new Folder(sourceDirectory), zipPath, CompressionLevel.Optimal);
            zip.UnZipToFolder(zipPath, destinationDirectory, overwriteFiles: true);

            await Assert.That(System.IO.File.GetLastWriteTime(extractedFile))
                .IsEqualTo(expectedTimestamp);
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }
}
