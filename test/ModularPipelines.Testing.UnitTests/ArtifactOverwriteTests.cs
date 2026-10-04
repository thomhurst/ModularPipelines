using ModularPipelines.FileSystem;

namespace ModularPipelines.Testing.UnitTests;

public class ArtifactOverwriteTests
{
    [Test]
    [UnixOnly]
    public async Task Directory_Download_Preserves_Literal_Backslash_InMemory_File_Name()
    {
        var run = await ModuleTester.For<LiteralBackslashArtifactModule, string>().ExecuteAsync();

        await Assert.That(run.Exception).IsNull();
        await Assert.That(run.Value).IsEqualTo("literal filename content");
    }

    [Test]
    public async Task InMemory_Provider_Uses_Host_Separator_By_Default()
    {
        IFileSystemProvider provider = new InMemoryFileSystemProvider();
        await Assert.That(provider.DirectorySeparatorChar).IsEqualTo(Path.DirectorySeparatorChar);
    }

    [Test]
    public async Task Repeated_Directory_Download_Replaces_Existing_InMemory_Files()
    {
        var run = await ModuleTester.For<RepeatedArtifactModule, string>().ExecuteAsync();

        await Assert.That(run.Exception).IsNull();
        await Assert.That(run.Value).IsEqualTo("artifact content");
    }

    [Test]
    public async Task Overwrite_Move_Preserves_Source_Metadata()
    {
        IFileSystemProvider provider = new InMemoryFileSystemProvider();
        var root = Path.Combine(provider.GetTempPath(), "overwrite");
        provider.CreateDirectory(root);
        var source = Path.Combine(root, "source.txt");
        var destination = Path.Combine(root, "destination.txt");
        await provider.WriteAllTextAsync(source, "new");
        await provider.WriteAllTextAsync(destination, "old");
        var timestamp = new DateTime(2020, 1, 2, 3, 4, 5, DateTimeKind.Utc);
        provider.SetLastWriteTimeUtc(source, timestamp);

        provider.MoveFile(source, destination, overwrite: true);

        await Assert.That(provider.FileExists(source)).IsFalse();
        await Assert.That(await provider.ReadAllTextAsync(destination)).IsEqualTo("new");
        await Assert.That(provider.GetLastWriteTimeUtc(destination)).IsEqualTo(timestamp);
    }

    [Test]
    public async Task Overwrite_Move_Failure_Preserves_Both_Files()
    {
        IFileSystemProvider provider = new InMemoryFileSystemProvider();
        var root = Path.Combine(provider.GetTempPath(), "overwrite");
        provider.CreateDirectory(root);
        var source = Path.Combine(root, "source.txt");
        var destination = Path.Combine(root, "destination.txt");
        await provider.WriteAllTextAsync(source, "new");
        await provider.WriteAllTextAsync(destination, "old");
        using (provider.Open(destination, FileMode.Open, FileAccess.ReadWrite))
        {
            await Assert.That(() => provider.MoveFile(source, destination, overwrite: true)).Throws<IOException>();
        }

        await Assert.That(await provider.ReadAllTextAsync(source)).IsEqualTo("new");
        await Assert.That(await provider.ReadAllTextAsync(destination)).IsEqualTo("old");
    }

    public sealed class UnixOnlyAttribute() : SkipAttribute("Literal backslashes in filenames require Unix path semantics")
    {
        public override Task<bool> ShouldSkip(TestRegisteredContext context)
            => Task.FromResult(OperatingSystem.IsWindows());
    }

    private sealed class LiteralBackslashArtifactModule : Module<string>
    {
        protected override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            var source = context.Files.GetFolder("literal-source").Create();
            await source.GetFile(@"payload\data.txt").WriteAsync("literal filename content", cancellationToken);
            await context.Artifacts.PublishDirectoryAsync("literal", source, cancellationToken);
            var destination = context.Files.GetFolder("literal-destination");
            await context.Artifacts.DownloadAsync<LiteralBackslashArtifactModule>("literal", destination, cancellationToken);
            return await destination.GetFile(@"payload\data.txt").ReadAsync(cancellationToken);
        }
    }

    private sealed class RepeatedArtifactModule : Module<string>
    {
        protected override async Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            var source = context.Files.GetFolder("artifact-source").Create();
            await source.GetFile("payload.txt").WriteAsync("artifact content", cancellationToken);
            await context.Artifacts.PublishDirectoryAsync("directory", source, cancellationToken);
            var destination = context.Files.GetFolder("artifact-destination");
            await context.Artifacts.DownloadAsync<RepeatedArtifactModule>("directory", destination, cancellationToken);
            await destination.GetFile("payload.txt").WriteAsync("old", cancellationToken);
            await context.Artifacts.DownloadAsync<RepeatedArtifactModule>("directory", destination, cancellationToken);
            return await destination.GetFile("payload.txt").ReadAsync(cancellationToken);
        }
    }
}
