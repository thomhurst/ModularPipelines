using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Options;

namespace ModularPipelines.Git;

/// <inheritdoc />
public class GitCommandRunner : IGitCommandRunner, IRawGitCommandRunner
{
    private readonly IPipelineContext _context;
    private readonly ILogger<GitCommandRunner> _logger;

    public GitCommandRunner(IPipelineContext context, ILogger<GitCommandRunner> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <inheritdoc />
    public Task<string> RunCommands(CommandExecutionOptions? commandEnvironmentOptions, params string?[] commands)
    {
        return RunCommands(commandEnvironmentOptions, default, commands);
    }

    /// <inheritdoc />
    public async Task<string> RunCommands(
        CommandExecutionOptions? commandEnvironmentOptions,
        CancellationToken cancellationToken,
        params string?[] commands)
    {
        var output = await RunCommandsCore(
            commandEnvironmentOptions,
            cancellationToken,
            commands).ConfigureAwait(false);
        return output.Trim();
    }

    Task<string> IRawGitCommandRunner.RunCommandsUntrimmed(
        CommandExecutionOptions? commandEnvironmentOptions,
        CancellationToken cancellationToken,
        params string?[] commands) =>
        RunCommandsCore(commandEnvironmentOptions, cancellationToken, commands);

    private async Task<string> RunCommandsCore(
        CommandExecutionOptions? commandEnvironmentOptions,
        CancellationToken cancellationToken,
        params string?[] commands)
    {
        commandEnvironmentOptions ??= new CommandExecutionOptions();

        var commandLineToolOptions = commandEnvironmentOptions.ToCommandLineToolOptions("git", commands.OfType<string>().ToArray());

        var executionOptions = commandEnvironmentOptions with
        {
            LogSettings = CommandLoggingOptions.Silent,
        };

        var commandResult = await _context.Shell.Command.ExecuteCommandLineToolAsync(
            commandLineToolOptions,
            executionOptions,
            cancellationToken).ConfigureAwait(false);

        return commandResult.StandardOutput;
    }

    /// <inheritdoc />
    public Task<string?> RunCommandsOrNull(CommandExecutionOptions? commandEnvironmentOptions, params string?[] commands)
    {
        return RunCommandsOrNull(commandEnvironmentOptions, default, commands);
    }

    /// <inheritdoc />
    public async Task<string?> RunCommandsOrNull(
        CommandExecutionOptions? commandEnvironmentOptions,
        CancellationToken cancellationToken,
        params string?[] commands)
    {
        try
        {
            var failureDetectingOptions = (commandEnvironmentOptions ?? new CommandExecutionOptions()) with
            {
                ThrowOnNonZeroExitCode = true,
            };
            return await RunCommands(
                failureDetectingOptions,
                cancellationToken,
                commands).ConfigureAwait(false);
        }
        catch (Exception ex) when (ex is not (OperationCanceledException or OutOfMemoryException or StackOverflowException))
        {
            _logger.LogDebug(ex, "Git command failed: {Commands}", string.Join(" ", commands.Where(c => c != null)));
            return null;
        }
    }
}
