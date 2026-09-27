using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace ModularPipelines.Distributed;

/// <summary>
/// Detects capabilities that this process provides.
/// </summary>
/// <remarks>
/// Register implementations with <see cref="CapabilityPipelineBuilderExtensions.AddCapabilityProvider{TProvider}"/>.
/// The process's capabilities are the union of all registered providers and
/// <see cref="DistributedOptions.Capabilities"/>. The framework registers a provider for the
/// current operating system by default. Providers run once per process.
/// Modules whose <c>[RequiresCapability]</c> or <c>[RequiresAnyCapability]</c> requirements these
/// capabilities do not satisfy are skipped when they would run locally; in distributed mode they
/// are routed to a worker that satisfies them.
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

/// <summary>
/// Resolves this process's capabilities once and shares them between the condition handler,
/// the distributed master, and workers.
/// </summary>
internal sealed class LocalCapabilityRegistry
{
    private readonly Lazy<Task<HashSet<Capability>>> _capabilities;

    public LocalCapabilityRegistry(
        IOptions<DistributedOptions> options,
        IEnumerable<ICapabilityProvider> providers,
        IHostApplicationLifetime? lifetime = null)
    {
        // The shared probe outlives any one caller's token, so stop it when the pipeline stops.
        var stopping = lifetime?.ApplicationStopping ?? CancellationToken.None;
        _capabilities = new Lazy<Task<HashSet<Capability>>>(
            () => LocalCapabilities.ResolveAsync(options.Value, providers, stopping));
    }

    public async Task<IReadOnlySet<Capability>> GetAsync(CancellationToken cancellationToken) =>
        await _capabilities.Value.WaitAsync(cancellationToken).ConfigureAwait(false);
}

internal static class LocalCapabilities
{
    public static async Task<IReadOnlySet<Capability>> GetAsync(
        LocalCapabilityRegistry? registry,
        DistributedOptions options,
        CancellationToken cancellationToken) =>
        registry is null
            ? await ResolveAsync(options, providers: null, cancellationToken).ConfigureAwait(false)
            : await registry.GetAsync(cancellationToken).ConfigureAwait(false);

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
