using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Modules;
using ModularPipelines.Modules.Behaviors;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Build.Modules;

public class FindProjectsModule : Module<IReadOnlyList<File>>, IAlwaysRun
{
    // Projects excluded from NuGet publishing (analyzers are excluded via Contains("Analyzers") filter)
    private static readonly HashSet<string> ExcludedProjects = new(StringComparer.OrdinalIgnoreCase)
    {
        "ModularPipelines.Build",
        "ModularPipelines.Examples",
    };

    public override Task<IReadOnlyList<File>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var projects = context.Git().RootDirectory
            .GetFiles(file => file.Path.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase)
                              && file.Folder?.Name.StartsWith("ModularPipelines", StringComparison.OrdinalIgnoreCase) == true
                              && file.Folder?.Parent?.Name == "src"
                              && !file.Path.Contains("Analyzers", StringComparison.OrdinalIgnoreCase)
                              && !ExcludedProjects.Contains(file.NameWithoutExtension))
            .ToList();

        return Task.FromResult<IReadOnlyList<File>?>(projects);
    }
}
