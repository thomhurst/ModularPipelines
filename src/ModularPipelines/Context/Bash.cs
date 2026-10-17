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

    public virtual Task<CommandResult> RunFileAsync(
        BashFileOptions options,
        CommandExecutionOptions? executionOptions = null,
        CancellationToken cancellationToken = default)
    {
        return _command.ExecuteCommandLineToolAsync(options, executionOptions, cancellationToken);
    }

    internal async Task<BashFileOptions> PrepareFileAsync(
        BashFileOptions options,
        CommandExecutionOptions executionOptions,
        CancellationToken cancellationToken)
    {
        var execOpts = executionOptions with
        {
            WorkingDirectory = executionOptions.WorkingDirectory is { } workingDirectory
                ? _pipelineWorkingDirectory.ResolvePath(workingDirectory)
                : _pipelineWorkingDirectory.Path,
        };
        var requestedTool = options.Tool ?? "bash";
        var tool = CliCommandFactory.ResolveWindowsCommand(
            requestedTool,
            execOpts.WorkingDirectory,
            execOpts.EnvironmentVariables)
            ?? ResolveFallbackTool(requestedTool, Path.GetDirectoryName(Environment.ProcessPath));
        var systemBash = Path.Combine(Environment.SystemDirectory, "bash.exe");
        var isWsl = string.Equals(tool, systemBash, StringComparison.OrdinalIgnoreCase);

        return options with
        {
            Tool = tool,
            // Git/MSYS Bash accepts Windows drive paths with forward slashes without WSL or cygpath.
            FilePath = isWsl
                ? await ToWslPath(options.FilePath, execOpts, cancellationToken).ConfigureAwait(false)
                : options.FilePath.Replace('\\', '/'),
        };
    }

    internal static string ResolveFallbackTool(string requestedTool, string? applicationDirectory)
    {
        if (!requestedTool.Equals("bash", StringComparison.OrdinalIgnoreCase)
            && !requestedTool.Equals("bash.exe", StringComparison.OrdinalIgnoreCase))
        {
            return requestedTool;
        }

        // CreateProcess checks the host executable's directory before the system directories.
        if (applicationDirectory is not null)
        {
            var applicationBash = Path.Combine(applicationDirectory, "bash.exe");
            if (File.Exists(applicationBash))
            {
                return applicationBash;
            }
        }

        var systemBash = Path.Combine(Environment.SystemDirectory, "bash.exe");
        return File.Exists(systemBash) ? systemBash : requestedTool;
    }

    private async Task<string> ToWslPath(
        string path,
        CommandExecutionOptions executionOptions,
        CancellationToken cancellationToken)
    {
        // Preserve paths already expressed in WSL's filesystem; resolve Windows paths against the command's directory.
        var absolutePath = path.StartsWith('/') ? path : Path.GetFullPath(path, executionOptions.WorkingDirectory!);
        if (executionOptions.InternalDryRun)
        {
            // A dry run displays the absolute Windows path without starting WSL to translate it.
            return absolutePath;
        }

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

        var converted = result.StandardOutput.TrimEnd('\r', '\n');
        if (converted.Length == 0)
        {
            throw new InvalidOperationException($"wslpath returned no path for '{absolutePath}'.");
        }

        return converted;
    }
}
