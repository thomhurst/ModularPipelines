using ModularPipelines.Cmd.Models;
using ModularPipelines.Models;

namespace ModularPipelines.Cmd;

public interface ICmd
{
    Task<CommandResult> Script(CmdScriptOptions options, CancellationToken cancellationToken = default);
}
