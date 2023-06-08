using CliWrap.Buffered;
using ModularPipelines.NuGet.Options;

namespace ModularPipelines.NuGet;

public interface INuGet
{
    Task<List<BufferedCommandResult>> UploadPackages(NuGetUploadOptions options);

    Task<BufferedCommandResult> AddSource(NuGetSourceOptions options);
}

public interface INuGet<T> : INuGet
{
}