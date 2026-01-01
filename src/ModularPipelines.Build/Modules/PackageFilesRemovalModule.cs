using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Modules;

namespace ModularPipelines.Build.Modules;

public class PackageFilesRemovalModule : Module<int>
{
    public override async Task<int> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var packageFiles = context.Git()
            .RootDirectory
            .GetFiles(path => path.Extension is ".nupkg");

        var count = 0;
        foreach (var packageFile in packageFiles)
        {
            packageFile.Delete();
            count++;
        }

        await Task.CompletedTask;
        return count;
    }
}