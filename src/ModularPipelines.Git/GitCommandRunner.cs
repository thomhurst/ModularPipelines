using Microsoft.Extensions.Logging;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Options;

namespace ModularPipelines.Git;

/// <inheritdoc />
public class GitCommandRunner : IGitCommandRunner
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
        commandEnvironmentOptions ??= new CommandExecutionOptions();

        var commandLineToolOptions = commandEnvironmentOptions.ToCommandLineToolOptions("git", commands.OfType<string>().ToArray());

        var executionOptions = new CommandExecutionOptions
        {
            LogSettings = CommandLoggingOptions.Silent,
        };

        var commandResult = await _context.Shell.Command.ExecuteCommandLineToolAsync(
            commandLineToolOptions,
            executionOptions,
            cancellationToken).ConfigureAwait(false);

        return commandResult.StandardOutput.Trim();
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
            return await RunCommands(commandEnvironmentOptions, cancellationToken, commands).ConfigureAwait(false);
        }
        catch (Exception ex) when (ex is not (OperationCanceledException or OutOfMemoryException or StackOverflowException))
        {
            _logger.LogDebug(ex, "Git command failed: {Commands}", string.Join(" ", commands.Where(c => c != null)));
            return null;
        }
    }
}
