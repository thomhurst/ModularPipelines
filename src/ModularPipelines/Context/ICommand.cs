using ModularPipelines.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Context;

public interface ICommand
{
    /// <summary>
    /// Execute a command line tool
    /// </summary>
    /// <param name="options"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<CommandResult> ExecuteCommandLineTool(CommandLineToolOptions options, CancellationToken cancellationToken = default);
}
