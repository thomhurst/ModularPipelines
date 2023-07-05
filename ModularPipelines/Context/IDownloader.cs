using ModularPipelines.Options;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Context;

public interface IDownloader
{
    public Task<File> DownloadFileAsync( DownloadOptions options, CancellationToken cancellationToken = default );
}
