using System.IO.Compression;

namespace ModularPipelines.Distributed;

/// <summary>
/// Backend-independent options for distributed artifacts. Configure them once with
/// <c>builder.Services.Configure&lt;ArtifactOptions&gt;(...)</c>; they apply to whichever
/// artifact store is registered. Storage-specific settings such as expiry and chunking live on
/// each backend's own options.
/// </summary>
public class ArtifactOptions
{
    /// <summary>
    /// Compression level for directory artifacts. Default: Fastest.
    /// </summary>
    public CompressionLevel CompressionLevel { get; set; } = CompressionLevel.Fastest;
}
