namespace ModularPipelines.Distributed;

internal static class DistributedFailurePublisher
{
    private static readonly TimeSpan PublicationTimeout = TimeSpan.FromSeconds(30);

    public static async Task PublishAsync(
        IDistributedWorkerCoordinator coordinator,
        SerializedModuleResult result)
    {
        // Claimed work needs a terminal result even when the worker is already cancelled.
        // Bound cleanup independently, including coordinators that ignore cancellation.
        using var publicationCts = new CancellationTokenSource(PublicationTimeout);
        await coordinator.PublishResultAsync(result, publicationCts.Token)
            .WaitAsync(publicationCts.Token)
            .ConfigureAwait(false);
    }
}
