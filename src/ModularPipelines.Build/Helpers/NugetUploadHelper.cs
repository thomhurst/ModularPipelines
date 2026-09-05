using EnumerableAsyncProcessor.Extensions;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Models;
using ModularPipelines.FileSystem;

namespace ModularPipelines.Build.Helpers;

public static class NugetUploadHelper
{
    public static async Task<CommandResult[]> UploadPackagesAsync(
        IModuleContext context,
        IEnumerable<FilePath> packagePaths,
        string source,
        string? apiKey,
        CancellationToken cancellationToken)
    {
        return await packagePaths
            .SelectAsync(async nugetFile => await context.Tools.DotNet.NuGet.PushAsync(new DotNetNuGetPushOptions([nugetFile.Path])
                {
                    Source = source,
                    ApiKey = apiKey,
                }, cancellationToken: cancellationToken),
                cancellationToken: cancellationToken)
            .ProcessOneAtATime();
    }
}
