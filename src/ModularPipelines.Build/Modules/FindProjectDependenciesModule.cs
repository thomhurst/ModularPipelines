using Microsoft.Build.Construction;
using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Modules;
using ModularPipelines.FileSystem;

namespace ModularPipelines.Build.Modules;

[DependsOn<FindProjectsModule>]
public class FindProjectDependenciesModule : Module<FindProjectDependenciesModule.ProjectDependencies>
{
    protected override async Task<ProjectDependencies> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var projects = await context.GetModule<FindProjectsModule>();

        var dependencies = new List<FilePath>();

        foreach (var file in projects.Value)
        {
            var projectRootElement = ProjectRootElement.Open(file)!;

            var projectReferences = projectRootElement.Items
                .Where(i => i.ItemType == "ProjectReference")
                .Select(i => i.Include);

            foreach (var reference in projectReferences)
            {
                var name = Path.GetFileName(reference);
                var project = projects.Value.FirstOrDefault(x => x.Name == name);

                if (project != null)
                {
                    dependencies.Add(project);
                }
            }
        }

        var projectDependencies = new ProjectDependencies(Dependencies: dependencies.Distinct().ToList(), Others: projects.Value.Except(dependencies).Distinct().ToList());

        LogProjects(context, projectDependencies);

        return projectDependencies;
    }

    private static void LogProjects(IModuleContext context, ProjectDependencies projectDependencies)
    {
        foreach (var project in projectDependencies.Dependencies)
        {
            context.Logger.LogInformation("Project {Project} is a Dependency of other projects", project);
        }

        foreach (var project in projectDependencies.Others)
        {
            context.Logger.LogDebug("Project {Project} is not a dependency of other projects", project);
        }

        if (projectDependencies.Others.Count > 0)
        {
            context.Logger.LogInformation(
                "{Count} projects are not dependencies of other projects",
                projectDependencies.Others.Count);
        }
    }

    public record ProjectDependencies(IReadOnlyList<FilePath> Dependencies, IReadOnlyList<FilePath> Others);
}
