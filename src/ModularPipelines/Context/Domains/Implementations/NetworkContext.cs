using ModularPipelines.Context.Domains.Network;

namespace ModularPipelines.Context.Domains.Implementations;

/// <summary>
/// Provides access to HTTP and download capabilities.
/// </summary>
internal class NetworkContext : INetworkContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NetworkContext"/> class.
    /// </summary>
    /// <param name="http">The HTTP context for HTTP operations.</param>
    /// <param name="downloader">The downloader context for file download operations.</param>
    public NetworkContext(IHttpContext http, IDownloaderContext downloader)
    {
        Http = http;
        Downloader = downloader;
    }

    /// <inheritdoc />
    public IHttpContext Http { get; }

    /// <inheritdoc />
    public IDownloaderContext Downloader { get; }
}
