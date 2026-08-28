using ModularPipelines.Context.Domains.Shell;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Context;

internal class Bash : IBashContext
{
    private static readonly TimeSpan WslPathExecutionTimeout = TimeSpan.FromSeconds(10);

    private readonly ICommandContext _command;

    public Bash(ICommandContext command)
    {
        _command = command;
    }

    public virtual Task<CommandResult> CommandAsync(BashCommandOptions options, CancellationToken cancellationToken = default)
    {
        return _command.ExecuteCommandLineToolAsync(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> FromFileAsync(BashFileOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options with
        {
            FilePath = await ToWslPath(options.FilePath, cancellationToken).ConfigureAwait(false),
        }, null, cancellationToken).ConfigureAwait(false);
    }

    private async Task<string> ToWslPath(string path, CancellationToken cancellationToken)
    {
        if (OperatingSystem.IsWindows())
        {
            var result = await _command.ExecuteCommandLineToolAsync(new CommandLineToolOptions("wsl")
            {
                Arguments = ["wslpath", "-a", path.Replace("\\", "\\\\")],
            }, new CommandExecutionOptions
            {
                ExecutionTimeout = WslPathExecutionTimeout,
            }, cancellationToken).ConfigureAwait(false);

            return result.StandardOutput.Trim();
        }

        return path;
    }
}
