namespace ModularPipelines.Distributed;

/// <summary>
/// Marks a wrapper that creates its distributed backend on first use, so run cleanup can reach, and
/// deduplicate, the backend itself rather than the wrapper.
/// </summary>
internal interface IDeferredDistributedBackend
{
    /// <summary>Gets the wrapped backend, creating it when it does not exist yet.</summary>
    ValueTask<object> GetBackendAsync(CancellationToken cancellationToken);
}
