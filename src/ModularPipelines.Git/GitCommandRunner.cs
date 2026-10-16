using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Git.Options;
using ModularPipelines.Options;

namespace ModularPipelines.Git;

/// <inheritdoc />
public class GitCommandRunner(IPipelineContext context, ILogger<GitCommandRunner> logger) : IGitCommandRunner, IRawGitCommandRunner
{
    private readonly IPipelineContext _context = context;
    private readonly ILogger<GitCommandRunner> _logger = logger;

    /// <inheritdoc />
    public Task<string> RunCommandsAsync(CommandExecutionOptions? commandEnvironmentOptions, params string?[] commands)
    {
        return RunCommandsAsync(commandEnvironmentOptions, default, commands);
    }

    /// <inheritdoc />
    public async Task<string> RunCommandsAsync(
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

        var commandLineToolOptions = new CommandLineToolOptions("git")
        {
            Arguments = [.. commands.OfType<string>()],
        };

        var executionOptions = commandEnvironmentOptions with
        {
            Logging = CommandLoggingOptions.Silent,
        };

        var commandResult = await _context.Shell.RunAsync(
            commandLineToolOptions,
            executionOptions,
            cancellationToken).ConfigureAwait(false);

        return commandResult.StandardOutput;
    }

    /// <inheritdoc />
    public Task<string?> RunCommandsOrNullAsync(CommandExecutionOptions? commandEnvironmentOptions, params string?[] commands)
    {
        return RunCommandsOrNullAsync(commandEnvironmentOptions, default, commands);
    }

    /// <inheritdoc />
    public async Task<string?> RunCommandsOrNullAsync(
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
            return await RunCommandsAsync(
                failureDetectingOptions,
                cancellationToken,
                commands).ConfigureAwait(false);
        }
        catch (Exception ex) when (ex is not (OperationCanceledException or OutOfMemoryException or StackOverflowException))
        {
            if (_logger.IsEnabled(LogLevel.Debug))
            {
                _logger.LogDebug(ex, "Git command failed: {Commands}", string.Join(" ", commands.Where(c => c != null)));
            }

            return null;
        }
    }

    /// <inheritdoc />
    public async Task<string> RunCommandsAsync(
        GitOptions options,
        CommandExecutionOptions? commandEnvironmentOptions,
        CancellationToken cancellationToken)
    {
        var result = await _context.Shell.RunAsync(
            options,
            (commandEnvironmentOptions ?? new CommandExecutionOptions()) with
            {
                Logging = CommandLoggingOptions.Silent,
            },
            cancellationToken).ConfigureAwait(false);
        return result.StandardOutput.Trim();
    }

    /// <inheritdoc />
    public async Task<string?> RunCommandsOrNullAsync(
        GitOptions options,
        CommandExecutionOptions? commandEnvironmentOptions,
        CancellationToken cancellationToken)
    {
        try
        {
            return await RunCommandsAsync(
                options,
                (commandEnvironmentOptions ?? new CommandExecutionOptions()) with
                {
                    ThrowOnNonZeroExitCode = true,
                },
                cancellationToken).ConfigureAwait(false);
        }
        catch (Exception exception) when (exception is not (OperationCanceledException or OutOfMemoryException or StackOverflowException))
        {
            _logger.LogDebug(exception, "Git command failed");
            return null;
        }
    }
}
