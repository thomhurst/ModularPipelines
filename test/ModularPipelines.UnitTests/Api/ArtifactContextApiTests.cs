using System.IO.Compression;
using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Context;
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Artifacts;
using ModularPipelines.Distributed.Extensions;
using ModularPipelines.Events;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Api;

public class ArtifactContextApiTests
{
    [Test]
    public async Task Artifact_Context_Is_A_Property_With_Optional_Cancellation_Tokens()
    {
        var artifactsProperty = typeof(IPipelineContext).GetProperty(nameof(IPipelineContext.Artifacts));
        var cancellationParameters = typeof(IArtifactContext)
            .GetMethods()
            .SelectMany(static method => method.GetParameters())
            .Where(static parameter => parameter.ParameterType == typeof(CancellationToken))
            .ToArray();

        using (Assert.Multiple())
        {
            await Assert.That(artifactsProperty).IsNotNull();
            await Assert.That(artifactsProperty!.PropertyType).IsEqualTo(typeof(IArtifactContext));
            await Assert.That(cancellationParameters).IsNotEmpty();
            await Assert.That(cancellationParameters.All(static parameter => parameter.IsOptional))
                .IsTrue();
            await Assert.That(typeof(IPipelineContext).Assembly.GetType(
                    "ModularPipelines.Distributed.Extensions.ArtifactContextExtensions"))
                .IsNull();
        }
    }

    [Test]
    public async Task Artifact_Store_Builder_Methods_Register_Symmetric_Services()
    {
        var storeBuilder = Pipeline.CreateBuilder()
            .AddDistributedArtifactStore<TestArtifactStore>();
        var factoryBuilder = Pipeline.CreateBuilder()
            .AddDistributedArtifactStoreFactory<TestArtifactStoreFactory>();

        using (Assert.Multiple())
        {
            await Assert.That(storeBuilder.Services.Any(descriptor =>
                    descriptor.ServiceType == typeof(IDistributedArtifactStore) &&
                    descriptor.ImplementationType == typeof(TestArtifactStore)))
                .IsTrue();
            await Assert.That(factoryBuilder.Services.Any(descriptor =>
                    descriptor.ServiceType == typeof(IDistributedArtifactStoreFactory) &&
                    descriptor.ImplementationType == typeof(TestArtifactStoreFactory)))
                .IsTrue();
        }
    }

    [Test]
    public async Task Distributed_Surface_Uses_TimeSpan_And_Run_Naming()
    {
        var assembly = typeof(DistributedOptions).Assembly;
        var typedDownload = typeof(IArtifactContext)
            .GetMethods()
            .Single(method => method.Name == nameof(IArtifactContext.DownloadAsync) && method.IsGenericMethod);

        using (Assert.Multiple())
        {
            await Assert.That(typeof(ArtifactOptions).GetProperty("TimeToLive")!.PropertyType)
                .IsEqualTo(typeof(TimeSpan));
            await Assert.That(typeof(ArtifactOptions).GetProperty("TimeToLiveSeconds")).IsNull();
            await Assert.That(typeof(DistributedOptions).GetProperty("CapabilityTimeout")!.PropertyType)
                .IsEqualTo(typeof(TimeSpan));
            await Assert.That(typeof(DistributedOptions).GetProperty("ModuleResultTimeout")!.PropertyType)
                .IsEqualTo(typeof(TimeSpan));
            await Assert.That(typeof(DistributedOptions).GetProperty("ExecutionIdentifier")).IsNull();
            await Assert.That(typeof(WorkerRegistration).GetProperty("ExecutionIdentifier")).IsNull();
            await Assert.That(typeof(WorkerRegistration).GetProperty("RunIdentifier")).IsNotNull();
            await Assert.That(assembly.GetType("ModularPipelines.Distributed.ModuleAssignmentConfig"))
                .IsNull();
            await Assert.That(assembly.GetType(
                    "ModularPipelines.Distributed.ModuleAssignmentConfiguration"))
                .IsNotNull();
            await Assert.That(typedDownload.GetGenericArguments()).HasSingleItem();
        }
    }

