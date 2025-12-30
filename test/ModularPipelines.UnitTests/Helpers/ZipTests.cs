using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

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
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();

            var directory = context.Git().RootDirectory.GetFolder("test")
                .GetFolder("ModularPipelines.UnitTests")
                .GetFolder("Data")
                .GetFolder("Zip");

            var fileToWrite = context.Environment.WorkingDirectory.GetFile("LoremData.zip");

            // Track for cleanup
            TrackArtifact(fileToWrite.Path);

            fileToWrite.Delete();

            context.Zip.ZipFolder(directory, fileToWrite.Path);

            return null;
        }
    }

    [Test]
    [NotInParallel(nameof(ZipTests), Order = 1)]
    public async Task Has_Not_Errored()
    {
        var moduleResult = await await RunModule<ZipModule>();

        using (Assert.Multiple())
        {
            await Assert.That(moduleResult.ModuleResultType).IsEqualTo(ModuleResultType.Success);
            await Assert.That(moduleResult.Exception).IsNull();
        }
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
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();

            var zipLocation = context.Environment.WorkingDirectory.GetFile("LoremData.zip");

            var unzippedLocation = context.Environment.WorkingDirectory.GetFolder("LoremDataUnzipped");

            // Track for cleanup
            TrackArtifact(unzippedLocation.Path);

            context.Zip.UnZipToFolder(zipLocation.Path, unzippedLocation.Path);

            return null;
        }
    }

    [Test]
    [NotInParallel(nameof(ZipTests), Order = 3)]
    public async Task UnZip_Has_Not_Errored()
    {
        var moduleResult = await await RunModule<UnZipModule>();

        using (Assert.Multiple())
        {
            await Assert.That(moduleResult.ModuleResultType).IsEqualTo(ModuleResultType.Success);
            await Assert.That(moduleResult.Exception).IsNull();
        }
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
            await Assert.That(expectedFolder.GetFiles("*", SearchOption.AllDirectories)).HasCount().EqualTo(1);
        }
    }
}
