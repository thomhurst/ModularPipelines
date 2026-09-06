using ModularPipelines.Build.Helpers;
using ModularPipelines.Distributed;
using Moq;

namespace ModularPipelines.UnitTests.Build;

public class BuildOutputSharingTests
{
    private const string Producer = "BuildSolutionsModule";
    private const string RepositoryRoot = "/repository";

    [Test]
    public async Task Standalone_Uses_Existing_Output_Without_Downloading()
    {
        var artifacts = new Mock<IArtifactContext>(MockBehavior.Strict);
        var sharing = new BuildOutputSharing(Microsoft.Extensions.Options.Options.Create(new DistributedOptions()));

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
        artifacts.Setup(x => x.DownloadAsync(Producer, "build-output", RepositoryRoot, cancellation.Token))
            .Returns(download.Task);
        var sharing = new BuildOutputSharing(Microsoft.Extensions.Options.Options.Create(new DistributedOptions { TotalInstances = 2 }));

        var restores = new Task[16];
        Parallel.For(0, restores.Length, index =>
            restores[index] = sharing.RestoreAsync(artifacts.Object, Producer, RepositoryRoot, cancellation.Token));

        foreach (var restore in restores)
        {
            await Assert.That(ReferenceEquals(restore, download.Task)).IsTrue();
        }

        download.SetResult(RepositoryRoot);
        await Task.WhenAll(restores);
        await sharing.RestoreAsync(artifacts.Object, Producer, RepositoryRoot, cancellation.Token);

        await Assert.That(sharing.IsEnabled).IsTrue();
        artifacts.Verify(x => x.DownloadAsync(Producer, "build-output", RepositoryRoot, cancellation.Token), Times.Once);
        artifacts.VerifyNoOtherCalls();
    }

    [Test]
    public async Task Failed_Restore_Is_Propagated_To_Every_Consumer()
    {
        var failure = new IOException("Artifact download failed.");
        var artifacts = new Mock<IArtifactContext>(MockBehavior.Strict);
        artifacts.Setup(x => x.DownloadAsync(Producer, "build-output", RepositoryRoot, CancellationToken.None))
            .Returns(Task.FromException<string>(failure));
        var sharing = new BuildOutputSharing(Microsoft.Extensions.Options.Options.Create(new DistributedOptions { TotalInstances = 2 }));

        for (var consumer = 0; consumer < 2; consumer++)
        {
            await Assert.That(() => sharing.RestoreAsync(
                    artifacts.Object, Producer, RepositoryRoot, CancellationToken.None))
                .Throws<IOException>();
        }

        artifacts.Verify(x => x.DownloadAsync(Producer, "build-output", RepositoryRoot, CancellationToken.None), Times.Once);
    }
}