    [Test]
    public async Task Artifact_Store_Factory_Is_Active_Without_Distributed_Mode()
    {
        var builder = TestPipelineBuilder.Create()
            .AddModule<ArtifactTestModule>()
            .AddDistributedArtifactStoreFactory<TestArtifactStoreFactory>();

        await using var pipeline = await builder.BuildAsync();
        var factory = pipeline.Services.GetRequiredService<IDistributedArtifactStoreFactory>();
        var store = pipeline.Services.GetRequiredService<IDistributedArtifactStore>();

        _ = await store.ListArtifactsAsync("module", CancellationToken.None);

        await Assert.That(((TestArtifactStoreFactory) factory).CreateCount).IsEqualTo(1);
    }

    [Test]
    public async Task Direct_Artifact_Store_Overrides_Earlier_Factory()
    {
        var builder = TestPipelineBuilder.Create()
            .AddModule<ArtifactTestModule>()
            .AddDistributedArtifactStoreFactory<TestArtifactStoreFactory>()
            .AddDistributedArtifactStore<TestArtifactStore>();

        await using var pipeline = await builder.BuildAsync();

        using (Assert.Multiple())
        {
            await Assert.That(pipeline.Services.GetRequiredService<IDistributedArtifactStore>())
                .IsTypeOf<TestArtifactStore>();
            await Assert.That(pipeline.Services.GetService<IDistributedArtifactStoreFactory>())
                .IsNull();
        }
    }

    [Test]
    public async Task Direct_Service_Registration_Overrides_Earlier_Artifact_Store_Factory()
    {
        var store = new TestArtifactStore();
        var builder = TestPipelineBuilder.Create()
            .AddModule<ArtifactTestModule>()
            .AddDistributedArtifactStoreFactory<TestArtifactStoreFactory>();
        builder.Services.AddSingleton<IDistributedArtifactStore>(store);

        await using var pipeline = await builder.BuildAsync();

        await Assert.That(pipeline.Services.GetRequiredService<IDistributedArtifactStore>())
            .IsSameReferenceAs(store);
    }

    [Test]
    public async Task Artifact_Store_Factory_Disposes_Created_Store()
    {
        var builder = TestPipelineBuilder.Create()
            .AddModule<ArtifactTestModule>()
            .AddDistributedArtifactStoreFactory<TestArtifactStoreFactory>();
        var pipeline = await builder.BuildAsync();
        var factory = (TestArtifactStoreFactory) pipeline.Services
            .GetRequiredService<IDistributedArtifactStoreFactory>();
        var store = pipeline.Services.GetRequiredService<IDistributedArtifactStore>();

        _ = await store.ListArtifactsAsync("module", CancellationToken.None);
        await pipeline.DisposeAsync();

        await Assert.That(factory.CreatedStore!.DisposeCount).IsEqualTo(1);
    }

    [Test]
    public async Task Artifact_Store_Factory_Synchronous_Disposal_Disposes_Async_Store()
    {
        var builder = TestPipelineBuilder.Create()
            .AddModule<ArtifactTestModule>()
            .AddDistributedArtifactStoreFactory<TestArtifactStoreFactory>();
        var pipeline = await builder.BuildAsync();
        var factory = (TestArtifactStoreFactory) pipeline.Services
            .GetRequiredService<IDistributedArtifactStoreFactory>();
        var store = pipeline.Services.GetRequiredService<IDistributedArtifactStore>();

        _ = await store.ListArtifactsAsync("module", CancellationToken.None);
        ((IDisposable) store).Dispose();

        await Assert.That(factory.CreatedStore!.DisposeCount).IsEqualTo(1);
        await pipeline.DisposeAsync();
    }

    [Test]
    public async Task Artifact_Store_Factory_Does_Not_Initialize_During_Disposal()
    {
        var builder = TestPipelineBuilder.Create()
            .AddModule<ArtifactTestModule>()
            .AddDistributedArtifactStoreFactory<TestArtifactStoreFactory>();
        var pipeline = await builder.BuildAsync();
        var factory = (TestArtifactStoreFactory) pipeline.Services
            .GetRequiredService<IDistributedArtifactStoreFactory>();

        _ = pipeline.Services.GetRequiredService<IDistributedArtifactStore>();
        await pipeline.DisposeAsync();

        await Assert.That(factory.CreateCount).IsEqualTo(0);
    }

