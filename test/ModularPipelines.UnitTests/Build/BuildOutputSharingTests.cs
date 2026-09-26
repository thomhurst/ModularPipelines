using Microsoft.Extensions.Hosting;
using ModularPipelines.Build.Helpers;
using ModularPipelines.Distributed;
using Moq;

namespace ModularPipelines.UnitTests.Build;

public class BuildOutputSharingTests
{
    private const string RepositoryRoot = "/repository";
    private static readonly ModuleId Producer = new("build.application");

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
        artifacts.Setup(x => x.DownloadAsync(Producer, "build-output", RepositoryRoot, It.IsAny<CancellationToken>()))
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
        artifacts.Verify(x => x.DownloadAsync(Producer, "build-output", RepositoryRoot, shutdown.Token), Times.Once);
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
        artifacts.VerifyNoOtherCalls();
    }

    [Test]
    public async Task Distributed_Consumers_Share_InFlight_And_Completed_Restore()
    {
        var download = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);
        using var cancellation = new CancellationTokenSource();
        var artifacts = new Mock<IArtifactContext>(MockBehavior.Strict);
        artifacts.Setup(x => x.DownloadAsync(Producer, "build-output", RepositoryRoot, CancellationToken.None))
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
        artifacts.Verify(x => x.DownloadAsync(Producer, "build-output", RepositoryRoot, CancellationToken.None), Times.Once);
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
        artifacts.Setup(x => x.DownloadAsync(Producer, "build-output", RepositoryRoot, It.IsAny<CancellationToken>()))
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
        artifacts.Verify(x => x.DownloadAsync(Producer, "build-output", RepositoryRoot, CancellationToken.None), Times.Once);
        artifacts.VerifyNoOtherCalls();
    }

    [Test]
    public async Task Failed_Restore_Is_Propagated_To_Every_Consumer()
    {
        var failure = new IOException("Artifact download failed.");
        var artifacts = new Mock<IArtifactContext>(MockBehavior.Strict);
        artifacts.Setup(x => x.DownloadAsync(Producer, "build-output", RepositoryRoot, CancellationToken.None))
            .Returns(Task.FromException<string>(failure));
        var sharing = CreateSharing();

        for (var consumer = 0; consumer < 2; consumer++)
        {
            await Assert.That(() => sharing.RestoreAsync(
                    artifacts.Object, Producer, RepositoryRoot, CancellationToken.None))
                .Throws<IOException>();
        }

        artifacts.Verify(x => x.DownloadAsync(Producer, "build-output", RepositoryRoot, CancellationToken.None), Times.Once);
    }
}
