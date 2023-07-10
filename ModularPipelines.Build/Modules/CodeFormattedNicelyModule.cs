using ModularPipelines.Context;
using ModularPipelines.DotNet.Extensions;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Git.Options;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Build.Modules;

public class CodeFormattedNicelyModule : Module<CommandResult>
{
    private static readonly string DotnetFormatGitMessage = "DotNet Format";

    protected override async Task<ModuleResult<CommandResult>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
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
                WorkingDirectory = context.Environment.GitRootDirectory!,
                VerifyNoChanges = true
            }, cancellationToken);
        }
        catch (Exception)
        {
            // Something dodgy went wrong - It should've been formatted but it still isn't?
            if (context.Git().Information.PreviousCommit?.Message?.Subject == DotnetFormatGitMessage)
            {
                throw;
            }

            var branchTriggeringPullRequest = context.Environment.EnvironmentVariables.GetEnvironmentVariable("PULL_REQUEST_BRANCH")!;

            await context.Git().Commands.Config(new GitConfigOptions
            {
                Local = true,
                Arguments = new List<string>
                {
                    "user.email", "thomhurst@users.noreply.github.com"
                }
            }, cancellationToken);

            await context.Git().Commands.Config(new GitConfigOptions
            {
                Local = true,
                Arguments = new List<string>
                {
                    "user.name", "Tom Longhurst"
                }
            }, cancellationToken);

            await context.Git().Commands.Remote(new GitRemoteOptions
            {
                Arguments = new[]
                {
                    "set-url", "origin",
                    $"https://x-access-token:{context.Environment.EnvironmentVariables.GetEnvironmentVariable("GITHUB_TOKEN")}@github.com/thomhurst/ModularPipelines"
                }
            }, cancellationToken);

            await context.Git().Commands.Fetch(new GitFetchOptions(), token: cancellationToken);

            await context.Git().Commands
                .Checkout(new GitCheckoutOptions(branchTriggeringPullRequest), cancellationToken);

            // Actually perform the formatting
            await context.DotNet().Format(new DotNetFormatOptions
            {
                WorkingDirectory = context.Environment.GitRootDirectory!,
                VerifyNoChanges = false
            }, cancellationToken);

            // Commit the formatting
            await context.Git().Commands.Add(new GitAddOptions
            {
                All = true
            }, token: cancellationToken);
            await context.Git().Commands.Commit(new GitCommitOptions
            {
                Message = DotnetFormatGitMessage
            }, token: cancellationToken);

            await context.Git().Commands.Push(new GitPushOptions
            {
                Arguments = new[] { "-u", "origin", $"HEAD:{branchTriggeringPullRequest}" }
            }, token: cancellationToken);

            // Fail this run - The git push will trigger a new run
            throw new Exception("Formatting code. This run will abort. Another run will trigger with the formatted code.");
        }
    }
}
