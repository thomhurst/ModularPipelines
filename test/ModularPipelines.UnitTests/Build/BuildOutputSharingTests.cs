using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Helpers;
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Artifacts;
using Moq;

namespace ModularPipelines.UnitTests.Build;

public class BuildOutputSharingTests
{
    private string RepositoryRoot = null!;
    private string ArchivePath => Path.Combine(RepositoryRoot, BuildOutputArchive.FileName);

    [Before(Test)]
    public async Task CreateArchive()
    {
        RepositoryRoot = Directory.CreateTempSubdirectory("build-sharing-test-").FullName;
        await File.WriteAllTextAsync(Path.Combine(RepositoryRoot, "Test.csproj"), "<Project />");
        var output = Directory.CreateDirectory(Path.Combine(RepositoryRoot, "bin", "Release"));
        await File.WriteAllTextAsync(Path.Combine(output.FullName, "Test.dll"), "test output");
        await BuildOutputArchive.CreateAsync(RepositoryRoot, ["Test.csproj"], CancellationToken.None);
    }

    [After(Test)]
    public void DeleteRepository() => Directory.Delete(RepositoryRoot, recursive: true);
    private static readonly ModuleId Producer = new("build.application");

    [Test]
    public async Task Configured_Transport_Publishes_And_Restores_Without_Using_Redis_Artifacts()
    {
        var artifacts = new Mock<IArtifactContext>(MockBehavior.Strict);
        var transport = new RecordingTransport();
        var sharing = new BuildOutputSharing(
            Microsoft.Extensions.Options.Options.Create(new DistributedOptions { TotalInstances = 2, InstanceIndex = 1 }),
            Mock.Of<IHostApplicationLifetime>(), transport);

        await sharing.PublishAsync(RepositoryRoot, CancellationToken.None);
        await Assert.That(File.Exists(ArchivePath)).IsFalse();
        await Task.WhenAll(Enumerable.Range(0, 16).Select(_ =>
            sharing.RestoreAsync(artifacts.Object, Producer, RepositoryRoot, CancellationToken.None)));

        await Assert.That(transport.Publications).IsEqualTo(1);
        await Assert.That(transport.Downloads).IsEqualTo(1);
        await Assert.That(transport.RepositoryRoot).IsEqualTo(RepositoryRoot);
        artifacts.VerifyNoOtherCalls();
        await Assert.That(File.Exists(ArchivePath)).IsFalse();
    }

    [Test]
    public async Task Configured_Transport_Failure_Is_Cached_Without_Falling_Back_To_Redis()
    {
        var artifacts = new Mock<IArtifactContext>(MockBehavior.Strict);
        var transport = new RecordingTransport { DownloadFailure = new IOException("GitHub unavailable") };
        var sharing = new BuildOutputSharing(
            Microsoft.Extensions.Options.Options.Create(new DistributedOptions { TotalInstances = 2, InstanceIndex = 1 }),
            Mock.Of<IHostApplicationLifetime>(), transport);

        for (var consumer = 0; consumer < 2; consumer++)
        {
            await Assert.That(() => sharing.RestoreAsync(artifacts.Object, Producer, RepositoryRoot, CancellationToken.None))
                .Throws<IOException>();
        }

        await Assert.That(transport.Downloads).IsEqualTo(1);
        artifacts.VerifyNoOtherCalls();
    }

    private sealed class RecordingTransport : IBuildOutputTransport
    {
        private byte[]? _archive;

        public int Publications { get; private set; }

        public int Downloads { get; private set; }

        public string? RepositoryRoot { get; private set; }

        public Exception? DownloadFailure { get; init; }

        public async Task PublishAsync(string repositoryRoot, CancellationToken cancellationToken)
        {
            Publications++;
            RepositoryRoot = repositoryRoot;
            _archive = await File.ReadAllBytesAsync(Path.Combine(repositoryRoot, BuildOutputArchive.FileName), cancellationToken);
        }

