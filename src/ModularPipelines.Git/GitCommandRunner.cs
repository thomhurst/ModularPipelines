using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Options;

namespace ModularPipelines.Git;

public class GitCommandRunner
{
    private readonly IPipelineContext _context;

    public GitCommandRunner(IPipelineContext context)
    {
        _context = context;
    }

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

    public async Task<string?> RunCommandsOrNull(CommandLineOptions? commandEnvironmentOptions, params string?[] commands)
    {
        try
        {
            return await RunCommands(commandEnvironmentOptions, commands);
        }
        catch
        {
            return null;
        }
    }
}