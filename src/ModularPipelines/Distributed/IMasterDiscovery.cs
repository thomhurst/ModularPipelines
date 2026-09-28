namespace ModularPipelines.Distributed;

/// <summary>
/// Advertises and discovers the endpoint exposed by a distributed pipeline master.
/// </summary>
/// <remarks>
/// Implementations can use Redis, Consul, DNS, or another discovery mechanism. Coordinator
/// transports that expose an endpoint (such as SignalR) consume the discovered
/// <see cref="MasterEndpoint"/> without depending on a particular discovery provider. The
/// endpoint can carry an access token, so implementations must store it where only the run's
/// processes can read it and must never log it. Follows the evolution policy described on
/// <see cref="IDistributedWorkerCoordinator"/>.
/// </remarks>
public interface IMasterDiscovery
{
    /// <summary>
    /// Advertises the endpoint exposed by the master.
    /// </summary>
    /// <param name="endpoint">The endpoint exposed by the master.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task AdvertiseMasterEndpointAsync(MasterEndpoint endpoint, CancellationToken cancellationToken);

    /// <summary>
    /// Discovers the endpoint exposed by the master.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The master's endpoint.</returns>
    Task<MasterEndpoint> DiscoverMasterEndpointAsync(CancellationToken cancellationToken);
}
