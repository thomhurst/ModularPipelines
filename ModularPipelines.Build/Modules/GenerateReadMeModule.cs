using System.Text;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using File = ModularPipelines.FileSystem.File;

namespace ModularPipelines.Build.Modules;

[DependsOn<PackagePathsParserModule>]
[DependsOn<NugetVersionGeneratorModule>]
public class GenerateReadMeModule : Module
{
    private static readonly string AutomatedReadMeUpdateGitMessage = "Automated README.md Update";

    protected override async Task<ModuleResult<IDictionary<string, object>>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var gitRootDirectory = context.Environment.GitRootDirectory!;

        var readMeActualOriginalContents = await gitRootDirectory.GetFile("README.md").ReadAsync();
        var readmeTemplateContents = await gitRootDirectory.GetFile("README_Template.md").ReadAsync();

        var producedPackages = await GetModule<PackagePathsParserModule>();
        var nugetVersion = await GetModule<NugetVersionGeneratorModule>();

        var availableModules = producedPackages.Value ?? new List<File>();

        var generatedContentStringBuilder = new StringBuilder();

        generatedContentStringBuilder.AppendLine("| Package | Version | URL |");
        generatedContentStringBuilder.AppendLine("| --- | --- | --- |");

        foreach (var availableModule in availableModules)
        {
            var moduleName = availableModule.NameWithoutExtension
                .Replace($".{nugetVersion.Value!}", string.Empty);

            var url = $"https://nuget.org/packages/{moduleName}";

            generatedContentStringBuilder.AppendLine($"| {moduleName} | [![nuget](https://img.shields.io/nuget/v/{moduleName}.svg)](https://www.nuget.org/packages/{moduleName}/) | [{url}]({url}) |");
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