using ModularPipelines.Models;
using ModularPipelines.Cmd.Models;

namespace ModularPipelines.Cmd;

public interface ICmd
{
    Task<CommandResult> Script(CmdScriptOptions options, CancellationToken cancellationToken = default);
}