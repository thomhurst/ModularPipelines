using ModularPipelines.Context.Domains.Shell;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Context;

internal class Bash : IBash, IBashContext
{
    private readonly ICommand _command;

    public Bash(ICommand command)
    {
        _command = command;
    }

    public virtual Task<CommandResult> Command(BashCommandOptions options, CancellationToken cancellationToken = default)
    {
        return _command.ExecuteCommandLineTool(options, null, cancellationToken);
    }

    public virtual async Task<CommandResult> FromFile(BashFileOptions options, CancellationToken cancellationToken = default)
    {
        return await _command.ExecuteCommandLineTool(options with
        {
            FilePath = await ToWslPath(options.FilePath).ConfigureAwait(false),
        }, null, cancellationToken).ConfigureAwait(false);
    }

    private async Task<string> ToWslPath(string path)
    {
        if (OperatingSystem.IsWindows())
        {
            var result = await _command.ExecuteCommandLineTool(new GenericCommandLineToolOptions("wsl")
            {
                Arguments = ["wslpath", "-a", path.Replace("\\", "\\\\")],
            }).ConfigureAwait(false);

            return result.StandardOutput.Trim();
        }

        return path;
    }
}
