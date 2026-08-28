using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.Build.Modules;

public class PackageFilesRemovalModule : Module<int>
{
    protected override async Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var repositoryInfo = await context.Tools.Git.Information.GetInfoAsync().ConfigureAwait(false)
            ?? throw new InvalidOperationException("Git repository information is unavailable.");
        var packageFiles = repositoryInfo.Root
            .GetFiles(path => path.Extension is ".nupkg");

        var count = 0;
        foreach (var packageFile in packageFiles)
        {
            packageFile.Delete();
            count++;
        }

        return count;
    }
}