        public Task DownloadAsync(string repositoryRoot, CancellationToken cancellationToken)
        {
            Downloads++;
            RepositoryRoot = repositoryRoot;
            return DownloadFailure is null
                ? File.WriteAllBytesAsync(Path.Combine(repositoryRoot, BuildOutputArchive.FileName), _archive!, cancellationToken)
                : Task.FromException(DownloadFailure);
        }
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Artifact_Lifecycle_Publishes_Only_The_Redis_Fallback(bool useTransport)
    {
        var artifacts = new Mock<IArtifactContext>(MockBehavior.Strict);
        var producer = ModuleId.FromType(typeof(ArchiveProducer));
        var reference = new ArtifactReference
        {
            ArtifactId = "archive",
            Name = "build-output",
            ModuleId = producer,
            SizeBytes = new FileInfo(ArchivePath).Length,
            UploadedAt = DateTimeOffset.UtcNow,
        };
        var store = new Mock<IDistributedArtifactStore>(MockBehavior.Strict);
        store.Setup(x => x.UploadAsync(It.IsAny<ArtifactDescriptor>(), It.IsAny<Stream>(), CancellationToken.None))
            .ReturnsAsync(reference);
        store.Setup(x => x.DownloadAsync(reference, CancellationToken.None))
            .Returns(() => Task.FromResult<Stream>(File.OpenRead(ArchivePath)));
        var options = Microsoft.Extensions.Options.Options.Create(new DistributedOptions { TotalInstances = 2 });
        var transport = useTransport ? new RecordingTransport() : null;
        var sharing = new BuildOutputSharing(options, Mock.Of<IHostApplicationLifetime>(), transport);
        var logger = new RecordingLogger();
        var lifecycle = new ArtifactLifecycleManager(store.Object, options, logger, RepositoryRoot);

        await sharing.PublishAsync(RepositoryRoot, CancellationToken.None);
        // BuildSolutionsModule has no declared output. Only the fallback producer runs
        // the attributed lifecycle, after its Redis fallback run condition passes.
        var references = await lifecycle.UploadProducedArtifactsAsync(typeof(ExternalArchiveProducer), CancellationToken.None);
        if (sharing.UsesRedisFallback)
        {
            references = await lifecycle.UploadProducedArtifactsAsync(typeof(ArchiveProducer), CancellationToken.None);
        }

        await Assert.That(references.Count).IsEqualTo(useTransport ? 0 : 1);
        if (!useTransport)
        {
            await Assert.That(references.Single()).IsEqualTo(reference);
            // Workers treat this accepted list as authoritative, even when empty.
            var accepted = new AcceptedArtifactRegistry();
            accepted.Record(producer, references);
            var worker = new ArtifactContextImpl(store.Object, options.Value, accepted);
            var downloaded = Path.Combine(RepositoryRoot, "downloaded-output.tar.gz");
            await worker.DownloadAsync(producer, "build-output", downloaded, CancellationToken.None);
            await Assert.That(new FileInfo(downloaded).Length).IsEqualTo(reference.SizeBytes);
        }
        store.Verify(x => x.UploadAsync(It.IsAny<ArtifactDescriptor>(), It.IsAny<Stream>(), CancellationToken.None),
            useTransport ? Times.Never() : Times.Once());
        await Assert.That(logger.Warnings).IsEmpty();
        await Assert.That(transport?.Publications ?? 0).IsEqualTo(useTransport ? 1 : 0);
        artifacts.VerifyNoOtherCalls();
    }

    [Test]
    public async Task Missing_Declared_Output_Still_Warns_And_Fails_When_Required()
    {
        File.Delete(ArchivePath);
        var store = new Mock<IDistributedArtifactStore>(MockBehavior.Strict);
        var logger = new RecordingLogger();
        var lifecycle = new ArtifactLifecycleManager(store.Object,
            Microsoft.Extensions.Options.Options.Create(new DistributedOptions { TotalInstances = 2 }),
            logger, RepositoryRoot);

        var references = await lifecycle.UploadProducedArtifactsAsync(typeof(ArchiveProducer), CancellationToken.None);

        await Assert.That(references).IsEmpty();
        await Assert.That(logger.Warnings).Contains(message => message.Contains("No files matched pattern", StringComparison.Ordinal));
        await Assert.That(() => ArtifactLifecycleManager.EnsureRequiredArtifactsProduced(
            typeof(ArchiveProducer), references, ["build-output"]))
            .Throws<InvalidOperationException>();
        store.VerifyNoOtherCalls();
    }

