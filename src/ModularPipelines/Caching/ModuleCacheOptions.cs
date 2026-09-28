namespace ModularPipelines.Caching;

/// <summary>
/// Configures fingerprint calculation and local module cache storage.
/// </summary>
/// <remarks>
/// Like <see cref="Options.PipelineOptions"/>, these options are immutable. Configure them with
/// <see cref="PipelineBuilderExtensions.AddModuleCache{TStore}(PipelineBuilder, Func{ModuleCacheOptions, ModuleCacheOptions})"/>
/// and a <c>with</c> expression, for example <c>options =&gt; options with { MaximumInputFiles = 10_000 }</c>.
/// </remarks>
public sealed record ModuleCacheOptions
{
    /// <summary>
    /// Gets the directory from which relative input and artifact patterns are resolved.
    /// </summary>
    public string WorkingDirectory { get; init; } = Directory.GetCurrentDirectory();

    /// <summary>
    /// Gets the filesystem cache directory.
    /// </summary>
    public string CacheDirectory { get; init; } = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "ModularPipelines",
        "ModuleCache");

    /// <summary>
    /// Gets the maximum number of files one module may expand from its input globs.
    /// </summary>
    public int MaximumInputFiles { get; init; } = 100_000;

    /// <summary>
    /// Gets the maximum number of entries stored in one module's artifact snapshot.
    /// </summary>
    public int MaximumArtifactEntries { get; init; } = 100_000;

    /// <summary>
    /// Gets the maximum uncompressed size of one module's artifact snapshot.
    /// </summary>
    public long MaximumArtifactBytes { get; init; } = 10L * 1024 * 1024 * 1024;

    /// <summary>
    /// Gets the maximum compressed size of one cache entry read from a cache store.
    /// </summary>
    public long MaximumCacheEntryBytes { get; init; } = 10L * 1024 * 1024 * 1024;

    /// <summary>
    /// Gets the maximum uncompressed size of a cached module result restored from a cache store.
    /// </summary>
    public long MaximumResultBytes { get; init; } = 64L * 1024 * 1024;

    /// <summary>
    /// Gets the maximum number of files hashed concurrently.
    /// </summary>
    public int MaximumHashConcurrency { get; init; } = Math.Max(1, Environment.ProcessorCount);
}
