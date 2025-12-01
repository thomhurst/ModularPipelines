using System.Text;
using Microsoft.Build.Construction;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Modules.Behaviors;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Build.Modules;

[DependsOn<FindProjectsModule>]
public class GenerateReadMeModule : IModule<IDictionary<string, object>>, IAlwaysRun
{
    public async Task<IDictionary<string, object>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var gitRootDirectory = context.Git().RootDirectory;

        var readMeActualOriginalContents = await gitRootDirectory.GetFile("README.md").ReadAsync(cancellationToken);
        var readmeTemplateContents = await gitRootDirectory.GetFile("README_Template.md").ReadAsync(cancellationToken);

        var generatedContentStringBuilder = new StringBuilder();

        generatedContentStringBuilder.AppendLine("| Package | Description | Version |");
        generatedContentStringBuilder.AppendLine("| --- | --- | --- |");

        var projects = context.GetModule<FindProjectsModule, IReadOnlyList<File>>();

        foreach (var project in projects.Value!
                     .Where(x => !x.NameWithoutExtension.StartsWith("ModularPipelines.Analyzers")))
        {
            var moduleName = project.NameWithoutExtension;

            var moduleDescription = GetModuleReadMeDescription(project);

            generatedContentStringBuilder.AppendLine(
                $"| {moduleName} | {moduleDescription} | [![nuget](https://img.shields.io/nuget/v/{moduleName}.svg)](https://www.nuget.org/packages/{moduleName}/) |");
        }

        var updatedContents = readmeTemplateContents.Replace("%%% AVAILABLE MODULES PLACEHOLDER %%%", generatedContentStringBuilder.ToString());

        if (updatedContents == readMeActualOriginalContents)
        {
            // Nothing to do here = It's already matching what we expect :)
            return null;
        }

        await gitRootDirectory.GetFile("README.md").WriteAsync(updatedContents, cancellationToken);

        return null;
    }

    private string GetModuleReadMeDescription(File file)
    {
        var projectRootElement = ProjectRootElement.Open(file)!;

        var descriptionProperty =
            projectRootElement.Properties.FirstOrDefault(p => p.Name == "Description")
            ?? projectRootElement.Properties.FirstOrDefault(p => p.Name == "ModularPipelineReadMeDescription")
            ?? throw new ArgumentNullException($"No ModularPipelineReadMeDescription property found in {file.Name}");

        return descriptionProperty.Value;
    }
}