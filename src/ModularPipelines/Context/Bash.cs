using ModularPipelines.Helpers.Internal;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Context;

internal class Bash(ICommandContext command, PipelineWorkingDirectory pipelineWorkingDirectory) : IBashContext
{
    private static readonly TimeSpan WslPathExecutionTimeout = TimeSpan.FromSeconds(10);

    private readonly ICommandContext _command = command;

    private readonly PipelineWorkingDirectory _pipelineWorkingDirectory = pipelineWorkingDirectory;

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
        if (!OperatingSystem.IsWindows())
        {
            return await _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken).ConfigureAwait(false);
        }

        var execOpts = (executionOptions ?? new CommandExecutionOptions()) with
        {
            WorkingDirectory = executionOptions?.WorkingDirectory is { } workingDirectory
                ? _pipelineWorkingDirectory.ResolvePath(workingDirectory)
                : _pipelineWorkingDirectory.Path,
        };
        var tool = CliCommandFactory.ResolveWindowsCommand(
            options.Tool ?? "bash",
            execOpts.WorkingDirectory,
            execOpts.EnvironmentVariables) ?? options.Tool ?? "bash";
        var isWsl = string.Equals(tool, Path.Combine(Environment.SystemDirectory, "bash.exe"), StringComparison.OrdinalIgnoreCase);

        return await _command.ExecuteCommandLineToolAsync(options with
        {
            Tool = tool,
            // Git/MSYS Bash accepts Windows drive paths with forward slashes without WSL or cygpath.
            FilePath = isWsl
                ? await ToWslPath(options.FilePath, execOpts, cancellationToken).ConfigureAwait(false)
                : options.FilePath.Replace('\\', '/'),
        }, execOpts, cancellationToken).ConfigureAwait(false);
    }

    private async Task<string> ToWslPath(
        string path,
        CommandExecutionOptions executionOptions,
        CancellationToken cancellationToken)
    {
        // Preserve paths already expressed in WSL's filesystem; resolve Windows paths against the command's directory.
        var absolutePath = path.StartsWith('/') ? path : Path.GetFullPath(path, executionOptions.WorkingDirectory!);
        var result = await _command.ExecuteCommandLineToolAsync(new CommandLineToolOptions(Path.Combine(Environment.SystemDirectory, "wsl.exe"))
        {
            Arguments = ["--exec", "wslpath", "-a", absolutePath],
        }, new CommandExecutionOptions
        {
            ExecutionTimeout = WslPathExecutionTimeout,
            WorkingDirectory = executionOptions.WorkingDirectory,
            EnvironmentVariables = executionOptions.EnvironmentVariables,
            CommandLineCredentials = executionOptions.CommandLineCredentials,
        }, cancellationToken).ConfigureAwait(false);

        return result.StandardOutput.TrimEnd('\r', '\n');
    }
}
