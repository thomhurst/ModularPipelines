using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Options;

namespace ModularPipelines.Git;

internal class GitCommandRunner
{
    private readonly IModuleContext _context;

    public GitCommandRunner(IModuleContext context)
    {
        _context = context;
    }

    public async Task<string> RunCommands(CommandLineOptions? commandEnvironmentOptions, params string?[] commands)
    {
        commandEnvironmentOptions ??= new CommandLineOptions
        {
            LogInput = false,
            LogOutput = false
        };

        var commandLineToolOptions = commandEnvironmentOptions.ToCommandLineToolOptions("git", commands.OfType<string>().ToArray()) with
        {
            LogInput = false,
            LogOutput = false
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
