using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Modules;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Build.Modules;

[DependsOn<FindProjectsModule>]
public class FindProjectDependenciesModule : Module<FindProjectDependenciesModule.ProjectDependencies>
{
    /// <inheritdoc/>
    protected override async Task<ProjectDependencies?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var projects = await GetModule<FindProjectsModule>();

        var dependencies = new List<File>();

        foreach (var file in projects.Value!)
        {
            var contents = await file.ReadLinesAsync();

            foreach (var projectReferenceLine in contents.Where(x => x.Contains("<ProjectReference")))
            {
                var name = projectReferenceLine.Split('\\').Last().Split('"').First();

                var project = projects.Value!.FirstOrDefault(x => x.Name == name);

                if (project != null)
                {
                    dependencies.Add(project);
                }
            }
        }

        var projectDependencies = new ProjectDependencies(Dependencies: dependencies.Distinct().ToList(), Others: projects.Value!.Except(dependencies).Distinct().ToList());

        LogProjects(context, projectDependencies);

        return projectDependencies;
    }

    private static void LogProjects(IPipelineContext context, ProjectDependencies projectDependencies)
    {
        foreach (var project in projectDependencies.Dependencies)
        {
            context.Logger.LogInformation("Project {Project} is a Dependency of other projects", project);
        }

        foreach (var project in projectDependencies.Others)
        {
            context.Logger.LogInformation("Project {Project} is a NOT Dependency of other projects", project);
        }
    }

    public record ProjectDependencies(IReadOnlyList<File> Dependencies, IReadOnlyList<File> Others);
}