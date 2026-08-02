namespace ModularPipelines.Caching;

/// <summary>
/// Configures fingerprint calculation and local module cache storage.
/// </summary>
public sealed class ModuleCacheOptions
{
    /// <summary>
    /// Gets or sets the directory from which relative input and artifact patterns are resolved.
    /// </summary>
    public string WorkingDirectory { get; set; } = Directory.GetCurrentDirectory();

    /// <summary>
    /// Gets or sets the filesystem cache directory.
    /// </summary>
    public string CacheDirectory { get; set; } = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "ModularPipelines",
        "ModuleCache");

    /// <summary>
    /// Gets or sets the maximum number of files one module may expand from its input globs.
    /// </summary>
    public int MaximumInputFiles { get; set; } = 100_000;

    /// <summary>
    /// Gets or sets the maximum number of entries stored in one module's artifact snapshot.
    /// </summary>
    public int MaximumArtifactEntries { get; set; } = 100_000;

    /// <summary>
    /// Gets or sets the maximum uncompressed size of one module's artifact snapshot.
    /// </summary>
    public long MaximumArtifactBytes { get; set; } = 10L * 1024 * 1024 * 1024;

    /// <summary>
    /// Gets or sets the maximum compressed size of one cache entry read from a cache store.
    /// </summary>
    public long MaximumCacheEntryBytes { get; set; } = 10L * 1024 * 1024 * 1024;

    /// <summary>
    /// Gets or sets the maximum number of files hashed concurrently.
    /// </summary>
    public int MaximumHashConcurrency { get; set; } = Math.Max(1, Environment.ProcessorCount);
}
