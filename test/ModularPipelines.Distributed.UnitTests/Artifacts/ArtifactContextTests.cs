using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Distributed.Artifacts;
using ModularPipelines.Logging;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;
using Moq;

namespace ModularPipelines.Distributed.UnitTests.Artifacts;

public class ArtifactContextTests
{
    [Test]
    public async Task Publish_Uses_Ambient_Module_Type()
    {
        ArtifactDescriptor? observedDescriptor = null;
        var store = new Mock<IDistributedArtifactStore>();
        store.Setup(artifactStore => artifactStore.UploadAsync(
                It.IsAny<ArtifactDescriptor>(),
                It.IsAny<Stream>(),
                It.IsAny<CancellationToken>()))
            .Callback<ArtifactDescriptor, Stream, CancellationToken>(
                (descriptor, _, _) => observedDescriptor = descriptor)
            .ReturnsAsync(new ArtifactReference(
                ArtifactId: "artifact-id",
                Name: "output",
                ModuleTypeName: typeof(ProducerModule).FullName!,
                SizeBytes: 0,
                ContentType: "application/octet-stream",
                UploadedAt: DateTimeOffset.UtcNow));
        var file = Path.GetTempFileName();

        try
        {
            var builder = TestPipelineBuilder.Create()
                .AddModule<PipelineArtifactProducerModule>();
            builder.Services.AddSingleton<IDistributedArtifactStore>(store.Object);
            builder.Services.AddSingleton(new ArtifactPublishState(file));
            await using var pipeline = await builder.BuildAsync();

            _ = await pipeline.RunAsync();

            await Assert.That(observedDescriptor!.ModuleTypeName)
                .IsEqualTo(typeof(PipelineArtifactProducerModule).FullName);
        }
        finally
        {
            File.Delete(file);
        }
    }

