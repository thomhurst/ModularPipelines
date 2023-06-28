using CliWrap.Buffered;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Git.Enums;
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
    
    public Task<BufferedCommandResult> Checkout(GitCheckoutOptions options, CancellationToken cancellationToken = default)
    {
        return CustomCommand(options, cancellationToken);
    }

    public Task<BufferedCommandResult> Version(GitOptions? options = null, CancellationToken cancellationToken = default)
    {
        return CustomCommand(ToGitCommandOptions(options, new []{"--version"}), cancellationToken);
    }

    public Task<BufferedCommandResult> Fetch(GitOptions? options = null, CancellationToken cancellationToken = default)
    {
        return CustomCommand(ToGitCommandOptions(options, new []{"fetch"}), cancellationToken);
    }

    public Task<BufferedCommandResult> Pull(GitOptions? options = null, CancellationToken cancellationToken = default)
    {
        return CustomCommand(ToGitCommandOptions(options, new []{"pull"}), cancellationToken);
    }

    public Task<BufferedCommandResult> Push(GitOptions? options = null, CancellationToken cancellationToken = default)
    {
        return CustomCommand(ToGitCommandOptions(options, new []{"push"}), cancellationToken);
    }

    public Task<BufferedCommandResult> Stage(GitStageOptions? options = null, CancellationToken cancellationToken = default)
    {
        var stageOption = options?.GitStageOption ?? GitStageOption.All;
        
        return CustomCommand(ToGitCommandOptions(options, new []{ "add", stageOption.GetCommandLineSwitch() }), cancellationToken);
    }

    public Task<BufferedCommandResult> Commit(string message, GitOptions? options = null, CancellationToken cancellationToken = default)
    {
        return CustomCommand(ToGitCommandOptions(options, new []{ "commit", "-m", message }), cancellationToken);
    }

    public Task<BufferedCommandResult> CustomCommand(GitCommandOptions options, CancellationToken cancellationToken)
    {
        return _context.Command.UsingCommandLineTool(options.ToCommandLineToolOptions("git", options.Arguments), cancellationToken);
    }

    private GitCommandOptions ToGitCommandOptions(CommandEnvironmentOptions? options, IEnumerable<string> arguments)
    {
        options ??= new CommandEnvironmentOptions();
        
        return new GitCommandOptions(arguments)
        {
            WorkingDirectory = options.WorkingDirectory,
            EnvironmentVariables = options.EnvironmentVariables,
            Credentials = options.Credentials,
            LogInput = options.LogInput,
            LogOutput = options.LogOutput
        };
    }
}