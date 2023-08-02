using System.Text;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Extensions;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Git.Options;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Build.Modules;

public class GenerateReadMeModule : Module
{
    private readonly Func<string, bool>[] _excludedProjects =
    {
        s => s == "ModularPipelines.Build.csproj",
        s => s.Contains("Test"),
        s => s.StartsWith("ModularPipelines.Analyzers"),
        s => s == "ModularPipelines.Examples.csproj"
    };
    
    private static readonly string AutomatedReadMeUpdateGitMessage = "Automated README.md Update";

    protected override async Task<ModuleResult<IDictionary<string, object>>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var gitRootDirectory = context.Environment.GitRootDirectory!;

        var readMeActualOriginalContents = await gitRootDirectory.GetFile("README.md").ReadAsync();
        var readmeTemplateContents = await gitRootDirectory.GetFile("README_Template.md").ReadAsync();

        var availableModules = gitRootDirectory
            .GetFiles(file => file.Extension == ".csproj")
            .Where(file => !_excludedProjects.Any(x => x(file.Name)));

        var generatedContentStringBuilder = new StringBuilder();

        generatedContentStringBuilder.AppendLine("| Package | Version | URL |");
        generatedContentStringBuilder.AppendLine("| --- | --- | --- |");
        
        foreach (var availableModule in availableModules)
        {
            var moduleName = availableModule.NameWithoutExtension;
            var url = $"https://nuget.org/packages/{moduleName}";
            
            generatedContentStringBuilder.AppendLine($"| {moduleName} | [![nuget](https://img.shields.io/nuget/v/{moduleName}.svg)](https://www.nuget.org/packages/{moduleName}/) | [{url}]({url} |");
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

    private async Task PushUpdatedCommit(IModuleContext context, CancellationToken cancellationToken)
    {
        var branchTriggeringPullRequest =
            context.Environment.EnvironmentVariables.GetEnvironmentVariable("PULL_REQUEST_BRANCH")!;

        await GitHelpers.SetUserCommitInformation(context, cancellationToken);

        await GitHelpers.CheckoutBranch(context, branchTriggeringPullRequest, cancellationToken);

        await GitHelpers.CommitAndPush(context, branchTriggeringPullRequest, AutomatedReadMeUpdateGitMessage,
            cancellationToken);

        // Fail this run - The git push will trigger a new run
        throw new Exception("Updating README.md with the latest packages. This run will abort. Another run will trigger with the formatted code.");
    }
}