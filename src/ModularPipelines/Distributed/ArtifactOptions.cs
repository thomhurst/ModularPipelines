using System.IO.Compression;

namespace ModularPipelines.Distributed;

/// <summary>
/// Configuration options for distributed artifact storage.
/// </summary>
public class ArtifactOptions
{
    /// <summary>
    /// Maximum size in bytes for a single upload before chunking. Default: 50 MB.
    /// </summary>
    public long MaxSingleUploadBytes { get; set; } = 50 * 1024 * 1024;

    /// <summary>
    /// Chunk size in bytes for large file uploads. Default: 4 MB.
    /// </summary>
    public int ChunkSizeBytes { get; set; } = 4 * 1024 * 1024;

    /// <summary>
    /// Compression level for directory artifacts. Default: Fastest.
    /// </summary>
    public CompressionLevel CompressionLevel { get; set; } = CompressionLevel.Fastest;

    /// <summary>
    /// Whether to automatically clean up artifacts after pipeline completion. Default: true.
    /// </summary>
    public bool AutoCleanup { get; set; } = true;

    /// <summary>
    /// Time-to-live for stored artifacts. Default: 1 hour.
    /// </summary>
    public TimeSpan TimeToLive { get; set; } = TimeSpan.FromHours(1);
}
