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

    public async Task<string> RunCommands(CommandEnvironmentOptions? commandEnvironmentOptions, params string?[] commands)
    {
        commandEnvironmentOptions ??= new CommandEnvironmentOptions
        {
            LogInput = false,
            LogOutput = false
        };
        
        var commandResult = await _context.Command.ExecuteCommandLineTool(commandEnvironmentOptions.ToCommandLineToolOptions("git", commands.OfType<string>().ToArray()));

        return commandResult.StandardOutput.Trim();
    }

    public async Task<string?> RunCommandsOrNull(CommandEnvironmentOptions? commandEnvironmentOptions, params string?[] commands)
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