    [Test]
    public async Task Artifact_Store_Factory_Disposes_Synchronous_Store()
    {
        var builder = TestPipelineBuilder.Create()
            .AddModule<ArtifactTestModule>()
            .AddDistributedArtifactStoreFactory<TestDisposableArtifactStoreFactory>();
        var pipeline = await builder.BuildAsync();
        var factory = (TestDisposableArtifactStoreFactory) pipeline.Services
            .GetRequiredService<IDistributedArtifactStoreFactory>();
        var store = pipeline.Services.GetRequiredService<IDistributedArtifactStore>();

        _ = await store.ListArtifactsAsync("module", CancellationToken.None);
        await pipeline.DisposeAsync();

        await Assert.That(factory.CreatedStore!.DisposeCount).IsEqualTo(1);
    }

    [Test]
    public async Task Artifact_Store_Factory_Synchronizes_Creation_With_Disposal()
    {
        var builder = TestPipelineBuilder.Create()
            .AddModule<ArtifactTestModule>()
            .AddDistributedArtifactStoreFactory<BlockingArtifactStoreFactory>();
        var pipeline = await builder.BuildAsync();
        var factory = (BlockingArtifactStoreFactory) pipeline.Services
            .GetRequiredService<IDistributedArtifactStoreFactory>();
        var store = pipeline.Services.GetRequiredService<IDistributedArtifactStore>();

        var accessTask = store.ListArtifactsAsync("module", CancellationToken.None);
        await factory.CreationStarted;
        var disposalTask = pipeline.DisposeAsync().AsTask();
        factory.CompleteCreation();

        _ = await Assert.ThrowsAsync<ObjectDisposedException>(() => accessTask);
        await disposalTask;
        await Assert.That(factory.CreatedStore.DisposeCount).IsEqualTo(1);
    }

    [Test]
    public async Task Ready_Hook_Artifacts_Use_The_Hook_Module_Type()
    {
        var builder = TestPipelineBuilder.Create()
            .AddModule<ReadyArtifactModule>()
            .AddDistributedArtifactStore<TestArtifactStore>();

        await using var pipeline = await builder.BuildAsync();
        var store = (TestArtifactStore) pipeline.Services
            .GetRequiredService<IDistributedArtifactStore>();

        await pipeline.RunAsync();

        await Assert.That(store.UploadedDescriptor!.ModuleTypeName)
            .IsEqualTo(typeof(ReadyArtifactModule).FullName);
    }

    [Test]
    public async Task Directory_Archive_Excludes_Destination_Inside_Source()
    {
        var sourceDirectory = Directory.CreateTempSubdirectory("artifact-archive-").FullName;
        var archivePath = Path.Combine(sourceDirectory, "artifact.zip");
        await File.WriteAllTextAsync(Path.Combine(sourceDirectory, "payload.txt"), "payload");
        await File.WriteAllTextAsync(archivePath, "stale archive");

        try
        {
            await ArtifactContextImpl.CreateDirectoryArchiveAsync(
                sourceDirectory,
                archivePath,
                CompressionLevel.Fastest,
                CancellationToken.None);

            using var archive = ZipFile.OpenRead(archivePath);
            await Assert.That(archive.Entries.Select(static entry => entry.FullName))
                .IsEquivalentTo(["payload.txt"]);
        }
        finally
        {
            Directory.Delete(sourceDirectory, recursive: true);
        }
    }

