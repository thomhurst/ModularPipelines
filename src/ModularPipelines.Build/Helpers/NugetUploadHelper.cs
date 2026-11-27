using ModularPipelines.Context;
using ModularPipelines.DotNet.Options;
using ModularPipelines.FileSystem;
using ModularPipelines.Models;

namespace ModularPipelines.Build.Helpers;

public static class NugetUploadHelper
{
    public static async Task<CommandResult[]> UploadPackagesAsync(
        IModuleContext context,
        IEnumerable<FilePath> packagePaths,
        string source,
        string? apiKey,
        CancellationToken cancellationToken,
        Action<DotNetNuGetPushOptions>? configurePush = null)
    {
        var results = new List<CommandResult>();
        foreach (var nugetFile in packagePaths)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var options = new DotNetNuGetPushOptions([nugetFile.Path])
            {
                Source = source,
                ApiKey = apiKey,
                SkipDuplicate = true,
            };
            configurePush?.Invoke(options);
            results.Add(await context.Tools.DotNet.NuGet.PushAsync(options, cancellationToken: cancellationToken).ConfigureAwait(false));
        }

        return results.ToArray();
    }
}
