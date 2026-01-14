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
    public async Task<string> RunCommands(CommandExecutionOptions? commandEnvironmentOptions, params string?[] commands)
    {
        commandEnvironmentOptions ??= new CommandExecutionOptions();

        var commandLineToolOptions = commandEnvironmentOptions.ToCommandLineToolOptions("git", commands.OfType<string>().ToArray());

        var executionOptions = new CommandExecutionOptions
        {
            LogSettings = CommandLoggingOptions.Silent,
        };

        var commandResult = await _context.Shell.Command.ExecuteCommandLineTool(commandLineToolOptions, executionOptions).ConfigureAwait(false);

        return commandResult.StandardOutput.Trim();
    }

    /// <inheritdoc />
    public async Task<string?> RunCommandsOrNull(CommandExecutionOptions? commandEnvironmentOptions, params string?[] commands)
    {
        try
        {
            return await RunCommands(commandEnvironmentOptions, commands).ConfigureAwait(false);
        }
        catch (Exception ex) when (ex is not (OutOfMemoryException or StackOverflowException))
        {
            _logger.LogDebug(ex, "Git command failed: {Commands}", string.Join(" ", commands.Where(c => c != null)));
            return null;
        }
    }
}
