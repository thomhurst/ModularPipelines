using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Context;

internal class Bash : IBash
{
    private readonly ICommand _command;

    public Bash(ICommand command)
    {
        _command = command;
    }

    public Task<CommandResult> Command(BashCommandOptions options, CancellationToken cancellationToken = default)
    {
        return _command.ExecuteCommandLineTool(options, cancellationToken);
    }

    public async Task<CommandResult> FromFile(BashFileOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options with
        {
            FilePath = await ToWslPath(options.FilePath),
        }, cancellationToken);
    }

    private async Task<string> ToWslPath(string path)
    {
        if (OperatingSystem.IsWindows())
        {
            var result = await _command.ExecuteCommandLineTool(new CommandLineToolOptions("wsl")
            {
                Arguments = ["wslpath", "-a", path.Replace("\\", "\\\\")],
            });

            return result.StandardOutput.Trim();
        }

        return path;
    }
}