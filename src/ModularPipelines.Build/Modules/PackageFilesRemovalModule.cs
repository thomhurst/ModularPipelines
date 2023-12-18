using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Modules;

namespace ModularPipelines.Build.Modules;

public class PackageFilesRemovalModule : Module
{
    /// <inheritdoc/>
    protected override Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var packageFiles = context.Git()
            .RootDirectory
            .GetFiles(path => path.Extension is ".nupkg");

        foreach (var packageFile in packageFiles)
        {
            packageFile.Delete();
        }

        return NothingAsync();
    }
}