namespace ModularPipelines.Distributed;

/// <summary>
/// Advertises and discovers the endpoint exposed by a distributed pipeline master.
/// </summary>
/// <remarks>
/// Implementations can use Redis, Consul, DNS, or another discovery mechanism.
/// Coordinator transports consume the discovered endpoint without depending on a
/// particular discovery provider.
/// </remarks>
public interface IMasterDiscovery
{
    /// <summary>
    /// Advertises the endpoint exposed by the master.
    /// </summary>
    /// <param name="masterEndpoint">The endpoint exposed by the master.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task AdvertiseMasterEndpointAsync(string masterEndpoint, CancellationToken cancellationToken);

    /// <summary>
    /// Discovers the endpoint exposed by the master.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The master's endpoint.</returns>
    Task<string> DiscoverMasterEndpointAsync(CancellationToken cancellationToken);
}
