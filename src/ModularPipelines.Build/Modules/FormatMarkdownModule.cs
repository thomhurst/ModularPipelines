using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Attributes;
using ModularPipelines.Build.Settings;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Git.Extensions;
using ModularPipelines.GitHub.Attributes;
using ModularPipelines.GitHub.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Modules.Behaviors;
using ModularPipelines.Node.Extensions;
using ModularPipelines.Node.Models;

namespace ModularPipelines.Build.Modules;

[SkipIfNoGitHubToken]
[SkipIfNoStandardGitHubToken]
[RunOnLinuxOnly]
[DependsOn<GenerateReadMeModule>]
public class FormatMarkdownModule : IModule<CommandResult>, ISkippable, IAlwaysRun
{
    private readonly IOptions<GitHubSettings> _gitHubSettings;

    public FormatMarkdownModule(IOptions<GitHubSettings> gitHubSettings)
    {
        _gitHubSettings = gitHubSettings;
    }

    public Task<SkipDecision> ShouldSkip(IPipelineContext context)
    {
        if (context.GitHub().EnvironmentVariables.EventName != "pull_request")
        {
            return SkipDecision.Skip("Not a pull request").AsTask();
        }

        if (string.IsNullOrEmpty(_gitHubSettings.Value.StandardToken))
        {
            return SkipDecision.Skip("No authentication token for git").AsTask();
        }

        return SkipDecision.DoNotSkip.AsTask();
    }

    public async Task<CommandResult?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        await context.Node().Npm.Install(new NpmInstallOptions
        {
            Arguments =
            [
                "remark-cli",
                "remark-preset-lint-consistent",
                "remark-preset-lint-recommended",
                "remark-lint-list-item-indent"
            ],
            SaveDev = true,
        }, cancellationToken);

        var filesToFormat = new List<string>
        {
            context.Git().RootDirectory.FindFile(x => x.Name == "README.md")!.Path,
            context.Git().RootDirectory.FindFile(x => x.Name == "README_Template.md")!.Path,
        };

        foreach (var fileToFormat in filesToFormat)
        {
            await context.Node().Npx.ExecuteAsync(new NpxOptions
            {
                Arguments =
                [
                    "remark",
                    fileToFormat,
                    "--use", "remark-lint",
                    "--use", "remark-preset-lint-consistent",
                    "--use", "remark-preset-lint-recommended",
                    "--output"
                ],
            }, cancellationToken);
        }

        if (!await GitHelpers.HasUncommittedChanges(context))
        {
            return null;
        }

        var branchTriggeringPullRequest = context.GitHub().EnvironmentVariables.HeadRef!;

        await GitHelpers.SetUserCommitInformation(context, cancellationToken);

        await GitHelpers.CheckoutBranch(context, branchTriggeringPullRequest, cancellationToken);

        await GitHelpers.CommitAndPush(context, branchTriggeringPullRequest, "Formatting Markdown", _gitHubSettings.Value.StandardToken!,
            cancellationToken);

        // Fail this run - The git push will trigger a new run
        throw new Exception("Formatting Markdown. This run will abort. Another run will trigger with the formatted markdown.");
    }
}