using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Settings;
using ModularPipelines.Context;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Node.Extensions;
using ModularPipelines.Node.Models;

namespace ModularPipelines.Build.Modules;

[DependsOn<GenerateReadMeModule>]
public class FormatMarkdownModule : Module<CommandResult>
{
    private readonly IOptions<GitHubSettings> _gitHubSettings;

    public override ModuleRunType ModuleRunType => ModuleRunType.AlwaysRun;

    protected override Task<bool> ShouldSkip(IPipelineContext context)
    {
        return Task.FromResult(string.IsNullOrEmpty(_gitHubSettings.Value.PullRequest?.Branch)
                               || string.IsNullOrEmpty(_gitHubSettings.Value.StandardToken));
    }

    public FormatMarkdownModule(IOptions<GitHubSettings> gitHubSettings)
    {
        _gitHubSettings = gitHubSettings;
    }
    
    protected override async Task<CommandResult?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        await context.Node().Npm.Install(new NpmInstallOptions
        {
            Arguments = new[]
            {
                "remark-cli",
                "remark-preset-lint-consistent",
                "remark-preset-lint-recommended", 
                "remark-lint-list-item-indent"
            },
            SaveDev = true
        }, cancellationToken);

        var filesToFormat = new List<string>
        {
            context.Git().RootDirectory.FindFile(x => x.Name == "README.md")!.Path,
            context.Git().RootDirectory.FindFile(x => x.Name == "README_Template.md")!.Path,
            context.Git().RootDirectory.FindFile(x => x.Name == "ReleaseNotes.md")!.Path
        };
        
        foreach (var fileToFormat in filesToFormat)
        {
            await context.Node().Npx.ExecuteAsync(new NpxOptions
            {
                Arguments = new[]
                {
                    "remark",
                    fileToFormat,
                    "--use", "remark-lint",
                    "--use", "remark-preset-lint-consistent",
                    "--use", "remark-preset-lint-recommended",
                    "--output"
                }
            }, cancellationToken);
        }

        if (!await GitHelpers.HasUncommittedChanges(context))
        {
            return await NothingAsync();
        }

        var branchTriggeringPullRequest = _gitHubSettings.Value.PullRequest!.Branch!;

        await GitHelpers.SetUserCommitInformation(context, cancellationToken);

        await GitHelpers.CheckoutBranch(context, branchTriggeringPullRequest, cancellationToken);

        await GitHelpers.CommitAndPush(context, branchTriggeringPullRequest, "Formatting Markdown", _gitHubSettings.Value.StandardToken!,
            cancellationToken);

        // Fail this run - The git push will trigger a new run
        throw new Exception("Formatting Markdown. This run will abort. Another run will trigger with the formatted markdown.");
    }
}