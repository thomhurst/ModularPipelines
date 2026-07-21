using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using ModularPipelines.Attributes;
using ModularPipelines.Distributed.Artifacts;
using ModularPipelines.Modules;

namespace ModularPipelines.Distributed.UnitTests.Artifacts;

[ProducesArtifact("build-output", "test-output")]
public class ProducerModule : Module<string>
{
    protected internal override Task<string?> ExecuteAsync(Context.IModuleContext context, CancellationToken cancellationToken)
        => Task.FromResult<string?>("done");
}

[ConsumesArtifact(typeof(ProducerModule), "build-output", RestorePath = "restored")]
public class ConsumerModule : Module<string>
{
    protected internal override Task<string?> ExecuteAsync(Context.IModuleContext context, CancellationToken cancellationToken)
        => Task.FromResult<string?>("done");
}

[ConsumesArtifact(typeof(ProducerModule), "build-output", RestorePath = "restored")]
public class SecondConsumerModule : Module<string>
{
    protected internal override Task<string?> ExecuteAsync(Context.IModuleContext context, CancellationToken cancellationToken)
        => Task.FromResult<string?>("done");
}

[ConsumesArtifact(typeof(ProducerModule), "build-output", RestorePath = "different-path")]
public class DifferentPathConsumerModule : Module<string>
{
    protected internal override Task<string?> ExecuteAsync(Context.IModuleContext context, CancellationToken cancellationToken)
        => Task.FromResult<string?>("done");
}

