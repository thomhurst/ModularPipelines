using CliWrap.Buffered;
using ModularPipelines.Command.Extensions;
using ModularPipelines.Command.Options;
using ModularPipelines.Context;
using ModularPipelines.Git.Options;

namespace ModularPipelines.Git;

public class Git<T> : IGit<T>
{
    private readonly IModuleContext<T> _context;

    public Git(IModuleContext<T> context)
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

    public Task<BufferedCommandResult> CustomCommand(GitCommandOptions options, CancellationToken cancellationToken)
    {
        return _context.Command().UsingCommandLineTool(options.ToCommandLineToolOptions("git", options.Arguments), cancellationToken);
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