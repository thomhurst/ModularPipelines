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
        CancellationToken cancellationToken)
    {
        var results = new List<CommandResult>();
        foreach (var nugetFile in packagePaths)
        {
            cancellationToken.ThrowIfCancellationRequested();
            results.Add(await context.Tools.DotNet.NuGet.PushAsync(new DotNetNuGetPushOptions(GetPushPaths(nugetFile))
            {
                Source = source,
                ApiKey = apiKey,
                SkipDuplicate = true,
            }, cancellationToken: cancellationToken).ConfigureAwait(false));
        }

        return results.ToArray();
    }

    private static string[] GetPushPaths(FilePath package)
    {
        var symbolsPath = Path.ChangeExtension(package.Path, ".snupkg");
        // NuGet skips automatic symbol uploads when the primary package already exists.
        // An explicit symbol input retries that upload independently of the primary.
        return package.Path.EndsWith(".nupkg", StringComparison.OrdinalIgnoreCase) && File.Exists(symbolsPath)
            ? [package.Path, symbolsPath]
            : [package.Path];
    }
}
