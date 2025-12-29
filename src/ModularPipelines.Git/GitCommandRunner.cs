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
    public async Task<string> RunCommands(CommandLineOptions? commandEnvironmentOptions, params string?[] commands)
    {
        commandEnvironmentOptions ??= new CommandLineOptions();

        var commandLineToolOptions = commandEnvironmentOptions.ToCommandLineToolOptions("git", commands.OfType<string>().ToArray()) with
        {
            LogSettings = CommandLoggingOptions.Silent,
        };

        var commandResult = await _context.Command.ExecuteCommandLineTool(commandLineToolOptions);

        return commandResult.StandardOutput.Trim();
    }

    /// <inheritdoc />
    public async Task<string?> RunCommandsOrNull(CommandLineOptions? commandEnvironmentOptions, params string?[] commands)
    {
        try
        {
            return await RunCommands(commandEnvironmentOptions, commands);
        }
        catch (Exception ex)
        {
            _logger.LogDebug(ex, "Git command failed: {Commands}", string.Join(" ", commands.Where(c => c != null)));
            return null;
        }
    }
}
