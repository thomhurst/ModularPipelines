namespace ModularPipelines.Distributed;

/// <summary>
/// Detects capabilities that this process advertises in distributed mode.
/// </summary>
/// <remarks>
/// Register implementations with <c>services.AddSingleton&lt;ICapabilityProvider, TProvider&gt;()</c>.
/// The master and every worker union the results of all registered providers with
/// <see cref="DistributedOptions.Capabilities"/>. The framework registers a provider for the
/// current operating system by default.
/// </remarks>
public interface ICapabilityProvider
{
    /// <summary>
    /// Returns the capabilities this process provides.
    /// </summary>
    Task<IEnumerable<Capability>> GetCapabilitiesAsync(CancellationToken cancellationToken);
}

/// <summary>
/// Advertises the operating system this process runs on.
/// </summary>
internal sealed class OperatingSystemCapabilityProvider : ICapabilityProvider
{
    public Task<IEnumerable<Capability>> GetCapabilitiesAsync(CancellationToken cancellationToken) =>
        Task.FromResult<IEnumerable<Capability>>(
            Capability.CurrentOperatingSystem is { } operatingSystem ? [operatingSystem] : []);
}

internal static class LocalCapabilities
{
    public static async Task<HashSet<Capability>> ResolveAsync(
        DistributedOptions options,
        IEnumerable<ICapabilityProvider>? providers,
        CancellationToken cancellationToken)
    {
        var capabilities = new HashSet<Capability>(options.Capabilities);
        foreach (var provider in providers ?? [])
        {
            capabilities.UnionWith(await provider.GetCapabilitiesAsync(cancellationToken).ConfigureAwait(false));
        }

        return capabilities;
    }
}
