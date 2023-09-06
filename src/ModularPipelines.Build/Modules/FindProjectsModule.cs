using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Build.Modules;

public class FindProjectsModule : Module<IReadOnlyList<File>>
{
    protected override async Task<ModuleResult<IReadOnlyList<File>>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        await Task.Yield();
        
        return context.Environment
            .GitRootDirectory!
            .GetFiles(f => GetProjectsPredicate(f, context))
            .ToList();
    }
    
    private bool GetProjectsPredicate(File file, IModuleContext context)
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

        // Not yet ready
        if (path.EndsWith("ModularPipelines.AWS.csproj"))
        {
            return false;
        }

        context.Logger.LogInformation("Found File: {File}", path);

        return true;
    }
}

[DependsOn<FindProjectsModule>]
public class FindProjectDependenciesModule : Module<FindProjectDependenciesModule.ProjectDependencies>
{
    public record ProjectDependencies(IReadOnlyList<File> Dependencies, IReadOnlyList<File> Others);

    protected override async Task<ModuleResult<ProjectDependencies>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var projects = await GetModule<FindProjectsModule>();

        var dependencies = new List<File>();
        var others = new List<File>();
        
        foreach (var file in projects.Value ?? ArraySegment<File>.Empty)
        {
            var contents = await file.ReadLinesAsync();
            var isDependency = true;
            foreach (var projectReferenceLine in contents.Where(x => x.Contains("<ProjectReference")))
            {
                isDependency = false;

                var name = projectReferenceLine.Split('\\').Last().Split('"').First();

                var project = projects.Value!.FirstOrDefault(x => x.Name == name);

                if (project != null)
                {
                    dependencies.Add(project);
                }
            }

            if (isDependency)
            {
                dependencies.Add(file);
            }
            else
            {
                others.Add(file);
            }
        }

        return new ProjectDependencies(Dependencies: dependencies.Distinct().ToList(), Others: others.Except(dependencies).Distinct().ToList());
    }
}