    [Test]
    public async Task Typed_Download_Uses_Producer_Module_Name()
    {
        var artifact = new ArtifactReference(
            ArtifactId: "artifact-id",
            Name: "output",
            ModuleTypeName: typeof(ProducerModule).FullName!,
            SizeBytes: 7,
            ContentType: "application/octet-stream",
            UploadedAt: DateTimeOffset.UtcNow);
        var store = new Mock<IDistributedArtifactStore>();
        store.Setup(artifactStore => artifactStore.ListArtifactsAsync(
                typeof(ProducerModule).FullName!,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync([artifact]);
        store.Setup(artifactStore => artifactStore.DownloadAsync(
                artifact,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => new MemoryStream("content"u8.ToArray()));
        var context = new ArtifactContextImpl(store.Object, new ArtifactOptions());
        var destinationDirectory = Directory.CreateTempSubdirectory("artifact-context-");
        var destinationPath = Path.Combine(destinationDirectory.FullName, "output.txt");

        try
        {
            var result = await context.DownloadAsync<ProducerModule>("output", destinationPath);

            using (Assert.Multiple())
            {
                await Assert.That(result).IsEqualTo(destinationPath);
                await Assert.That(await File.ReadAllTextAsync(destinationPath)).IsEqualTo("content");
            }

            store.Verify(artifactStore => artifactStore.ListArtifactsAsync(
                typeof(ProducerModule).FullName!,
                It.IsAny<CancellationToken>()));
        }
        finally
        {
            destinationDirectory.Delete(recursive: true);
        }
    }

    [Test]
    public async Task Publish_Directory_Streams_From_Deleted_Temporary_Archive()
    {
        string? uploadedArchivePath = null;
        byte[]? uploadedContent = null;
        var store = new Mock<IDistributedArtifactStore>();
        store.Setup(artifactStore => artifactStore.UploadAsync(
                It.IsAny<ArtifactDescriptor>(),
                It.IsAny<Stream>(),
                It.IsAny<CancellationToken>()))
            .Returns<ArtifactDescriptor, Stream, CancellationToken>(async (descriptor, stream, cancellationToken) =>
            {
                uploadedArchivePath = (stream as FileStream)?.Name;
                using var copy = new MemoryStream();
                await stream.CopyToAsync(copy, cancellationToken);
                uploadedContent = copy.ToArray();
                return new ArtifactReference(
                    ArtifactId: "artifact-id",
                    Name: descriptor.Name,
                    ModuleTypeName: descriptor.ModuleTypeName,
                    SizeBytes: copy.Length,
                    ContentType: descriptor.ContentType,
                    UploadedAt: DateTimeOffset.UtcNow);
            });
        var context = new ArtifactContextImpl(store.Object, new ArtifactOptions());
        var sourceDirectory = Directory.CreateTempSubdirectory("artifact-context-source-");

        try
        {
            await File.WriteAllTextAsync(Path.Combine(sourceDirectory.FullName, "output.txt"), "content");
            using (new ModuleOutputContextScope(typeof(ProducerModule)))
            {
                await context.PublishDirectoryAsync("output", sourceDirectory.FullName, CancellationToken.None);
            }

            using var archiveStream = new MemoryStream(uploadedContent!);
            using var archive = new System.IO.Compression.ZipArchive(archiveStream, System.IO.Compression.ZipArchiveMode.Read);
            await Assert.That(uploadedArchivePath).IsNotNull();
            await Assert.That(File.Exists(uploadedArchivePath!)).IsFalse();
            await Assert.That(archive.GetEntry("output.txt")).IsNotNull();
        }
        finally
        {
            sourceDirectory.Delete(recursive: true);
        }
    }

    [Test]
    public async Task Download_Supports_Bare_Relative_File_Name()
    {
        var artifact = new ArtifactReference(
            ArtifactId: "artifact-id",
            Name: "output",
            ModuleTypeName: typeof(ProducerModule).FullName!,
            SizeBytes: 7,
            ContentType: "application/octet-stream",
            UploadedAt: DateTimeOffset.UtcNow);
        var store = new Mock<IDistributedArtifactStore>();
        store.Setup(artifactStore => artifactStore.ListArtifactsAsync(
                typeof(ProducerModule).FullName!,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync([artifact]);
        store.Setup(artifactStore => artifactStore.DownloadAsync(
                artifact,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => new MemoryStream("content"u8.ToArray()));
        var context = new ArtifactContextImpl(store.Object, new ArtifactOptions());
        var destinationPath = $"artifact-context-{Guid.NewGuid():N}.txt";

        try
        {
            var result = await context.DownloadAsync<ProducerModule>("output", destinationPath);

            await Assert.That(result).IsEqualTo(destinationPath);
            await Assert.That(await File.ReadAllTextAsync(destinationPath)).IsEqualTo("content");
        }
        finally
        {
            File.Delete(destinationPath);
        }
    }

    [Test]
    public async Task Download_Selects_Latest_Named_Artifact()
    {
        var firstAttempt = CreateArtifact("first", DateTimeOffset.UtcNow.AddMinutes(-1));
        var successfulRetry = CreateArtifact("retry", DateTimeOffset.UtcNow);
        var store = new Mock<IDistributedArtifactStore>();
        store.Setup(artifactStore => artifactStore.ListArtifactsAsync(
                typeof(ProducerModule).FullName!,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync([firstAttempt, successfulRetry]);
        store.Setup(artifactStore => artifactStore.DownloadAsync(
                successfulRetry,
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => new MemoryStream("current"u8.ToArray()));
        var context = new ArtifactContextImpl(store.Object, new ArtifactOptions());
        var destinationPath = Path.GetTempFileName();

        try
        {
            await context.DownloadAsync<ProducerModule>("output", destinationPath);

            await Assert.That(await File.ReadAllTextAsync(destinationPath)).IsEqualTo("current");
            store.Verify(artifactStore => artifactStore.DownloadAsync(
                successfulRetry,
                It.IsAny<CancellationToken>()));
        }
        finally
        {
            File.Delete(destinationPath);
        }
    }

    private static ArtifactReference CreateArtifact(string id, DateTimeOffset uploadedAt) =>
        new(
            ArtifactId: id,
            Name: "output",
            ModuleTypeName: typeof(ProducerModule).FullName!,
            SizeBytes: 7,
            ContentType: "application/octet-stream",
            UploadedAt: uploadedAt);

    private sealed class ProducerModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
            => Task.FromResult(string.Empty);
    }

    private sealed class PipelineArtifactProducerModule(ArtifactPublishState state) : Module<string>
    {
        protected internal override async Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
        {
            await context.Artifacts.PublishFileAsync(
                "output",
                state.FilePath,
                cancellationToken);
            return string.Empty;
        }
    }

    private sealed record ArtifactPublishState(string FilePath);
}
