namespace ModularPipelines.Distributed;

/// <summary>
/// Factory for creating distributed artifact stores with async initialization.
/// Use when the storage provider requires async setup (connecting to a server, creating buckets, etc.).
/// </summary>
public interface IDistributedArtifactStoreFactory
{
    Task<IDistributedArtifactStore> CreateAsync(CancellationToken cancellationToken);
}
