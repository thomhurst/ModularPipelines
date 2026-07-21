namespace ModularPipelines.Distributed.SignalR.Discovery;

/// <summary>
/// Enables dynamic discovery of the master's SignalR URL.
/// When registered in DI, the factory uses this instead of the static <c>MasterUrl</c> option.
/// </summary>
/// <remarks>
/// Implementations can use Redis, Consul, DNS, or any other mechanism to advertise and discover the master URL.
/// The SignalR package itself has no dependency on any specific discovery mechanism.
/// </remarks>
public interface ISignalRMasterDiscovery
{
    /// <summary>
    /// Master calls this to advertise its URL after starting the SignalR server.
    /// </summary>
    /// <param name="masterUrl">The full URL of the master SignalR server (e.g., "http://10.0.0.5:5099").</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task AdvertiseMasterUrlAsync(string masterUrl, CancellationToken cancellationToken);

    /// <summary>
    /// Workers call this to discover the master's URL before connecting.
    /// Implementations should block until the URL is available or the token is cancelled.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The full URL of the master SignalR server.</returns>
    Task<string> DiscoverMasterUrlAsync(CancellationToken cancellationToken);
}
