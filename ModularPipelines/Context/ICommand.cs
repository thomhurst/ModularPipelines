using ModularPipelines.Options;
using ModularPipelines.Models;

namespace ModularPipelines.Context;

public interface ICommand
{
    Task<CommandResult> ExecuteCommandLineTool(CommandLineToolOptions options, CancellationToken cancellationToken = default);
}