    [Test]
    public async Task Directory_Archive_Excludes_Destination_Through_Source_Link()
    {
        var sourceDirectory = Directory.CreateTempSubdirectory("artifact-archive-source-");
        var linkDirectory = Directory.CreateTempSubdirectory("artifact-archive-link-");
        var sourceLink = Path.Combine(linkDirectory.FullName, "source");
        var archivePath = Path.Combine(sourceDirectory.FullName, "artifact.zip");
        Directory.CreateSymbolicLink(sourceLink, sourceDirectory.FullName);
        await File.WriteAllTextAsync(Path.Combine(sourceDirectory.FullName, "payload.txt"), "payload");

        try
        {
            await ArtifactContextImpl.CreateDirectoryArchiveAsync(
                sourceLink,
                archivePath,
                CompressionLevel.Fastest,
                CancellationToken.None);

            using var archive = ZipFile.OpenRead(archivePath);
            await Assert.That(archive.Entries.Select(static entry => entry.FullName))
                .IsEquivalentTo(["payload.txt"]);
        }
        finally
        {
            Directory.Delete(sourceLink);
            linkDirectory.Delete();
            sourceDirectory.Delete(recursive: true);
        }
    }

    [Test]
    public async Task Directory_Archive_Observes_Cancellation_During_Compression()
    {
        var sourceDirectory = Directory.CreateTempSubdirectory("artifact-archive-cancellation-");
        var archivePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid():N}.zip");
        var payloadPath = Path.Combine(sourceDirectory.FullName, "payload.bin");

