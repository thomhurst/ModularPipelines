using System.Text;
using Microsoft.Build.Construction;
using ModularPipelines.Attributes;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Build.Modules;

[DependsOn<FindProjectsModule>]
public class GenerateReadMeModule : Module<None>
{
    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
        .WithAlwaysRun()
        .Build();

    protected override async Task<None> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var repositoryInfo = await context.Git().Information.GetInfoAsync().ConfigureAwait(false)
            ?? throw new InvalidOperationException("Git repository information is unavailable.");
        var gitRootDirectory = repositoryInfo.Root;

        var readMeActualOriginalContents = await gitRootDirectory.GetFile("README.md").ReadAsync(cancellationToken);
        var readmeTemplateContents = await gitRootDirectory.GetFile("README_Template.md").ReadAsync(cancellationToken);

        var generatedContentStringBuilder = new StringBuilder();

        generatedContentStringBuilder.AppendLine("| Package | Description | Version |");
        generatedContentStringBuilder.AppendLine("| --- | --- | --- |");

        var projects = await context.GetModule<FindProjectsModule>();

        foreach (var project in projects.Value
                     .Where(x => !x.NameWithoutExtension.StartsWith("ModularPipelines.Analyzers")))
        {
            var moduleName = project.NameWithoutExtension;

            var moduleDescription = GetModuleReadMeDescription(project);

            generatedContentStringBuilder.AppendLine(
                $"| {moduleName} | {moduleDescription} | [![nuget](https://img.shields.io/nuget/v/{moduleName}.svg)](https://www.nuget.org/packages/{moduleName}/) |");
        }

        var generatedContent = generatedContentStringBuilder.ToString().TrimEnd('\r', '\n');
        var updatedContents = readmeTemplateContents.Replace("%%% AVAILABLE MODULES PLACEHOLDER %%%", generatedContent);

        if (updatedContents == readMeActualOriginalContents)
        {
            // Nothing to do here = It's already matching what we expect :)
            return None.Value;
        }

        await gitRootDirectory.GetFile("README.md").WriteAsync(updatedContents, cancellationToken);

        return None.Value;
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