    private sealed class RecordingLogger : ILogger<ArtifactLifecycleManager>
    {
        public List<string> Warnings { get; } = [];

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
            Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (logLevel >= LogLevel.Warning)
            {
                Warnings.Add(formatter(state, exception));
            }
        }
    }

    [ProducesArtifact("build-output", BuildOutputArchive.FileName)]
    private sealed class ArchiveProducer;

    private sealed class ExternalArchiveProducer;

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Host_Shutdown_Cancels_Shared_Download_After_Consumer_Cancellation(
        bool cancelConsumersFirst)
    {
        using var shutdown = new CancellationTokenSource();
        using var consumerCancellation = new CancellationTokenSource();
        var download = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
        var downloadToken = CancellationToken.None;
        Task<string>? sharedDownload = null;
        var artifacts = new Mock<IArtifactContext>(MockBehavior.Strict);
        artifacts.Setup(x => x.DownloadAsync(Producer, "build-output", ArchivePath, It.IsAny<CancellationToken>()))
            .Returns((ModuleId _, string _, string _, CancellationToken token) =>
            {
                downloadToken = token;
                return sharedDownload = download.Task.WaitAsync(token);
            });
        var sharing = CreateSharing(shutdownToken: shutdown.Token);
        var first = sharing.RestoreAsync(artifacts.Object, Producer, RepositoryRoot, consumerCancellation.Token);
        var second = sharing.RestoreAsync(artifacts.Object, Producer, RepositoryRoot, consumerCancellation.Token);

        if (cancelConsumersFirst)
        {
            consumerCancellation.Cancel();
            await Assert.That(first.IsCanceled).IsTrue();
            await Assert.That(second.IsCanceled).IsTrue();
            await Assert.That(sharedDownload!.IsCompleted).IsFalse();
        }

        shutdown.Cancel();

        await Assert.That(downloadToken.IsCancellationRequested).IsTrue();
        await Assert.That(sharedDownload!.IsCanceled).IsTrue();
        await Assert.That(() => first).Throws<OperationCanceledException>();
        await Assert.That(() => second).Throws<OperationCanceledException>();
        artifacts.Verify(x => x.DownloadAsync(Producer, "build-output", ArchivePath, shutdown.Token), Times.Once);
        artifacts.VerifyNoOtherCalls();
    }

    private static BuildOutputSharing CreateSharing(int totalInstances = 2, int instanceIndex = 1, CancellationToken shutdownToken = default) =>
        new(Microsoft.Extensions.Options.Options.Create(new DistributedOptions { TotalInstances = totalInstances, InstanceIndex = instanceIndex }),
            Mock.Of<IHostApplicationLifetime>(lifetime => lifetime.ApplicationStopping == shutdownToken));

    [Test]
    public async Task Master_Uses_Its_Existing_Output_Without_Downloading()
    {
        var artifacts = new Mock<IArtifactContext>(MockBehavior.Strict);
        var sharing = CreateSharing(instanceIndex: 0);

        await sharing.RestoreAsync(artifacts.Object, Producer, RepositoryRoot, CancellationToken.None);

        await Assert.That(sharing.IsEnabled).IsTrue();
        artifacts.VerifyNoOtherCalls();
    }

    [Test]
    public async Task Standalone_Uses_Existing_Output_Without_Downloading()
    {
        var artifacts = new Mock<IArtifactContext>(MockBehavior.Strict);
        var sharing = CreateSharing(totalInstances: 1);

        await sharing.RestoreAsync(artifacts.Object, Producer, RepositoryRoot, CancellationToken.None);

        await Assert.That(sharing.IsEnabled).IsFalse();
        await Assert.That(sharing.UsesRedisFallback).IsFalse();
        artifacts.VerifyNoOtherCalls();
    }