        try
        {
            await using (var payload = File.Create(payloadPath))
            {
                payload.SetLength(64 * 1024 * 1024);
            }

            using var cancellationTokenSource = new CancellationTokenSource();
            var archiveTask = ArtifactContextImpl.CreateDirectoryArchiveAsync(
                sourceDirectory.FullName,
                archivePath,
                CompressionLevel.Optimal,
                cancellationTokenSource.Token);
            cancellationTokenSource.Cancel();

            await Assert.ThrowsAsync<OperationCanceledException>(() => archiveTask);
        }
        finally
        {
            File.Delete(archivePath);
            sourceDirectory.Delete(recursive: true);
        }
    }

    [Test]
    public async Task Directory_Archive_Extraction_Observes_Cancellation()
    {
        await using var archiveStream = new MemoryStream();
        using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, leaveOpen: true))
        await using (var entryStream = archive.CreateEntry(
                         "payload.bin",
                         CompressionLevel.NoCompression).Open())
        {
            await entryStream.WriteAsync(new byte[1024 * 1024]);
        }

        var archiveBytes = archiveStream.ToArray();
        for (var iteration = 0; iteration < 10; iteration++)
        {
            await VerifyArchiveExtractionCancellationAsync(archiveBytes, iteration);
        }
    }

    private static async Task VerifyArchiveExtractionCancellationAsync(
        byte[] archiveBytes,
        int iteration)
    {
        var destinationDirectory = Directory.CreateTempSubdirectory(
            $"artifact-extraction-{iteration}-");
        await using var archiveStream = new MemoryStream(archiveBytes, writable: false);
        await using var blockingStream = new BlockingReadStream(archiveStream);
        using var archiveToExtract = new ZipArchive(
            blockingStream,
            ZipArchiveMode.Read,
            leaveOpen: true);
        using var cancellationTokenSource = new CancellationTokenSource();
        blockingStream.BlockReads();
        var extractionTask = ArtifactContextImpl.ExtractDirectoryArchiveAsync(
            archiveToExtract,
            destinationDirectory.FullName,
            cancellationTokenSource.Token);

        try
        {
            await blockingStream.ReadStarted.WaitAsync(TimeSpan.FromSeconds(5));
            cancellationTokenSource.Cancel();
            await Assert.ThrowsAsync<OperationCanceledException>(() =>
                extractionTask);
        }
        finally
        {
            await cancellationTokenSource.CancelAsync();
            blockingStream.ReleaseReads();
            await extractionTask.ConfigureAwait(ConfigureAwaitOptions.SuppressThrowing);
            destinationDirectory.Delete(recursive: true);
        }
    }

    [Test]
    public async Task Directory_Archive_Extraction_Rejects_Traversal()
    {
        await using var archiveStream = new MemoryStream();
        using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, leaveOpen: true))
        {
            archive.CreateEntry("../outside.txt");
        }

        archiveStream.Position = 0;
        using var archiveToExtract = new ZipArchive(archiveStream, ZipArchiveMode.Read);
        var destinationDirectory = Directory.CreateTempSubdirectory("artifact-extraction-");

        try
        {
            await Assert.ThrowsAsync<IOException>(() =>
                ArtifactContextImpl.ExtractDirectoryArchiveAsync(
                    archiveToExtract,
                    destinationDirectory.FullName,
                    CancellationToken.None));
        }
        finally
        {
            destinationDirectory.Delete(recursive: true);
        }
    }

    [Test]
    public async Task Directory_Archive_Extraction_Rejects_Linked_Parent()
    {
        await using var archiveStream = new MemoryStream();
        using (var archive = new ZipArchive(archiveStream, ZipArchiveMode.Create, leaveOpen: true))
        await using (var entryStream = archive.CreateEntry("link/payload.txt").Open())
        await using (var writer = new StreamWriter(entryStream))
        {
            await writer.WriteAsync("payload");
        }

        archiveStream.Position = 0;
        using var archiveToExtract = new ZipArchive(archiveStream, ZipArchiveMode.Read);
        var destinationDirectory = Directory.CreateTempSubdirectory("artifact-extraction-");
        var outsideDirectory = Directory.CreateTempSubdirectory("artifact-outside-");
        var linkedDirectory = Path.Combine(destinationDirectory.FullName, "link");
        Directory.CreateSymbolicLink(linkedDirectory, outsideDirectory.FullName);

        try
        {
            await Assert.ThrowsAsync<IOException>(() =>
                ArtifactContextImpl.ExtractDirectoryArchiveAsync(
                    archiveToExtract,
                    destinationDirectory.FullName,
                    CancellationToken.None));
            await Assert.That(File.Exists(Path.Combine(outsideDirectory.FullName, "payload.txt")))
                .IsFalse();
        }
        finally
        {
            Directory.Delete(linkedDirectory);
            destinationDirectory.Delete();
            outsideDirectory.Delete(recursive: true);
        }
    }

    [Test]
    public async Task Directory_Archive_Path_Comparison_Is_Case_Insensitive_Only_On_Windows()
    {
        var comparison = ArtifactContextImpl.GetArchivePathComparison();
        var expected = OperatingSystem.IsWindows()
            ? StringComparison.OrdinalIgnoreCase
            : StringComparison.Ordinal;

        await Assert.That(comparison).IsEqualTo(expected);
    }

    private sealed class ArtifactTestModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
            => Task.FromResult(string.Empty);
    }

    private sealed class BlockingReadStream(Stream inner) : Stream
    {
        private readonly TaskCompletionSource _readStarted = new(
            TaskCreationOptions.RunContinuationsAsynchronously);
        private readonly TaskCompletionSource _releaseReads = new(
            TaskCreationOptions.RunContinuationsAsynchronously);
        private bool _blockReads;

        public Task ReadStarted => _readStarted.Task;

        public override bool CanRead => inner.CanRead;

        public override bool CanSeek => inner.CanSeek;

        public override bool CanWrite => inner.CanWrite;

        public override long Length => inner.Length;

        public override long Position
        {
            get => inner.Position;
            set => inner.Position = value;
        }

        public void BlockReads() => _blockReads = true;

        public void ReleaseReads() => _releaseReads.TrySetResult();

        public override void Flush() => inner.Flush();

        public override int Read(byte[] buffer, int offset, int count) =>
            inner.Read(buffer, offset, count);

        public override async Task<int> ReadAsync(
            byte[] buffer,
            int offset,
            int count,
            CancellationToken cancellationToken)
        {
            await WaitForReleaseAsync(cancellationToken);
            return await inner.ReadAsync(buffer, offset, count, cancellationToken);
        }

        public override async ValueTask<int> ReadAsync(
            Memory<byte> buffer,
            CancellationToken cancellationToken = default)
        {
            await WaitForReleaseAsync(cancellationToken);
            return await inner.ReadAsync(buffer, cancellationToken);
        }

        public override long Seek(long offset, SeekOrigin origin) => inner.Seek(offset, origin);

        public override void SetLength(long value) => inner.SetLength(value);

        public override void Write(byte[] buffer, int offset, int count) =>
            inner.Write(buffer, offset, count);

        private Task WaitForReleaseAsync(CancellationToken cancellationToken)
        {
            if (!_blockReads)
            {
                return Task.CompletedTask;
            }

            _readStarted.TrySetResult();
            return _releaseReads.Task.WaitAsync(cancellationToken);
        }
    }

    [PublishArtifactOnReady]
    private sealed class ReadyArtifactModule : Module<string>
    {
        protected internal override Task<string> ExecuteAsync(
            IModuleContext context,
            CancellationToken cancellationToken)
            => Task.FromResult(string.Empty);
    }

    private sealed class PublishArtifactOnReadyAttribute : Attribute, IModuleReadyHandler
    {
        public async Task OnModuleReadyAsync(IModuleHookContext context)
        {
            var path = Path.GetTempFileName();
            try
            {
                await context.Artifacts.PublishFileAsync("ready", path);
            }
            finally
            {
                File.Delete(path);
            }
        }
    }

    public class TestArtifactStore : IDistributedArtifactStore
    {
        public ArtifactDescriptor? UploadedDescriptor { get; private set; }

        public Task<ArtifactReference> UploadAsync(
            ArtifactDescriptor descriptor,
            Stream data,
            CancellationToken cancellationToken)
        {
            UploadedDescriptor = descriptor;
            return Task.FromResult(new ArtifactReference(
                Guid.NewGuid().ToString("N"),
                descriptor.Name,
                descriptor.ModuleTypeName,
                data.Length,
                descriptor.ContentType,
                DateTimeOffset.UtcNow));
        }

        public Task<Stream> DownloadAsync(
            ArtifactReference reference,
            CancellationToken cancellationToken)
            => throw new NotSupportedException();

        public Task<IReadOnlyList<ArtifactReference>> ListArtifactsAsync(
            string moduleTypeName,
            CancellationToken cancellationToken)
            => Task.FromResult<IReadOnlyList<ArtifactReference>>([]);

        public Task DeleteAsync(
            ArtifactReference reference,
            CancellationToken cancellationToken)
            => throw new NotSupportedException();
    }

    public sealed class TestAsyncDisposableArtifactStore : TestArtifactStore, IAsyncDisposable
    {
        public int DisposeCount { get; private set; }

        public ValueTask DisposeAsync()
        {
            DisposeCount++;
            return ValueTask.CompletedTask;
        }
    }

    public sealed class TestDisposableArtifactStore : TestArtifactStore, IDisposable
    {
        public int DisposeCount { get; private set; }

        public void Dispose() => DisposeCount++;
    }

    public sealed class TestArtifactStoreFactory : IDistributedArtifactStoreFactory
    {
        public int CreateCount { get; private set; }

        public TestAsyncDisposableArtifactStore? CreatedStore { get; private set; }

        public Task<IDistributedArtifactStore> CreateAsync(CancellationToken cancellationToken)
        {
            CreateCount++;
            CreatedStore = new TestAsyncDisposableArtifactStore();
            return Task.FromResult<IDistributedArtifactStore>(CreatedStore);
        }
    }

    public sealed class TestDisposableArtifactStoreFactory : IDistributedArtifactStoreFactory
    {
        public TestDisposableArtifactStore? CreatedStore { get; private set; }

        public Task<IDistributedArtifactStore> CreateAsync(CancellationToken cancellationToken)
        {
            CreatedStore = new TestDisposableArtifactStore();
            return Task.FromResult<IDistributedArtifactStore>(CreatedStore);
        }
    }

    public sealed class BlockingArtifactStoreFactory : IDistributedArtifactStoreFactory
    {
        private readonly TaskCompletionSource _creationStarted = new(
            TaskCreationOptions.RunContinuationsAsynchronously);
        private readonly TaskCompletionSource _completeCreation = new(
            TaskCreationOptions.RunContinuationsAsynchronously);

        public Task CreationStarted => _creationStarted.Task;

        public TestAsyncDisposableArtifactStore CreatedStore { get; } = new();

        public void CompleteCreation() => _completeCreation.SetResult();

        public async Task<IDistributedArtifactStore> CreateAsync(
            CancellationToken cancellationToken)
        {
            _creationStarted.SetResult();
            await _completeCreation.Task.WaitAsync(cancellationToken);
            return CreatedStore;
        }
    }
}
