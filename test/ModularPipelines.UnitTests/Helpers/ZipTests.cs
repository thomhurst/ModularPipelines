using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using TUnit.Assertions;
using TUnit.Core;

namespace ModularPipelines.UnitTests.Helpers;

[Parallelizable(ParallelScope.Fixtures)]
public class ZipTests : TestBase
{
    private class ZipModule : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();

            var directory = context.Git().RootDirectory.GetFolder("test")
                .GetFolder("ModularPipelines.UnitTests")
                .GetFolder("Data")
                .GetFolder("Zip");

            var fileToWrite = context.Environment.WorkingDirectory.GetFile("LoremData.zip");

            fileToWrite.Delete();

            context.Zip.ZipFolder(directory, fileToWrite.Path);

            return null;
        }
    }

    [Test]
    [Order(1)]
    public async Task Has_Not_Errored()
    {
        var module = await RunModule<ZipModule>();

        var moduleResult = await module;

        Assert.Multiple(() =>
        {
            Assert.That(moduleResult.ModuleResultType).Is.EqualTo(ModuleResultType.Success);
            Assert.That(moduleResult.Exception).Is.Null();
        });
    }

    [Test]
    [Order(2)]
    public async Task Zip_File_Exists()
    {
        await RunModule<ZipModule>();

        var expectedFile = new FileInfo(Path.Combine(TestContext.CurrentContext.WorkDirectory, "LoremData.zip");

        Assert.Multiple(() =>
        {
            Assert.That(expectedFile.Exists).Is.True();
            Assert.That(expectedFile.Length).Is.GreaterThan(5000);
        });
    }

    private class UnZipModule : Module<string>
    {
        protected override async Task<string?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();

            var zipLocation = context.Environment.WorkingDirectory.GetFile("LoremData.zip");

            var unzippedLocation = context.Environment.WorkingDirectory.GetFolder("LoremDataUnzipped");

            context.Zip.UnZipToFolder(zipLocation.Path, unzippedLocation.Path);

            return null;
        }
    }

    [Test]
    [Order(3)]
    public async Task UnZip_Has_Not_Errored()
    {
        var module = await RunModule<UnZipModule>();

        var moduleResult = await module;

        Assert.Multiple(() =>
        {
            Assert.That(moduleResult.ModuleResultType).Is.EqualTo(ModuleResultType.Success);
            Assert.That(moduleResult.Exception).Is.Null();
        });
    }

    [Test]
    [Order(4)]
    public async Task UnZipped_Folder_Exists()
    {
        await RunModule<UnZipModule>();

        var expectedFolder = new DirectoryInfo(Path.Combine(TestContext.CurrentContext.WorkDirectory, "LoremDataUnzipped");

        Assert.Multiple(() =>
        {
            Assert.That(expectedFolder.Exists).Is.True();
            Assert.That(expectedFolder.GetFiles("*", SearchOption.AllDirectories)).Has.Length.EqualTo(1);
        });
    }
}