    [Test]
    public async Task Distributed_Consumers_Share_InFlight_And_Completed_Restore()
    {
        var download = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
        using var cancellation = new CancellationTokenSource();
        var artifacts = new Mock<IArtifactContext>(MockBehavior.Strict);
        artifacts.Setup(x => x.DownloadAsync(Producer, "build-output", ArchivePath, CancellationToken.None))
            .Returns(download.Task);
        var sharing = CreateSharing();

        var restores = new Task[16];
        Parallel.For(0, restores.Length, index =>
            restores[index] = sharing.RestoreAsync(artifacts.Object, Producer, RepositoryRoot, cancellation.Token));

        foreach (var restore in restores)
        {
            await Assert.That(restore.IsCompleted).IsFalse();
        }

        download.SetResult(RepositoryRoot);
        await Task.WhenAll(restores);
        await sharing.RestoreAsync(artifacts.Object, Producer, RepositoryRoot, cancellation.Token);

        await Assert.That(sharing.IsEnabled).IsTrue();
        artifacts.Verify(x => x.DownloadAsync(Producer, "build-output", ArchivePath, CancellationToken.None), Times.Once);
        artifacts.VerifyNoOtherCalls();
    }

    [Test]
    [Arguments(true)]
    [Arguments(false)]
    public async Task Canceling_One_Consumer_Does_Not_Cancel_The_Shared_Restore(bool cancelFirstConsumer)
    {
        var download = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
        using var firstCancellation = new CancellationTokenSource();
        using var secondCancellation = new CancellationTokenSource();
        var artifacts = new Mock<IArtifactContext>(MockBehavior.Strict);
        artifacts.Setup(x => x.DownloadAsync(Producer, "build-output", ArchivePath, It.IsAny<CancellationToken>()))
            .Returns((ModuleId _, string _, string _, CancellationToken token) => download.Task.WaitAsync(token));
        var sharing = CreateSharing();

        var first = sharing.RestoreAsync(artifacts.Object, Producer, RepositoryRoot, firstCancellation.Token);
        var second = sharing.RestoreAsync(artifacts.Object, Producer, RepositoryRoot, secondCancellation.Token);
        var canceled = cancelFirstConsumer ? first : second;
        var remaining = cancelFirstConsumer ? second : first;

        (cancelFirstConsumer ? firstCancellation : secondCancellation).Cancel();

        await Assert.That(canceled.IsCanceled).IsTrue();
        await Assert.That(remaining.IsCompleted).IsFalse();
        download.SetResult(RepositoryRoot);
        await remaining.WaitAsync(TimeSpan.FromSeconds(10));
        await sharing.RestoreAsync(artifacts.Object, Producer, RepositoryRoot, CancellationToken.None);
        artifacts.Verify(x => x.DownloadAsync(Producer, "build-output", ArchivePath, CancellationToken.None), Times.Once);
        artifacts.VerifyNoOtherCalls();
    }

    [Test]
    public async Task Failed_Restore_Is_Propagated_To_Every_Consumer()
    {
        var failure = new IOException("Artifact download failed.");
        var artifacts = new Mock<IArtifactContext>(MockBehavior.Strict);
        artifacts.Setup(x => x.DownloadAsync(Producer, "build-output", ArchivePath, CancellationToken.None))
            .Returns(Task.FromException<string>(failure));
        var sharing = CreateSharing();

        for (var consumer = 0; consumer < 2; consumer++)
        {
            await Assert.That(() => sharing.RestoreAsync(
                    artifacts.Object, Producer, RepositoryRoot, CancellationToken.None))
                .Throws<IOException>();
        }

        artifacts.Verify(x => x.DownloadAsync(Producer, "build-output", ArchivePath, CancellationToken.None), Times.Once);
    }
}
