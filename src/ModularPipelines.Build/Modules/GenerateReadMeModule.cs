using System.Text;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Settings;
using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Modules;

namespace ModularPipelines.Build.Modules;

[DependsOn<PackagePathsParserModule>]
[DependsOn<NugetVersionGeneratorModule>]
public class GenerateReadMeModule : Module
{
    private readonly IOptions<GitHubSettings> _githubSettings;
    private static readonly string AutomatedReadMeUpdateGitMessage = "Automated README.md Update";

    public GenerateReadMeModule(IOptions<GitHubSettings> githubSettings)
    {
        _githubSettings = githubSettings;
    }
    
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

        if (context.Git().Information.PreviousCommit?.Message?.Subject == AutomatedReadMeUpdateGitMessage)
        {
            // Unexpected
            return await NothingAsync();
        }

        await PushUpdatedCommit(context, cancellationToken);

        throw new Exception();
    }

    private async Task PushUpdatedCommit(IPipelineContext context, CancellationToken cancellationToken)
    {
        var branchTriggeringPullRequest = _githubSettings.Value.PullRequest!.Branch!;

        await GitHelpers.SetUserCommitInformation(context, cancellationToken);

        await GitHelpers.CheckoutBranch(context, branchTriggeringPullRequest, cancellationToken);

        await GitHelpers.CommitAndPush(context, branchTriggeringPullRequest, AutomatedReadMeUpdateGitMessage, _githubSettings.Value.StandardToken!,
            cancellationToken);

        // Fail this run - The git push will trigger a new run
        throw new Exception("Updating README.md with the latest packages. This run will abort. Another run will trigger with the formatted code.");
    }
}