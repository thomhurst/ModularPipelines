using ModularPipelines.Context;
using ModularPipelines.DotNet.Extensions;
using ModularPipelines.DotNet.Options;
using ModularPipelines.Git.Extensions;
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

            await context.DotNet().Format(new DotNetFormatOptions
            {
                WorkingDirectory = context.Environment.GitRootDirectory!,
                VerifyNoChanges = false
            }, cancellationToken);

            var branchTriggeringPullRequest = context.Environment.EnvironmentVariables.GetEnvironmentVariable("PULL_REQUEST_BRANCH")!;

            await GitHelpers.SetUserCommitInformation(context, cancellationToken);

            await GitHelpers.CheckoutBranch(context, branchTriggeringPullRequest, cancellationToken);

            await GitHelpers.CommitAndPush(context, branchTriggeringPullRequest, DotnetFormatGitMessage,
                cancellationToken);

            // Fail this run - The git push will trigger a new run
            throw new Exception("Formatting code. This run will abort. Another run will trigger with the formatted code.");
        }
    }
}
