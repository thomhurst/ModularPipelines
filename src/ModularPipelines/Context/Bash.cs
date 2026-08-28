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

    public virtual Task<CommandResult> RunAsync(
        string script,
        CommandExecutionOptions? executionOptions = null,
        CancellationToken cancellationToken = default)
    {
        return RunAsync(new BashCommandOptions(script), executionOptions, cancellationToken);
    }

    public virtual Task<CommandResult> RunAsync(
        BashCommandOptions options,
        CommandExecutionOptions? executionOptions = null,
        CancellationToken cancellationToken = default)
    {
        return _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken);
    }

    public virtual Task<CommandResult> RunFileAsync(
        string path,
        CommandExecutionOptions? executionOptions = null,
        CancellationToken cancellationToken = default)
    {
        return RunFileAsync(new BashFileOptions(path), executionOptions, cancellationToken);
    }

    public virtual async Task<CommandResult> RunFileAsync(
        BashFileOptions options,
        CommandExecutionOptions? executionOptions = null,
        CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineToolAsync(options with
        {
            FilePath = await ToWslPath(
                options.FilePath,
                executionOptions?.WorkingDirectory,
                cancellationToken).ConfigureAwait(false),
        }, executionOptions, cancellationToken).ConfigureAwait(false);
    }

    private async Task<string> ToWslPath(
        string path,
        string? workingDirectory,
        CancellationToken cancellationToken)
    {
        if (OperatingSystem.IsWindows())
        {
            var result = await _command.ExecuteCommandLineToolAsync(new CommandLineToolOptions("wsl")
            {
                Arguments = ["wslpath", "-a", path.Replace("\\", "\\\\")],
            }, new CommandExecutionOptions
            {
                ExecutionTimeout = WslPathExecutionTimeout,
                WorkingDirectory = workingDirectory,
            }, cancellationToken).ConfigureAwait(false);

            return result.StandardOutput.Trim();
        }

        return path;
    }
}
