using System.IO.Compression;
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
    protected internal override Task<string> ExecuteAsync(ModularPipelines.IModuleContext context, CancellationToken cancellationToken)
        => Task.FromResult<string>("done");
}

[ProducesArtifact("single-glob-output", "single-match-*")]
public class SingleGlobProducerModule : Module<string>
{
    protected internal override Task<string> ExecuteAsync(ModularPipelines.IModuleContext context, CancellationToken cancellationToken)
        => Task.FromResult("done");
}

[ProducesArtifact("single-file-glob-output", "release-*/manifest.json")]
public class SingleFileGlobProducerModule : Module<string>
{
    protected internal override Task<string> ExecuteAsync(ModularPipelines.IModuleContext context, CancellationToken cancellationToken)
        => Task.FromResult("done");
}

[ConsumesArtifact(typeof(ProducerModule), "build-output", RestorePath = "restored")]
public class ConsumerModule : Module<string>
{
    protected internal override Task<string> ExecuteAsync(ModularPipelines.IModuleContext context, CancellationToken cancellationToken)
        => Task.FromResult<string>("done");
}

[ConsumesArtifact(typeof(ProducerModule), "build-output", RestorePath = "restored")]
public class SecondConsumerModule : Module<string>
{
    protected internal override Task<string> ExecuteAsync(ModularPipelines.IModuleContext context, CancellationToken cancellationToken)
        => Task.FromResult<string>("done");
}

[ConsumesArtifact(typeof(ProducerModule), "build-output", RestorePath = "different-path")]
public class DifferentPathConsumerModule : Module<string>
{
    protected internal override Task<string> ExecuteAsync(ModularPipelines.IModuleContext context, CancellationToken cancellationToken)
        => Task.FromResult<string>("done");
}

