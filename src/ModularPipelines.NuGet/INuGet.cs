using ModularPipelines.Models;
using ModularPipelines.NuGet.Options;

namespace ModularPipelines.NuGet;

public interface INuGet
{
    Task<CommandResult[]> UploadPackages(NuGetUploadOptions options);

    Task<CommandResult> AddSource(NuGetSourceOptions options);
}