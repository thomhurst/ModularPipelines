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
        catch (Exception e)
        {
            // Something dodgy went wrong - It should've been formatted but it still isn't?
            if (context.Git().Information.PreviousCommit?.Message?.Subject == DotnetFormatGitMessage)
            {
                throw;
            }

            // Actually perform the formatting
            await context.DotNet().Format(new DotNetFormatOptions
            {
                WorkingDirectory = context.Environment.GitRootDirectory!,
                VerifyNoChanges = false
            }, cancellationToken);

            // Commit the formatting
            await context.Git().Operations.CustomCommand(new GitCommandOptions
            {
                Arguments = new List<string>
                {
                    "config", "user.email", "--local", "thomhurst@users.noreply.github.com"
                }
            }, cancellationToken);

            await context.Git().Operations.CustomCommand(new GitCommandOptions
            {
                Arguments = new List<string>
                {
                    "config", "user.name", "--local", "Tom Longhurst"
                }
            }, cancellationToken);

            await context.Git().Operations.Stage(cancellationToken: cancellationToken);
            await context.Git().Operations.Commit(DotnetFormatGitMessage, cancellationToken: cancellationToken);
            await context.Git().Operations.CustomCommand(new GitCommandOptions
            {
                Arguments = new[]
                {
                    "remote", "set-url", "origin",
                    $"https://x-access-token:{context.Environment.EnvironmentVariables.GetEnvironmentVariable("GITHUB_TOKEN")}@github.com/thomhurst/ModularPipelines"
                }
            }, cancellationToken);
            await context.Git().Operations.SetUpstream(new GitSetUpstreamOptions(context.Environment.EnvironmentVariables.GetEnvironmentVariable("PULL_REQUEST_BRANCH")!), cancellationToken: cancellationToken);
            await context.Git().Operations.Push(cancellationToken: cancellationToken);

            // Fail this run - The git push will trigger a new run
            throw new Exception("Formatting code. This run will abort. Another run will trigger with the formatted code.");
        }
    }
}
