using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Build.Modules;

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

        var projectDependencies = new ProjectDependencies(Dependencies: dependencies.Distinct().ToList(), Others: others.Except(dependencies).Distinct().ToList());
        
        foreach (var project in projectDependencies.Dependencies)
        {
            context.Logger.LogInformation("Project {Project} is a Dependency of other projects", project);
        }
        
        foreach (var project in projectDependencies.Others)
        {
            context.Logger.LogInformation("Project {Project} is a NOT Dependency of other projects", project);
        }
        
        return projectDependencies;
    }
}