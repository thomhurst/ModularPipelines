using ModularPipelines.Context;
using ModularPipelines.Distributed.Artifacts;
using ModularPipelines.Logging;
using ModularPipelines.Modules;
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
        IArtifactContext context = new ArtifactContextImpl(store.Object, new ArtifactOptions());
        var file = Path.GetTempFileName();
        var previousModuleType = ModuleLogger.CurrentModuleType.Value;

        try
        {
            ModuleLogger.CurrentModuleType.Value = typeof(ProducerModule);

            await context.PublishFileAsync("output", file);

            await Assert.That(observedDescriptor!.ModuleTypeName)
                .IsEqualTo(typeof(ProducerModule).FullName);
        }
        finally
        {
            ModuleLogger.CurrentModuleType.Value = previousModuleType;
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
        var previousModuleType = ModuleLogger.CurrentModuleType.Value;

        try
        {
            await File.WriteAllTextAsync(Path.Combine(sourceDirectory.FullName, "output.txt"), "content");
            ModuleLogger.CurrentModuleType.Value = typeof(ProducerModule);

            await context.PublishDirectoryAsync("output", sourceDirectory.FullName, CancellationToken.None);

            using var archiveStream = new MemoryStream(uploadedContent!);
            using var archive = new System.IO.Compression.ZipArchive(archiveStream, System.IO.Compression.ZipArchiveMode.Read);
            await Assert.That(uploadedArchivePath).IsNotNull();
            await Assert.That(File.Exists(uploadedArchivePath!)).IsFalse();
            await Assert.That(archive.GetEntry("output.txt")).IsNotNull();
        }
        finally
        {
            ModuleLogger.CurrentModuleType.Value = previousModuleType;
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

    private sealed class ProducerModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
            => Task.FromResult(string.Empty);
    }
}
