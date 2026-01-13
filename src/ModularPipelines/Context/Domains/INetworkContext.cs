using ModularPipelines.Context.Domains.Network;

namespace ModularPipelines.Context.Domains;

/// <summary>
/// Provides HTTP and download capabilities.
/// </summary>
public interface INetworkContext
{
    /// <summary>
    /// HTTP client with retry policies.
    /// </summary>
    IHttpContext Http { get; }

    /// <summary>
    /// File download operations.
    /// </summary>
    IDownloaderContext Downloader { get; }
}
