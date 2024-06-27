using Microsoft.Extensions.Options;
using ModularPipelines.Attributes;
using ModularPipelines.Build.Attributes;
using ModularPipelines.Build.Settings;
using ModularPipelines.Context;
using ModularPipelines.DotNet.Extensions;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Extensions;
using ModularPipelines.Git.Extensions;
using ModularPipelines.GitHub.Attributes;
using ModularPipelines.GitHub.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Build.Modules;

[SkipIfNoGitHubToken]
[SkipIfNoStandardGitHubToken]
[SkipOnMainBranch]
[RunOnLinuxOnly]
public class CodeFormattedNicelyModule : Module<CommandResult>
{
    public override ModuleRunType ModuleRunType => ModuleRunType.AlwaysRun;

    private const string DotnetFormatGitMessage = "DotNet Format";

    private readonly IOptions<GitHubSettings> _githubSettings;

    public CodeFormattedNicelyModule(IOptions<GitHubSettings> githubSettings)
    {
        _githubSettings = githubSettings;
    }

    protected override Task<SkipDecision> ShouldSkip(IPipelineContext context)
    {
        if (context.GitHub().EnvironmentVariables.EventName != "pull_request")
        {
            return SkipDecision.Skip("Not a pull request").AsTask();
        }
        
        if (string.IsNullOrEmpty(_githubSettings.Value.StandardToken))
        {
            return SkipDecision.Skip("No authentication token for git").AsTask();
        }

        return SkipDecision.DoNotSkip.AsTask();
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
            await context.DotNet().Format(new DotNetFormatOptions
            {
                Arguments = new[] { "whitespace" },
                WorkingDirectory = context.Git().RootDirectory,
                VerifyNoChanges = true,
                Severity = "info",
            }, cancellationToken);

            // The code hasn't been formatted nicely!
            return await context.DotNet().Format(new DotNetFormatOptions
            {
                WorkingDirectory = context.Git().RootDirectory,
                VerifyNoChanges = true,
                Severity = "info",
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
                Severity = "info",
            }, cancellationToken);

            await context.DotNet().Format(new DotNetFormatOptions
            {
                Arguments = new[] { "whitespace" },
                WorkingDirectory = context.Git().RootDirectory,
                VerifyNoChanges = false,
                Severity = "info",
            }, cancellationToken);

            var branchTriggeringPullRequest = context.GitHub().EnvironmentVariables.RefName!;

            await GitHelpers.SetUserCommitInformation(context, cancellationToken);

            await GitHelpers.CheckoutBranch(context, branchTriggeringPullRequest, cancellationToken);

            await GitHelpers.CommitAndPush(context, branchTriggeringPullRequest, DotnetFormatGitMessage, _githubSettings.Value.StandardToken!,
                cancellationToken);

            // Fail this run - The git push will trigger a new run
            throw new Exception("Formatting code. This run will abort. Another run will trigger with the formatted code.");
        }
    }
}