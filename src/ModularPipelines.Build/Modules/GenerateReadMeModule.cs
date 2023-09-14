using System.Text;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Modules;

namespace ModularPipelines.Build.Modules;

[DependsOn<PackagePathsParserModule>]
[DependsOn<NugetVersionGeneratorModule>]
public class GenerateReadMeModule : Module
{
    protected override async Task<IDictionary<string, object>?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        var gitRootDirectory = context.Git().RootDirectory;

        var readMeActualOriginalContents = await gitRootDirectory.GetFile("README.md").ReadAsync();
        var readmeTemplateContents = await gitRootDirectory.GetFile("README_Template.md").ReadAsync();

        var availableModules = (await GetModule<PackagePathsParserModule>())
            .Value!
            .Where(x => !x.NameWithoutExtension.StartsWith("ModularPipelines.Analyzers"))
            .OrderBy(x => x.NameWithoutExtension)
            .ToList();

        var nugetVersion = await GetModule<NugetVersionGeneratorModule>();

        var generatedContentStringBuilder = new StringBuilder();

        generatedContentStringBuilder.AppendLine("| Package | Version |");
        generatedContentStringBuilder.AppendLine("| --- | --- |");

        foreach (var availableModule in availableModules)
        {
            var moduleName = availableModule.NameWithoutExtension
                .Replace($".{nugetVersion.Value!}", string.Empty);
            
            generatedContentStringBuilder.AppendLine($"| {moduleName} | [![nuget](https://img.shields.io/nuget/v/{moduleName}.svg)](https://www.nuget.org/packages/{moduleName}/) |");
        }

        var updatedContents = readmeTemplateContents.Replace("%%% AVAILABLE MODULES PLACEHOLDER %%%", generatedContentStringBuilder.ToString());

        if (updatedContents == readMeActualOriginalContents)
        {
            // Nothing to do here = It's already matching what we expect :)
            return await NothingAsync();
        }

        await gitRootDirectory.GetFile("README.md").WriteAsync(updatedContents);

        return await NothingAsync();
    }
}