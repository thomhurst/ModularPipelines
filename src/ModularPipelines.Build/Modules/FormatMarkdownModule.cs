using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Attributes;
using ModularPipelines.Build.Settings;
using ModularPipelines.Configuration;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Git.Extensions;
using ModularPipelines.GitHub.Attributes;
using ModularPipelines.GitHub.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Node.Extensions;
using ModularPipelines.Node.Models;

namespace ModularPipelines.Build.Modules;

[SkipIfNoGitHubToken]
[SkipIfNoStandardGitHubToken]
[RunOnLinuxOnly]
[DependsOn<GenerateReadMeModule>]
public class FormatMarkdownModule : Module<CommandResult>
{
    private readonly IOptions<GitHubSettings> _gitHubSettings;

    public FormatMarkdownModule(IOptions<GitHubSettings> gitHubSettings)
    {
        _gitHubSettings = gitHubSettings;
    }

    protected override ModuleConfiguration Configure() => ModuleConfiguration.Create()
        .WithSkipWhen(ctx =>
        {
            if (ctx.GitHub().EnvironmentVariables.EventName != "pull_request")
            {
                return SkipDecision.Skip("Not a pull request");
            }

            if (string.IsNullOrEmpty(_gitHubSettings.Value.StandardToken))
            {
                return SkipDecision.Skip("No authentication token for git");
            }

            return SkipDecision.DoNotSkip;
        })
        .WithAlwaysRun()
        .Build();

    public override async Task<CommandResult?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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

        // Log that we're completing early - the git push will trigger a new run
        context.Logger.LogInformation("Formatting Markdown complete. The git push will trigger a new run with the formatted markdown.");

        return null;
    }
}