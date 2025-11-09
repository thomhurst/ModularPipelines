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
using ModularPipelines.Modules.Behaviors;

namespace ModularPipelines.Build.Modules;

[SkipIfNoGitHubToken]
[SkipIfNoStandardGitHubToken]
[SkipOnMainBranch]
[RunOnLinuxOnly]
[AlwaysRun]
public class CodeFormattedNicelyModule : Module<CommandResult>, IModuleSkipLogic, IModuleErrorHandling
{

    private const string DotnetFormatGitMessage = "DotNet Format";

    private readonly IOptions<GitHubSettings> _githubSettings;

    public CodeFormattedNicelyModule(IOptions<GitHubSettings> githubSettings)
    {
        _githubSettings = githubSettings;
    }

    public Task<bool> ShouldIgnoreFailuresAsync(IPipelineContext context, Exception exception)
    {
        var shouldIgnore = exception.Message.Contains("Authentication failed", StringComparison.OrdinalIgnoreCase) ||
                          exception.Message.Contains("Invalid username or token", StringComparison.OrdinalIgnoreCase) ||
                          exception.Message.Contains("Password authentication is not supported", StringComparison.OrdinalIgnoreCase);

        return Task.FromResult(shouldIgnore);
    }

    public Task<SkipDecision> ShouldSkipAsync(IPipelineContext context)
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
    public override async Task<CommandResult?> ExecuteAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        if (!context.Git().Information.BranchName!.Contains("pull"))
        {
            return null;
        }

        try
        {
            await context.DotNet().Format(new DotNetFormatOptions
            {
                Arguments = ["whitespace"],
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
                Arguments = ["whitespace"],
                WorkingDirectory = context.Git().RootDirectory,
                VerifyNoChanges = false,
                Severity = "info",
            }, cancellationToken);

            var branchTriggeringPullRequest = context.GitHub().EnvironmentVariables.HeadRef!;

            await GitHelpers.SetUserCommitInformation(context, cancellationToken);

            await GitHelpers.CheckoutBranch(context, branchTriggeringPullRequest, cancellationToken);

            await GitHelpers.CommitAndPush(context, branchTriggeringPullRequest, DotnetFormatGitMessage, _githubSettings.Value.StandardToken!,
                cancellationToken);

            // Fail this run - The git push will trigger a new run
            throw new Exception("Formatting code. This run will abort. Another run will trigger with the formatted code.");
        }
    }
}