public class ArtifactLifecycleManagerTests
{
    [Test]
    [Arguments(typeof(ProducerModule), "test-output/output.txt", CompressionLevel.NoCompression)]
    [Arguments(typeof(ProducerModule), "test-output/output.txt", CompressionLevel.Optimal)]
    [Arguments(typeof(SingleGlobProducerModule), "single-match-directory/output.txt", CompressionLevel.NoCompression)]
    [Arguments(typeof(SingleGlobProducerModule), "single-match-directory/output.txt", CompressionLevel.Optimal)]
    [Arguments(typeof(SingleFileGlobProducerModule), "release-v1/manifest.json", CompressionLevel.NoCompression)]
    [Arguments(typeof(SingleFileGlobProducerModule), "release-v1/manifest.json", CompressionLevel.Optimal)]
    public async Task UploadProducedArtifacts_Uses_Distributed_Compression_Level(
        Type moduleType, string relativePath, CompressionLevel compressionLevel)
    {
        var workingDirectory = Directory.CreateTempSubdirectory("artifact-compression-");
        try
        {
            var filePath = Path.Combine(workingDirectory.FullName, relativePath);
            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
            await File.WriteAllTextAsync(filePath, new string('x', 8192));
            long? compressedLength = null;
            long? originalLength = null;
            var store = new Mock<IDistributedArtifactStore>();
            store.Setup(value => value.UploadAsync(
                    It.IsAny<ArtifactDescriptor>(), It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .Returns((ArtifactDescriptor descriptor, Stream stream, CancellationToken _) =>
                {
                    using var archive = new ZipArchive(stream, ZipArchiveMode.Read, leaveOpen: true);
                    var entry = archive.Entries.Single(entry => entry.Length > 0);
                    compressedLength = entry.CompressedLength;
                    originalLength = entry.Length;
                    return Task.FromResult(Reference("compressed", DateTimeOffset.UtcNow));
                });
            var manager = new ArtifactLifecycleManager(
                store.Object,
                Microsoft.Extensions.Options.Options.Create(new DistributedOptions
                {
                    ArtifactCompressionLevel = compressionLevel,
                }),
                Mock.Of<ILogger<ArtifactLifecycleManager>>(),
                workingDirectory.FullName);

            await manager.UploadProducedArtifactsAsync(moduleType, CancellationToken.None);

            await Assert.That(originalLength).IsEqualTo(8192);
            if (compressionLevel == CompressionLevel.NoCompression)
            {
                await Assert.That(compressedLength).IsEqualTo(originalLength);
            }
            else
            {
                await Assert.That(compressedLength!.Value).IsLessThan(originalLength!.Value);
            }
        }
        finally
        {
            workingDirectory.Delete(recursive: true);
        }
    }

    [Test]
    public async Task Consumer_Downloads_The_Reference_From_The_Accepted_Producer_Result()
    {
        var accepted = Reference("accepted", DateTimeOffset.UtcNow.AddHours(-1));
        var newerUpload = Reference("superseded", DateTimeOffset.UtcNow);
        var registry = new AcceptedArtifactRegistry();
        registry.Record(ModuleId.FromType(typeof(ProducerModule)), [accepted]);
        var store = new Mock<IDistributedArtifactStore>();
        store.Setup(x => x.ListArtifactsAsync(It.IsAny<ModuleId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([accepted, newerUpload]);

        var resolved = await ArtifactLifecycleManager.ResolveArtifactAsync(
            store.Object,
            registry,
            ModuleId.FromType(typeof(ProducerModule)),
            "build-output",
            CancellationToken.None);

        await Assert.That(resolved).IsSameReferenceAs(accepted);
        store.Verify(x => x.ListArtifactsAsync(It.IsAny<ModuleId>(), It.IsAny<CancellationToken>()), Times.Never());
    }

    [Test]
    public async Task Accepted_Result_Without_The_Artifact_Means_It_Is_Missing()
    {
        var registry = new AcceptedArtifactRegistry();
        registry.Record(ModuleId.FromType(typeof(ProducerModule)), []);
        var store = new Mock<IDistributedArtifactStore>();
        store.Setup(x => x.ListArtifactsAsync(It.IsAny<ModuleId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([Reference("stale", DateTimeOffset.UtcNow)]);

        var resolved = await ArtifactLifecycleManager.ResolveArtifactAsync(
            store.Object,
            registry,
            ModuleId.FromType(typeof(ProducerModule)),
            "build-output",
            CancellationToken.None);

        await Assert.That(resolved).IsNull();
    }

    [Test]
    public async Task Required_Artifact_Check_Reports_Missing_Artifacts()
    {
        await Assert.That(() => ArtifactLifecycleManager.EnsureRequiredArtifactsProduced(
                typeof(ProducerModule),
                [],
                ["build-output"]))
            .Throws<InvalidOperationException>();
        ArtifactLifecycleManager.EnsureRequiredArtifactsProduced(
            typeof(ProducerModule),
            [Reference("id", DateTimeOffset.UtcNow)],
            ["build-output"]);
    }

    private static ArtifactReference Reference(string id, DateTimeOffset uploadedAt) => new()
    {
        ArtifactId = id,
        Name = "build-output",
        ModuleId = ModuleId.FromType(typeof(ProducerModule)),
        SizeBytes = 1,
        UploadedAt = uploadedAt,
    };

    [Test]
    public async Task UploadProducedArtifacts_Preserves_Single_Glob_File_Path()
    {
        var workingDirectory = Directory.CreateTempSubdirectory("artifact-single-file-glob-test-");
        var releaseDirectory = Directory.CreateDirectory(
            Path.Combine(workingDirectory.FullName, "release-v1"));
        await File.WriteAllTextAsync(Path.Combine(releaseDirectory.FullName, "manifest.json"), "release");

        ArtifactDescriptor? uploadedDescriptor = null;
        IReadOnlyList<string>? archivedEntries = null;
        var expectedReference = new ArtifactReference
        {
            ArtifactId = "id1",
            Name = "single-file-glob-output",
            ModuleId = typeof(SingleFileGlobProducerModule).FullName!,
            SizeBytes = 100,
            ContentType = "application/zip",
            UploadedAt = DateTimeOffset.UtcNow,
        };
        var mockStore = new Mock<IDistributedArtifactStore>();
        mockStore
            .Setup(store => store.UploadAsync(
                It.IsAny<ArtifactDescriptor>(),
                It.IsAny<Stream>(),
                It.IsAny<CancellationToken>()))
            .Returns((ArtifactDescriptor descriptor, Stream stream, CancellationToken _) =>
            {
                uploadedDescriptor = descriptor;
                using var archive = new ZipArchive(stream, ZipArchiveMode.Read, leaveOpen: true);
                archivedEntries = [.. archive.Entries.Select(entry => entry.FullName)];
                return Task.FromResult(expectedReference);
            });

        var manager = new ArtifactLifecycleManager(
            mockStore.Object,
            Microsoft.Extensions.Options.Options.Create(new DistributedOptions()),
            Mock.Of<ILogger<ArtifactLifecycleManager>>(),
            workingDirectory.FullName);

        try
        {
            await manager.UploadProducedArtifactsAsync(
                typeof(SingleFileGlobProducerModule),
                CancellationToken.None);

            using (Assert.Multiple())
            {
                await Assert.That(uploadedDescriptor!.ContentType).IsEqualTo("application/zip");
                await Assert.That(archivedEntries).IsEquivalentTo(["release-v1/manifest.json"]);
            }
        }
        finally
        {
            workingDirectory.Delete(recursive: true);
        }
    }

    [Test]
    public async Task UploadProducedArtifacts_Preserves_Single_Glob_Directory_Root()
    {
        var workingDirectory = Directory.CreateTempSubdirectory("artifact-single-glob-test-");
        var matchedDirectory = Path.Combine(workingDirectory.FullName, "single-match-directory");
        Directory.CreateDirectory(Path.Combine(matchedDirectory, "empty-child"));
        File.WriteAllText(Path.Combine(matchedDirectory, "output.txt"), "hello");

        IReadOnlyList<string>? archivedEntries = null;
        var expectedReference = new ArtifactReference
        {
            ArtifactId = "id1",
            Name = "single-glob-output",
            ModuleId = typeof(SingleGlobProducerModule).FullName!,
            SizeBytes = 100,
            ContentType = "application/zip",
            UploadedAt = DateTimeOffset.UtcNow,
        };
        var mockStore = new Mock<IDistributedArtifactStore>();
        mockStore
            .Setup(store => store.UploadAsync(
                It.IsAny<ArtifactDescriptor>(),
                It.IsAny<Stream>(),
                It.IsAny<CancellationToken>()))
            .Returns((ArtifactDescriptor _, Stream stream, CancellationToken _) =>
            {
                using var archive = new ZipArchive(stream, ZipArchiveMode.Read, leaveOpen: true);
                archivedEntries = [.. archive.Entries.Select(entry => entry.FullName)];
                return Task.FromResult(expectedReference);
            });

        var manager = new ArtifactLifecycleManager(
            mockStore.Object,
            Microsoft.Extensions.Options.Options.Create(new DistributedOptions()),
            Mock.Of<ILogger<ArtifactLifecycleManager>>(),
            workingDirectory.FullName);

        try
        {
            await manager.UploadProducedArtifactsAsync(
                typeof(SingleGlobProducerModule),
                CancellationToken.None);

            await Assert.That(archivedEntries).IsEquivalentTo(
            [
                "single-match-directory/",
                "single-match-directory/empty-child/",
                "single-match-directory/output.txt",
            ]);
        }
        finally
        {
            workingDirectory.Delete(recursive: true);
        }
    }

    [Test]
    public async Task UploadProducedArtifacts_Scans_Attributes()
    {
        var workingDirectory = Path.Combine(
            Path.GetTempPath(),
            $"artifact-upload-test-{Guid.NewGuid():N}");
        var artifactDirectory = Path.Combine(workingDirectory, "test-output");
        Directory.CreateDirectory(artifactDirectory);
        File.WriteAllText(Path.Combine(artifactDirectory, "test.txt"), "hello");

        var mockStore = new Mock<IDistributedArtifactStore>();
        var expectedRef = new ArtifactReference
        {
            ArtifactId = "id1",
            Name = "build-output",
            ModuleId = typeof(ProducerModule).FullName!,
            SizeBytes = 100,
            ContentType = "application/octet-stream",
            UploadedAt = DateTimeOffset.UtcNow,
        };
        mockStore
            .Setup(s => s.UploadAsync(It.IsAny<ArtifactDescriptor>(), It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedRef);

        var options = Microsoft.Extensions.Options.Options.Create(new DistributedOptions());
        var logger = Mock.Of<ILogger<ArtifactLifecycleManager>>();
        var manager = new ArtifactLifecycleManager(
            mockStore.Object,
            options,
            logger,
            workingDirectory);

        try
        {
            var refs = await manager.UploadProducedArtifactsAsync(
                typeof(ProducerModule),
                CancellationToken.None);
            await Assert.That(refs.Count).IsEqualTo(1);
            await Assert.That(refs[0].Name).IsEqualTo("build-output");
        }
        finally
        {
            Directory.Delete(workingDirectory, true);
        }
    }

    [Test]
    public async Task UploadProducedArtifacts_Returns_Empty_When_No_Attributes()
    {
        var mockStore = new Mock<IDistributedArtifactStore>();
        var options = Microsoft.Extensions.Options.Options.Create(new DistributedOptions());
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
        var workingDirectory = Path.Combine(
            Path.GetTempPath(),
            $"artifact-download-test-{Guid.NewGuid():N}");
        Directory.CreateDirectory(workingDirectory);
        var mockStore = new Mock<IDistributedArtifactStore>();
        var artifactRef = new ArtifactReference
        {
            ArtifactId = "id1",
            Name = "build-output",
            ModuleId = typeof(ProducerModule).FullName!,
            SizeBytes = 100,
            ContentType = "application/octet-stream",
            UploadedAt = DateTimeOffset.UtcNow,
        };

        mockStore
            .Setup(s => s.ListArtifactsAsync(typeof(ProducerModule).FullName!, It.IsAny<CancellationToken>()))
            .ReturnsAsync([artifactRef]);

        mockStore
            .Setup(s => s.DownloadAsync(artifactRef, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new MemoryStream([1, 2, 3]));

        var options = Microsoft.Extensions.Options.Options.Create(new DistributedOptions());
        var logger = Mock.Of<ILogger<ArtifactLifecycleManager>>();
        var manager = new ArtifactLifecycleManager(
            mockStore.Object,
            options,
            logger,
            workingDirectory);

        try
        {
            await manager.DownloadConsumedArtifactsAsync(
                typeof(ConsumerModule),
                CancellationToken.None);

            mockStore.Verify(s => s.ListArtifactsAsync(typeof(ProducerModule).FullName!, It.IsAny<CancellationToken>()), Times.Once);
            mockStore.Verify(s => s.DownloadAsync(artifactRef, It.IsAny<CancellationToken>()), Times.Once);
        }
        finally
        {
            Directory.Delete(workingDirectory, true);
        }
    }

    [Test]
    public async Task DownloadConsumedArtifacts_NoOp_When_No_Attributes()
    {
        var mockStore = new Mock<IDistributedArtifactStore>();
        var options = Microsoft.Extensions.Options.Options.Create(new DistributedOptions());
        var logger = Mock.Of<ILogger<ArtifactLifecycleManager>>();
        var manager = new ArtifactLifecycleManager(mockStore.Object, options, logger);

        await manager.DownloadConsumedArtifactsAsync(typeof(Module<string>), CancellationToken.None);

        mockStore.Verify(s => s.ListArtifactsAsync(It.IsAny<ModuleId>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    public async Task Restore_Cache_Distinguishes_Colons_In_Module_And_Artifact_Ids()
    {
        var store = new Mock<IDistributedArtifactStore>();
        store.Setup(instance => instance.ListArtifactsAsync(It.IsAny<ModuleId>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);
        var manager = new ArtifactLifecycleManager(store.Object,
            Microsoft.Extensions.Options.Options.Create(new DistributedOptions()),
            Mock.Of<ILogger<ArtifactLifecycleManager>>());
        await manager.DownloadConsumedArtifactsForPathAsync("a:b", "c", Path.GetTempPath(),
            typeof(ConsumerModule), CancellationToken.None);
        await manager.DownloadConsumedArtifactsForPathAsync("a", "b:c", Path.GetTempPath(),
            typeof(ConsumerModule), CancellationToken.None);
        store.Verify(instance => instance.ListArtifactsAsync("a:b", It.IsAny<CancellationToken>()), Times.Once());
        store.Verify(instance => instance.ListArtifactsAsync("a", It.IsAny<CancellationToken>()), Times.Once());
    }

    [Test]
    public async Task DownloadConsumedArtifacts_Deduplicates_Same_Artifact_Same_Path()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), $"dedup-test-{Guid.NewGuid():N}");
        Directory.CreateDirectory(tempDir);
        try
        {
            var mockStore = new Mock<IDistributedArtifactStore>();
            var artifactRef = new ArtifactReference
            {
                ArtifactId = "id1",
                Name = "build-output",
                ModuleId = typeof(ProducerModule).FullName!,
                SizeBytes = 100,
                ContentType = "application/octet-stream",
                UploadedAt = DateTimeOffset.UtcNow,
            };

            mockStore
                .Setup(s => s.ListArtifactsAsync(typeof(ProducerModule).FullName!, It.IsAny<CancellationToken>()))
                .ReturnsAsync([artifactRef]);

            mockStore
                .Setup(s => s.DownloadAsync(artifactRef, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new MemoryStream([1, 2, 3]));

            var options = Microsoft.Extensions.Options.Options.Create(new DistributedOptions());
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
            var artifactRef = new ArtifactReference
            {
                ArtifactId = "id1",
                Name = "build-output",
                ModuleId = typeof(ProducerModule).FullName!,
                SizeBytes = 100,
                ContentType = "application/octet-stream",
                UploadedAt = DateTimeOffset.UtcNow,
            };

            mockStore
                .Setup(s => s.ListArtifactsAsync(typeof(ProducerModule).FullName!, It.IsAny<CancellationToken>()))
                .ReturnsAsync([artifactRef]);

            var callCount = 0;
            mockStore
                .Setup(s => s.DownloadAsync(artifactRef, It.IsAny<CancellationToken>()))
                .ReturnsAsync(() =>
                {
                    callCount++;
                    return new MemoryStream([1, 2, 3]);
                });

            var options = Microsoft.Extensions.Options.Options.Create(new DistributedOptions());
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
            var artifactRef = new ArtifactReference
            {
                ArtifactId = "id1",
                Name = "build-output",
                ModuleId = typeof(ProducerModule).FullName!,
                SizeBytes = 100,
                ContentType = "application/octet-stream",
                UploadedAt = DateTimeOffset.UtcNow,
            };

            mockStore
                .Setup(s => s.ListArtifactsAsync(typeof(ProducerModule).FullName!, It.IsAny<CancellationToken>()))
                .ReturnsAsync([artifactRef]);

            var downloadCount = 0;
            mockStore
                .Setup(s => s.DownloadAsync(artifactRef, It.IsAny<CancellationToken>()))
                .ReturnsAsync(() =>
                {
                    Interlocked.Increment(ref downloadCount);
                    return new MemoryStream([1, 2, 3]);
                });

            var options = Microsoft.Extensions.Options.Options.Create(new DistributedOptions());
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
