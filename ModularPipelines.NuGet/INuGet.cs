using CliWrap.Buffered;
using ModularPipelines.NuGet.Options;

namespace ModularPipelines.NuGet;

public interface INuGet
{
    Task<List<BufferedCommandResult>> UploadPackage(NuGetUploadOptions options);

    Task<BufferedCommandResult> AddSource(NuGetSourceOptions options);
}