public class ArtifactLifecycleManagerTests
{
    [Test]
    public async Task UploadProducedArtifacts_Scans_Attributes()
    {
        var mockStore = new Mock<IDistributedArtifactStore>();
        var expectedRef = new ArtifactReference("id1", "build-output", typeof(ProducerModule).FullName!, 100, "application/octet-stream", DateTimeOffset.UtcNow);
        mockStore
            .Setup(s => s.UploadAsync(It.IsAny<ArtifactDescriptor>(), It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedRef);

        var options = Microsoft.Extensions.Options.Options.Create(new ArtifactOptions());
        var logger = Mock.Of<ILogger<ArtifactLifecycleManager>>();
        var manager = new ArtifactLifecycleManager(mockStore.Object, options, logger);

        // Create a temporary directory to match the path pattern
        var tempDir = Path.Combine(Path.GetTempPath(), "test-output");
        Directory.CreateDirectory(tempDir);
        File.WriteAllText(Path.Combine(tempDir, "test.txt"), "hello");

        try
        {
            // Change to temp parent so "test-output" resolves
            var originalDir = Directory.GetCurrentDirectory();
            Directory.SetCurrentDirectory(Path.GetTempPath());
            try
            {
                var refs = await manager.UploadProducedArtifactsAsync(typeof(ProducerModule), CancellationToken.None);
                await Assert.That(refs.Count).IsEqualTo(1);
                await Assert.That(refs[0].Name).IsEqualTo("build-output");
            }
            finally
            {
                Directory.SetCurrentDirectory(originalDir);
            }
        }
        finally
        {
            Directory.Delete(tempDir, true);
        }
    }

    [Test]
    public async Task UploadProducedArtifacts_Returns_Empty_When_No_Attributes()
    {
        var mockStore = new Mock<IDistributedArtifactStore>();
        var options = Microsoft.Extensions.Options.Options.Create(new ArtifactOptions());
        var logger = Mock.Of<ILogger<ArtifactLifecycleManager>>();
        var manager = new ArtifactLifecycleManager(mockStore.Object, options, logger);

        // Module<string> has no ProducesArtifact attribute
        var refs = await manager.UploadProducedArtifactsAsync(typeof(Module<string>), CancellationToken.None);

        await Assert.That(refs.Count).IsEqualTo(0);
        mockStore.Verify(s => s.UploadAsync(It.IsAny<ArtifactDescriptor>(), It.IsAny<Stream>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    public async Task DownloadConsumedArtifacts_Scans_ConsumesAttribute()
    {
        var mockStore = new Mock<IDistributedArtifactStore>();
        var artifactRef = new ArtifactReference("id1", "build-output", typeof(ProducerModule).FullName!, 100, "application/octet-stream", DateTimeOffset.UtcNow);

        mockStore
            .Setup(s => s.ListArtifactsAsync(typeof(ProducerModule).FullName!, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<ArtifactReference> { artifactRef });

        mockStore
            .Setup(s => s.DownloadAsync(artifactRef, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new MemoryStream(new byte[] { 1, 2, 3 }));

        var options = Microsoft.Extensions.Options.Options.Create(new ArtifactOptions());
        var logger = Mock.Of<ILogger<ArtifactLifecycleManager>>();
        var manager = new ArtifactLifecycleManager(mockStore.Object, options, logger);

        await manager.DownloadConsumedArtifactsAsync(typeof(ConsumerModule), CancellationToken.None);

        mockStore.Verify(s => s.ListArtifactsAsync(typeof(ProducerModule).FullName!, It.IsAny<CancellationToken>()), Times.Once);
        mockStore.Verify(s => s.DownloadAsync(artifactRef, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task DownloadConsumedArtifacts_NoOp_When_No_Attributes()
    {
        var mockStore = new Mock<IDistributedArtifactStore>();
        var options = Microsoft.Extensions.Options.Options.Create(new ArtifactOptions());
        var logger = Mock.Of<ILogger<ArtifactLifecycleManager>>();
        var manager = new ArtifactLifecycleManager(mockStore.Object, options, logger);

        await manager.DownloadConsumedArtifactsAsync(typeof(Module<string>), CancellationToken.None);

        mockStore.Verify(s => s.ListArtifactsAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    public async Task DownloadConsumedArtifacts_Deduplicates_Same_Artifact_Same_Path()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), $"dedup-test-{Guid.NewGuid():N}");
        Directory.CreateDirectory(tempDir);
        try
        {
            var mockStore = new Mock<IDistributedArtifactStore>();
            var artifactRef = new ArtifactReference("id1", "build-output", typeof(ProducerModule).FullName!, 100, "application/octet-stream", DateTimeOffset.UtcNow);

            mockStore
                .Setup(s => s.ListArtifactsAsync(typeof(ProducerModule).FullName!, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ArtifactReference> { artifactRef });

            mockStore
                .Setup(s => s.DownloadAsync(artifactRef, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MemoryStream(new byte[] { 1, 2, 3 }));

            var options = Microsoft.Extensions.Options.Options.Create(new ArtifactOptions());
            var logger = Mock.Of<ILogger<ArtifactLifecycleManager>>();
            var manager = new ArtifactLifecycleManager(mockStore.Object, options, logger);

            // Simulate two modules consuming same artifact to same absolute path.
            // We call twice with the same resolved restorePath to exercise the dedup logic.
            var restorePath = Path.Combine(tempDir, "restored");
            await manager.DownloadConsumedArtifactsForPathAsync(typeof(ProducerModule).FullName!, "build-output", restorePath, typeof(ConsumerModule), CancellationToken.None);
            await manager.DownloadConsumedArtifactsForPathAsync(typeof(ProducerModule).FullName!, "build-output", restorePath, typeof(SecondConsumerModule), CancellationToken.None);

            // Download should only happen once
            mockStore.Verify(s => s.DownloadAsync(artifactRef, It.IsAny<CancellationToken>()), Times.Once);
        }
        finally
        {
            Directory.Delete(tempDir, true);
        }
    }

    [Test]
    public async Task DownloadConsumedArtifacts_Downloads_Again_For_Different_Path()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), $"diffpath-test-{Guid.NewGuid():N}");
        Directory.CreateDirectory(tempDir);
        try
        {
            var mockStore = new Mock<IDistributedArtifactStore>();
            var artifactRef = new ArtifactReference("id1", "build-output", typeof(ProducerModule).FullName!, 100, "application/octet-stream", DateTimeOffset.UtcNow);

            mockStore
                .Setup(s => s.ListArtifactsAsync(typeof(ProducerModule).FullName!, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ArtifactReference> { artifactRef });

            var callCount = 0;
            mockStore
                .Setup(s => s.DownloadAsync(artifactRef, It.IsAny<CancellationToken>()))
                .ReturnsAsync(() =>
                {
                    callCount++;
                    return new MemoryStream(new byte[] { 1, 2, 3 });
                });

            var options = Microsoft.Extensions.Options.Options.Create(new ArtifactOptions());
            var logger = Mock.Of<ILogger<ArtifactLifecycleManager>>();
            var manager = new ArtifactLifecycleManager(mockStore.Object, options, logger);

            var pathA = Path.Combine(tempDir, "restored");
            var pathB = Path.Combine(tempDir, "different-path");
            await manager.DownloadConsumedArtifactsForPathAsync(typeof(ProducerModule).FullName!, "build-output", pathA, typeof(ConsumerModule), CancellationToken.None);
            await manager.DownloadConsumedArtifactsForPathAsync(typeof(ProducerModule).FullName!, "build-output", pathB, typeof(DifferentPathConsumerModule), CancellationToken.None);

            // Download should happen twice — different restore paths
            await Assert.That(callCount).IsEqualTo(2);
        }
        finally
        {
            Directory.Delete(tempDir, true);
        }
    }

    [Test]
    public async Task DownloadConsumedArtifacts_Concurrent_Same_Path_Downloads_Once()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), $"concurrent-test-{Guid.NewGuid():N}");
        Directory.CreateDirectory(tempDir);
        try
        {
            var mockStore = new Mock<IDistributedArtifactStore>();
            var artifactRef = new ArtifactReference("id1", "build-output", typeof(ProducerModule).FullName!, 100, "application/octet-stream", DateTimeOffset.UtcNow);

            mockStore
                .Setup(s => s.ListArtifactsAsync(typeof(ProducerModule).FullName!, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ArtifactReference> { artifactRef });

            var downloadCount = 0;
            mockStore
                .Setup(s => s.DownloadAsync(artifactRef, It.IsAny<CancellationToken>()))
                .ReturnsAsync(() =>
                {
                    Interlocked.Increment(ref downloadCount);
                    return new MemoryStream(new byte[] { 1, 2, 3 });
                });

            var options = Microsoft.Extensions.Options.Options.Create(new ArtifactOptions());
            var logger = Mock.Of<ILogger<ArtifactLifecycleManager>>();
            var manager = new ArtifactLifecycleManager(mockStore.Object, options, logger);

            var restorePath = Path.Combine(tempDir, "restored");

            // Both run concurrently targeting the same path
            var task1 = manager.DownloadConsumedArtifactsForPathAsync(typeof(ProducerModule).FullName!, "build-output", restorePath, typeof(ConsumerModule), CancellationToken.None);
            var task2 = manager.DownloadConsumedArtifactsForPathAsync(typeof(ProducerModule).FullName!, "build-output", restorePath, typeof(SecondConsumerModule), CancellationToken.None);

            await Task.WhenAll(task1, task2);

            // Only one download despite concurrent requests
            await Assert.That(downloadCount).IsEqualTo(1);
        }
        finally
        {
            Directory.Delete(tempDir, true);
        }
    }
}
