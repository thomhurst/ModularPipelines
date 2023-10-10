using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Build.Modules;

public class FindProjectsModule : Module<IReadOnlyList<File>>
{
    public override ModuleRunType ModuleRunType => ModuleRunType.AlwaysRun;

    /// <inheritdoc/>
    protected override async Task<IReadOnlyList<File>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        await Task.Yield();

        return context.Git()
            .RootDirectory!
            .GetFiles(f => GetProjectsPredicate(f, context))
            .OrderBy(x => x.NameWithoutExtension)
            .ToList();
    }

    private bool GetProjectsPredicate(File file, IPipelineContext context)
    {
        var path = file.Path;

        if (!path.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        if (path.Contains("Tests", StringComparison.OrdinalIgnoreCase))
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

        if (path.Contains("ModularPipelines.Analyzers"))
        {
            return false;
        }

        context.Logger.LogInformation("Found File: {File}", path);

        return true;
    }
}