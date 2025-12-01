using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Modules.Behaviors;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Build.Modules;

public class FindProjectsModule : IModule<IReadOnlyList<File>>, IAlwaysRun
{
    public async Task<IReadOnlyList<File>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        await Task.Yield();

        return context.Git()
            .RootDirectory!
            .GetFiles(f => GetProjectsPredicate(f, context))
            .OrderBy(x => x.NameWithoutExtension)
            .ToList();
    }

    private bool GetProjectsPredicate(File file, IModuleContext context)
    {
        var path = file.Path;

        if (!path.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (path.Contains("Test", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (path.EndsWith("ModularPipelines.Build.csproj")
            || path.Contains("Example"))
        {
            return false;
        }

        if (path.EndsWith("ModularPipelines.Analyzers.Package.csproj"))
        {
            return true;
        }

        if (path.Contains("Development"))
        {
            return false;
        }

        if (path.Contains("ModularPipelines.Analyzers"))
        {
            return false;
        }

        context.Logger.LogInformation("Found File: {File}", path);

        return true;
    }
}