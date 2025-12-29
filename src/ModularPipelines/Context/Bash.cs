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

    public virtual Task<CommandResult> Command(BashCommandOptions options, CancellationToken cancellationToken = default)
    {
        return _command.ExecuteCommandLineTool(options, cancellationToken);
    }

    public virtual async Task<CommandResult> FromFile(BashFileOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options with
        {
            FilePath = await ToWslPath(options.FilePath).ConfigureAwait(false),
        }, cancellationToken).ConfigureAwait(false);
    }

    private async Task<string> ToWslPath(string path)
    {
        if (OperatingSystem.IsWindows())
        {
            var result = await _command.ExecuteCommandLineTool(new CommandLineToolOptions("wsl")
            {
                Arguments = ["wslpath", "-a", path.Replace("\\", "\\\\")],
            }).ConfigureAwait(false);

            return result.StandardOutput.Trim();
        }

        return path;
    }
}