using CliWrap.Buffered;
using ModularPipelines.Command.Extensions;
using ModularPipelines.Command.Options;
using ModularPipelines.Context;
using ModularPipelines.Git.Options;

namespace ModularPipelines.Git;

public class Git : IGit
{
    public IModuleContext Context { get; }

    public Git(IModuleContext context)
    {
        Context = context;
    }
    
    public Task<BufferedCommandResult> Checkout(GitCheckoutOptions options)
    {
        return Run(options);
    }

    public Task<BufferedCommandResult> Version(GitOptions? options = null)
    {
        var opts = new GitArgumentOptions(new[] {"--version"})
        {
            EnvironmentVariables = options?.EnvironmentVariables,
            WorkingDirectory = options?.WorkingDirectory
        };

        return Run(opts);
    }

    public Task<BufferedCommandResult> Fetch(GitOptions? options = null)
    {
        var opts = new GitArgumentOptions(new[] {"fetch"})
        {
            EnvironmentVariables = options?.EnvironmentVariables,
            WorkingDirectory = options?.WorkingDirectory
        };

        return Run(opts);
    }

    public Task<BufferedCommandResult> Pull(GitOptions? options = null)
    {
        var opts = new GitArgumentOptions(new[] {"pull"})
        {
            EnvironmentVariables = options?.EnvironmentVariables,
            WorkingDirectory = options?.WorkingDirectory
        };

        return Run(opts);
    }

    public Task<BufferedCommandResult> Push(GitOptions? options = null)
    {
        var opts = new GitArgumentOptions(new[] {"push"})
        {
            EnvironmentVariables = options?.EnvironmentVariables,
            WorkingDirectory = options?.WorkingDirectory
        };

        return Run(opts);
    }

    private Task<BufferedCommandResult> Run(GitArgumentOptions options)
    {
        return Context.Command().UsingCommandLineTool(new CommandLineToolOptions("git")
        {
            Arguments = options.Arguments,
            EnvironmentVariables = options.EnvironmentVariables,
            WorkingDirectory = options.WorkingDirectory
        });
    }
}