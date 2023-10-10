using Microsoft.Extensions.Options;
using ModularPipelines.Build.Attributes;
using ModularPipelines.Build.Settings;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Extensions;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Git.Extensions;
using ModularPipelines.GitHub.Attributes;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Build.Modules;

[SkipIfDependabot]
[SkipOnMainBranch]
public class CodeFormattedNicelyModule : Module<CommandResult>
{
    public override ModuleRunType ModuleRunType => ModuleRunType.AlwaysRun;
    
    private const string DotnetFormatGitMessage = "DotNet Format";

    private readonly IOptions<GitHubSettings> _githubSettings;

    public CodeFormattedNicelyModule(IOptions<GitHubSettings> githubSettings)
    {
        _githubSettings = githubSettings;
    }

    /// <inheritdoc/>
    protected override async Task<CommandResult?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        if (!context.Git().Information.BranchName!.Contains("pull"))
        {
            return await NothingAsync();
        }

        try
        {
            // The code hasn't been formatted nicely!
            return await context.DotNet().Format(new DotNetFormatOptions
            {
                WorkingDirectory = context.Git().RootDirectory,
                VerifyNoChanges = true,
            }, cancellationToken);
        }
        catch (Exception)
        {
            // Something dodgy went wrong - It should've been formatted but it still isn't?
            if (context.Git().Information.PreviousCommit?.Message?.Subject == DotnetFormatGitMessage)
            {
                throw;
            }

            ArgumentNullException.ThrowIfNull(_githubSettings.Value.StandardToken);

            await context.DotNet().Format(new DotNetFormatOptions
            {
                WorkingDirectory = context.Git().RootDirectory,
                VerifyNoChanges = false,
            }, cancellationToken);

            var branchTriggeringPullRequest = _githubSettings.Value.PullRequest?.Branch!;

            await GitHelpers.SetUserCommitInformation(context, cancellationToken);

            await GitHelpers.CheckoutBranch(context, branchTriggeringPullRequest, cancellationToken);

            await GitHelpers.CommitAndPush(context, branchTriggeringPullRequest, DotnetFormatGitMessage, _githubSettings.Value.StandardToken!,
                cancellationToken);

            // Fail this run - The git push will trigger a new run
            throw new Exception("Formatting code. This run will abort. Another run will trigger with the formatted code.");
        }
    }
}
