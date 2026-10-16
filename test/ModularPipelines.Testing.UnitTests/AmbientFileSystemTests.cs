using ModularPipelines.Context;
using ModularPipelines.FileSystem;
using ModularPipelines.Modules;

namespace ModularPipelines.Testing.UnitTests;

public class AmbientFileSystemTests
{
    [Test]
    public async Task StringPathsUseIsolatedProviderDuringConstructionAndExecution()
    {
        var root = Path.Combine(Path.GetTempPath(), $"ambient-files-{Guid.NewGuid():N}");
        try
        {
            var runs = await Task.WhenAll(
                ModuleTester.For<StringPathsModule, FilePath[]>().WithService(root).ExecuteAsync(),
                ModuleTester.For<StringPathsModule, FilePath[]>().WithService(root).ExecuteAsync());

            foreach (var run in runs)
            {
                await Assert.That(run.Exception).IsNull();
                foreach (var file in run.Value!)
                {
                    await Assert.That(run.FileSystem.FileExists(file.Path)).IsTrue();
                    await Assert.That(await file.ReadAsync()).IsEqualTo("virtual");
                    await Assert.That(System.IO.File.Exists(file.Path)).IsFalse();
                }
            }

            // Returned paths keep their provider after the ambient scope and pipeline end.
            await runs[0].Value![0].WriteAsync("first run only");
            await Assert.That(await runs[1].Value![0].ReadAsync()).IsEqualTo("virtual");
            await Assert.That(Directory.Exists(root)).IsFalse();
        }
        finally
        {
            if (Directory.Exists(root))
            {
                Directory.Delete(root, recursive: true);
            }
        }
    }

    public sealed class StringPathsModule(string root) : Module<FilePath[]>
    {
        private readonly FolderPath _root = new(root);
        private readonly FilePath _constructedFile = new(Path.Combine(root, "constructed.txt"));

        protected override async Task<FilePath[]> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            _root.Create();
            await Task.Yield();
            FolderPath convertedFolder = Path.Combine(_root.Path, "converted")!;
            convertedFolder.Create();
            FilePath convertedFile = Path.Combine(convertedFolder.Path, "converted.txt")!;
            FilePath infoFile = new FileInfo(Path.Combine(_root.Path, "info.txt"));
            FolderPath infoFolder = new DirectoryInfo(Path.Combine(_root.Path, "info"));
            infoFolder.Create();
            var temporaryFile = FilePath.GetNewTemporaryFilePath();
            var temporaryFolder = FolderPath.CreateTemporaryFolder();
            FilePath[] files =
            [
                _constructedFile,
                new FilePath(Path.Combine(_root.Path, "executed.txt")),
                convertedFile,
                infoFile,
                infoFolder.GetFile("child.txt"),
                temporaryFile,
                temporaryFolder.GetFile("child.txt"),
            ];

            foreach (var file in files)
            {
                await file.WriteAsync("virtual", cancellationToken);
            }

            return files;
        }
    }
}
