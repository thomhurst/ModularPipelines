using ModularPipelines.Models;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Git.Enums;
using ModularPipelines.Git.Extensions;
using ModularPipelines.Git.Options;
using ModularPipelines.Options;

namespace ModularPipelines.Git;

public class GitOperations : IGitOperations
{
    private readonly IModuleContext _context;

    public GitOperations(IModuleContext context)
    {
        _context = context;
    }

    public Task<CommandResult> Checkout(GitCheckoutOptions options, CancellationToken cancellationToken = default)
    {
        return CustomCommand(options.WithArguments(options.BranchName), cancellationToken);
    }

    public Task<CommandResult> Version(GitOptions? options = null, CancellationToken cancellationToken = default)
    {
        return CustomCommand(ToGitCommandOptions(options, new[] { "--version" }), cancellationToken);
    }

    public Task<CommandResult> Fetch(GitOptions? options = null, CancellationToken cancellationToken = default)
    {
        return CustomCommand(ToGitCommandOptions(options, new[] { "fetch" }), cancellationToken);
    }

    public Task<CommandResult> Pull(GitOptions? options = null, CancellationToken cancellationToken = default)
    {
        return CustomCommand(ToGitCommandOptions(options, new[] { "pull" }), cancellationToken);
    }

    public Task<CommandResult> SetUpstream(GitSetUpstreamOptions? options = null, CancellationToken cancellationToken = default)
    {
        options ??= new GitSetUpstreamOptions(_context.Git().Information.BranchName!);
        return _context.Command.ExecuteCommandLineTool(options.WithArguments(options.BranchName), cancellationToken);
    }

    public Task<CommandResult> Push(GitOptions? options = null, CancellationToken cancellationToken = default)
    {
        return CustomCommand(ToGitCommandOptions(options, new[] { "push" }), cancellationToken);
    }

    public Task<CommandResult> Stage(GitStageOptions? options = null, CancellationToken cancellationToken = default)
    {
        var stageOption = options?.GitStageOption ?? GitStageOption.All;

        return CustomCommand(ToGitCommandOptions(options, new[] { "add", stageOption.GetCommandLineSwitch() }), cancellationToken);
    }

    public Task<CommandResult> Commit(string message, GitOptions? options = null, CancellationToken cancellationToken = default)
    {
        return CustomCommand(ToGitCommandOptions(options, new[] { "commit", "-m", message }), cancellationToken);
    }

    public Task<CommandResult> CustomCommand(CommandLineToolOptions options, CancellationToken cancellationToken = default)
    {
        return _context.Command.ExecuteCommandLineTool(options, cancellationToken);
    }

    private CommandLineToolOptions ToGitCommandOptions(CommandLineToolOptions? options, IEnumerable<string> arguments)
    {
        options ??= new CommandLineToolOptions("git");

        return options.WithArguments(arguments);
    }
}
